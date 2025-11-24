using System.Text.Json;
using Confluent.Kafka;
using Moq;
using OrderApi.Application.Events;
using OrderApi.Messaging;

namespace OrderApi.Tests.EventPublishers;

public class KafkaEventPublisherTests
{
    [Fact]
    public async Task Publish_ShouldSendMessageToCorrectTopic()
    {
        // Arrange
        var mockProducer = new Mock<IProducer<string, string>>();
        var publisher = new KafkaEventPublisher(mockProducer.Object);
        var @event = new OrderCreatedEvent(Guid.NewGuid(), 123);

        // Act
        await publisher.PublishAsync(@event, CancellationToken.None);

        // Assert
        mockProducer.Verify(p => p.ProduceAsync(
            It.Is<string>(x => x == "order-created-event"),
            It.Is<Message<string, string>>(m => !string.IsNullOrWhiteSpace(m.Key)
                                                && JsonSerializer.Deserialize<OrderCreatedEvent>(m.Value,
                                                    (JsonSerializerOptions?)null)!.TotalAmount == @event.TotalAmount),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}