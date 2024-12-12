using KfkAdmin.Interfaces.Common;
using KfkAdmin.Models.Entities;

namespace KfkAdmin.Interfaces.Repositories;

public interface IPartitionRepository : IBaseKafkaRepository
{
    Task<List<Partition>> GetAllAsync();
    Task<List<Partition>> GetByTopicNameAsync(string name);
}