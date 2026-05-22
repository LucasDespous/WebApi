using Ecommerce.Application.DTOs;
using Ecommerce.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthUseCase _authUseCase;

    public AuthController(IAuthUseCase authUseCase)
    {
        _authUseCase = authUseCase;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _authUseCase.RegisterAsync(request, cancellationToken);
        return response is null ? BadRequest("El email ya se encuentra registrado.") : Created("api/users/me", response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _authUseCase.LoginAsync(request, cancellationToken);
        return response is null ? Unauthorized() : Ok(response);
    }
}
