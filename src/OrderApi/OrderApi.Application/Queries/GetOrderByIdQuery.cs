using OrderApi.Application.Interfaces.Queries;
using OrderApi.Domain.Entities;

namespace OrderApi.Application.Queries;

public record GetOrderByIdQuery(Guid OrderId) : IQuery<Order?>;