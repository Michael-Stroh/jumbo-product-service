using Jumbo.ProductCatalog.Core.Commands;
using Jumbo.ProductCatalog.Core.Interfaces;
using Jumbo.ProductCatalog.Domain.Common;
using MediatR;

namespace Jumbo.ProductCatalog.Core.Handlers;

public sealed class DeleteProductCommandHandler(IProductCatalogService service)
    : IRequestHandler<DeleteProductCommand, Result>
{
    public Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken) =>
        service.DeleteAsync(request.Id, cancellationToken);
}
