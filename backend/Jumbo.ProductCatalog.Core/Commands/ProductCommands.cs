using Jumbo.ProductCatalog.Core.DTOs;
using Jumbo.ProductCatalog.Domain.Common;
using MediatR;

namespace Jumbo.ProductCatalog.Core.Commands;

public record CreateProductCommand(CreateProductRequest Request) : IRequest<Result<ProductDto>>;
public record UpdateProductCommand(Guid Id, UpdateProductRequest Request) : IRequest<Result<ProductDto>>;
public record DeleteProductCommand(Guid Id) : IRequest<Result>;
public record ImportProductsCommand(IReadOnlyList<CreateProductRequest> Items) : IRequest<ImportResult>;
