using Confluent.Kafka;
using System.Net.Sockets;
using System.Net;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

public class RequestNotification
{
    public Guid Id { get; set; }
    public Guid SenderId { get; set; }
    public Guid ReceiverId { get; set; }
    public string Status { get; set; }
    public string Message { get; set; }
}

public class Program
{
    public static async Task Main()
    {
        var ipAddress = IPAddress.Parse("127.0.0.1");
        var ipEndpoint = new IPEndPoint(ipAddress, 30000);
        var tcpClient = new TcpClient();
        await tcpClient.ConnectAsync(ipEndpoint);

        var streamWriter = new StreamWriter(tcpClient.GetStream());
        streamWriter.AutoFlush = true;
        var getClientType = "RequestsTableNotificationTopic";
        await streamWriter.WriteAsync(getClientType.ToCharArray(), 0, getClientType.Length);

        CancellationToken stoppingToken = new CancellationToken();
        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = "localhost:29092",
            GroupId = Guid.NewGuid().ToString(),
            AutoOffsetReset = AutoOffsetReset.Latest,
            EnableAutoCommit = true,
            Acks = Acks.All
        };

        var builder = new ConsumerBuilder<string, string>(consumerConfig);

        var _consumer = builder.Build();
        _consumer.Subscribe("RequestsTableNotificationTopic");

        while (true)
        {
            var consumeResult = _consumer.Consume(stoppingToken);
            var messageValue = consumeResult.Message.Value;

            var requestNotificationModel = JsonConvert.DeserializeObject<RequestNotification>(messageValue);

            var messageSending = JsonConvert.SerializeObject(requestNotificationModel);
            streamWriter.Write(messageSending + "BONUS");
            streamWriter.Flush();
        }
    }
}