﻿using Confluent.Kafka;
using KfkAdmin.Interfaces.Providers;
using KfkAdmin.Interfaces.Services;
using KfkAdmin.Providers.Kafka;
using KfkAdmin.Services;

namespace KfkAdmin.Extensions.Startup;

public static class KafkaExtension
{
    public const string CONSUMER_GROUP_ID = "kfk-admin-group";
    
    public static void AddKafkaExtension(this IServiceCollection services)
    {
        //var host = Environment.GetEnvironmentVariable("KafkaHost");
        var host = "localhost:9092,localhost:9093";
        
        services.AddTransient<IAdminClient>(_ =>
        {
            var config = new AdminClientConfig
            {
                BootstrapServers = host
            };
            return new AdminClientBuilder(config).Build();
        });
        
        services.AddTransient<IConsumer<string?, string>>(x => new ConsumerBuilder<string?, string>(new ConsumerConfig()
        {
            BootstrapServers = host,
            GroupId = CONSUMER_GROUP_ID,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false,
            MetadataMaxAgeMs = 1000
        }).Build());
        
        services.AddTransient<IProducer<string?, string>>(x => new ProducerBuilder<string?, string>(new ProducerConfig()
        {
            BootstrapServers = host,
        }).Build());

        services.AddScoped<IKafkaRepositoryProvider, RepositoryProvider>();
        services.AddScoped<ITopicService, TopicService>();
    }
}