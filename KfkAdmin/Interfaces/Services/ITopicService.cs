using KfkAdmin.Interfaces.Common;
using KfkAdmin.Models;

namespace KfkAdmin.Interfaces.Services;

public interface ITopicService : IBaseKafkaService
{
    Task<List<Topic>> GetAllAsync();
    
    record TopicDto(string Name, int PartitionCount, short ReplicationFactor, bool IsInternal);
    Task CreateTopicAsync(TopicDto topic);
}