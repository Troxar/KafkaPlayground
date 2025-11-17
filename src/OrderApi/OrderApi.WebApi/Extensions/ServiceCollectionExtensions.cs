using OrderApi.Application.Commands;
using OrderApi.Application.Interfaces.Commands;
using OrderApi.Application.Interfaces.Queries;
using OrderApi.Application.Queries;
using OrderApi.Domain.Entities;

namespace OrderApi.WebApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandler<CreateOrderCommand, Guid>, CreateOrderHandler>();
        services.AddScoped<IQueryHandler<GetOrderByIdQuery, Order?>, GetOrderByIdHandler>();
        return services;
    }
}