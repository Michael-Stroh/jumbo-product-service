using Jumbo.ProductCatalog.Core.DTOs;
using Jumbo.ProductCatalog.Domain.Common;
using MediatR;

namespace Jumbo.ProductCatalog.Core.Commands;

public record ImportProductsCommand(IReadOnlyList<CreateProductRequest> Items) : IRequest<ImportResult>;
