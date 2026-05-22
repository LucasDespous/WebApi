using Ecommerce.Application.DTOs;
using Ecommerce.Application.Interfaces;
using Ecommerce.Application.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryUseCase _categoryUseCase;

    public CategoriesController(ICategoryUseCase categoryUseCase)
    {
        _categoryUseCase = categoryUseCase;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        => Ok(await _categoryUseCase.GetAllAsync(cancellationToken));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var response = await _categoryUseCase.GetByIdAsync(id, cancellationToken);
        return response is null ? NotFound() : Ok(response);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CategoryRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _categoryUseCase.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, CategoryRequestDto request, CancellationToken cancellationToken)
        => await _categoryUseCase.UpdateAsync(id, request, cancellationToken) ? NoContent() : NotFound();

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _categoryUseCase.DeleteAsync(id, cancellationToken);
        return result switch
        {
            DeleteCategoryResult.Deleted => NoContent(),
            DeleteCategoryResult.NotFound => NotFound(),
            DeleteCategoryResult.HasProducts => BadRequest("No se puede eliminar una categoria con productos asociados. Elimine o reasigne los productos primero."),
            _ => BadRequest()
        };
    }
}
