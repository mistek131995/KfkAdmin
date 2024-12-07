using KfkAdmin.Interfaces.Common;

namespace KfkAdmin.Interfaces.Repositories;

public interface IMessageRepository : IBaseKafkaRepository
{
    Task<List<string>> GetByTopicNameAsync(string topicName);
}