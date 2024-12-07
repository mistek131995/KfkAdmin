using KfkAdmin.Interfaces.Providers;
using KfkAdmin.Interfaces.Services;

namespace KfkAdmin.Domain.Services;

public class MessageService(IKafkaRepositoryProvider repositoryProvider) : IMessageService
{
    public async Task<List<string>> GetByTopicAsync(string topicName) => 
        await repositoryProvider.MessageRepository.GetByTopicNameAsync(topicName);
}