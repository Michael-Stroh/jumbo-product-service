using Jumbo.ProductCatalog.UI.Configs;

namespace Jumbo.ProductCatalog.UI.Configs;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddProjectOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ApiOptions>(configuration.GetSection(ApiOptions.SectionName));

        return services;
    }
}
