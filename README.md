# Jumbo Product Service

Assessment project for Jumbo solution engineer - a product catalogue service with a Blazor frontend, ASP.NET Core API, background worker, and layered backend architecture.

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
└── docs/
    ├── architecture-decisions.md
    ├── business-rules.md
    └── deployment-azure.md
```

Test project lives inside the backend folder:

```text
├── backend/
│   └── tests/
│       └── Jumbo.ProductCatalog.Tests/     # Unit tests (xUnit)
```

## Prerequisites

- [.NET SDK 10.0.x](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (for SQL Server + Azurite)

## Docker setup

`docker-compose.yml` spins up two services:

| Service | Image | Ports | Purpose |
|---|---|---|---|
| `sqlserver` | `mssql/server:2022-latest` | `1433` | SQL Server database |
| `azurite` | `azure-storage/azurite` | `10000–10002` | Local Azure Blob/Queue/Table emulator |

### 1. Configure environment

```bash
cp .env.example .env
```

Open `.env` and set a strong SQL Server SA password (must meet SQL Server complexity rules - min 8 chars, mixed case, digit, symbol):

```
MSSQL_SA_PASSWORD=Your_Strong_Password1!
```

### 2. Start containers

```bash
docker compose up -d
```

Verify both containers are healthy before continuing:

```bash
docker compose ps
```

`sqlserver` uses a healthcheck; wait until its status shows `healthy` (usually ~30 s).

### 3. Apply database migrations

Install the EF Core CLI tool if you don't have it:

```bash
dotnet tool install --global dotnet-ef
```

Run migrations (replace the password with the one you set in `.env`):

```bash
dotnet ef database update \
  --project backend/Jumbo.ProductCatalog.Infrastructure \
  --startup-project backend/Jumbo.ProductCatalog.Api \
  --connection "Server=localhost,1433;Database=JumboProductCatalog;User Id=sa;Password=Your_Strong_Password1!;TrustServerCertificate=True"
```

### 4. Set local app secrets

The `.env` file only configures Docker containers. The .NET apps read the database connection string via `dotnet user-secrets`:

```bash
dotnet user-secrets set "Database:ConnectionString" \
  "Server=localhost,1433;Database=JumboProductCatalog;User Id=sa;Password=Your_Strong_Password1!;TrustServerCertificate=True" \
  --project backend/Jumbo.ProductCatalog.Api

dotnet user-secrets set "Database:ConnectionString" \
  "Server=localhost,1433;Database=JumboProductCatalog;User Id=sa;Password=Your_Strong_Password1!;TrustServerCertificate=True" \
  --project backend/Jumbo.ProductCatalog.Worker
```

The blob storage connection string does **not** need to be set manually for local development — `appsettings.Development.json` already contains `UseDevelopmentStorage=true`, which points to the Azurite container. If connecting to a real Azure Storage account instead, override it with:

```bash
dotnet user-secrets set "BlobStorage:ConnectionString" \
  "DefaultEndpointsProtocol=https;AccountName=<account>;AccountKey=<key>;EndpointSuffix=core.windows.net" \
  --project backend/Jumbo.ProductCatalog.Api

dotnet user-secrets set "BlobStorage:ConnectionString" \
  "DefaultEndpointsProtocol=https;AccountName=<account>;AccountKey=<key>;EndpointSuffix=core.windows.net" \
  --project backend/Jumbo.ProductCatalog.Worker
```

### Stopping / resetting

```bash
docker compose down          # stop containers, keep volumes
docker compose down -v       # stop and delete all data volumes
```

## Getting started

### 1. Start infrastructure

Complete the [Docker setup](#docker-setup) steps above first.

### 2. Run the API

```bash
dotnet restore
dotnet build
dotnet run --project backend/Jumbo.ProductCatalog.Api
```

The API listens on `https://localhost:7170` (HTTPS) / `http://localhost:5174` (HTTP) by default (see `backend/Jumbo.ProductCatalog.Api/Properties/launchSettings.json`). The browser opens the Scalar API explorer at `http://localhost:5174/scalar/v1`.

### 3. Run the frontend

The Blazor UI points at the API via the `Api:BaseUrl` setting, which is already set to `http://localhost:5174` in `appsettings.Development.json`.

```bash
dotnet run --project frontend/Jumbo.ProductCatalog.UI
```

Opens at `http://localhost:5015` (or the HTTPS equivalent - see `frontend/Jumbo.ProductCatalog.UI/Properties/launchSettings.json`).

### 4. Run the worker

```bash
dotnet run --project backend/Jumbo.ProductCatalog.Worker
```

The worker runs an export immediately on startup, then repeats every 5 minutes (configurable via `Export:IntervalMinutes` in `appsettings.json`). You should see these log lines:

```
info: Jumbo.ProductCatalog.Worker.Worker  Export cycle started
info: Jumbo.ProductCatalog.Core.Services.ExportService  Export completed: N products written to active-products-yyyyMMddHHmmss.json
info: Jumbo.ProductCatalog.Worker.Worker  Export cycle completed: N products exported to active-products-yyyyMMddHHmmss.json
```

#### Verify blobs in Azurite

Install the Azure CLI if you don't have it: <https://docs.microsoft.com/cli/azure/install-azure-cli>

List exported files in the `product-catalog-exports` container:

```bash
az storage blob list \
  --connection-string "UseDevelopmentStorage=true" \
  --container-name product-catalog-exports \
  --output table
```

Each successful export run adds a new `active-products-yyyyMMddHHmmss.json` blob. An empty table means no export has run yet (no active products, or the worker hasn't started).

To download and inspect a specific export (replace the filename with one from the list above):

```bash
az storage blob download \
  --connection-string "UseDevelopmentStorage=true" \
  --container-name product-catalog-exports \
  --name active-products-yyyyMMddHHmmss.json \
  --file export.json && cat export.json
```

### 5. Run the tests

```bash
dotnet test Jumbo.ProductCatalog.sln
```

## Notes

- Targeting `net10.0`.
- `NuGet.Config` pins the feed to `nuget.org` only so restore works without private company credentials.
