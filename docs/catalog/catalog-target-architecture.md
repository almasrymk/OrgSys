# Catalog Bounded Context — Target Architecture (Phase 1)

Builds on `docs/catalog/catalog-current-state.md`. User-confirmed direction (2026-09-15): **full
physical relocation** of Product/Classification/Unit/Property/PropertyElement/
ProductPropertyElement into a new `Catalog` module, using the Parties/CommercialDocuments
no-op-migration technique; **defer** `ProductVariant` and multi-barcode-per-product (no confirmed
business need today — brief §20 anti-over-split rule applies).

## 1. Module layout (matches the established 4-project convention exactly)

```
Modules/
  Catalog/
    Catalog.Domain/
      Entities/          Product.cs, ProductUnit.cs, Category.cs(*), Unit.cs(*), Brand.cs,
                          Property.cs, PropertyElement.cs, ProductPropertyElement.cs,
                          PriceList.cs, PriceListEntry.cs
      Enums/              ProductType.cs (new), TrackingType.cs, CostingMethod.cs (relocated,
                          unchanged), AttributeDataType.cs (new)
      Repositories/       IProductRepository, ICategoryRepository, IBrandRepository,
                          IUnitRepository, IPriceListRepository
      Exceptions/         ProductDomainException-derived types
      AssemblyMarker.cs, GlobalUsings.cs
    Catalog.Application/
      Products/{Commands,Queries}/, MappingProfile.cs, ProductDto.cs, ...
      Categories/, Brands/, Units/, Attributes/, PriceLists/
    Catalog.Infrastructure/
      Persistence/        ProductRepository.cs, CategoryRepository.cs, ... (thin wrappers over
                          the existing generic IRepository<T>, same as Payables/Receivables)
      DependencyInjection/ServiceCollectionExtensions.cs  (AddCatalogModule)
      Seeding/            CatalogDataSeeder.cs (only if seed data is needed)
    Catalog.Contracts/
      Products/           GetProductByIdQuery, GetProductNamesQuery (superset of Inventory's
                          existing one), ProductLookupDto, ProductSnapshotDto
      Pricing/            ResolvePriceQuery, ResolvedPriceDto
      IntegrationEvents/  ProductCreatedIntegrationEvent, ProductDeactivatedIntegrationEvent,
                          ProductPriceChangedIntegrationEvent (only if a real subscriber exists —
                          none does yet; stub the contracts, wire handlers later if needed)
```

(*) See §3 — `Classification`/`Unit` C# class **names are kept unchanged** during relocation
(only their project/namespace moves), consistent with how `Dealer` and `Invoice` kept their exact
names during the Parties/CommercialDocuments extractions. `Category` is used only as the
*conceptual* term in this document and in DTO/CQRS naming going forward (`CategoryDto`,
`GetCategoriesQuery`) — the underlying entity type stays `Classification` to avoid unnecessary
churn across ~15 existing consumers. This is a deliberate, documented deviation from the brief's
literal "use `Category`" instruction, justified by rule #19 (follow the established low-risk
relocation pattern) and the brief's own Phase 2 escape valve ("if migration would create
unnecessary breakage... preserve compatibility, document the decision").

## 2. Aggregate design

### Product (aggregate root) — relocated + extended

Keeps its exact current shape (`Name, Nickname?, Barcode?, Description?, Price, Cost,
ClassificationId/Classification, DealerId?/Dealer, Recipe?, TrackingType, CostingMethod,
ExpiryTracking, IsActive, ProductUnits, ProductRecipes→stays in Inventory (see §4),
ProductPropertyElements`), **plus one new field**:

```csharp
public virtual ProductType ProductType { get; set; } = ProductType.StockItem;
```

`ProductType { StockItem, NonStockItem, Service }` — new enum, `Catalog.Domain.Enums`. Backfilled
during migration from existing data: `TrackingType != None → StockItem`; `TrackingType == None →
NonStockItem` (safe default — nothing in OrgSys today can distinguish an existing "no tracking"
product from a service, so no row is silently reclassified as `Service`; users can reclassify
via `ChangeProductType` after migration). Domain invariant (enforced in Application, matching the
existing convention documented on `TrackingType`/`CostingMethod`): `TrackingType` must be `None`
when `ProductType == Service`.

Stays exactly as-is by design (per the brief's own PHASE 29/3 boundary, confirmed unchanged by
discovery): no `StockQuantity`, no `AvailableQuantity`, no `WarehouseId`, no `AverageCost` — those
remain Inventory-only, computed from `InventoryBalance`/`InventoryCostLayer` keyed by `ProductId`.

### ProductUnit (child of Product) — relocated unchanged

Already implements the brief's "product-specific UOM conversion" (Phase 5) exactly:
`ProductId, UnitId, Rate, DefaultUnit`. No change to shape. This satisfies Phase 5's
`ProductUnitConversion` requirement without a new entity.

### Category (`Classification` class, relocated unchanged)

No hierarchy today (`ParentCategoryId`) and **none is added** in this pass — no evidence any
existing or requested capability needs one (brief's Phase 4 hierarchy example is illustrative, not
a confirmed OrgSys requirement; adding it now with zero consumers would be exactly the speculative
scaffolding the brief's own §20 warns against). Flagged as a clean, low-risk future addition if the
user confirms real need — the entity's `BaseModel.ParentId` field already exists as inherited
plumbing and is literally sitting there unused for this purpose today, so the seam already exists.

### Unit (relocated unchanged)

No `UnitOfMeasureCategory`/dimension grouping added in this pass — same reasoning as Category
hierarchy: zero existing consumers need it, and `ProductUnit.Rate` already provides the
product-specific conversion the brief actually requires. Global cross-dimension-safe conversion
(Phase 5's "do NOT allow Kg→Piece unless product-specific") is enforced exactly as OrgSys already
enforces it — a conversion only ever exists in the context of one `Product`'s `ProductUnit` row,
never globally — no unsafe global conversion path exists to guard against because no global
conversion table exists at all.

### Property / PropertyElement / ProductPropertyElement (relocated + one addition)

Relocated unchanged, **plus** one new field on `Property`:

```csharp
public virtual AttributeDataType DataType { get; set; } = AttributeDataType.Selection;
```

`AttributeDataType { Text, Number, Boolean, Date, Selection, MultiSelection }` — new enum. Default
`Selection` preserves exact current behavior for every existing `Property` row (they are all
implicitly selection-lists today). `IsVariantDefining` is **not** added — it would have zero
consumers with `ProductVariant` deferred, and is pure speculative scaffolding per the user's
"defer both" decision.

### Brand (new aggregate)

```csharp
[Table("Brand")]
public class Brand : BaseModel
{
    [Required] public virtual string Name { get; set; }
    public virtual string? Description { get; set; }
    public virtual bool IsActive { get; set; } = true;
}
```

`Product` gets a new optional `BrandId`/`Brand` nav (nullable — brief's own Phase 28 edge case "No
Brand" must be supported). `BaseModel` already provides `Code`/`CodeNumber`, matching the brief's
`Id/Code/Name/Description?/IsActive` shape without re-adding redundant fields.

### PriceList / PriceListEntry (new aggregate, standalone — not a Product child collection)

```csharp
[Table("PriceList")]
public class PriceList : BaseModel
{
    [Required] public virtual string Name { get; set; }
    public virtual long? CurrencyId { get; set; }        // FK -> MasterData.Currency (accepted exception, same as every other module's Currency reference)
    public virtual DateTime? ValidFrom { get; set; }
    public virtual DateTime? ValidTo { get; set; }
    public virtual bool IsDefault { get; set; }
    public virtual bool IsActive { get; set; } = true;
}

[Table("PriceListEntry")]
public class PriceListEntry : BaseModel
{
    public virtual long PriceListId { get; set; }
    public virtual PriceList? PriceList { get; set; }     // in-context nav, same aggregate
    public virtual long ProductId { get; set; }            // scalar only — Product is the SAME module here, so a nav is fine and consistent (ProductUnit does the same)
    public virtual Product? Product { get; set; }
    public virtual long? UnitId { get; set; }
    public virtual Unit? Unit { get; set; }
    [Column(TypeName = "decimal(18,2)")] public virtual decimal Price { get; set; }
    public virtual decimal? MinQuantity { get; set; }
    public virtual DateTime? ValidFrom { get; set; }
    public virtual DateTime? ValidTo { get; set; }
    public virtual bool IsActive { get; set; } = true;
}
```

Deliberately **not** `Product.PriceListEntries` — per the brief's own Phase 10/"future pricing
extraction" guidance, `PriceList` is its own aggregate root referencing `Product` by ID, not a
child collection hanging off `Product`, so Pricing can be extracted to its own module later without
touching `Product` at all.

Price resolution: `Catalog.Application.Pricing.ResolvePriceQueryHandler` — takes
`ProductId, PriceListId?, Quantity, UnitId?, Date`, returns `ResolvedPriceDto`. If no
`PriceListId` given, falls back to `Product.Price` (kept as the "default/simple price" the brief
explicitly allows for backwards compatibility) — this is the only place `Product.Price` retains
real meaning going forward; new commercial pricing should flow through `PriceListEntry`.

## 3. What stays where (confirms `catalog-current-state.md` §9, now final for this pass)

| Stays in Inventory | Stays in MasterData | Stays elsewhere |
|---|---|---|
| `ProductRecipe` (BOM/costing composition — brief's own Phase 29 boundary) | `Country, City, District, Currency, PaymentType, ReferenceType` (genuine cross-domain reference data, confirmed by discovery — none of these are Product-specific) | `Dealer` → Parties (unchanged) |
| `Stock, Transaction(Product), InventoryBalance, InventoryBatch, InventoryCostLayer, InventoryReceipt(Line), InventoryIssue(Line), InventorySerial, StockAdjustment(Line+Reason), StockReservation, StockTransfer(Line), WarehouseLocation` | | `Invoice/InvoiceProduct/InvoiceType` → CommercialDocuments (unchanged) |

## 4. Dependency direction (enforced by Architecture.Tests, Phase 24 of the brief)

```
Catalog.Domain          → BuildingBlocks/OrgSys.SharedKernel only
Catalog.Application     → Catalog.Domain, Catalog.Contracts
Catalog.Infrastructure  → Catalog.Domain, Catalog.Application, BuildingBlocks
Catalog.Contracts       → (no dependencies, DTOs/queries only)

Inventory.Domain        → Catalog.Domain   [NEW accepted exception — Stock/Transaction/etc. keep
                                             their existing EF navigations to Product/ProductUnit,
                                             same shape as the existing Inventory→Parties exception
                                             for Product.Dealer]
Purchasing.Domain       → Catalog.Domain   [exception renamed from the existing
                                             Purchasing→MasterData Unit-navigation entry, since
                                             Unit now lives in Catalog]
Treasury.Application    → Catalog.Domain?  [only if Treasury's existing MasterData/Currency Unit
                                             mapping support actually touches Unit — verify during
                                             Step 15-19; likely no change, Treasury's Unit
                                             references were about Currency, not Product's Unit]
Reporting.Application   → Catalog.Domain   [exception renamed from existing
                                             Reporting→Inventory/Reporting→MasterData entries for
                                             Product/Classification reads — by-design, unchanged
                                             reasoning]
```

No Domain project may reference `Catalog.Domain` except through a documented, reasoned exception
following the exact template already used for every other cross-module navigation in this
codebase. Catalog.Domain itself must never reference any other module's Domain — Product's
existing `DealerId`/`Dealer` navigation to `Parties.Domain.Dealer` is the one pre-existing
exception it inherits from its current Inventory-owned form, carried forward as
`("Catalog", "Parties", "Product.Dealer keeps its existing EF navigation, same as it had in
Inventory.Domain before relocation.")`.

## 5. Zero-schema-diff relocation technique (copied from Parties/CommercialDocuments precedent)

1. `[Table("Product")]`, `[Table("ProductUnit")]`, `[Table("Classification")]`, `[Table("Unit")]`,
   `[Table("Property")]`, `[Table("PropertyElement")]`, `[Table("ProductPropertyElement")]` all
   stay exactly as-is — only the C# namespace changes (`Inventory.Domain`/`MasterData.Domain` →
   `Catalog.Domain`).
2. Existing FK column names (`ClassificationId`, `UnitId`, `ProductId`, etc.) are preserved
   unchanged — no property renames on the relocated types themselves in this pass (see §1's
   decision to keep `Classification`/`Unit` class names too, for exactly this reason: renaming
   the class is free, renaming the FK property is not, without explicit `[Column(...)]`
   overrides that add risk for no benefit).
3. `OrgContext.OnModelCreating` Fluent config for these 7 entities moves into a new
   `ConfigureCatalog(modelBuilder)` helper (mirroring `ConfigureInventoryHardening`), called from
   the same place; no `HasColumnName`/`ToTable` calls change value, only the C# type reference.
4. After the move, run `dotnet ef migrations add VerifyCatalogRelocationNoOp` and inspect the
   generated `Up()`/`Down()` — expect it to be **empty** (or contain only harmless
   metadata-only changes like `.Annotation` clr-type bookkeeping). If it contains any real
   `DropColumn`/`AddColumn`/`RenameTable`, that is a bug in the relocation, not an intended change,
   and must be fixed before proceeding — this is the same verification gate the Parties extraction
   used.
5. New columns (`Product.ProductType`, `Property.DataType`, `Product.BrandId`) and new tables
   (`Brand`, `PriceList`, `PriceListEntry`) go into a **second**, separate migration
   (`AddCatalogModule`) so the zero-diff relocation and the genuinely-new schema changes stay
   independently reviewable and revertable.
6. Neither migration is applied to the live `OrgConnection` database in this session without
   fresh, explicit user consent (per this session's standing project memory on DB migration
   caution) — migrations are generated and left for review/application on explicit instruction.

## 6. Integration events (Phase 11)

Given the existing `OrgSys.EventBus` is in-process/MediatR-backed with **zero current outbox or
cross-request subscribers for any module** (confirmed by discovery — Payables/Receivables'
`*IntegrationEventHandler`s are the only real consumers anywhere, and they listen to
Sales/CommercialDocuments events, not anything Product-related), Catalog's integration events are
scoped to **contracts only** in this pass (`ProductCreatedIntegrationEvent`,
`ProductDeactivatedIntegrationEvent`, `ProductPriceChangedIntegrationEvent` types defined in
`Catalog.Contracts/IntegrationEvents/`) — published from Application command handlers using the
existing `IIntegrationEventPublisher`, but **no other module subscribes yet** since none currently
needs to react to product changes. This avoids Phase 11's warning against "creating events simply
because CRUD happened" while still leaving the wiring in place for a real future consumer,
consistent with how every other recent module (Payables, Receivables) introduced its own events
only where a real subscriber existed.

## 7. API surface (Phase 21)

New area `API/Controllers/Org/Catalog/`: `ProductController`, `CategoryController` (wraps
`Classification`), `BrandController`, `UnitController`, `PropertyController` (attributes),
`PriceListController`. The existing `Setting/ProductController.cs`, `Setting/
ClassificationController.cs`, `Setting/UnitController.cs` are **superseded** — see
`catalog-data-migration-plan.md` for the exact deprecation sequencing (old controllers keep working
against the relocated types via updated `using`s until Angular is repointed, then are deleted in
Step 23, not before).

## 8. Angular (Phase 22) — sequencing note

Per the brief's own Phase 20/22 ordering ("update Angular integration only after backend contracts
stabilize"), Angular changes are the **last** implementation step, after the backend Catalog module
builds, migrates, and passes tests. The existing `features/administration/products/` feature is
repointed to whatever the new controller route ends up being (`/api/catalog/products` vs the
current `/Product`) with the same component/model/service shapes — no visual redesign. `Category`
(Classification) and `Unit` get their own first CRUD screens for the first time, following the
identical folder pattern already established by `products`/`inventory`/`warehouse`. `Brand` and
`PriceList` are new features in the same style. This document does not restate the full Angular
plan — it is produced as part of Step 20 execution, not Phase 1 planning.
