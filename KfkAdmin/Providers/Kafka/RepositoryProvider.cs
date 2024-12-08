using KfkAdmin.Infrastructure.Repositories.Kafka;
using KfkAdmin.Interfaces.Common;
using KfkAdmin.Interfaces.Providers;
using KfkAdmin.Interfaces.Repositories;

namespace KfkAdmin.Providers.Kafka;

public class RepositoryProvider(IServiceProvider serviceProvider) : IKafkaRepositoryProvider
{
    private readonly Dictionary<Type, IBaseKafkaRepository> _repositories = new();
    
    private T Get<T>() where T : IBaseKafkaRepository
    {
        var type = typeof(T);

        if (_repositories.TryGetValue(type, out var repository))
        {
            return (T)repository;
        }

        repository = ActivatorUtilities.CreateInstance<T>(serviceProvider);

        _repositories[type] = repository;

        return (T)repository;
    }
    
    public IBrokerRepository BrokerRepository => Get<BrokerRepository>();
    public ITopicRepository TopicRepository => Get<TopicRepository>();
    public IPartitionRepository PartitionRepository => Get<PartitionRepository>();
    public IMessageRepository MessageRepository => Get<MessageRepository>();
    public IConsumerGroupRepository ConsumerGroupRepository => Get<ConsumerGroupRepository>();
}