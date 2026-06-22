using Jumbo.ProductCatalog.Core.DTOs;
using MediatR;

namespace Jumbo.ProductCatalog.Core.Queries;

public record GetAllProductsQuery : IRequest<IReadOnlyList<ProductDto>>;
