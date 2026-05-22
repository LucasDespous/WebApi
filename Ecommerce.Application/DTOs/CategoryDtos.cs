namespace Ecommerce.Application.DTOs;

public record CategoryRequestDto(string Name, string Description);
public record CategoryResponseDto(Guid Id, string Name, string Description);
