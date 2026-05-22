using Ecommerce.Application.DTOs;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.UseCases;

public class AuthUseCase : IAuthUseCase
{
    private readonly IUserRepository _users;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthUseCase(IUserRepository users, IPasswordHasher passwordHasher, IJwtTokenService jwtTokenService)
    {
        _users = users;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<AuthResponseDto?> RegisterAsync(RegisterRequestDto request, CancellationToken cancellationToken)
    {
        if (await _users.ExistsByEmailAsync(request.Email, cancellationToken))
        {
            return null;
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email.Trim().ToLowerInvariant(),
            PasswordHash = _passwordHasher.Hash(request.Password),
            Role = UserRole.User
        };

        await _users.AddAsync(user, cancellationToken);
        var token = _jwtTokenService.GenerateToken(user);
        return new AuthResponseDto(token, ToResponse(user));
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken)
    {
        var user = await _users.GetByEmailAsync(request.Email.Trim().ToLowerInvariant(), cancellationToken);
        if (user is null || !_passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            return null;
        }

        return new AuthResponseDto(_jwtTokenService.GenerateToken(user), ToResponse(user));
    }

    private static UserResponseDto ToResponse(User user)
        => new(user.Id, user.FirstName, user.LastName, user.Email, user.Role.ToString());
}
