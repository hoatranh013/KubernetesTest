using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using TcpChatApplicationServer.Models;

namespace TcpChatApplicationServer.Services
{
    public class ClientUser : IServiceClient
    {
        IModel channel;
        object locked = new object();
        public ClientUser()
        {
            var rabbitMqFactory = new ConnectionFactory();
            rabbitMqFactory.UserName = "guest";
            rabbitMqFactory.Password = "password";
            rabbitMqFactory.VirtualHost = "/";
            rabbitMqFactory.HostName = "127.0.0.1";
            rabbitMqFactory.Port = 5672;

            var conn = rabbitMqFactory.CreateConnection();
            channel = conn.CreateModel();
            try
            {
                channel.ExchangeDeclare("message-exchange", ExchangeType.Direct, true, false);
            }
            catch
            {
                Console.WriteLine("Exchange Exists");
            }

            try
            {
                channel.QueueDeclare("message-queue", durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            }
            catch
            {
                Console.WriteLine("Queue Exists");
            }

            channel.QueueBind("message-queue", "message-exchange", "info");
        }
        public void Handle(ref List<ClientInformation> clientInformations, TcpClient tcpClient)
        {
            var getUsername = new char[8];
            var authenStreamWriter = new StreamWriter(tcpClient.GetStream());
            authenStreamWriter.AutoFlush = true;
            authenStreamWriter.Write("Please Enter Your Username");

            var streamReaderUsername = new StreamReader(tcpClient.GetStream());
            streamReaderUsername.Read(getUsername, 0, getUsername.Length);

            Monitor.Enter(locked);
            clientInformations.Add(new ClientInformation
            {
                tcpClient = tcpClient,
                ClientName = String.Join("", getUsername),
                ClientId = Guid.NewGuid()
            });
            Monitor.Exit(locked);

            var getMessage = new char[1024];
            var tcpClientStream = tcpClient.GetStream();
            var tcpClientStreamReader = new StreamReader(tcpClientStream);
            while (true)
            {
                int bytesRead = tcpClientStreamReader.Read(getMessage, 0, getMessage.Length);
                var getMessageContent = String.Join("", getMessage);
                getMessage = new char[1024];
                var getClientInformation = clientInformations.FirstOrDefault(x => x.tcpClient == tcpClient).ClientName;

                getMessageContent = getClientInformation + " Said - " + getMessageContent;

                foreach (var otherClient in clientInformations)
                {
                    var streamWriter = new StreamWriter(otherClient.tcpClient.GetStream());
                    streamWriter.AutoFlush = true;
                    streamWriter.Write(getMessageContent);
                    streamWriter.Flush();
                }

                channel.BasicPublish("message-exchange", "info", false, null, Encoding.UTF8.GetBytes(getMessageContent));
            }
        }
    }
}
