# Architecture Decisions

### Single solution

One `Jumbo.ProductCatalog.sln`, one repo, two top-level folders (`frontend/`, `backend/`)

### Result pattern

Application services return `Result<T>` for expected failures (something like not found). It makes an easy and reproducible way to handle failures.

### UI has its own models

The Blazor UI defines its own `ProductDto`, `Category`, etc. to match the API's JSON shapes. Sharing a DTO assembly with the backend would break the UI on any internal rename, even if the JSON contract didn't change.

### Blazor InteractiveServer

All product pages use `@rendermode InteractiveServer`. We need inline deletes and live form validation, static SSR requires full page reloads for that. InteractiveServer means each tab holds an open SignalR connection, so the UI runs as a persistent process rather than a static host.

### Soft deletes

`DeleteAsync` sets `IsArchived = true`. Keeps history which means we don't break any relationships and have an audit trail. All queries filter archived records out automatically.

### Background export

The Worker serialises active products to JSON and uploads a timestamped blob to Azure Blob Storage. Interval is configurable via `Export:IntervalMinutes`.

### ViewModels for Blazor pages

Each page has a scoped ViewModel that owns HTTP calls, loading/error state, and submit guarding. Razor components just delegate to it. JS interop (`window.confirm`) stays in the component.
