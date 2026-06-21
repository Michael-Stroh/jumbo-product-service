# Jumbo Product Service

Assessment project for Jumbo solution engineer — a product catalogue service with a Blazor frontend, ASP.NET Core API, background worker, and layered backend architecture.

## Project structure

```text
├── backend/
│   ├── Jumbo.ProductCatalog.Api/            # ASP.NET Core Web API
│   ├── Jumbo.ProductCatalog.Worker/         # Background service host
│   ├── Jumbo.ProductCatalog.Core/           # Use cases and service interfaces
│   ├── Jumbo.ProductCatalog.Domain/         # Core entities and business rules
│   └── Jumbo.ProductCatalog.Infrastructure/ # Data access and external services
├── frontend/
│   └── Jumbo.ProductCatalog.UI/             # Blazor frontend
├── tests/
│   ├── Jumbo.ProductCatalog.Api.Tests/
│   └── Jumbo.ProductCatalog.Core.Tests/
└── docs/
    ├── architecture-decisions.md
```

## Prerequisites

- [.NET SDK 10.0.x](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (for SQL Server + Azurite)

## Getting started

### 1. Start infrastructure

```bash
cp .env.example .env          # fill in MSSQL_SA_PASSWORD
docker compose up -d
```

### 2. Set local app secrets

The `.env` file only configures Docker containers. The .NET apps read secrets via `dotnet user-secrets`:

```bash
dotnet user-secrets set "Database:ConnectionString" \
  "Server=localhost,1433;Database=JumboProductCatalog;User Id=sa;Password=<your-password>;TrustServerCertificate=True" \
  --project backend/Jumbo.ProductCatalog.Api

dotnet user-secrets set "BlobStorage:ConnectionString" \
  "DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tiqIFOGWWhbhwznHXVjmQT==;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;" \
  --project backend/Jumbo.ProductCatalog.Api
```

Repeat for `Jumbo.ProductCatalog.Worker` if needed. See `docs/secrets-guide.md` for the full secrets strategy.

### 3. Run

```bash
dotnet restore
dotnet build
dotnet run --project backend/Jumbo.ProductCatalog.Api
```

## Notes

- Targeting `net10.0`.
- `NuGet.Config` pins the feed to `nuget.org` only so restore works without private company credentials.
