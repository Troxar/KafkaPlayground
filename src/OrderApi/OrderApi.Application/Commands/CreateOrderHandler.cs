using OrderApi.Application.Interfaces;
using OrderApi.Application.Interfaces.Commands;
using OrderApi.Domain.Entities;
using OrderApi.Domain.ValueObjects;

namespace OrderApi.Application.Commands;

public class CreateOrderHandler : ICommandHandler<CreateOrderCommand, Guid>
{
    private readonly IOrderRepository _repository;

    public CreateOrderHandler(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> HandleAsync(CreateOrderCommand command, CancellationToken ct)
    {
        var items = command.Items.Select(x => new OrderItem(x.ProductId, x.Quantity, x.UnitPrice));
        var order = new Order(command.CustomerId, items);

        await _repository.AddAsync(order, ct);
        await _repository.SaveChangesAsync(ct);

        return order.Id;
    }
}