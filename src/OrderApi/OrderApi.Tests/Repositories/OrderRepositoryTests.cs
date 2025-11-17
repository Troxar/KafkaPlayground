using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using OrderApi.Domain.Entities;
using OrderApi.Persistence.DbContext;
using OrderApi.Persistence.Repositories;

namespace OrderApi.Tests.Repositories;

public class OrderRepositoryTests
{
    [Fact]
    public async Task Add_ShouldAddOrderSuccessfully()
    {
        // Arrange
        await using var context = GetInMemoryDbContext();
        var repository = new OrderRepository(context);
        var order = new Order(Guid.NewGuid(), []);

        // Act
        await repository.AddAsync(order, CancellationToken.None);
        await context.SaveChangesAsync();

        // Assert
        var saved = await repository.GetByIdAsync(order.Id, CancellationToken.None);
        saved.Should().NotBeNull();
        saved.Id.Should().Be(order.Id);
    }

    [Fact]
    public async Task GetById_ShouldReturnNull_WhenOrderDoesNotExist()
    {
        // Arrange
        await using var context = GetInMemoryDbContext();
        var repository = new OrderRepository(context);

        // Act
        var result = await repository.GetByIdAsync(Guid.NewGuid(), CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    private static OrderDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<OrderDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new OrderDbContext(options);
    }
}