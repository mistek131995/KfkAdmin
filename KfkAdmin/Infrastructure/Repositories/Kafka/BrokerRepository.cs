using Confluent.Kafka;
using KfkAdmin.Interfaces.Repositories;
using KfkAdmin.Models.Entities;

namespace KfkAdmin.Infrastructure.Repositories.Kafka;

public class BrokerRepository(IAdminClient adminClient) : IBrokerRepository
{
    public async Task<List<Broker>> GetAllAsync()
    {
        var metadatas = adminClient.GetMetadata(TimeSpan.FromSeconds(10));

        return metadatas.Brokers.Select(x => new Broker()
        {
            BrokerId = x.BrokerId,
            Host = x.Host,
            Port = x.Port
        }).ToList();
    }
}