using Marten;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class Message
{
    public Guid Id { get; set; }
    public string Content { get; set; }
}
public class HelloWorld
{
    public static async Task Main(string[] args)
    {
        using var store = DocumentStore.For("Host=172.18.0.2;Port=5432;Username=myuser;Password=mypassword;Database=test");

        var session = store.LightweightSession(System.Data.IsolationLevel.ReadCommitted);


        var rabbitMqFactory = new RabbitMQ.Client.ConnectionFactory();
        rabbitMqFactory.UserName = "guest";
        rabbitMqFactory.Password = "password";
        rabbitMqFactory.VirtualHost = "/";
        rabbitMqFactory.HostName = "172.18.0.3";
        rabbitMqFactory.Port = 5672;
        var conn = rabbitMqFactory.CreateConnection();

        var channel = conn.CreateModel();
        var rabbitmqConsumer = new EventingBasicConsumer(channel);
        rabbitmqConsumer.Received += async (sender, args) =>
        {
            var body = args.Body.ToArray();
            var message = Encoding.UTF8.GetString(body).Replace("\0", "");

            var messageObject = new Message
            {
                Id = Guid.NewGuid(),
                Content = message
            };

            var inputMessages = new List<Message>();
            inputMessages.Add(messageObject);
            session.Store<Message>(inputMessages);
            await session.SaveChangesAsync();
            channel.BasicAck(args.DeliveryTag, false);
        };
        string consumerTag = channel.BasicConsume("message-queue", false, rabbitmqConsumer);
        while (true)
        {

        }    
    }
}