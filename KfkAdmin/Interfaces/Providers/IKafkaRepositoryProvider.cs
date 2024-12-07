using KfkAdmin.Interfaces.Repositories;

namespace KfkAdmin.Interfaces.Providers;

public interface IKafkaRepositoryProvider
{
    ITopicRepository TopicRepository { get; }
    IMessageRepository MessageRepository { get; }
    IConsumerGroupRepository ConsumerGroupRepository { get; }
}