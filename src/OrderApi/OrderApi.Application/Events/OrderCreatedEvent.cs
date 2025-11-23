namespace OrderApi.Application.Events;

public record OrderCreatedEvent(Guid OrderId, decimal TotalAmount);