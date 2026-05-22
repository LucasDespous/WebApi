using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Interfaces;

public interface IJwtTokenService
{
    string GenerateToken(User user);
}
