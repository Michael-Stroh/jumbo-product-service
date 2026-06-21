namespace Jumbo.ProductService.Domain.Configs;

public sealed class DatabaseConfig
{
    public const string SectionName = "Database";

    public string ConnectionString { get; init; } = string.Empty;
}
