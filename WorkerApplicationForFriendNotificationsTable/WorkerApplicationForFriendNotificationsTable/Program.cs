using Confluent.Kafka;
using Dapper;
using Newtonsoft.Json;
using Npgsql;

public class FriendNotification
{
    public Guid Id { get; set; }
    public Guid SenderId { get; set; }
    public Guid ReceiverId { get; set; }
    public string Status { get; set; }
}

public class Program
{
    public static async Task Main()
    {
        var producerconfig = new ProducerConfig
        {
            BootstrapServers = "localhost:29092",
            EnableIdempotence = true,
            Acks = Acks.All,
        };

        var _producer = new ProducerBuilder<string, string>(producerconfig)
        .Build();

        while (true)
        {
            try
            {
                var getQuery = "SELECT * FROM \"FriendNotificationsTable\" WHERE \"Status\" = 'Accepted' OR \"Status\" = 'Refused' LIMIT 1";
                var updateQuery = "UPDATE \"FriendNotificationsTable\" SET \"Status\" = 'Have Handled' WHERE \"Id\" = CAST(@Id AS uuid)";

                var requestNotification = new FriendNotification();

                using (var connection = new NpgsqlConnection("Host=10.99.62.254;Username=postgres;Password=Bu6!ERGA@2024;Database=SocialDatabase"))
                {
                    requestNotification = connection.QuerySingle<FriendNotification>(getQuery);
                }

                var kafkamessage = new Message<string, string> { Key = requestNotification.Id.ToString(), Value = JsonConvert.SerializeObject(requestNotification) };

                var producerResult = await _producer.ProduceAsync("FriendNotificationsTable", kafkamessage);
                Console.WriteLine($"Message sent to topic: {producerResult.Topic} | Partition: {producerResult.Partition} | Offset: {producerResult.Offset}");

                using (var connection = new NpgsqlConnection("Host=10.99.62.254;Username=postgres;Password=Bu6!ERGA@2024;Database=SocialDatabase"))
                {
                    connection.Execute(updateQuery, new { Id = requestNotification.Id });
                }
            }
            catch (Exception ex)
            {
                Thread.Sleep(1500);
                continue;
            }

        }
    }
}