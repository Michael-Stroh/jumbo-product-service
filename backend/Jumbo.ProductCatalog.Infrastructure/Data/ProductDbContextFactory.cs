using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Jumbo.ProductCatalog.Infrastructure.Data;

/// <summary>
/// Enables EF Core CLI tools to build the context without a running application.
/// Run migrations from the solution root:
///   dotnet ef migrations add &lt;Name&gt; --project backend/Jumbo.ProductCatalog.Infrastructure --startup-project backend/Jumbo.ProductCatalog.Api
/// </summary>
internal sealed class ProductDbContextFactory : IDesignTimeDbContextFactory<ProductDbContext>
{
    public ProductDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING")
            ?? "Server=localhost,1433;Database=JumboProductCatalog;User Id=sa;Password=YourSAPassword!;TrustServerCertificate=True";

        var options = new DbContextOptionsBuilder<ProductDbContext>()
            .UseSqlServer(connectionString)
            .Options;

        return new ProductDbContext(options);
    }
}
