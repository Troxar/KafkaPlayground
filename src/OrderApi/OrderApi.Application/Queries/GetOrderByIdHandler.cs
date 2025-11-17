using OrderApi.Application.Interfaces;
using OrderApi.Application.Interfaces.Queries;
using OrderApi.Domain.Entities;

namespace OrderApi.Application.Queries;

public class GetOrderByIdHandler : IQueryHandler<GetOrderByIdQuery, Order?>
{
    private readonly IOrderRepository _repository;

    public GetOrderByIdHandler(IOrderRepository repository)
    {
        _repository = repository;
    }

    public Task<Order?> HandleAsync(GetOrderByIdQuery query, CancellationToken ct)
    {
        return _repository.GetByIdAsync(query.OrderId, ct);
    }
}