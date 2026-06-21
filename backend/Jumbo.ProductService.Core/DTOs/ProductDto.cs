using Jumbo.ProductService.Domain.Enums;

namespace Jumbo.ProductService.Core.DTOs;

public sealed record ProductDto(
    int Id,
    string Code,
    string Name,
    Category Category,
    string? Content,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
