using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OrderApi.Application.Commands;
using OrderApi.Application.Dtos;
using OrderApi.Application.Interfaces.Commands;
using OrderApi.Application.Interfaces.Queries;
using OrderApi.Application.Queries;
using OrderApi.Domain.Entities;
using OrderApi.WebApi.Controllers;

namespace OrderApi.Tests.Controllers;

public class OrdersControllerTests
{
    [Fact]
    public async Task CreateOrder_ShouldReturnCreateAtAction_WithOrderId()
    {
        // Arrange
        var command = new CreateOrderCommand(Guid.NewGuid(), [new OrderItemDto(Guid.NewGuid(), 1, 1)]);
        var createdId = Guid.NewGuid();

        var handlerMock = new Mock<ICommandHandler<CreateOrderCommand, Guid>>();
        handlerMock.Setup(x => x.HandleAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdId);

        var controller = new OrdersController();

        // Act
        var result = await controller.CreateOrder(handlerMock.Object, command, CancellationToken.None);

        // Assert
        var created = result as CreatedAtActionResult;
        created.Should().NotBeNull();
        created.ActionName.Should().Be(nameof(OrdersController.GetOrderById));
        created.RouteValues!["id"].Should().Be(createdId);
        created.Value.Should().BeEquivalentTo(new { id = createdId });

        handlerMock.Verify(h => h.HandleAsync(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetOrderById_ShouldReturnOk_WhenOrderExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var order = new Order(id, []);

        var handlerMock = new Mock<IQueryHandler<GetOrderByIdQuery, Order?>>();
        handlerMock.Setup(h =>
                h.HandleAsync(It.Is<GetOrderByIdQuery>(x => x.OrderId == id), It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);

        var controller = new OrdersController();

        // Act
        var result = await controller.GetOrderById(handlerMock.Object, id, CancellationToken.None);

        // Assert
        var ok = result as OkObjectResult;
        ok.Should().NotBeNull();
        ok.Value.Should().BeEquivalentTo(order);
    }

    [Fact]
    public async Task GetOrderById_ShouldReturnNotFound_WhenOrderDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();

        var handlerMock = new Mock<IQueryHandler<GetOrderByIdQuery, Order?>>();
        handlerMock.Setup(h =>
                h.HandleAsync(It.IsAny<GetOrderByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Order?)null);

        var controller = new OrdersController();

        // Act
        var result = await controller.GetOrderById(handlerMock.Object, id, CancellationToken.None);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }
}