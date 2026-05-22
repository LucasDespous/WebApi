using Ecommerce.Application.DTOs;

namespace Ecommerce.Application.Interfaces;

public interface IUserUseCase
{
    Task<UserResponseDto?> GetProfileAsync(Guid userId, CancellationToken cancellationToken);
}
