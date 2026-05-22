using Ecommerce.Application.DTOs;
using Ecommerce.Application.UseCases;

namespace Ecommerce.Application.Interfaces;

public interface ICategoryUseCase
{
    Task<IReadOnlyCollection<CategoryResponseDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<CategoryResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<CategoryResponseDto> CreateAsync(CategoryRequestDto request, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(Guid id, CategoryRequestDto request, CancellationToken cancellationToken);
    Task<DeleteCategoryResult> DeleteAsync(Guid id, CancellationToken cancellationToken);
}
