using Jumbo.ProductCatalog.Core.Interfaces;
using Jumbo.ProductCatalog.Domain.Entities;
using Jumbo.ProductCatalog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Jumbo.ProductCatalog.Infrastructure.Repositories;

public sealed class ProductRepository(ProductDbContext context) : IProductRepository
{
    public Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        context.Products.FirstOrDefaultAsync(p => p.Id == id, ct);

    public Task<Product?> GetByCodeAsync(string code, CancellationToken ct = default) =>
        context.Products.FirstOrDefaultAsync(p => p.Code == code, ct);

    // Bypasses the global IsArchived filter — used to detect archived products for reactivation.
    public Task<Product?> GetByCodeIncludingArchivedAsync(string code, CancellationToken ct = default) =>
        context.Products.IgnoreQueryFilters().FirstOrDefaultAsync(p => p.Code == code, ct);

    public async Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken ct = default) =>
        await context.Products.ToListAsync(ct);

    public async Task<IReadOnlyList<Product>> GetActiveAsync(CancellationToken ct = default) =>
        await context.Products.Where(p => p.IsActive).ToListAsync(ct);

    public async Task AddAsync(Product product, CancellationToken ct = default)
    {
        await context.Products.AddAsync(product, ct);
        await context.SaveChangesAsync(ct);
    }

    public Task UpdateAsync(Product product, CancellationToken ct = default)
    {
        context.Update(product);
        return context.SaveChangesAsync(ct);
    }

    // soft delete — sets IsArchived=true; global query filter keeps archived rows invisible.
    public Task DeleteAsync(Product product, CancellationToken ct = default)
    {
        product.IsArchived = true;
        context.Update(product);
        return context.SaveChangesAsync(ct);
    }
}
