using OrderApi.Application.Dtos;
using OrderApi.Application.Interfaces.Commands;

namespace OrderApi.Application.Commands;

public record CreateOrderCommand(Guid CustomerId, IReadOnlyCollection<OrderItemDto> Items) : ICommand<Guid>;