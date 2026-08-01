using Br.OrderManagement.Application.DTOs.Order;
using Br.OrderManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Br.OrderManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController(IOrderService orderService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await orderService.GetAllAsync());
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var order = await orderService.GetByIdAsync(id);

        if (order == null)
            return NotFound();

        return Ok(order);
    }

    [HttpPost]
    public async Task<IActionResult> Post(CreateOrderDto dto)
    {
        var id = await orderService.CreateAsync(dto);

        return CreatedAtAction(nameof(Get), new { id }, id);
    }

    [HttpPut("{id:guid}/confirm")]
    public async Task<IActionResult> Confirm(Guid id)
    {
        await orderService.ConfirmAsync(id);

        return NoContent();
    }

    [HttpPut("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid id)
    {
        await orderService.CancelAsync(id);

        return NoContent();
    }

    [HttpPut("{id:guid}/finish")]
    public async Task<IActionResult> Finish(Guid id)
    {
        await orderService.FinishAsync(id);

        return NoContent();
    }
}