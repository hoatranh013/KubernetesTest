// Online C# Editor for free
// Write, Edit and Run your C# code using C# Online Compiler

using RabbitMQ.Client;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class HelloWorld
{
    public static async Task Main(string[] args)
    {
        var tcpClients = new List<TcpClient>();

        var rabbitMqFactory = new ConnectionFactory();
        rabbitMqFactory.UserName = "guest";
        rabbitMqFactory.Password = "password";
        rabbitMqFactory.VirtualHost = "/";
        rabbitMqFactory.HostName = "172.19.0.3";
        rabbitMqFactory.Port = 5672;

        var conn = rabbitMqFactory.CreateConnection();
        var channel = conn.CreateModel();
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
                var getMessage = new char[1024];
                var tcpClientStream = tcpClient.GetStream();
                var tcpClientStreamReader = new StreamReader(tcpClientStream);
                while (true)
                {
                    int bytesRead =  await tcpClientStreamReader.ReadAsync(getMessage, 0, getMessage.Length);
                    var getMessageContent = String.Join("", getMessage);
                    getMessage = new char[1024];
                    foreach (var otherClient in tcpClients)
                    {
                        var streamWriter = new StreamWriter(otherClient.GetStream());
                        streamWriter.AutoFlush = true;
                        await streamWriter.WriteAsync(getMessageContent);
                        await streamWriter.FlushAsync();
                    }

                    channel.BasicPublish("message-exchange", "info", false, null, Encoding.UTF8.GetBytes(getMessageContent));

                }
            });
        }

    }
}