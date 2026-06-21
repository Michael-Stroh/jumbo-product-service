using Jumbo.ProductCatalog.Core.DTOs;
using Jumbo.ProductCatalog.Core.Interfaces;
using Jumbo.ProductCatalog.Core.Queries;
using MediatR;

namespace Jumbo.ProductCatalog.Core.Handlers;

public sealed class GetActiveProductsQueryHandler(IProductCatalogService service)
    : IRequestHandler<GetActiveProductsQuery, IReadOnlyList<ProductDto>>
{
    public Task<IReadOnlyList<ProductDto>> Handle(GetActiveProductsQuery request, CancellationToken cancellationToken) =>
        service.GetActiveAsync(cancellationToken);
}
