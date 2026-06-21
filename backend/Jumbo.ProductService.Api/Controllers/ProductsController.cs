using Jumbo.ProductService.Core.DTOs;
using Jumbo.ProductService.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Jumbo.ProductService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ProductsController(IProductService productService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct) =>
        Ok(await productService.GetAllAsync(ct));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var product = await productService.GetByIdAsync(id, ct);
        return product is null ? NotFound() : Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductRequest request, CancellationToken ct)
    {
        var result = await productService.CreateAsync(request, ct);
        if (!result.IsSuccess)
        {
            return Problem(title: "Validation error", detail: result.Error, statusCode: StatusCodes.Status400BadRequest);
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProductRequest request, CancellationToken ct)
    {
        var result = await productService.UpdateAsync(id, request, ct);
        if (!result.IsSuccess)
        {
            return Problem(title: "Not found", detail: result.Error, statusCode: StatusCodes.Status404NotFound);
        }

        return Ok(result.Value);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var result = await productService.DeleteAsync(id, ct);
        if (!result.IsSuccess)
        {
            return Problem(title: "Not found", detail: result.Error, statusCode: StatusCodes.Status404NotFound);
        }

        return NoContent();
    }
}
