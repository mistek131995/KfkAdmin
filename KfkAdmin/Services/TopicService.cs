using Confluent.Kafka;
using Confluent.Kafka.Admin;
using KfkAdmin.Interfaces.Providers;
using KfkAdmin.Interfaces.Services;
using KfkAdmin.Models.Entities;

namespace KfkAdmin.Services;

public class TopicService(IAdminClient adminClient, IConsumer<string?, string> consumer, IProducer<string?, string> producer, IKafkaRepositoryProvider repositoryProvider) : ITopicService
{
    public async Task TransferDataAsync(string fromName, string toName)
    {
        var metadata = await Task.Run(() => adminClient.GetMetadata(fromName, TimeSpan.FromSeconds(5)));
        var topicPartitions = metadata.Topics
            .First(t => t.Topic == fromName).Partitions
            .Select(p => new TopicPartitionOffset(new TopicPartition(fromName, p.PartitionId), Offset.Beginning))
            .ToList();
        
        consumer.Assign(topicPartitions);
        
        while (true)
        {
            var consumeResult = await Task.Run(() => consumer.Consume(TimeSpan.FromSeconds(2)));
            if (consumeResult == null)
            {
                break;
            }

            await producer.ProduceAsync(toName, new Message<string?, string>
            {
                Key = consumeResult.Message.Key,
                Value = consumeResult.Message.Value,
                Headers = consumeResult.Message.Headers
            });
        }
        
        await TransferOffsetsAsync(fromName, toName);
    }

    public async Task RenameAsync(string oldName, string newName)
    {
        var oldTopic = await repositoryProvider.TopicRepository.GetByNameAsync(oldName) ?? 
                       throw new NullReferenceException($"Топик с именем {oldName} не найден");

        await repositoryProvider.TopicRepository.CreateAsync(new Topic()
        {
            Name = newName,
            PartitionCount = oldTopic.PartitionCount,
            ReplicationFactor = oldTopic.ReplicationFactor
        });
        
        await TransferDataAsync(oldName, newName);
        
        await repositoryProvider.TopicRepository.DeleteAsync(oldName);
    }

    private async Task TransferOffsetsAsync(string oldTopic, string newTopic)
    {
        // Получение всех Consumer Groups, связанных с топиком
        var groupIds = await GetConsumerGroupsForTopicAsync(oldTopic);

        foreach (var groupId in groupIds)
        {
            try
            {
                // Получаем отступы для каждой группы
                var offsets = consumer.Committed(
                    new List<TopicPartition> { new TopicPartition(oldTopic, 0) }, // Обновите для всех партиций
                    TimeSpan.FromSeconds(5)
                );

                var newOffsets = offsets.Select(o => new TopicPartitionOffset(
                    new TopicPartition(newTopic, o.Partition),
                    o.Offset
                )).ToList();

                // Обновляем отступы для нового топика
                var offsetsForNewTopic = new List<ConsumerGroupTopicPartitionOffsets>
                {
                    new(groupId, newOffsets)
                };

                await adminClient.AlterConsumerGroupOffsetsAsync(offsetsForNewTopic);
                Console.WriteLine($"Offsets transferred for group '{groupId}'.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error transferring offsets for group '{groupId}': {ex.Message}");
            }
        }
    }

    private async Task<List<string>> GetConsumerGroupsForTopicAsync(string topic)
    {
        var groups = await Task.Run(() => adminClient.ListGroups(TimeSpan.FromSeconds(5)));
        var groupIds = groups
            .Where(g => g.State == "STABLE") // Только активные группы
            .Select(g => g.Group)
            .ToList();

        return groupIds;
    }

}