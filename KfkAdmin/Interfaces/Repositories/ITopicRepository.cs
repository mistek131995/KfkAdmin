using KfkAdmin.Interfaces.Common;
using KfkAdmin.Models;

namespace KfkAdmin.Interfaces.Repositories;

public interface ITopicRepository : IBaseKafkaRepository
{
    Task<List<Topic>> GetAllAsync();
    Task CreateAsync(Topic topic);
}