using Jumbo.ProductCatalog.Core.DTOs;
using Jumbo.ProductCatalog.Core.Interfaces;
using Jumbo.ProductCatalog.Domain.Common;
using Jumbo.ProductCatalog.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Jumbo.ProductCatalog.Core.Services;

public sealed partial class ProductCatalogService(IProductRepository repository, ILogger<ProductCatalogService> logger) : IProductCatalogService
{
    public async Task<ProductDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var product = await repository.GetByIdAsync(id, ct);
        return product is null ? null : ToDto(product);
    }

    public async Task<IReadOnlyList<ProductDto>> GetAllAsync(CancellationToken ct = default)
    {
        var products = await repository.GetAllAsync(ct);
        return products.Select(ToDto).ToList();
    }

    public async Task<IReadOnlyList<ProductDto>> GetActiveAsync(CancellationToken ct = default)
    {
        var products = await repository.GetActiveAsync(ct);
        return products.Select(ToDto).ToList();
    }

    public async Task<Result<ProductDto>> CreateAsync(CreateProductRequest request, CancellationToken ct = default)
    {
        var existing = await repository.GetByCodeIncludingArchivedAsync(request.Code, ct);

        if (existing is { IsArchived: false })
        {
            return Result.Failure<ProductDto>($"A product with code '{request.Code}' already exists.");
        }

        if (existing is not null)
        {
            // Reactivate the archived record with the new data rather than inserting a duplicate row.
            existing.IsArchived = false;
            existing.Name = request.Name;
            existing.Category = request.Category;
            existing.Content = request.Content;
            existing.IsActive = request.IsActive;
            await repository.UpdateAsync(existing, ct);
            LogProductReactivated(existing.Code);
            return Result.Success(ToDto(existing));
        }

        var product = new Product
        {
            Code = request.Code,
            Name = request.Name,
            Category = request.Category,
            Content = request.Content,
            IsActive = request.IsActive,
        };

        await repository.AddAsync(product, ct);
        LogProductCreated(product.Code);
        return Result.Success(ToDto(product));
    }

    public async Task<Result<ProductDto>> UpdateAsync(Guid id, UpdateProductRequest request, CancellationToken ct = default)
    {
        var product = await repository.GetByIdAsync(id, ct);
        if (product is null)
        {
            return Result.Failure<ProductDto>($"Product with id {id} was not found.");
        }

        product.Name = request.Name;
        product.Category = request.Category;
        product.Content = request.Content;
        product.IsActive = request.IsActive;

        await repository.UpdateAsync(product, ct);
        return Result.Success(ToDto(product));
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var product = await repository.GetByIdAsync(id, ct);
        if (product is null)
        {
            return Result.Failure($"Product with id {id} was not found.");
        }

        await repository.DeleteAsync(product, ct);
        LogProductArchived(product.Code);
        return Result.Success();
    }

    private static ProductDto ToDto(Product p) =>
        new(p.Id, p.Code, p.Name, p.Category, p.Content, p.IsActive, p.CreatedAt, p.UpdatedAt);

    [LoggerMessage(LogLevel.Information, "Product created: {Code}")]
    private partial void LogProductCreated(string code);

    [LoggerMessage(LogLevel.Information, "Product archived: {Code}")]
    private partial void LogProductArchived(string code);

    [LoggerMessage(LogLevel.Information, "Product reactivated: {Code}")]
    private partial void LogProductReactivated(string code);
}
