using Jumbo.ProductCatalog.Core.DTOs;
using Jumbo.ProductCatalog.Core.Interfaces;
using Jumbo.ProductCatalog.Core.Queries;
using MediatR;

namespace Jumbo.ProductCatalog.Core.Handlers;

public sealed class GetProductByIdQueryHandler(IProductCatalogService service)
    : IRequestHandler<GetProductByIdQuery, ProductDto?>
{
    public Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken) =>
        service.GetByIdAsync(request.Id, cancellationToken);
}
