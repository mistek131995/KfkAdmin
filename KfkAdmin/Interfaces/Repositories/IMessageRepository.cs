using KfkAdmin.Interfaces.Common;
using KfkAdmin.Models.Entities;

namespace KfkAdmin.Interfaces.Repositories;

public interface IMessageRepository : IBaseKafkaRepository
{
    Task<List<string>> GetByTopicNameAsync(string topicName);
    
    Task SendMessagesAsync(Message messages);
}