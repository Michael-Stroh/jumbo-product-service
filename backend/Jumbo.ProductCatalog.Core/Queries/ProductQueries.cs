using Jumbo.ProductCatalog.Core.DTOs;
using MediatR;

namespace Jumbo.ProductCatalog.Core.Queries;

public record GetAllProductsQuery : IRequest<IReadOnlyList<ProductDto>>;
public record GetActiveProductsQuery : IRequest<IReadOnlyList<ProductDto>>;
public record GetProductByIdQuery(Guid Id) : IRequest<ProductDto?>;
