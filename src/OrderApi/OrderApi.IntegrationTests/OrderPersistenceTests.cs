using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using OrderApi.Domain.Entities;
using OrderApi.Domain.ValueObjects;
using OrderApi.IntegrationTests.Infrastructure;

namespace OrderApi.IntegrationTests;

public class OrderPersistenceTests : IClassFixture<TestDatabase>
{
    private readonly TestDatabase _database;

    public OrderPersistenceTests(TestDatabase database)
    {
        _database = database;
    }

    [Fact]
    public async Task ShouldSaveAndLoadOrder_WithItems()
    {
        // Arrange
        var order = new Order(Guid.NewGuid(),
        [
            CreateOrderItem(),
            CreateOrderItem()
        ]);

        await using (var context = _database.CreateDbContext())
        {
            context.Orders.Add(order);
            await context.SaveChangesAsync();
        }

        // Act
        Order? loaded;
        await using (var context = _database.CreateDbContext())
        {
            loaded = await context.Orders
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.Id == order.Id);
        }

        // Assert
        loaded.Should().NotBeNull();
        loaded.Items.Should().HaveCount(order.Items.Count);
        loaded.Items.Select(x => new { x.ProductId, x.Quantity, x.UnitPrice })
            .Should().BeEquivalentTo(order.Items.Select(x => new { x.ProductId, x.Quantity, x.UnitPrice }));
    }

    private static OrderItem CreateOrderItem()
    {
        var random = new Random();
        return new OrderItem(Guid.NewGuid(), random.Next(100), random.Next(1000));
    }
}