using System.ComponentModel.DataAnnotations;
using Jumbo.ProductService.Domain.Enums;

namespace Jumbo.ProductService.Core.DTOs;

public sealed record CreateProductRequest(
    [Required][MaxLength(50)] string Code,
    [Required][MaxLength(50)] string Name,
    [Required] Category Category,
    string? Content,
    bool IsActive = true
);
