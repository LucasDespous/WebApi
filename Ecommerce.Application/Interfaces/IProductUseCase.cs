using Ecommerce.Application.DTOs;

namespace Ecommerce.Application.Interfaces;

public interface IProductUseCase
{
    Task<IReadOnlyCollection<ProductResponseDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<IReadOnlyCollection<ProductResponseDto>> SearchAsync(string name, CancellationToken cancellationToken);
    Task<ProductResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<ProductResponseDto?> CreateAsync(ProductRequestDto request, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(Guid id, ProductRequestDto request, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
}
