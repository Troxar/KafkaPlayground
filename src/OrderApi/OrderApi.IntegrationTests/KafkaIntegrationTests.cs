using System.Text.Json;
using Confluent.Kafka;
using FluentAssertions;
using OrderApi.Application.Events;
using OrderApi.Messaging;
using Testcontainers.Kafka;

namespace OrderApi.IntegrationTests;

public class KafkaIntegrationTests : IAsyncLifetime
{
    private readonly KafkaContainer _kafkaContainer;

    public KafkaIntegrationTests()
    {
        _kafkaContainer = new KafkaBuilder()
            .WithImage("confluentinc/cp-kafka:7.4.3")
            .Build();
    }

    public async Task InitializeAsync()
    {
        await _kafkaContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _kafkaContainer.DisposeAsync();
    }

    [Fact]
    public async Task Publisher_ShouldSend_And_ConsumerShouldReadMessage()
    {
        // Arrange - publish
        var config = new ProducerConfig
        {
            BootstrapServers = _kafkaContainer.GetBootstrapAddress()
        };
        var producer = new ProducerBuilder<string, string>(config).Build();
        var publisher = new KafkaEventPublisher(producer);
        var @event = new OrderCreatedEvent(Guid.NewGuid(), 123);

        // Act - publish
        await publisher.PublishAsync(@event, CancellationToken.None);

        // Arrange - consume
        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = _kafkaContainer.GetBootstrapAddress(),
            GroupId = "test-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        using var consumer = new ConsumerBuilder<string, string>(consumerConfig).Build();
        consumer.Subscribe("order-created-event");

        // Act - consume
        var consumeResult = consumer.Consume(TimeSpan.FromSeconds(5));

        // Assert
        consumeResult.Should().NotBeNull();
        var payload = JsonSerializer.Deserialize<OrderCreatedEvent>(consumeResult.Message.Value);
        payload.Should().NotBeNull();
        payload.TotalAmount.Should().Be(@event.TotalAmount);
    }
}