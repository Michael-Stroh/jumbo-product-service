using System.ComponentModel.DataAnnotations;
using Jumbo.ProductCatalog.Domain.Enums;

namespace Jumbo.ProductCatalog.Core.DTOs;

public sealed record UpdateProductRequest(
    [Required][MaxLength(50)] string Name,
    [Required] Category Category,
    string? Content,
    bool IsActive
);
