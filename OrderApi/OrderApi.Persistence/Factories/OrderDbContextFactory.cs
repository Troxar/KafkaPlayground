using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using OrderApi.Persistence.DbContext;

namespace OrderApi.Persistence.Factories;

public class OrderDbContextFactory : IDesignTimeDbContextFactory<OrderDbContext>
{
    public OrderDbContext CreateDbContext(string[] args)
    {
        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../OrderApi.WebApi");
        var config = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", false)
            .AddJsonFile("appsettings.Development.json", true)
            .AddEnvironmentVariables()
            .Build();

        const string connectionStringName = nameof(OrderDbContext);
        var connectionString = config.GetConnectionString(connectionStringName)
                               ?? throw new InvalidOperationException(
                                   $"Connection string {connectionStringName} not found.");

        var options = new DbContextOptionsBuilder<OrderDbContext>()
            .UseNpgsql(connectionString)
            .Options;

        return new OrderDbContext(options);
    }
}