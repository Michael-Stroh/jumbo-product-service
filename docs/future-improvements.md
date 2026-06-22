# Future Improvements

Things to tackle next given more time.

- **Auth**: endpoints have `[Authorize]` commented out. Would want to wire up JWT Bearer (Azure AD B2C or Entra ID) with a `product:write` role policy.
- **Health checks**: `/healthz` is registered but returns nothing useful. Worth adding SQL Server and Blob Storage probes.
- **Managed Identity**: Connection strings work fine for now, but swapping in `DefaultAzureCredential` means we can use Managed Identity.
- **Pagination**: The product list endpoint returns everything. Easy to ignore until the catalogue gets big, then it's a problem.
- **Application Insights**: The correlation ID middleware already handles `X-Correlation-ID`, so adding `AddApplicationInsightsTelemetry` would basically give cross-service tracing for free.
- **Service Bus**: Right now nothing reacts to changes, it's all request-driven. Publishing events like `ProductCreated` or `ProductUpdated` would let the Worker (and anything else) subscribe independently instead of polling on a timer.
- **Connection pooling**: Default SQL pool settings can quietly exhaust connections under load.
- **Cache write-through**: Replace the commented-out `OutputCache` with a manual `IMemoryCache` layer, long TTL, invalidate and repopulate on each write.
