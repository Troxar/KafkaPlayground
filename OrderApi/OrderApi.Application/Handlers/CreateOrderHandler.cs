using OrderApi.Application.Commands;
using OrderApi.Application.Interfaces.Handlers;

namespace OrderApi.Application.Handlers;

public class CreateOrderHandler : ICreateOrderHandler
{
    public Task<Guid> CreateOrderAsync(CreateOrderCommand command, CancellationToken ct)
    {
        // TODO: create order
        return Task.FromResult(Guid.NewGuid());
    }
}