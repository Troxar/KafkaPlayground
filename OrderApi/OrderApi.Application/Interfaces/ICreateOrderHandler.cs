using OrderApi.Application.Commands;

namespace OrderApi.Application.Interfaces;

public interface ICreateOrderHandler
{
    Task<Guid> CreateOrderAsync(CreateOrderCommand command, CancellationToken ct);
}