using System.Text.Json;
using Confluent.Kafka;
using OrderApi.Application.Interfaces;
using OrderApi.Messaging.Extensions;

namespace OrderApi.Messaging;

public class KafkaEventPublisher : IEventPublisher
{
    private readonly IProducer<string, string> _producer;

    public KafkaEventPublisher(IProducer<string, string> producer)
    {
        _producer = producer;
    }

    public async Task PublishAsync<T>(T @event, CancellationToken ct)
    {
        var json = JsonSerializer.Serialize(@event);
        var topic = typeof(T).Name.PascalToKebabCase();
        var message = new Message<string, string> { Key = Guid.NewGuid().ToString(), Value = json };
        await _producer.ProduceAsync(topic, message, ct);
    }
}