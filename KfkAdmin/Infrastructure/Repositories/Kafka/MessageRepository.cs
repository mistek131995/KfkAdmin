using Confluent.Kafka;
using KfkAdmin.Interfaces.Repositories;

namespace KfkAdmin.Infrastructure.Repositories.Kafka;

public class MessageRepository : IMessageRepository
{
    public async Task<List<string>> GetByTopicNameAsync(string topicName)
    {
        return await Task.Run(() =>
        {
            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = Environment.GetEnvironmentVariable("KafkaHost"),
                GroupId = "blazor-group",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };

            var messages = new List<string>();

            using var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();

            consumer.Subscribe(topicName);

            for (int i = 0; i < 100; i++)
            {
                try
                {
                    var result = consumer.Consume(TimeSpan.FromSeconds(5));
                    if (result != null)
                    {
                        messages.Add(result.Message.Value);
                    }
                }
                catch (ConsumeException ex)
                {
                    Console.WriteLine($"Ошибка при чтении Kafka: {ex.Message}");
                }
            }

            consumer.Close();
            
            return messages; 
        });
    }
}