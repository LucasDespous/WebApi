namespace Ecommerce.Application.DTOs;

public record ProductRequestDto(string Name, string Description, decimal Price, int Stock, Guid CategoryId);
public record ProductResponseDto(Guid Id, string Name, string Description, decimal Price, int Stock, Guid CategoryId, string CategoryName);
