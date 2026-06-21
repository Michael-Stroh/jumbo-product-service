using Jumbo.ProductCatalog.Core.Commands;
using Jumbo.ProductCatalog.Core.DTOs;
using Jumbo.ProductCatalog.Core.Interfaces;
using Jumbo.ProductCatalog.Domain.Common;
using MediatR;

namespace Jumbo.ProductCatalog.Core.Handlers;

public sealed class UpdateProductCommandHandler(IProductCatalogService service)
    : IRequestHandler<UpdateProductCommand, Result<ProductDto>>
{
    public Task<Result<ProductDto>> Handle(UpdateProductCommand request, CancellationToken cancellationToken) =>
        service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
