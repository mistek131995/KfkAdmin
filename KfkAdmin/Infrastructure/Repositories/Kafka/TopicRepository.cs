using Confluent.Kafka;
using Confluent.Kafka.Admin;
using KfkAdmin.Interfaces.Repositories;
using KfkAdmin.Models;
using Partition = KfkAdmin.Models.Partition;

namespace KfkAdmin.Infrastructure.Repositories.Kafka;

public class TopicRepository(IAdminClient adminClient) : ITopicRepository
{
    public async Task<List<Topic>> GetAllAsync()
    {
        var metadata = await Task.Run(() => adminClient.GetMetadata(TimeSpan.FromSeconds(10)));

        return metadata.Topics.Select(topic => new Topic()
        {
            Name = topic.Topic, 
            PartitionCount = topic.Partitions.Count, 
            ReplicationFactor = (short)(topic.Partitions.FirstOrDefault()?.Replicas.Length ?? 0),
            Partitions = topic.Partitions.Select(partition => new Partition()
            {
                Id = partition.PartitionId,
            }).ToList()
        }).ToList();
    }

    public async Task CreateAsync(Topic topic)
    {
        var topicSpec = new TopicSpecification
        {
            Name = topic.Name,
            NumPartitions = topic.PartitionCount,
            ReplicationFactor = topic.ReplicationFactor,
        };
        
        await adminClient.CreateTopicsAsync(new List<TopicSpecification> { topicSpec });
    }
}