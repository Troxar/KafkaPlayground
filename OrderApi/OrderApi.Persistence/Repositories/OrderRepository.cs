using OrderApi.Application.Interfaces;
using OrderApi.Domain.Entities;

namespace OrderApi.Persistence.Repositories;

public class OrderRepository : IOrderRepository
{
    public Task AddAsync(Order order, CancellationToken ct)
    {
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken ct)
    {
        return Task.CompletedTask;
    }
}