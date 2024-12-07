using Confluent.Kafka;
using Confluent.Kafka.Admin;
using KfkAdmin.Interfaces.Repositories;
using KfkAdmin.Models.Entities;
using Partition = KfkAdmin.Models.Entities.Partition;

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

    public async Task<Topic?> GetByNameAsync(string name)
    {
        var metadata = await Task.Run(() => adminClient.GetMetadata(TimeSpan.FromSeconds(10)));
        var topic = metadata.Topics.FirstOrDefault(x => x.Topic == name);

        if (topic == null)
            return null;
        else
            return new Topic()
            {
                Name = topic.Topic,
                PartitionCount = topic.Partitions.Count,
                ReplicationFactor = (short)(topic.Partitions.FirstOrDefault()?.Replicas.Length ?? 0)
            };
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

    public Task UpdateAsync(Topic topic)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(string name) => await adminClient.DeleteTopicsAsync([name]);
    public async Task DeleteAsync(List<string> names) => await adminClient.DeleteTopicsAsync(names);
}