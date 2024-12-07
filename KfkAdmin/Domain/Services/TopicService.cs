using System.Data;
using KfkAdmin.Interfaces.Providers;
using KfkAdmin.Interfaces.Services;
using KfkAdmin.Models;
using KfkAdmin.Models.Entities;
using static KfkAdmin.Interfaces.Services.ITopicService;

namespace KfkAdmin.Domain.Services;

public class TopicService(IKafkaRepositoryProvider repositoryProvider) : ITopicService
{
    public async Task<List<Topic>> GetAllAsync() => 
        await repositoryProvider.TopicRepository.GetAllAsync();

    public async Task<Topic> GetByNameAsync(string name)
    {
        var topic = await repositoryProvider.TopicRepository.GetByNameAsync(name) ?? 
                    throw new NullReferenceException("Топик не найден");
        
        return topic;
    }

    public async Task CreateTopicAsync(Topic topic)
    {
        var name = topic.Name.Trim().Replace(" ", "_");
        var existingTopic = await repositoryProvider.TopicRepository.GetByNameAsync(name);
        
        if(existingTopic != null)
            throw new DuplicateNameException("Топик с таким именем уже существует.");
            
        await repositoryProvider.TopicRepository.CreateAsync(topic);
    }
}