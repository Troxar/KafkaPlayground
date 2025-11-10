using OrderApi.Application.Commands;

namespace OrderApi.Application.Interfaces.Handlers;

public interface ICreateOrderHandler
{
    Task<Guid> CreateOrderAsync(CreateOrderCommand command, CancellationToken ct);
}