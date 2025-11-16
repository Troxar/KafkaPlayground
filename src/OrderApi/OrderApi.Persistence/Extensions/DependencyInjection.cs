using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderApi.Application.Interfaces;
using OrderApi.Persistence.DbContext;
using OrderApi.Persistence.Repositories;

namespace OrderApi.Persistence.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddDbContext<OrderDbContext>((sp, options) =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            var connectionString = config.GetConnectionString(nameof(OrderDbContext));
            options.UseNpgsql(connectionString);
        });
        services.AddScoped<IOrderRepository, OrderRepository>();
        return services;
    }
}