using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Interfaces;

public interface IProductRepository
{
    Task<IReadOnlyCollection<Product>> GetAllAsync(CancellationToken cancellationToken);
    Task<IReadOnlyCollection<Product>> SearchByNameAsync(string name, CancellationToken cancellationToken);
    Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Product?> GetByIdForUpdateAsync(Guid id, CancellationToken cancellationToken);
    Task AddAsync(Product product, CancellationToken cancellationToken);
    Task UpdateAsync(Product product, CancellationToken cancellationToken);
    Task DeleteAsync(Product product, CancellationToken cancellationToken);
}
