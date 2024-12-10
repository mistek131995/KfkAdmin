using KfkAdmin.Interfaces.Common;
using KfkAdmin.Models;
using KfkAdmin.Models.Entities;

namespace KfkAdmin.Interfaces.Repositories;

public interface ITopicRepository : IBaseKafkaRepository
{
    Task<List<Topic>> GetAllAsync();
    Task<List<Topic>> GetByBrokerIdAsync(int brokerId);
    Task<Topic?> GetByNameAsync(string name);
    
    Task CreateAsync(Topic topic);
    Task DeleteAsync(string name);
    Task DeleteAsync(List<string> names);
}