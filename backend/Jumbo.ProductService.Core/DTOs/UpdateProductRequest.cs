using System.ComponentModel.DataAnnotations;
using Jumbo.ProductService.Domain.Enums;

namespace Jumbo.ProductService.Core.DTOs;

public sealed record UpdateProductRequest(
    [Required][MaxLength(50)] string Name,
    [Required] Category Category,
    string? Content,
    bool IsActive
);
