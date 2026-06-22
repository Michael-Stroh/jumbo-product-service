using Jumbo.ProductCatalog.Core.DTOs;
using Jumbo.ProductCatalog.Domain.Common;
using MediatR;

namespace Jumbo.ProductCatalog.Core.Commands;

public record CreateProductCommand(CreateProductRequest Request) : IRequest<Result<ProductDto>>;
