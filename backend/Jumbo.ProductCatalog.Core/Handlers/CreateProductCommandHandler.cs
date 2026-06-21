using Jumbo.ProductCatalog.Core.Commands;
using Jumbo.ProductCatalog.Core.DTOs;
using Jumbo.ProductCatalog.Core.Interfaces;
using Jumbo.ProductCatalog.Domain.Common;
using MediatR;

namespace Jumbo.ProductCatalog.Core.Handlers;

public sealed class CreateProductCommandHandler(IProductCatalogService service)
    : IRequestHandler<CreateProductCommand, Result<ProductDto>>
{
    public Task<Result<ProductDto>> Handle(CreateProductCommand request, CancellationToken cancellationToken) =>
        service.CreateAsync(request.Request, cancellationToken);
}
