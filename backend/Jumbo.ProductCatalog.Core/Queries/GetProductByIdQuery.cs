using Jumbo.ProductCatalog.Core.DTOs;
using MediatR;

namespace Jumbo.ProductCatalog.Core.Queries;

public record GetProductByIdQuery(Guid Id) : IRequest<ProductDto?>;
