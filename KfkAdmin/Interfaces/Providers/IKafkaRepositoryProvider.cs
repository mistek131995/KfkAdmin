using KfkAdmin.Interfaces.Repositories;

namespace KfkAdmin.Interfaces.Providers;

public interface IKafkaRepositoryProvider
{
    ITopicRepository TopicRepository { get; }
}