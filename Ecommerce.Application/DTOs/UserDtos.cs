namespace Ecommerce.Application.DTOs;

public record UserResponseDto(Guid Id, string FirstName, string LastName, string Email, string Role);
