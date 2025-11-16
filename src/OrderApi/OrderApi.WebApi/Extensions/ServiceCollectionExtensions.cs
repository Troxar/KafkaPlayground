using OrderApi.Application.Handlers;
using OrderApi.Application.Interfaces;

namespace OrderApi.WebApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<ICreateOrderHandler, CreateOrderHandler>();
        return services;
    }
}