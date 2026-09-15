# Catalog Bounded Context — Phase 0 Discovery Report

Analysis only — no source, config, or migration files were modified to produce this report.
Findings are backed by direct inspection (Read/Grep/Glob) of `D:\Work\MK\Source\OrgSys`, branch
`Latest`, as of 2026-09-15. Recent commits on this branch: Sales Quotation/SalesOrder + Purchasing
hardening (`48a48637`), Advances/Custody skeleton (`79df098a`), Accounts Payable (`89f66c01`),
Accounts Receivable (`58630b4e`). Working tree currently has uncommitted Inventory-hardening WIP
(Balances/Receipts/Issues/Transfers/Adjustments/Reservations/Serials/WarehouseLocations, a new
`AddInventoryHardening` migration, and Angular warehouse screens) — this report does not touch any
of that.

---

## 0. Prior art: a Catalog module was already proposed and rejected once — and later partially overridden

Before anything else: `docs/shared-business-capabilities-review.md` §2 already asked "should
Product/Category/Unit become a Catalog module?" and answered **no**, on grounds that at the time
(HEAD `723ad0f9`) there was exactly one `Product` table, exactly one category concept
(`Classification`), exactly one `Unit` table, no `Brand`, no `PriceList`, no `Barcode` entity, and
Sales/Purchasing already referenced `Product` by ID only — so a `Catalog` module would have been
"pure speculative scaffolding" (the brief's own §20 anti-over-split rule). The same review also
rejected extracting `CommercialDocuments` (Invoice) from Sales for the same "no duplication to
fix" reasoning.

**That second rejection was later overridden**: git history and the current repo state show
`Modules/CommercialDocuments/` now exists and genuinely owns `Invoice`/`InvoiceProduct`/
`InvoiceType`, physically relocated out of `Sales.Domain` (per this session's own project memory:
"CommercialDocuments module extracted 2026-09-13, Invoice moved out of Sales.Domain"). The
`Parties` module the same review *did* recommend was also built and is now the established pattern
for `Dealer`. Both extractions used the same proven technique: move the class/table under
`[Table("X")]` unchanged (same column names), update the ~6 consuming modules' project references
from `X.Domain` to `X.Contracts`, and verify zero schema drift with a no-op EF migration diff. Both
are now referenced throughout `Tests/Architecture.Tests/` as first-class modules.

**Conclusion for this task**: the original rejection's premise — "nothing to extract, no
duplication, no new business content" — no longer holds for Catalog. This task's brief explicitly
scopes substantial *new* business capability that does not exist anywhere in OrgSys today: `Brand`,
`PriceList`/`PriceListEntry`, multi-barcode-per-product, product-specific UOM conversion beyond the
existing flat `ProductUnit.Rate`, and a first-class attribute/variant system beyond the existing
`Property`/`PropertyElement` tables. That is real, new, cohesive business ownership — the same bar
`Parties` and `CommercialDocuments` cleared. Per rule #19 ("when this prompt conflicts with an
established pattern, follow the established pattern and document why"): the established pattern
here is not "never build Catalog" — it is "only extract a module when it has genuine, non-trivial
business content to own," and on re-evaluation Catalog now clears that bar the same way
CommercialDocuments did. This report proceeds on that basis. The physical-relocation mechanics
(preserve `[Table(...)]`, no-op migration verification, Architecture.Tests module registration) are
copied directly from the Parties/CommercialDocuments precedent rather than invented.

---

## 1. Everything that exists today, by concept

### 1.1 Product (sole owner: `Inventory.Domain`)

`Modules/Inventory/Inventory.Domain/Entities/Product.cs`, table `Product`, inherits `BaseModel`
(`Id, CodeNumber, Code, MaskText, ParentId, TypeId, Hide, ImgPath, Status` — from
`BuildingBlocks/OrgSys.SharedKernel/BaseModel.cs`). Properties:

```
Name (required), Nickname?, Barcode?, Description?
Price (decimal 18,2), Cost (decimal 18,2)
ClassificationId (required FK -> MasterData.Classification), Classification nav
DealerId? (FK -> Parties.Dealer), Dealer nav
Recipe? (string — legacy, unrelated to ProductRecipe entity below)
TrackingType (enum: None/Batch/Serial-ish) — default None
CostingMethod (enum) — default WeightedAverage
ExpiryTracking (bool)
IsActive (bool) — default true
ProductUnits, ProductRecipes, ProductPropertyElements (collections)
```

No strongly-typed ID (`long Id` from `BaseModel`). No `ProductType`/`ItemType`/`StockType` enum —
there is currently no way to mark a Product as "service" vs "stock item" vs "non-stock item";
`TrackingType.None` + no stock movements is the closest de-facto equivalent of a service today, but
it's not an explicit classification.

Child/related entities, all in `Modules/Inventory/Inventory.Domain/Entities/`, all Inventory-owned,
all with real (in-context) EF navigations to `Product`:

| Entity | Table | Purpose |
|---|---|---|
| `ProductUnit` | `ProductUnit` | Product↔Unit conversion: `ProductId, UnitId, Rate (decimal), DefaultUnit (bool)` — this **is** the existing "product-specific UOM conversion" the brief asks for (Phase 5), just without dimension-safety checks |
| `Property` | `Property` | Attribute definition: `Name` + `PropertyElements` collection (no `DataType`, always implicitly "selection") |
| `PropertyElement` | `PropertyElement` | Attribute option: `PropertyId, Name` (e.g. Property "Color" → Elements "Red","Blue") |
| `ProductPropertyElement` | `ProductPropertyElement` | Product↔PropertyElement assignment: `ProductId?, PropertyId?, PropertyElementId?, IsChecked (bool)` — this **is** the existing attribute-assignment mechanism the brief asks for in Phase 7, pre-existing but never exposed as first-class CRUD |
| `ProductRecipe` | `ProductRecipe` | BOM-ish: `ProductId, RecipeId, UnitId, Quantity` — cost composition, not directly in Catalog's scope |

Downstream Inventory entities (stock, not Catalog's concern, listed for completeness — all live in
`Modules/Inventory/Inventory.Domain/Entities/` and carry a scalar `ProductId` FK, real in-context
navigation): `Stock`, `Transaction`/`TransactionProduct`, `Inventory`/`InventoryProduct`,
`InventoryBalance`, `InventoryBatch`, `InventoryCostLayer`, `InventoryReceipt(Line)`,
`InventoryIssue(Line)`, `InventorySerial`, `StockAdjustment(Line)`, `StockAdjustmentReason`,
`StockReservation`, `StockTransfer(Line)`, `WarehouseLocation`.

### 1.2 Category — currently named `Classification` (owner: `MasterData.Domain`)

`Modules/MasterData/MasterData.Domain/Entities/Classification.cs`, table `Classification`:
`Name (required, 3-50 chars), BePurchased (bool), BeSold (bool), BeManufactured (bool)`. Flat, no
`ParentClassificationId` — **no hierarchy support exists today**. Referenced only by
`Product.ClassificationId` (required FK).

### 1.3 Unit / UOM (owner: `MasterData.Domain`)

`Modules/MasterData/MasterData.Domain/Entities/Unit.cs`, table `Unit`: just `Name?` (nullable, no
`[Required]`, 2-50 chars). No `UnitOfMeasureCategory`/dimension grouping (Quantity/Weight/
Volume/Length/Time) — every `Unit` row is a flat, untyped label. Global conversion (`ProductUnit`
above) is the only conversion mechanism; there is no global `Unit A -> Unit B` conversion table
independent of a Product.

Every transactional line entity across the whole solution carries its own `UnitId` (real EF nav to
`MasterData.Unit`, **not** a snapshot): `TransactionProduct`, `InventoryProduct`,
`PurchaseOrderProduct`, `PurchaseRequisitionProduct`, `InvoiceProduct`. `SalesOrderLine` and
`QuotationLine` (new Sales aggregates, Domain-only so far) hold `UnitId` as a scalar with no
navigation, consistent with the "reference other bounded contexts by ID only" convention.

### 1.4 Brand — does not exist

Confirmed zero matches repo-wide (grep across all `*.cs`, all docs). No `Brand`, `Manufacturer`, or
equivalent concept anywhere. This is genuinely new scope.

### 1.5 Barcode / SKU — does not exist as a first-class concept

`Product.Barcode` is a single nullable `string` column — no multi-barcode-per-product table, no
uniqueness constraint today (no unique index on `Barcode` in `OrgContextModelSnapshot.cs`). "SKU"
as a named concept does not exist anywhere; `Product.Code`/`CodeNumber` (inherited from
`BaseModel`) fills that role today.

### 1.6 Attributes — exists, unexposed

`Property`/`PropertyElement`/`ProductPropertyElement` (§1.1 table) already implement exactly the
"AttributeDefinition / AttributeOption / product-attribute-assignment" shape the brief describes in
Phase 7 — but there is **no Application-layer CQRS, no controller, no Angular screen** for any of
the three; `ProductPropertyElement` rows can only be created today through whatever internal
mapping the Product save flow does (needs confirming against `Inventory.Application/Products/`
commands before Phase 7 implementation — not fully traced in this pass). No `DataType`
(Text/Number/Boolean/Date/Selection/MultiSelection) exists; every `Property` is implicitly a
selection list. No `IsVariantDefining` flag exists — there is **no product-variant concept** at
all (no `ProductVariant` entity, no variant SKU/barcode).

### 1.7 Price / PriceList — does not exist beyond two flat columns

`Product.Price` (selling) and `Product.Cost` (cost) are the *only* pricing data anywhere in OrgSys.
No `PriceList`, `PriceListEntry`, `PriceLevel`, quantity breaks, currency-specific pricing, or
validity dates exist anywhere (grep-confirmed, zero hits). This is genuinely new scope, exactly as
the brief anticipates in Phase 10.

### 1.8 Services — not a distinct concept

No `Service`/`ProductType` distinction exists. A "service" today would just be a `Product` row with
no stock movements ever recorded against it — nothing in the domain model prevents or represents
this explicitly.

---

## 2. Consumers and current dependency shape

| Consumer | How it references Product/Unit/Classification | Evidence |
|---|---|---|
| Sales (legacy `Order`/`Invoice`, now superseded by CommercialDocuments for Invoice) | `InvoiceProduct.ProductId` — scalar FK only, navigation deliberately dropped | `Modules/CommercialDocuments/CommercialDocuments.Domain/Entities/InvoiceProduct.cs` |
| Sales (new `SalesOrderLine`/`QuotationLine`, Domain-only, not yet wired to API) | `ProductId` scalar + a snapshotted `ProductName` string at line-add time — already following the brief's "historical snapshot" pattern without being told to | `Modules/Sales/Sales.Domain/Entities/SalesOrderLine.cs`, `QuotationLine.cs` |
| Purchasing | `PurchaseOrderProduct.ProductId`, `PurchaseRequisitionProduct.ProductId` — scalar FK; `.Unit` navigation kept (documented Architecture.Tests exception: `("Purchasing", "MasterData", "...Unit navigation, same convention as CommercialDocuments.Domain.InvoiceProduct")`) | `Modules/Purchasing/Purchasing.Domain/Entities/*.cs`, `Tests/Architecture.Tests/ModuleDependencyTests.cs:55` |
| CommercialDocuments | `InvoiceProduct.ProductId` scalar; no `Product` navigation | as above |
| Reporting | Reads `Product`/`Classification`/`TransactionProduct.Product` **directly via EF `Include`** — a documented, accepted, by-design exception (`Reporting` has no `Reporting.Domain`, it's a read-only cross-module aggregator) | `Modules/Reporting/Reporting.Application/Warehouse/Queries/GetMovementReportQueryHandler.cs`, `GetBalanceReportQueryHandler.cs` |
| Cross-module lookup (the one correct existing pattern) | `Inventory.Contracts/Products/GetProductNamesQuery.cs` — batch id→name query, used by Sales instead of a direct navigation | `Modules/Inventory/Inventory.Contracts/Products/` |

No module physically duplicates Product/Category/Unit data — every consumer goes through an ID
(with the single, by-design Reporting exception). There is nothing to de-duplicate; the work here
is **relocation + genuine new capability**, not consolidation of parallel models.

---

## 3. Current tables (from `OrgContextModelSnapshot.cs` / entity `[Table]` attributes)

`Product, ProductUnit, Property, PropertyElement, ProductPropertyElement, ProductRecipe,
Classification, Unit` — all in the single shared database, default `dbo` schema (no SQL-schema
separation anywhere in this codebase; `OrgContext`'s `Schema` property is explicitly commented
out). Single shared `OrgContext : DbContext, IOrgContext`
(`BuildingBlocks/OrgSys.DatabaseMigrator/Persistence/OrgContext.cs`) declares one `DbSet<T>` per
entity across **every** module — there are no per-module DbContexts to create for Catalog either.
Migrations live in one shared folder (`BuildingBlocks/OrgSys.DatabaseMigrator/Migrations/`), one
`__MigrationsHistory` table, one connection string (`OrgConnection` — **confirmed a live remote
database per this session's own project memory; `dotnet ef database update` must never run without
fresh explicit user consent**, regardless of anything this report or later phases say).

---

## 4. Established architectural conventions (from Payables/Receivables/Parties/CommercialDocuments — the templates to copy)

Full detail gathered from direct inspection; summarized here, to be applied without modification in
Phase 1+:

1. **Project layout**: exactly 4 csproj per module — `<Module>.Domain`, `.Application`,
   `.Infrastructure`, `.Contracts`. No exceptions among the 13 real modules.
2. **No strongly-typed IDs anywhere** — every entity uses plain `long Id` from `BaseModel`. Do not
   introduce `ProductId`/`CategoryId` record structs for Catalog; it would be inconsistent with
   every other module built so far (rule #19).
3. **Entity base classes**: `BaseModel` (master data) or `MovementModel : BaseModel` (transactional
   documents with Create/Modify user+date, Branch, Shift). Catalog's Product/Category/Brand/Unit
   are master data → `BaseModel`. No `AuditableEntity`, no automatic audit-field population
   (SharedKernel has no `SaveChanges` interceptor) — audit fields, where present, are passed
   explicitly into factory methods by command handlers.
4. **No `IEntityTypeConfiguration<T>` anywhere** — zero hits repo-wide. All EF Fluent config lives
   inline in `OrgContext.OnModelCreating`, with a dedicated private static helper per recent
   effort (e.g. `ConfigureInventoryHardening(modelBuilder)`). A `ConfigureCatalog(modelBuilder)`
   helper is the established shape to follow, not per-entity config classes.
5. **No SQL schema separation** — `catalog.Products` etc. would be inconsistent; tables stay in
   `dbo`, physical table names set via `[Table("...")]` on the entity.
6. **Repositories**: per-aggregate interfaces in `<Module>.Domain/Repositories/I<Entity>Repository`,
   implemented in `<Module>.Infrastructure/Persistence/` as thin wrappers composed over the
   existing generic `OrgSys.SharedKernel.IRepository<TEntity>` (constructor-injected) — not a
   from-scratch EF implementation, and not one repository per child entity (no
   `IProductBarcodeRepository`, no `IPriceListEntryRepository`).
7. **CQRS**: MediatR command/query records + DTOs in `<Module>.Contracts/<Feature>/`; handlers only
   in `<Module>.Application/<Feature>/{Commands,Queries}/`, returning `Result<T>`
   (`SharedKernel.Result` — `HttpStatusCode + payload + List<Error>?`), not exceptions, not a
   `Result<T>` return from Domain factory methods (Domain throws `<Entity>DomainException`
   subclasses instead).
8. **Domain events**: `IDomainEvent` marker (SharedKernel); no shared `AggregateRoot` base is used
   by `MovementModel`-derived types (can't multiply-inherit) — instead every new aggregate
   hand-rolls the identical `_domainEvents` list / `DomainEvents` / `ClearDomainEvents()` /
   `protected void Raise(...)` shape inline. Catalog's `Product` (master data, not
   `MovementModel`) **can** use the real `AggregateRoot` base directly since it has no competing
   base-class need — confirm this is fine or match the hand-rolled shape for consistency; either is
   defensible, hand-rolled matches more precedent.
9. **Integration events**: `BuildingBlocks/OrgSys.EventBus` (`IIntegrationEvent`,
   `IIntegrationEventPublisher`, MediatR-backed, in-process, no outbox). Inbound handlers live in
   `<Module>.Application/<Feature>/Integration/*IntegrationEventHandler.cs`.
10. **DI wiring**: `<Module>.Infrastructure/DependencyInjection/ServiceCollectionExtensions.cs`
    exposing `Add<Module>Module(this IServiceCollection)` — registers MediatR from the Application
    assembly, FluentValidation validators from the Application assembly, AutoMapper profile,
    per-aggregate repositories. No module registers its own DbContext.
11. **Multi-tenancy**: **does not exist anywhere in this codebase** — no `TenantId`/`CompanyId`/
    `OrganizationId`, no global query filters in `OrgContext`. Catalog must **not** invent tenant
    columns/uniqueness scoping; treat the brief's "TenantId + ProductCode" uniqueness guidance as
    inapplicable to this codebase — uniqueness should just be `ProductCode` unique, full stop
    (`BranchId` exists on `MovementModel` for a different purpose — operational branch, not tenant
    — and Product/Category/Unit/Brand are `BaseModel`, not `MovementModel`, so they don't even have
    it).
12. **Architecture tests are real and enforced**: `Tests/Architecture.Tests/ModuleDependencyTests.cs`
    and `ModuleLayerDependencyTests.cs` (NetArchTest + xUnit) enumerate every module's
    Domain/Application assembly in static arrays and assert no cross-module Domain→Domain,
    Domain→Infrastructure, Application→other-Domain, Application→other-Infrastructure references
    exist, except entries in explicit, reasoned `AcceptedDomainExceptions`/
    `AcceptedApplicationDomainExceptions` allow-lists. **Catalog must be added to `ModuleDomains` in
    both files** (and to `ModuleApplications` in `ModuleLayerDependencyTests.cs`) or these tests
    silently skip checking it.
13. **API controllers**: `API/Controllers/Org/<Area>/`, area names are legacy groupings not 1:1 with
    module names (e.g. `Setting/ProductController.cs`, `Setting/ClassificationController.cs`,
    `Setting/UnitController.cs` — all three already exist and would need to be reconciled with new
    Catalog controllers). The newest bounded contexts (Sales, Payables, Receivables, Advances) have
    **no controllers yet** — Catalog can establish `API/Controllers/Org/Catalog/` as the
    first "new-style" area name, or follow `Setting/` precedent; recommend the former since Payables/
    Receivables will need the same treatment eventually and Catalog can set the pattern.
14. **Best full-stack templates**: **Payables and Receivables** (`89f66c01`, `58630b4e`) for
    Domain+Contracts+Application+Infrastructure+DI end-to-end (Payables' own code explicitly says
    "mirrors Receivables exactly"). **Parties and CommercialDocuments** for the specific
    "physically relocate an existing entity out of another module, preserve `[Table(...)]`, verify
    zero-diff migration" technique this task needs for `Product`/`Classification`/`Unit`.

---

## 5. Legacy / non-modular code

There is **no duplicate legacy Item/Product implementation**. The top-level legacy `OrgSys/`
project's `ProductController.cs` (`OrgSys/Areas/Setting/Controllers/ProductController.cs`) is a
thin legacy MVC UI that directly calls `Inventory.Application.Products` commands/DTOs — it is a
consumer, not a second data model. Legacy root `Domain/`/`Application/` projects have empty
`Entity`/product-related folders for this concept. The only "legacy" angle is that `Product`,
`Classification`, and `Unit` currently live in the *wrong* modules relative to this task's target
(Inventory and MasterData respectively) rather than in a dedicated Catalog module — this is a
relocation task, not a legacy-cleanup task.

---

## 6. Angular frontend — current state

One real CRUD feature exists: `OrgSys.Angular/src/app/features/administration/products/` (list +
reactive form with a `FormArray` for multi-unit `ProductUnits`, calling `Product`
create/update/search/delete + a custom `GetMax` endpoint). `Category` (backend `Classification`)
and `Unit` are **lookup-only stubs** living inside unrelated features (`reports/`,
`invoices/` lookup services) specifically because "no dedicated Angular admin screen exists yet" —
confirmed by an explicit comment in the Angular source itself. `Brand`, `SKU`, `Attribute`,
`PriceList` have zero Angular presence — genuinely new frontend work. Feature-folder convention
(routes/models/services/pages/{list,form}) is consistent and documented in §4 of the companion
Angular-discovery findings; template/UI stack is Bootstrap 4 + a custom "Dore" admin template
(not PrimeNG/Material/Tailwind) — must not be changed.

`menu.config.ts` currently has one relevant entry: `Products` → `/administration/products`
(permission keys `Products.All,Products.View`). No `Category`/`Unit`/`Catalog`/`Brand`/`PriceList`
menu entries exist.

---

## 7. Duplications found

**None.** Every concept in scope has exactly one physical owner today. This materially changes the
"legacy data migration" phase from the brief's assumption (consolidating N parallel tables into
one) to a simpler shape: **relocate 8 existing tables into a new module, unchanged**, then build
net-new tables (`Brand`, `PriceList`, `PriceListEntry`, `ProductBarcode` if multi-barcode is
wanted) alongside them.

---

## 8. Migration risks

1. **Live remote database.** `OrgConnection` is a live remote DB (per this session's persistent
   project memory). Any `dotnet ef migrations add` is safe to run and inspect; `dotnet ef database
   update` must **never** run without fresh, explicit user consent in this exact session, regardless
   of how many prior migrations in this task were approved.
2. **Blast radius of relocating `Product`/`Classification`/`Unit`**: every consumer listed in §2
   needs its project reference and `using`s changed from `Inventory.Domain`/`MasterData.Domain` to
   `Catalog.Contracts` (or kept as a documented `AcceptedDomainExceptions` entry where an EF
   navigation is kept in-place, mirroring how Purchasing/Treasury/CommercialDocuments kept
   navigations to the relocated `Dealer`/`Invoice`). Reporting's direct `Include(p => p.Product)`
   style reads are the biggest volume of touch points and are an *accepted* exception pattern
   already, not a violation to fix.
3. **In-flight Inventory hardening work.** The working tree has substantial uncommitted Inventory
   work (Balances/Receipts/Issues/Transfers/Adjustments/Reservations/Serials/WarehouseLocations,
   an unapplied `AddInventoryHardening` migration). None of it touches `Product`'s own shape, but
   several of its new entities (`InventoryBalance`, `InventoryReceiptLine`, etc.) hold `ProductId`
   FKs that will need the same relocation treatment as the older Inventory entities. This report
   does not modify any of that WIP.
4. **`ProductPropertyElement`/`Property`/`PropertyElement` currently live in `Inventory.Domain`**,
   not `MasterData.Domain` — they need the same relocation as `Product` itself since they're
   Product's own attribute-assignment mechanism, not general reference data.
5. **No existing unique index on `Barcode`** — adding one during relocation could fail against live
   data if duplicate barcodes already exist; must be verified against actual data before adding a
   `IsUnique()` constraint (the brief's Phase 3 "Barcode unique where applicable" invariant needs a
   data check, not just a migration).
6. **`Classification.Name` currently has a `[StringLength(50, MinimumLength = 3)]` non-nullable
   constraint with no explicit `[Required]`** but is used as a required FK from Product — worth
   preserving exactly during relocation rather than "fixing" opportunistically (rule: relocate
   unchanged first, improve later, matching the Parties/CommercialDocuments playbook).

---

## 9. Proposed target ownership (input to Phase 1's formal ownership matrix)

| Data | Current Owner | Proposed Owner | Action |
|---|---|---|---|
| `Product` (+ `ProductType` — new) | Inventory.Domain | **Catalog** | Relocate, add `ProductType` enum |
| `ProductUnit` | Inventory.Domain | **Catalog** | Relocate unchanged |
| `Property`/`PropertyElement`/`ProductPropertyElement` | Inventory.Domain | **Catalog** | Relocate, enrich with `DataType`/`IsVariantDefining` |
| `Classification` (→ `Category`?) | MasterData.Domain | **Catalog** | Relocate; evaluate rename vs keep (Phase 1 decision — MasterData's own docs call `Classification` reference data, but it's 100%-exclusively a Product concept, unlike `Country`/`Currency`) |
| `Unit` | MasterData.Domain | **Catalog** | Relocate — same reasoning as Classification; every other MasterData entity (`Country/City/District/Currency/PaymentType/ReferenceType`) is genuinely cross-domain reference data, `Unit` is not |
| `ProductRecipe` | Inventory.Domain | **stays Inventory** | It's a BOM/costing composition concept, not Catalog's concern (brief's own PHASE 29 boundary) |
| `Brand` | — (new) | **Catalog** | New |
| `PriceList`/`PriceListEntry` | — (new) | **Catalog** | New |
| Multi-barcode (if pursued beyond the existing single `Product.Barcode` column) | — (new) | **Catalog** | New, optional — needs a justification check against actual business need in Phase 1, not assumed |
| Stock/Warehouse/Batch/Serial/Costing | Inventory.Domain | **stays Inventory** | Unchanged |
| Supplier/Customer (`Dealer`) | Parties.Domain | **stays Parties** | Unchanged |
| Sales Order/Quotation | Sales.Domain | **stays Sales** | Unchanged |
| Invoice | CommercialDocuments.Domain | **stays CommercialDocuments** | Unchanged |

This table is provisional input to the formal Phase 1 ownership matrix
(`docs/catalog/catalog-ownership.md`) and target architecture doc
(`docs/catalog/catalog-target-architecture.md`), not yet a final decision.

---

## 10. Open questions carried into Phase 1 (not blocking, but must be resolved there)

1. Rename `Classification` → `Category` (matches this task's ubiquitous language) or keep the name
   physically (`[Table("Classification")]`) and only rename in Catalog's own C# type — precedent
   (`Parties` kept `[Table("Dealer")]` while giving it a new home) favors **keep the physical name,
   consider a new class name only if it aids clarity** — needs a one-line decision in Phase 1, not
   a re-litigation.
2. Whether `Unit`/`Classification` moving out of `MasterData.Domain` leaves `MasterData` with a
   thin-enough remainder to keep its own name, or whether this accelerates the already-proposed
   `MasterData → ReferenceData` rename from the earlier review — out of scope to decide here,
   flagged for Phase 1.
3. Multi-barcode-per-product and full `ProductVariant` are the two biggest net-new-complexity items
   in the brief; Phase 1/3 must explicitly evaluate real business need before building them (the
   brief itself says "before implementing variants, inspect whether OrgSys actually needs them" —
   today, nothing in OrgSys needs them; recommend deferring `ProductVariant` unless the user
   confirms real demand, per rule #2 "do not create another generic CRUD architecture" /
   over-engineering caution baked into this task's own brief).
