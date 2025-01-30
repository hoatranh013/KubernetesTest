using Confluent.Kafka;
public class Program
{
    public static async Task Main()
    {
        var producerconfig = new ProducerConfig
        {
            BootstrapServers = "kafka:9093",
            EnableIdempotence = true,
            Acks = Acks.All,
        };

        var getKafkaTopics = File.ReadAllLines("kafka_topics.txt");

        foreach (var topic in getKafkaTopics)
        {
            var _producer = new ProducerBuilder<Null, string>(producerconfig)
            .Build();

            var kafkamessage = new Message<Null, string> { Value = topic };

            var producerResult = await _producer.ProduceAsync(topic, kafkamessage);
        }
    }
}