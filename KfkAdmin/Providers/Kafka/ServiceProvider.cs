using KfkAdmin.Domain.Services;
using KfkAdmin.Interfaces.Common;
using KfkAdmin.Interfaces.Providers;
using KfkAdmin.Interfaces.Services;

namespace KfkAdmin.Providers.Kafka;

public class ServiceProvider(IServiceProvider serviceProvider) : IKafkaServiceProvider
{
    private readonly Dictionary<Type, IBaseKafkaService> _services = new();

    private T Get<T>() where T : IBaseKafkaService
    {
        var type = typeof(T);

        if (_services.TryGetValue(type, out var repository))
        {
            return (T)repository;
        }

        repository = ActivatorUtilities.CreateInstance<T>(serviceProvider);

        _services[type] = repository;

        return (T)repository;
    }
    
    public ITopicService TopicService => Get<TopicService>();
    public IMessageService MessageService => Get<MessageService>();
    public IConsumerGroupService ConsumerGroupService => Get<ConsumerGroupService>();
}