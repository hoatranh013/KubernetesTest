
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using TcpChatApplicationServer.Models;
using TcpChatApplicationServer.Services;

public class HelloWorld
{
    public static async Task Main(string[] args)
    {
        var tcpClients = new List<TcpClient>();

        var clientInformations = new List<ClientInformation>();

        var getAllClientServices = Assembly.GetExecutingAssembly().GetTypes().Where(x => !x.IsInterface && x.IsAssignableTo(typeof(IServiceClient)));

        var getServiceInstances = getAllClientServices.Select(x => new ServiceClient 
        { 
            serviceClient = x.GetConstructor(new Type[] { }).Invoke(new object[] { }), 
            serviceName = x.Name 
        }).ToList();


        var ipAddress = IPAddress.Parse("0.0.0.0");
        var ipEndpoint = new IPEndPoint(ipAddress, 30000);
        var tcpServer = new TcpListener(ipEndpoint);
        tcpServer.Start();
        while (true)
        {
            var tcpClient = await tcpServer.AcceptTcpClientAsync();
            tcpClients.Add(tcpClient);
            Task.Run(async () =>
            {
                var clientType = new char[60];
                var clientTypeReader = new StreamReader(tcpClient.GetStream());
                await clientTypeReader.ReadAsync(clientType, 0, clientType.Length);
                var getClientTypeService = String.Join("", clientType.Where(x => x != '\0'));

                var getInstance = getServiceInstances.FirstOrDefault(x => x.serviceName == getClientTypeService).serviceClient;
                var getServiceMethod = getAllClientServices.FirstOrDefault(x => x.Name == getClientTypeService).GetMethod("Handle");

                getServiceMethod.Invoke(getInstance, new object[] { clientInformations, tcpClient });
            });
        }

    }
}