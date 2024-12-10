using KfkAdmin.Interfaces.Common;
using KfkAdmin.Models;
using KfkAdmin.Models.Entities;

namespace KfkAdmin.Interfaces.Repositories;

public interface ITopicRepository : IBaseKafkaRepository
{
    List<Topic> GetAll();
    List<Topic> GetByBrokerId(int brokerId);
    Topic? GetByName(string name);
    
    Task CreateAsync(Topic topic);
    Task DeleteAsync(string name);
    Task DeleteAsync(List<string> names);
}