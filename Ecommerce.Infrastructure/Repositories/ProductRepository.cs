using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly EcommerceDbContext _context;

    public ProductRepository(EcommerceDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<Product>> GetAllAsync(CancellationToken cancellationToken)
        => await _context.Products.AsNoTracking().Include(x => x.Category).OrderBy(x => x.Name).ToListAsync(cancellationToken);

    public async Task<IReadOnlyCollection<Product>> SearchByNameAsync(string name, CancellationToken cancellationToken)
        => await _context.Products.AsNoTracking().Include(x => x.Category).Where(x => x.Name.Contains(name)).OrderBy(x => x.Name).ToListAsync(cancellationToken);

    public Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => _context.Products.AsNoTracking().Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<Product?> GetByIdForUpdateAsync(Guid id, CancellationToken cancellationToken)
        => _context.Products.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task AddAsync(Product product, CancellationToken cancellationToken)
    {
        await _context.Products.AddAsync(product, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Product product, CancellationToken cancellationToken)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Product product, CancellationToken cancellationToken)
    {
        _context.Products.Remove(product);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
