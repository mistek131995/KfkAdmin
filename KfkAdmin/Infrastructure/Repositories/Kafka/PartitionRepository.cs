using Confluent.Kafka;
using KfkAdmin.Interfaces.Repositories;
using Partition = KfkAdmin.Models.Entities.Partition;

namespace KfkAdmin.Infrastructure.Repositories.Kafka;

public class PartitionRepository(IAdminClient adminClient) : IPartitionRepository
{
    public List<Partition> GetAll()
    {
        var metadata = adminClient.GetMetadata(TimeSpan.FromSeconds(10));
        
        return metadata.Topics
            .SelectMany(t => t.Partitions)
            .Select(x => new Partition()
            {
                Id = x.PartitionId,
                BrokerId = x.Leader,
            }).ToList();
    }
}