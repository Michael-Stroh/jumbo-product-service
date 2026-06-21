namespace Jumbo.ProductService.Domain.Configs;

public sealed class CorsConfig
{
    public const string SectionName = "Cors";

    public string[] AllowedOrigins { get; init; } = [];
}
