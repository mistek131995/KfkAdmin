using Confluent.Kafka;
using KfkAdmin.Interfaces.Repositories;
using KfkAdmin.Models.Entities;

namespace KfkAdmin.Infrastructure.Repositories.Kafka;

public class MessageRepository(IConsumer<string?, string> consumer, IProducer<string?, string> producer, IAdminClient adminClient)
    : IMessageRepository
{
    public async Task<List<Message>> GetByTopicNameAsync(string topicName)
    {
        var messages = new List<Message>();

        var metadata = adminClient.GetMetadata(topicName, TimeSpan.FromSeconds(5));
        var topicPartitions = metadata.Topics
            .First(t => t.Topic == topicName).Partitions
            .Select(p => new TopicPartitionOffset(new TopicPartition(topicName, p.PartitionId), Offset.Beginning))
            .ToList();

        consumer.Assign(topicPartitions);

        await Task.Yield();
        
        while (true)
        {
            var consumeResult = consumer.Consume(TimeSpan.FromSeconds(2));
            if (consumeResult == null)
            {
                return messages;
            }

            messages.Add(new Message
            {
                Key = consumeResult.Message.Key,
                Payload = consumeResult.Message.Value,
                Headers = consumeResult.Message.Headers?.ToDictionary(h => h.Key, h => h.GetValueBytes()),
                Topic = consumeResult.Topic
            });
        }
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