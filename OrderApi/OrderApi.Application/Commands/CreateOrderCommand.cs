using OrderApi.Application.Dtos;

namespace OrderApi.Application.Commands;

public record CreateOrderCommand(Guid CustomerId, IReadOnlyCollection<OrderItemDto> Items);