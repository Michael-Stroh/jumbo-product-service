using Jumbo.ProductCatalog.Core.DTOs;
using Jumbo.ProductCatalog.Core.Interfaces;
using Jumbo.ProductCatalog.Core.Queries;
using MediatR;

namespace Jumbo.ProductCatalog.Core.Handlers;

public sealed class GetAllProductsQueryHandler(IProductCatalogService service)
    : IRequestHandler<GetAllProductsQuery, IReadOnlyList<ProductDto>>
{
    public Task<IReadOnlyList<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken) =>
        service.GetAllAsync(cancellationToken);
}
