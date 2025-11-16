using Microsoft.AspNetCore.Mvc;
using OrderApi.Application.Commands;
using OrderApi.Application.Interfaces;

namespace OrderApi.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly ICreateOrderHandler _createOrderHandler;

    public OrdersController(ICreateOrderHandler createOrderHandler)
    {
        _createOrderHandler = createOrderHandler;
    }

    [HttpPost]
    public async Task<ActionResult> CreateOrder([FromBody] CreateOrderCommand command, CancellationToken ct)
    {
        var id = await _createOrderHandler.CreateOrderAsync(command, ct);
        return CreatedAtAction(nameof(GetOrderById), new { id }, new { id });
    }

    [HttpGet("{id:guid}")]
    public ActionResult<Guid> GetOrderById(Guid id, CancellationToken ct)
    {
        // TODO: get order by id
        return Ok(id);
    }
}