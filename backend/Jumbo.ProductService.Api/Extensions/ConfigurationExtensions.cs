using Jumbo.ProductService.Domain.Configs;

namespace Jumbo.ProductService.Api.Extensions;

public static class ConfigurationExtensions
{
    /// <summary>
    /// Binds all strongly-typed options sections. Add new IOptions registrations here.
    /// </summary>
    public static IServiceCollection AddProjectOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CorsConfig>(configuration.GetSection(CorsConfig.SectionName));
        services.Configure<DatabaseConfig>(configuration.GetSection(DatabaseConfig.SectionName));

        return services;
    }
}
