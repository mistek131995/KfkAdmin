using KfkAdmin.Interfaces.Repositories;

namespace KfkAdmin.Interfaces.Providers;

public interface IKafkaRepositoryProvider
{
    IBrokerRepository BrokerRepository { get; }
    ITopicRepository TopicRepository { get; }
    IPartitionRepository PartitionRepository { get; }
    IMessageRepository MessageRepository { get; }
    IConsumerGroupRepository ConsumerGroupRepository { get; }
}