namespace Jumbo.ProductCatalog.UI.Models;

// ponytail: minimal RFC 7807 subset — only the fields the UI displays
public sealed record ProblemDetailsDto(string? Title, string? Detail, int? Status);
