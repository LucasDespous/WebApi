using Ecommerce.Application.DTOs;
using Ecommerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductUseCase _productUseCase;

    public ProductsController(IProductUseCase productUseCase)
    {
        _productUseCase = productUseCase;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        => Ok(await _productUseCase.GetAllAsync(cancellationToken));

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string name, CancellationToken cancellationToken)
        => Ok(await _productUseCase.SearchAsync(name, cancellationToken));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var response = await _productUseCase.GetByIdAsync(id, cancellationToken);
        return response is null ? NotFound() : Ok(response);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(ProductRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _productUseCase.CreateAsync(request, cancellationToken);
        return response is null ? NotFound("La categoria indicada no existe.") : CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, ProductRequestDto request, CancellationToken cancellationToken)
        => await _productUseCase.UpdateAsync(id, request, cancellationToken) ? NoContent() : NotFound();

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        => await _productUseCase.DeleteAsync(id, cancellationToken) ? NoContent() : NotFound();
}
