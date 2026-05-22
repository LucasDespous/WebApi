using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly EcommerceDbContext _context;

    public CategoryRepository(EcommerceDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<Category>> GetAllAsync(CancellationToken cancellationToken)
        => await _context.Categories.AsNoTracking().OrderBy(x => x.Name).ToListAsync(cancellationToken);

    public Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => _context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<bool> HasProductsAsync(Guid id, CancellationToken cancellationToken)
        => _context.Products.AsNoTracking().AnyAsync(x => x.CategoryId == id, cancellationToken);

    public async Task AddAsync(Category category, CancellationToken cancellationToken)
    {
        await _context.Categories.AddAsync(category, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Category category, CancellationToken cancellationToken)
    {
        _context.Categories.Update(category);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Category category, CancellationToken cancellationToken)
    {
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
