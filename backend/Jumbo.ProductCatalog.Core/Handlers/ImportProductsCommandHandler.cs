using Jumbo.ProductCatalog.Core.Commands;
using Jumbo.ProductCatalog.Core.DTOs;
using Jumbo.ProductCatalog.Core.Interfaces;
using MediatR;

namespace Jumbo.ProductCatalog.Core.Handlers;

public sealed class ImportProductsCommandHandler(IProductCatalogService service)
    : IRequestHandler<ImportProductsCommand, ImportResult>
{
    public Task<ImportResult> Handle(ImportProductsCommand request, CancellationToken cancellationToken) =>
        service.ImportAsync(request.Items, cancellationToken);
}
