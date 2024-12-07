using KfkAdmin.Interfaces.Common;
using KfkAdmin.Models.Entities;

namespace KfkAdmin.Interfaces.Repositories;

public interface IConsumerGroupRepository : IBaseKafkaRepository
{
    Task<List<ConsumerGroup>> GetAllAsync();
}