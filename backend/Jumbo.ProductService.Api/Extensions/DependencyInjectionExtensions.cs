using Jumbo.ProductService.Core.Interfaces;
using Jumbo.ProductService.Domain.Configs;
using Jumbo.ProductService.Infrastructure.Data;
using Jumbo.ProductService.Infrastructure.Data.Interceptors;
using Jumbo.ProductService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Jumbo.ProductService.Api.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IProductService, Core.Services.ProductService>();
        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetSection(DatabaseConfig.SectionName)[nameof(DatabaseConfig.ConnectionString)]
            ?? throw new InvalidOperationException($"Missing required configuration '{DatabaseConfig.SectionName}:{nameof(DatabaseConfig.ConnectionString)}'.");

        services.AddSingleton<UpdateTimestampsInterceptor>();

        services.AddDbContext<ProductDbContext>((sp, options) =>
            options.UseSqlServer(connectionString)
                   .AddInterceptors(sp.GetRequiredService<UpdateTimestampsInterceptor>()));

        services.AddScoped<IProductRepository, ProductRepository>();

        return services;
    }
}
