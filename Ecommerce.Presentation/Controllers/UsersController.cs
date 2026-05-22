using System.Security.Claims;
using Ecommerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserUseCase _userUseCase;

    public UsersController(IUserUseCase userUseCase)
    {
        _userUseCase = userUseCase;
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetProfile(CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var response = await _userUseCase.GetProfileAsync(userId, cancellationToken);
        return response is null ? NotFound() : Ok(response);
    }
}
