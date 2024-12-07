using Confluent.Kafka;
using Confluent.Kafka.Admin;
using KfkAdmin.Interfaces.Repositories;
using KfkAdmin.Models.Entities;

namespace KfkAdmin.Infrastructure.Repositories.Kafka;

public class ConsumerGroupRepository(IAdminClient adminClient) : IConsumerGroupRepository
{
    public async Task<List<ConsumerGroup>> GetAllAsync()
    {
        var options = new ListConsumerGroupsOptions
        {
            RequestTimeout = TimeSpan.FromSeconds(10) // Указываем тайм-аут
        };
        var groups = await adminClient.ListConsumerGroupsAsync(options);

        return groups.Valid.Select(x => new ConsumerGroup()
        {
            GroupId = x.GroupId,
            State = x.State.ToString(),
            Type = x.Type.ToString(),
            IsSimpleConsumerGroup = x.IsSimpleConsumerGroup
        }).ToList();
    }
}