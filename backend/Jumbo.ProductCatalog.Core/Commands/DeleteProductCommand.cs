using Jumbo.ProductCatalog.Domain.Common;
using MediatR;

namespace Jumbo.ProductCatalog.Core.Commands;

public record DeleteProductCommand(Guid Id) : IRequest<Result>;
