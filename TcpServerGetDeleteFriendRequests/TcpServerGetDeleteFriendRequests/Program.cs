using Grpc.Net.Client;
using GrpcServiceInteractingBetweenUsers;
using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;

public class TcpServerGetDeleteFriendRequests
{
    public static async Task Main(string[] args)
    {
        var ipAddress = IPAddress.Parse("0.0.0.0");
        var ipEndpoint = new IPEndPoint(ipAddress, 30007);
        var tcpServer = new TcpListener(ipEndpoint);
        var grpcChannel = GrpcChannel.ForAddress("http://grpcserviceinteractingbetweenusers:8080");
        var sendFriendRequestService = new SendFriendRequestHandler.SendFriendRequestHandlerClient(grpcChannel);
        tcpServer.Start();
        while (true)
        {
            var tcpClient = await tcpServer.AcceptTcpClientAsync();
            Task.Run(async () =>
            {
                var getContent = new char[1024];
                var tcpClientStream = tcpClient.GetStream();
                var streamReader = new StreamReader(tcpClientStream);
                await streamReader.ReadAsync(getContent, 0, getContent.Length);
                var getJsonMessage = String.Join("", getContent.Where(x => x != '\0'));
                var request = JsonConvert.DeserializeObject<SendFriendRequestServiceRequest>(getJsonMessage);
                await sendFriendRequestService.DeleteFriendAsync(request);
            });

        }

    }
}