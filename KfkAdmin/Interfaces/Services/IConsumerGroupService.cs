using KfkAdmin.Interfaces.Common;
using KfkAdmin.Models.Entities;

namespace KfkAdmin.Interfaces.Services;

public interface IConsumerGroupService : IBaseKafkaService
{
    Task<List<ConsumerGroup>> GetAll();
}