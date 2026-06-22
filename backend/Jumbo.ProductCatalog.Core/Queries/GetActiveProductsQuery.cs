using Jumbo.ProductCatalog.Core.DTOs;
using MediatR;

namespace Jumbo.ProductCatalog.Core.Queries;

public record GetActiveProductsQuery : IRequest<IReadOnlyList<ProductDto>>;
