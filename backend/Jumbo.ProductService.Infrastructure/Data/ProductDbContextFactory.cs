using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Jumbo.ProductService.Infrastructure.Data;

/// <summary>
/// Enables EF Core CLI tools to build the context without a running application.
/// Run migrations from the solution root:
///   dotnet ef migrations add &lt;Name&gt; --project backend/Jumbo.ProductService.Infrastructure --startup-project backend/Jumbo.ProductService.Api
/// </summary>
internal sealed class ProductDbContextFactory : IDesignTimeDbContextFactory<ProductDbContext>
{
    public ProductDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING")
            ?? "Server=localhost,1433;Database=JumboProductService;User Id=sa;Password=YourSAPassword!;TrustServerCertificate=True";

        var options = new DbContextOptionsBuilder<ProductDbContext>()
            .UseSqlServer(connectionString)
            .Options;

        return new ProductDbContext(options);
    }
}
