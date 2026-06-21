using Jumbo.ProductService.Core.DTOs;
using Jumbo.ProductService.Domain.Common;

namespace Jumbo.ProductService.Core.Interfaces;

public interface IProductService
{
    Task<ProductDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<ProductDto>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<ProductDto>> GetActiveAsync(CancellationToken ct = default);
    Task<Result<ProductDto>> CreateAsync(CreateProductRequest request, CancellationToken ct = default);
    Task<Result<ProductDto>> UpdateAsync(Guid id, UpdateProductRequest request, CancellationToken ct = default);
    Task<Result> DeleteAsync(Guid id, CancellationToken ct = default);
}
