using KfkAdmin.Interfaces.Providers;
using KfkAdmin.Interfaces.Services;
using KfkAdmin.Models;
using static KfkAdmin.Interfaces.Services.ITopicService;

namespace KfkAdmin.Domain.Services;

public class TopicService(IKafkaRepositoryProvider repositoryProvider) : ITopicService
{
    public async Task<List<Topic>> GetAllAsync() => 
        await repositoryProvider.TopicRepository.GetAllAsync();

    public async Task CreateTopicAsync(TopicDto topicDto)
    {
        var name = topicDto.Name.Trim().Replace(" ", "_");
        
        await repositoryProvider.TopicRepository.CreateAsync(new Topic()
        {
            Name = name,
            PartitionCount = topicDto.PartitionCount,
            ReplicationFactor = topicDto.ReplicationFactor,
            IsInternal = topicDto.IsInternal,
        });
    }
}