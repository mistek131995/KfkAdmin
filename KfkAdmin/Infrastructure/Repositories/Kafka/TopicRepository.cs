using Confluent.Kafka;
using Confluent.Kafka.Admin;
using KfkAdmin.Interfaces.Repositories;
using KfkAdmin.Models.Entities;

namespace KfkAdmin.Infrastructure.Repositories.Kafka;

public class TopicRepository(IAdminClient adminClient, IConsumer<string?, string> consumer) : ITopicRepository
{
    public async Task<List<Topic>> GetAllAsync()
    {
        var metadata = await Task.Run(() => adminClient.GetMetadata(TimeSpan.FromSeconds(10)));

        var topics = new List<Topic>();
        
        foreach (var topic in metadata.Topics)
        {
            topics.Add(new Topic()
            {
                Name = topic.Topic, 
                BrokerIds = topic.Partitions.Select(x => x.Leader).Distinct().ToList(),
                PartitionCount = topic.Partitions.Count, 
                ReplicationFactor = (short)(topic.Partitions.FirstOrDefault()?.Replicas.Length ?? 0),
                MessageCount = await GetMessageCount(topic)
            });
        }
        
        return topics.ToList();
    }

    public async Task<List<Topic>> GetByBrokerIdAsync(int brokerId)
    {
        var metadata = await Task.Run(() => adminClient.GetMetadata(TimeSpan.FromSeconds(10)));

        var topics = new List<Topic>();

        foreach (var topic in metadata.Topics.Where(x => x.Partitions.Any(p => p.Leader == brokerId)))
        {
            topics.Add(new Topic()
            {
                Name = topic.Topic, 
                BrokerIds = topic.Partitions.Select(x => x.Leader).Distinct().ToList(),
                PartitionCount = topic.Partitions.Count, 
                ReplicationFactor = (short)(topic.Partitions.FirstOrDefault()?.Replicas.Length ?? 0),
                MessageCount = await GetMessageCount(topic)
            });
        }

        return topics;
    }

    public async Task<Topic?> GetByNameAsync(string name)
    {
        var metadata = await Task.Run(() => adminClient.GetMetadata(TimeSpan.FromSeconds(10)));
        var topic = metadata.Topics.FirstOrDefault(x => x.Topic == name);

        if (topic == null)
            return null;
        
        return new Topic()
        {
            Name = topic.Topic,
            PartitionCount = topic.Partitions.Count,
            ReplicationFactor = (short)(topic.Partitions.FirstOrDefault()?.Replicas.Length ?? 0),
            BrokerIds = topic.Partitions.Select(x => x.Leader).Distinct().ToList(),
            MessageCount = await GetMessageCount(topic)
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

    public async Task DeleteAsync(string name) => await adminClient.DeleteTopicsAsync([name]);
    public async Task DeleteAsync(List<string> names) => await adminClient.DeleteTopicsAsync(names);
    
    private async Task<long> GetMessageCount(TopicMetadata topicMetadata)
    {
        long count = 0;

        foreach (var partition in topicMetadata.Partitions)
        {
            var offset = await Task.Run(() => consumer.QueryWatermarkOffsets(new TopicPartition(topicMetadata.Topic, partition.PartitionId), TimeSpan.FromSeconds(10)));
            
            if(offset is not null)
                count += offset.High.Value - offset.Low.Value;
        }
        
        return count;
    }
}