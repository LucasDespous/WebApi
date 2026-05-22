namespace Ecommerce.Application.DTOs;

public record RegisterRequestDto(string FirstName, string LastName, string Email, string Password);
public record LoginRequestDto(string Email, string Password);
public record AuthResponseDto(string Token, UserResponseDto User);
