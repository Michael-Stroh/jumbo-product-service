using Jumbo.ProductService.Core.Interfaces;
using Jumbo.ProductService.Domain.Configs;
using Jumbo.ProductService.Infrastructure.Data;
using Jumbo.ProductService.Infrastructure.Data.Interceptors;
using Jumbo.ProductService.Infrastructure.Repositories;
using Jumbo.ProductService.Worker;
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
builder.Services.AddScoped<IProductService, Jumbo.ProductService.Core.Services.ProductService>();

builder.Services.AddHostedService<Worker>();

/*
    ==========================================
    Run
    ==========================================
*/

var host = builder.Build();
host.Run();
