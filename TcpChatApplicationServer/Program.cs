// Online C# Editor for free
// Write, Edit and Run your C# code using C# Online Compiler

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class HelloWorld
{
    public static async Task Main(string[] args)
    {
        var tcpClients = new List<TcpClient>();

        var ipAddress = IPAddress.Parse("127.0.0.1");
        var ipEndpoint = new IPEndPoint(ipAddress, 2204);
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
                    await tcpClientStreamReader.ReadAsync(getMessage, 0, getMessage.Length);
                    var getMessageContent = String.Join("", getMessage);
                    getMessage = new char[1024];
                    foreach (var otherClient in tcpClients)
                    {
                        var streamWriter = new StreamWriter(otherClient.GetStream());
                        streamWriter.AutoFlush = true;
                        await streamWriter.WriteAsync(getMessageContent);
                        await streamWriter.FlushAsync();
                    }
                }
            });
        }

    }
}