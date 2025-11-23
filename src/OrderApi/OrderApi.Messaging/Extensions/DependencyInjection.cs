using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrderApi.Application.Interfaces;
using OrderApi.Messaging.Options;

namespace OrderApi.Messaging.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<KafkaOptions>()
            .Bind(configuration.GetSection("Kafka"))
            .ValidateDataAnnotations();

        services.AddSingleton<IProducer<string, string>>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<KafkaOptions>>().Value;
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = options.BootstrapServers
            };
            return new ProducerBuilder<string, string>(producerConfig).Build();
        });
        services.AddScoped<IEventPublisher, KafkaEventPublisher>();
        return services;
    }
}