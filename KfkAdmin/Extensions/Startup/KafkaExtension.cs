using Confluent.Kafka;
using KfkAdmin.Interfaces.Providers;
using KfkAdmin.Interfaces.Services;
using KfkAdmin.Providers.Kafka;
using KfkAdmin.Services;

namespace KfkAdmin.Extensions.Startup;

public static class KafkaExtension
{
    public static void AddKafkaExtension(this IServiceCollection services)
    {
        //var host = Environment.GetEnvironmentVariable("KafkaHost");
        var host = "localhost:9092,localhost:9093";
        
        services.AddScoped<IAdminClient>(_ =>
        {
            var config = new AdminClientConfig
            {
                BootstrapServers = host
            };
            return new AdminClientBuilder(config).Build();
        });
        
        services.AddScoped<IConsumer<Ignore, string>>(sp => new ConsumerBuilder<Ignore, string>(new ConsumerConfig()
        {
            BootstrapServers = host,
            GroupId = "kfk-admin-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        }).Build());

        services.AddScoped<IKafkaRepositoryProvider, RepositoryProvider>();
        services.AddScoped<ITopicService, TopicService>();
    }
}