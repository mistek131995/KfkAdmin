using KfkAdmin.Interfaces.Services;

namespace KfkAdmin.Interfaces.Providers;

public interface IKafkaServiceProvider
{
    ITopicService TopicService { get; }
}