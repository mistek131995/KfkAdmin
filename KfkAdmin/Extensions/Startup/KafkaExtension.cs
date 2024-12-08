using Confluent.Kafka;
using KfkAdmin.Interfaces.Providers;
using KfkAdmin.Providers.Kafka;

namespace KfkAdmin.Extensions.Startup;

public static class KafkaExtension
{
    public static void AddKafkaExtension(this IServiceCollection services)
    {
        //var host = Environment.GetEnvironmentVariable("KafkaHost");
        
        services.AddSingleton<IAdminClient>(_ =>
        {
            var config = new AdminClientConfig
            {
                //BootstrapServers = host
                BootstrapServers = "localhost:9092,localhost:9093"
            };
            return new AdminClientBuilder(config).Build();
        });

        services.AddScoped<IKafkaRepositoryProvider, RepositoryProvider>();
    }
}