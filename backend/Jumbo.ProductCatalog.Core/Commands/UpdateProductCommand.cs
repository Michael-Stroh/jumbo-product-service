using Jumbo.ProductCatalog.Core.DTOs;
using Jumbo.ProductCatalog.Domain.Common;
using MediatR;

namespace Jumbo.ProductCatalog.Core.Commands;

public record UpdateProductCommand(Guid Id, UpdateProductRequest Request) : IRequest<Result<ProductDto>>;
