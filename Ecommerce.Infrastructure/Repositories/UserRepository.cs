using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly EcommerceDbContext _context;

    public UserRepository(EcommerceDbContext context)
    {
        _context = context;
    }

    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        => _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

    public Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken)
        => _context.Users.AsNoTracking().AnyAsync(x => x.Email == email.Trim().ToLowerInvariant(), cancellationToken);

    public async Task AddAsync(User user, CancellationToken cancellationToken)
    {
        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
