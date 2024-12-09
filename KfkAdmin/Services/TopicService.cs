using Confluent.Kafka;
using Confluent.Kafka.Admin;
using KfkAdmin.Interfaces.Providers;
using KfkAdmin.Interfaces.Repositories;
using KfkAdmin.Interfaces.Services;
using KfkAdmin.Models.Entities;

namespace KfkAdmin.Services;

public class TopicService(IAdminClient adminClient, IConsumer<Ignore, string> consumer, IProducer<string?, string> producer, IKafkaRepositoryProvider repositoryProvider) : ITopicService
{
    public async Task TransferDataAsync(string fromName, string toName)
    {
        consumer.Subscribe([fromName]);
        
        while (true)
        {
            var consumeResult = consumer.Consume(TimeSpan.FromMilliseconds(500));

            if (consumeResult == null)
            {
                // Если сообщения больше нет, выходим
                break;
            }

            // Отправляем сообщение в новый топик
            await producer.ProduceAsync(toName, new Message<string?, string> { Value = consumeResult.Message.Value });

            Console.WriteLine($"Transferred message: {consumeResult.Message.Value}");
        }

        consumer.Unsubscribe();
        
        var groupIds = await GetGroupsForTopicAsync(fromName);
        
        await TransferOffsetsAsync(fromName, toName, groupIds);
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

private async Task TransferOffsetsAsync(string oldTopic, string newTopic, List<string> groupIds)
{
    // Получение информации о разделах старого топика
    var metadata = adminClient.GetMetadata(TimeSpan.FromSeconds(5));
    var oldTopicPartitions = metadata.Topics
        .FirstOrDefault(t => t.Topic == oldTopic)?.Partitions
        .Select(p => new TopicPartition(oldTopic, p.PartitionId))
        .ToList();

    if (oldTopicPartitions == null || oldTopicPartitions.Count == 0)
    {
        return;
    }

    foreach (var groupId in groupIds)
    {
        var oldOffsets = consumer.Committed(oldTopicPartitions, TimeSpan.FromSeconds(10));

        // Создание списка отступов для нового топика
        var newOffsets = oldOffsets
            .Where(o => o.Offset != Offset.Unset)
            .Select(o => new TopicPartitionOffset(new TopicPartition(newTopic, o.Partition), o.Offset))
            .ToList();

        if (newOffsets.Any())
        {
            // Преобразование в формат ConsumerGroupTopicPartitionOffsets
            var offsetsForNewTopic = new List<ConsumerGroupTopicPartitionOffsets>
            {
                new (groupId, newOffsets)
            };

            // Применение отступов к новому топику
            await adminClient.AlterConsumerGroupOffsetsAsync(offsetsForNewTopic);
        }
    }
}

    
    private async Task<List<string>> GetGroupsForTopicAsync(string topicName)
    {
        // Получение всех групп
        var allGroups = (await adminClient.ListConsumerGroupsAsync(new ListConsumerGroupsOptions()
            {
                RequestTimeout = TimeSpan.FromSeconds(10)
            })).Valid
            .Select(g => g.GroupId)
            .ToList();

        var result = new List<string>();

        // Описание групп
        var groupDescriptions = await adminClient.DescribeConsumerGroupsAsync(allGroups);

        foreach (var group in groupDescriptions.ConsumerGroupDescriptions)
        {
            if (group.State == ConsumerGroupState.Stable)
            {
                foreach (var assignment in group.Members.Select(m => m.Assignment))
                {
                    // Проверяем, привязан ли раздел к топику
                    if (assignment.TopicPartitions.Any(tp => tp.Topic == topicName))
                    {
                        result.Add(group.GroupId);
                        break;
                    }
                }
            }
        }

        return result;
    }

}