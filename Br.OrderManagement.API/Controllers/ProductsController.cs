using Br.OrderManagement.Application.DTOs.Product;
using Br.OrderManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Br.OrderManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductService productService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await productService.GetAllAsync());
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var product = await productService.GetByIdAsync(id);

        if (product == null)
            return NotFound();

        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Post(CreateProductDto dto)
    {
        var id = await productService.CreateAsync(dto);

        return CreatedAtAction(nameof(Get), new { id }, id);
    }

    [HttpPut]
    public async Task<IActionResult> Put(UpdateProductDto dto)
    {
        await productService.UpdateAsync(dto);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await productService.DeleteAsync(id);

        return NoContent();
    }
}