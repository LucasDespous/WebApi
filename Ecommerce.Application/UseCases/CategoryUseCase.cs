using Ecommerce.Application.DTOs;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.UseCases;

public class CategoryUseCase : ICategoryUseCase
{
    private readonly ICategoryRepository _categories;

    public CategoryUseCase(ICategoryRepository categories)
    {
        _categories = categories;
    }

    public async Task<IReadOnlyCollection<CategoryResponseDto>> GetAllAsync(CancellationToken cancellationToken)
        => (await _categories.GetAllAsync(cancellationToken)).Select(ToResponse).ToList();

    public async Task<CategoryResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var category = await _categories.GetByIdAsync(id, cancellationToken);
        return category is null ? null : ToResponse(category);
    }

    public async Task<CategoryResponseDto> CreateAsync(CategoryRequestDto request, CancellationToken cancellationToken)
    {
        var category = new Category { Id = Guid.NewGuid(), Name = request.Name, Description = request.Description };
        await _categories.AddAsync(category, cancellationToken);
        return ToResponse(category);
    }

    public async Task<bool> UpdateAsync(Guid id, CategoryRequestDto request, CancellationToken cancellationToken)
    {
        var category = await _categories.GetByIdAsync(id, cancellationToken);
        if (category is null) return false;
        category.Name = request.Name;
        category.Description = request.Description;
        await _categories.UpdateAsync(category, cancellationToken);
        return true;
    }

    public async Task<DeleteCategoryResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var category = await _categories.GetByIdAsync(id, cancellationToken);
        if (category is null) return DeleteCategoryResult.NotFound;
        if (await _categories.HasProductsAsync(id, cancellationToken)) return DeleteCategoryResult.HasProducts;

        await _categories.DeleteAsync(category, cancellationToken);
        return DeleteCategoryResult.Deleted;
    }

    private static CategoryResponseDto ToResponse(Category category)
        => new(category.Id, category.Name, category.Description);
}
