using Microsoft.EntityFrameworkCore;
using OrderApi.Persistence.DbContext;
using Testcontainers.PostgreSql;

namespace OrderApi.IntegrationTests.Infrastructure;

public class TestDatabase : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container;

    public string ConnectionString => _container.GetConnectionString();

    public TestDatabase()
    {
        _container = new PostgreSqlBuilder()
            .WithDatabase("orderapi")
            .WithUsername("test")
            .WithPassword("password")
            .WithImage("postgres:15")
            .Build();
    }

    public async Task InitializeAsync()
    {
        await _container.StartAsync();

        var options = CreateDbContextOptions();
        
        await using var context = new OrderDbContext(options);
        await context.Database.MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        await _container.StopAsync();
    }

    public OrderDbContext CreateDbContext()
    {
        var options = CreateDbContextOptions();
        return new OrderDbContext(options);
    }
    
    private DbContextOptions<OrderDbContext> CreateDbContextOptions()
    {
        var options = new DbContextOptionsBuilder<OrderDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;
        return options;
    }
}