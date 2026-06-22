# Azure Deployment Guide

## Resources

| Resource | Purpose | Recommended SKU |
|----------|---------|-----------------|
| Azure SQL Database | Product data (EF Core migrations target) | General Purpose, 2 vCore |
| Azure Blob Storage | Export file sink | Standard LRS |
| Azure App Service (Linux) | Host `Jumbo.ProductCatalog.Api` | B2 minimum |
| Azure App Service (Linux) | Host `Jumbo.ProductCatalog.Worker` | B1 minimum |
| Azure Key Vault | Connection strings and secrets | Standard |

## Secrets

Store the following in Key Vault and reference them in each App Service's configuration blade as Key Vault references:

| Secret name | Value |
|-------------|-------|
| `Database--ConnectionString` | Azure SQL connection string |
| `BlobStorage--ConnectionString` | Azure Storage connection string |

Reference syntax in App Service application settings:

```
@Microsoft.KeyVault(SecretUri=https://<vault-name>.vault.azure.net/secrets/<secret-name>/)
```

The app reads these via `IConfiguration`, which picks up environment variables and app settings automatically - no code changes needed.

## App Service environment variables

Set on both API and Worker App Services:

```
ASPNETCORE_ENVIRONMENT = Production
```

## EF Core migrations

Run as part of the release pipeline **before** the app slot is swapped:

```bash
dotnet ef database update \
  --project backend/Jumbo.ProductCatalog.Infrastructure \
  --startup-project backend/Jumbo.ProductCatalog.Api \
  --connection "$DATABASE_CONNECTION_STRING"
```

The `ProductDbContextFactory` in Infrastructure reads `DATABASE_CONNECTION_STRING` as a fallback when no `IConfiguration` is available (CLI context). Set this variable in the release agent.

## Worker hosting

`Jumbo.ProductCatalog.Worker` is a hosted `BackgroundService`. Two options:

1. **Azure App Service with `AlwaysOn: true`** - simplest; the worker process runs continuously and the export timer fires every `Export:IntervalMinutes` minutes (default 5). No extra resources required.

2. **Azure Container App Job** - isolates the worker compute from the API. Configure a cron schedule aligned to `Export:IntervalMinutes`. Preferred when the worker load or scaling requirements differ from the API.

## Health check

The API exposes `/healthz` (mapped via `app.MapHealthChecks("/healthz")` in `Program.cs`). Configure this path as the Azure App Service health probe in the **Health check** blade.

## Local vs. production infrastructure

`docker-compose.yml` runs SQL Server 2022 and Azurite containers for **local development only**. Do not reference it in any production pipeline. The connection strings in `appsettings.Development.json` point to these containers and must not be used in production.

## Authentication (deferred - Phase 7)

Authentication is not yet implemented. **Do not expose the API to the public internet before Phase 7 is complete.** In the interim, restrict the API App Service with an Azure Virtual Network integration and limit inbound traffic to the UI's outbound IP range.
