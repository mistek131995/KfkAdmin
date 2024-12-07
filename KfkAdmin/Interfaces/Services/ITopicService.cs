using KfkAdmin.Interfaces.Common;
using KfkAdmin.Models;
using KfkAdmin.Models.Entities;

namespace KfkAdmin.Interfaces.Services;

public interface ITopicService : IBaseKafkaService
{
    Task<List<Topic>> GetAllAsync();
    Task<Topic> GetByNameAsync(string name);
    
    Task CreateTopicAsync(Topic topic);
}