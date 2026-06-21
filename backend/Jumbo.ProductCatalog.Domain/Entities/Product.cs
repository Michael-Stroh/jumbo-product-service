using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Jumbo.ProductCatalog.Domain.Common;
using Jumbo.ProductCatalog.Domain.Enums;

namespace Jumbo.ProductCatalog.Domain.Entities;

[Table("products")]
public sealed class Product : BaseEntity
{
    [Column("code")]
    [MaxLength(50)]
    public required string Code { get; set; }

    [Column("name")]
    [MaxLength(50)]
    public required string Name { get; set; }

    [Column("category")]
    public required Category Category { get; set; }

    [Column("content")]
    public string? Content { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; } = true;
}
