# Catalog Bounded Context — Legacy Data Migration Plan

Companion to `catalog-current-state.md` and `catalog-target-architecture.md`. Since discovery found
**no duplicate Product/Item tables anywhere** (§7 of the current-state report), this is not a
data-consolidation migration — it is a **project/namespace relocation of 7 existing tables**
(zero schema diff) plus **3 net-new tables** and **3 new columns** on existing tables.

## 1. Relocated entities (zero column/table change — namespace move only)

| Old | New | Table | Column mapping | Data transformation | FK impact | Order |
|---|---|---|---|---|---|---|
| `Inventory.Domain.Product` | `Catalog.Domain.Product` | `Product` (unchanged) | 1:1, unchanged | None. Existing rows keep all values as-is. | Every FK *to* `Product.Id` (from `TransactionProduct`, `InventoryProduct`, `PurchaseOrderProduct`, `PurchaseRequisitionProduct`, `InvoiceProduct`, `ProductRecipe`, and new Inventory-hardening tables) is untouched — same `Id` values, same table, only the owning C# project changes | 1 |
| `Inventory.Domain.ProductUnit` | `Catalog.Domain.ProductUnit` | `ProductUnit` (unchanged) | 1:1, unchanged | None | FK to `Product.Id`/`Unit.Id` unchanged | 1 |
| `Inventory.Domain.Property` | `Catalog.Domain.Property` | `Property` (unchanged) | 1:1, unchanged, **plus new nullable-with-default `DataType` column** (see §2) | New column defaults to `Selection` (0) for every existing row — preserves current implicit behavior exactly | none | 2 (bundled with §2, same migration) |
| `Inventory.Domain.PropertyElement` | `Catalog.Domain.PropertyElement` | `PropertyElement` (unchanged) | 1:1, unchanged | None | FK to `Property.Id` unchanged | 1 |
| `Inventory.Domain.ProductPropertyElement` | `Catalog.Domain.ProductPropertyElement` | `ProductPropertyElement` (unchanged) | 1:1, unchanged | None | FK to `Product.Id`/`Property.Id`/`PropertyElement.Id` unchanged | 1 |
| `MasterData.Domain.Classification` | `Catalog.Domain.Classification` (C# name kept — see target-architecture §1) | `Classification` (unchanged) | 1:1, unchanged | None | FK from `Product.ClassificationId` unchanged | 1 |
| `MasterData.Domain.Unit` | `Catalog.Domain.Unit` (C# name kept) | `Unit` (unchanged) | 1:1, unchanged | None | FK from `ProductUnit.UnitId`, `TransactionProduct.UnitId`, `InventoryProduct.UnitId`, `PurchaseOrderProduct.UnitId`, `PurchaseRequisitionProduct.UnitId`, `InvoiceProduct.UnitId` unchanged | 1 |

**`ProductRecipe` stays in `Inventory.Domain` — not relocated** (see `catalog-ownership.md`;
BOM/costing composition, not product definition).

## 2. New columns on relocated entities (second migration, `AddCatalogModule`)

| Table | New column | Type | Default | Backfill rule |
|---|---|---|---|---|
| `Product` | `ProductType` | `int` (enum: StockItem=0, NonStockItem=1, Service=2) | `0` (StockItem) at the DB level, but backfilled per-row by a data migration step, not left at the blanket default | `TrackingType <> 0 (None) → StockItem (0)`; `TrackingType = 0 (None) → NonStockItem (1)`. No existing row is auto-classified `Service (2)` — nothing in current data distinguishes a service from a untracked non-stock item, so this must never be guessed |
| `Product` | `BrandId` | `long`, nullable | `NULL` | All existing rows get `NULL` (no brand) — matches brief's Phase 28 "No Brand" edge case exactly |
| `Property` | `DataType` | `int` (enum, `Selection=4` matching the enum's actual ordinal — see target-architecture §2) | `4` (Selection) | Every existing row — preserves current implicit all-selection behavior |

## 3. Net-new tables (same migration as §2)

| Table | Purpose | Seeded data |
|---|---|---|
| `Brand` | New Catalog aggregate | None — empty until users create brands |
| `PriceList` | New Catalog aggregate | None required; optionally seed one `IsDefault = true` list if the Application layer's price-resolution fallback needs a concrete row to point `Product.Price` reads at — decide during Step 10 implementation, not a hard requirement since `Product.Price` remains a valid fallback with no `PriceList` at all |
| `PriceListEntry` | Child of `PriceList` | None |

## 4. Migration order (both EF migrations, generated but **not applied** without explicit consent)

1. **`RelocateCatalogEntities`** — moves the 7 existing tables' owning C# types into
   `Catalog.Domain`; `OrgContext.OnModelCreating` config for them moves into a new
   `ConfigureCatalog(modelBuilder)` helper. Verified as a zero-diff migration (empty `Up()`/`Down()`
   beyond harmless CLR-type metadata) before proceeding — see target-architecture §5 step 4.
2. **`AddCatalogModule`** — adds `Product.ProductType`, `Product.BrandId` (+ FK), `Property.DataType`,
   and the three new tables (`Brand`, `PriceList`, `PriceListEntry`) with their indexes.
3. A one-time **data backfill** for `Product.ProductType` (per §2's rule) — implemented as either
   (a) raw SQL in the migration's `Up()` (`UPDATE Product SET ProductType = CASE WHEN TrackingType
   <> 0 THEN 0 ELSE 1 END`), matching how this codebase already does backfills in prior migrations
   (confirms with the existing `AddInventoryHardening` migration's own pattern before writing), or
   (b) a one-off Application-layer console step if raw SQL in a migration isn't this repo's
   convention — confirm against precedent before implementing, don't invent a new convention.
4. Consumer code changes (project references, `using`s, Architecture.Tests exception-list updates
   — see `catalog-target-architecture.md` §4) happen in the same PR/session as migration #1, since
   the code won't compile with the old references removed until the relocation is complete — this
   is not a separately-sequenced step, it's the same atomic change as #1.
5. Neither migration touches the live `OrgConnection` database (`dotnet ef database update`) without
   fresh, explicit user consent in this session, regardless of any earlier approval.

## 5. IDs preserved

Every relocated table keeps its existing primary keys, `Id` values, and every foreign key pointing
at it — this is a C#-project/namespace move plus two additive schema changes, not a data migration
in the traditional sense (no `INSERT ... SELECT` from an old table into a new one is needed
anywhere, because there is no old/new table pair — there is only one table, whose owning code
moves).

## 6. Rollback

`Down()` on both migrations is the standard EF-generated inverse (drop new columns/tables for
migration 2; migration 1's `Down()` should also be verified empty). If migration 1's relocation
needs to be reverted, the C# code revert (moving files back to `Inventory.Domain`/
`MasterData.Domain`) is the primary rollback mechanism — the DB schema never changes for it, so
there is nothing to roll back at the data layer for that step alone.
