using Ecommerce.Application.DTOs;

namespace Ecommerce.Application.Interfaces;

public interface IAuthUseCase
{
    Task<AuthResponseDto?> RegisterAsync(RegisterRequestDto request, CancellationToken cancellationToken);
    Task<AuthResponseDto?> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken);
}
