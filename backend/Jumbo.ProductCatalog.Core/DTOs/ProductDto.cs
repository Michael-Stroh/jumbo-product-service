using Jumbo.ProductCatalog.Domain.Enums;

namespace Jumbo.ProductCatalog.Core.DTOs;

public sealed record ProductDto(
    Guid Id,
    string Code,
    string Name,
    Category Category,
    string? Content,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
