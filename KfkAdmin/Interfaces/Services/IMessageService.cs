using KfkAdmin.Interfaces.Common;

namespace KfkAdmin.Interfaces.Services;

public interface IMessageService : IBaseKafkaService
{
    public Task<List<string>> GetByTopicAsync(string topicName);
}