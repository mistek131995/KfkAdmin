using KfkAdmin.Interfaces.Providers;
using KfkAdmin.Interfaces.Services;
using KfkAdmin.Models.Entities;

namespace KfkAdmin.Domain.Services;

public class ConsumerGroupService(IKafkaRepositoryProvider repositoryProvider) : IConsumerGroupService
{
    public async Task<List<ConsumerGroup>> GetAll() => 
        await repositoryProvider.ConsumerGroupRepository.GetAllAsync();
}