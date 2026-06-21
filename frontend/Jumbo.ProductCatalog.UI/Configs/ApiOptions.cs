namespace Jumbo.ProductCatalog.UI.Configs;

public sealed class ApiOptions
{
    public const string SectionName = "Api";

    public required string BaseUrl { get; init; }
}
