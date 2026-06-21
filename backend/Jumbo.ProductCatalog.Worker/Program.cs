using Jumbo.ProductCatalog.Core.Interfaces;
using Jumbo.ProductCatalog.Domain.Configs;
using Jumbo.ProductCatalog.Infrastructure.Data;
using Jumbo.ProductCatalog.Infrastructure.Data.Interceptors;
using Jumbo.ProductCatalog.Infrastructure.Repositories;
using Jumbo.ProductCatalog.Worker;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

/*
    ==========================================
    Services
    ==========================================
*/

var connectionString = builder.Configuration.GetSection(DatabaseConfig.SectionName)[nameof(DatabaseConfig.ConnectionString)]
    ?? throw new InvalidOperationException($"Missing required configuration '{DatabaseConfig.SectionName}:{nameof(DatabaseConfig.ConnectionString)}'.");

builder.Services.AddSingleton<UpdateTimestampsInterceptor>();

builder.Services.AddDbContext<ProductDbContext>((sp, options) =>
    options.UseSqlServer(connectionString)
           .AddInterceptors(sp.GetRequiredService<UpdateTimestampsInterceptor>()));

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductCatalogService, Jumbo.ProductCatalog.Core.Services.ProductCatalogService>();

builder.Services.AddHostedService<Worker>();

/*
    ==========================================
    Run
    ==========================================
*/

var host = builder.Build();
host.Run();
