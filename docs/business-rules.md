# Business Rules

This file is the source of truth for all domain business rules in the Product Catalog.

## Product identity

| Rule | Enforcement | Why |
|---|---|---|
| `Code` must be unique across all non-archived products | Unique DB index; service-layer check in `ProductCatalogService.CreateAsync` | Codes are business identifiers (e.g. `F1001`) used externally; duplicates would be ambiguous |
| `Code` max 50 characters | `[MaxLength(50)]` on `CreateProductRequest` and `Product` entity | Prevents unbounded storage; consistent with the `Name` field |
| `Code` is immutable after creation | Absent from `UpdateProductRequest` | Changing a business key would silently break any external reference |

---

## Product fields

| Rule | Enforcement | Why |
|---|---|---|
| `Name` is required, max 50 characters | `[Required]` + `[MaxLength(50)]` on DTO and entity | A product without a name is not meaningful; max 50 matches warehouse/label constraints |
| `Category` is required and must be `Food` or `NonFood` | `[Required]` + `Category` enum on DTO | Every product must be classifiable; the assessment domain defines exactly two categories |
| `Content` is optional with no length limit | No annotation; `nvarchar(max)` in DB | Content is descriptive copy; length varies widely and no business constraint was specified |

---

## Active vs. archived flags

| Rule | Enforcement | Why |
|---|---|---|
| `IsActive` represents business status - whether the product is for sale | Set to `true` by default on creation; mutable via `UpdateProductRequest` | A product can be temporarily de-listed without being deleted |
| `IsArchived` represents record-level soft delete - the product has been removed | Set to `false` by default; set to `true` by `DeleteAsync`; never reset except via reactivation | Decoupled from `IsActive` so that archival and de-listing are independent operations |
| The two flags are independent | Separate columns, separate code paths | A product can be inactive but not archived (off-sale but still a record) |

---

## Soft delete

| Rule | Enforcement | Why |
|---|---|---|
| All deletes are soft - `IsArchived = true` instead of `DELETE` | `ProductRepository.DeleteAsync` sets the flag and calls `SaveChanges` | Preserves audit history; allows reactivation; prevents accidental data loss |
| Archived products are automatically excluded from all standard queries | Global EF Core query filter: `.HasQueryFilter(p => !p.IsArchived)` on `ProductDbContext` | Every query is safe by default; callers cannot accidentally surface deleted records |

---

## Reactivation

| Rule | Enforcement | Why |
|---|---|---|
| Creating a product with the code of an archived product reactivates it instead of failing | `ProductCatalogService.CreateAsync` calls `GetByCodeIncludingArchivedAsync` (uses `.IgnoreQueryFilters()`); if archived, updates all fields and sets `IsArchived = false` | Re-adding a previously deleted product is a common catalogue operation; this avoids requiring a separate explicit "unarchive" endpoint |

---

## Audit timestamps

| Rule | Enforcement | Why |
|---|---|---|
| `CreatedAt` is set once at creation and never updated | `UpdateTimestampsInterceptor` sets it only on `EntityState.Added`; EF Core model configured with `ValueGeneratedOnAdd` + `PropertySaveBehavior.Ignore` | An immutable creation timestamp is required for audit trails |
| `UpdatedAt` is stamped on creation and on every subsequent save | `UpdateTimestampsInterceptor` sets it on both `Added` and `Modified` states | Tracks the last change without any application-layer code |
| Both timestamps are always UTC | `DateTime.UtcNow` in the interceptor | Avoids timezone ambiguity, especially across hosted environments |

---

## Query visibility

| Rule | Enforcement | Why |
|---|---|---|
| `GetAllAsync` returns all non-archived products | Global query filter applied automatically | "All products" means the live catalogue, not historical records |
| `GetActiveAsync` returns only products where `IsActive = true` and `IsArchived = false` | Explicit `WHERE` clause in `ProductRepository.GetActiveAsync` + global filter | Provides a sales-ready view of the catalogue |
| `GetByIdAsync` and `GetByCodeAsync` return `null` for archived products | Global query filter applied automatically | Prevents exposing deleted records via direct lookup |

---

## Failure signalling

| Rule | Enforcement | Why |
|---|---|---|
| Application layer uses `Result<T>` for expected domain failures (not found, duplicate code, validation) | All `ProductCatalogService` methods return `Result<T>` or `Result`; defined in `Domain/Common/Result.cs` | Separates known business failures from unexpected exceptions; callers handle them explicitly |
| Infrastructure exceptions are not caught - they bubble to the global exception handler | No `try/catch` in service or repository code | Infrastructure failures (DB down, timeout) are operational, not business, concerns |

---

## Background export

| Rule | Enforcement | Why |
|---|---|---|
| Export includes only active, non-archived products | `ExportService` calls `IProductRepository.GetActiveAsync` (`IsActive = true` and global `!IsArchived` filter) | The export is a sales-ready feed; inactive or deleted products must not appear |
| Export runs on a configurable interval | `ExportConfig.IntervalMinutes` in `appsettings.json`; default 5 minutes | Allows tuning without a code change |
| Each export produces a new file; existing files are not overwritten | File name includes a UTC timestamp (`active-products-yyyyMMddHHmmss.json`) | Preserves an audit trail of every export run |
