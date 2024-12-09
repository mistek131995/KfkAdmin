using Confluent.Kafka;
using KfkAdmin.Interfaces.Repositories;
using KfkAdmin.Models.Entities;

namespace KfkAdmin.Infrastructure.Repositories.Kafka;

public class MessageRepository(IConsumer<Ignore, string> consumer, IProducer<string?, string> producer)
    : IMessageRepository
{
    public async Task<List<string>> GetByTopicNameAsync(string topicName)
    {
        return await Task.Run(() =>
        {
            var messages = new List<string>();

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

    public async Task SendMessagesAsync(Message message)
    {
        var headers = new Headers();

        foreach (var messageHeader in message.Headers)
        {
            headers.Add(messageHeader.Key, messageHeader.Value);
        }

        await producer.ProduceAsync(message.Topic, new Message<string?, string>()
        {
            Value = message.Payload,
            Key = message.Key,
            Headers = headers
        });
    }
}