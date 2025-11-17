using Microsoft.AspNetCore.Mvc;
using OrderApi.Application.Commands;
using OrderApi.Application.Interfaces.Commands;
using OrderApi.Application.Interfaces.Queries;
using OrderApi.Application.Queries;
using OrderApi.Domain.Entities;

namespace OrderApi.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult> CreateOrder([FromServices] ICommandHandler<CreateOrderCommand, Guid> handler,
        [FromBody] CreateOrderCommand command, CancellationToken ct)
    {
        var id = await handler.HandleAsync(command, ct);
        return CreatedAtAction(nameof(GetOrderById), new { id }, new { id });
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType<Order>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetOrderById([FromServices] IQueryHandler<GetOrderByIdQuery, Order?> handler,
        Guid id,
        CancellationToken ct)
    {
        var query = new GetOrderByIdQuery(id);
        var order = await handler.HandleAsync(query, ct);

        return order is null
            ? NotFound()
            : Ok(order);
    }
}