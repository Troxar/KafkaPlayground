using OrderApi.Application.Events;
using OrderApi.Application.Interfaces;
using OrderApi.Application.Interfaces.Commands;
using OrderApi.Domain.Entities;
using OrderApi.Domain.ValueObjects;

namespace OrderApi.Application.Commands;

public class CreateOrderHandler : ICommandHandler<CreateOrderCommand, Guid>
{
    private readonly IOrderRepository _repository;
    private readonly IEventPublisher _publisher;

    public CreateOrderHandler(IOrderRepository repository, IEventPublisher publisher)
    {
        _repository = repository;
        _publisher = publisher;
    }

    public async Task<Guid> HandleAsync(CreateOrderCommand command, CancellationToken ct)
    {
        var items = command.Items.Select(x => new OrderItem(x.ProductId, x.Quantity, x.UnitPrice));
        var order = new Order(command.CustomerId, items);

        await _repository.AddAsync(order, ct);
        await _repository.SaveChangesAsync(ct);

        var @event = new OrderCreatedEvent(order.Id, order.TotalAmount);
        await _publisher.PublishAsync(@event, ct);

        return order.Id;
    }
}