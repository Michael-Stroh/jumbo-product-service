using Jumbo.ProductService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Jumbo.ProductService.Infrastructure.Data;

public sealed class ProductDbContext(DbContextOptions<ProductDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            // Exclude archived rows from all queries automatically.
            entity.HasQueryFilter(p => !p.IsArchived);

            // Code is a globally unique business key
            entity.HasIndex(p => p.Code).IsUnique();
        });
    }
}
