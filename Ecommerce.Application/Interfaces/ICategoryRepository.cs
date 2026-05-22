using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Interfaces;

public interface ICategoryRepository
{
    Task<IReadOnlyCollection<Category>> GetAllAsync(CancellationToken cancellationToken);
    Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> HasProductsAsync(Guid id, CancellationToken cancellationToken);
    Task AddAsync(Category category, CancellationToken cancellationToken);
    Task UpdateAsync(Category category, CancellationToken cancellationToken);
    Task DeleteAsync(Category category, CancellationToken cancellationToken);
}
