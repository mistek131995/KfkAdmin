using KfkAdmin.Interfaces.Services;

namespace KfkAdmin.Interfaces.Providers;

public interface IKafkaServiceProvider
{
    ITopicService TopicService { get; }
    IMessageService MessageService { get; }
    IConsumerGroupService ConsumerGroupService { get; }
}