using Ecommerce.Application.DTOs;
using Ecommerce.Application.Interfaces;

namespace Ecommerce.Application.UseCases;

public class UserUseCase : IUserUseCase
{
    private readonly IUserRepository _users;

    public UserUseCase(IUserRepository users)
    {
        _users = users;
    }

    public async Task<UserResponseDto?> GetProfileAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _users.GetByIdAsync(userId, cancellationToken);
        return user is null ? null : new UserResponseDto(user.Id, user.FirstName, user.LastName, user.Email, user.Role.ToString());
    }
}
