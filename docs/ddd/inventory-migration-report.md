# Inventory / Warehouse — Migration Report

Session started 2026-09-14, continued 2026-09-15. Covers Phases 1-7 (discovery through the core
receipt/issue/transfer/adjustment/reservation posting pipeline), plus Batch/Serial CQRS, the
read-model/query layer, API controllers, and a first slice of Angular UI. See
`docs/ddd/inventory-current-state.md` (Phase 1) and `docs/ddd/inventory-target-architecture.md`
(Phase 2) for the discovery and design work this builds on.

## A. Before

Inventory was already a DDD-shaped module (`Modules/Inventory/{Domain,Application,Contracts,
Infrastructure}`) but tactically anemic: `Product`/`Stock`/`Transaction`/`Inventory` were plain
AutoMapper-mapped data bags with no invariants; stock movement direction was decided by scattered
`TypeId == n` magic numbers; there was no materialized balance (every balance query summed
`TransactionProduct` from scratch); no reservations, batch/lot, serial, or FIFO costing existed;
no optimistic concurrency existed anywhere in OrgSys; and `Tests/Inventory.Domain.Tests` did not
exist. Full detail in `docs/ddd/inventory-current-state.md`.

## B. After

- A tactically-hardened Domain layer following the same pattern as `Advances.Domain.Custody` /
  `Payables.Domain.Payable` (factory methods, private setters, domain events, sealed exception
  hierarchy) for every new aggregate, while the *existing* `Product`/`Stock`/`Transaction` entities
  keep their original AutoMapper-CRUD shape (only additive fields/methods), so nothing existing
  regresses.
- `InventoryBalance` is now the materialized, concurrency-safe current-state projection
  (`RowVersion` + a repository that returns a tracked entity rather than the generic
  `IRepository<T>.UpdateAsync`'s detached-overwrite idiom, which would have silently defeated
  optimistic concurrency).
- One canonical posting pipeline (`InventoryLedgerPoster`) turns every new document
  (Receipt/Issue/Transfer/Adjustment) into `Transaction`/`TransactionProduct` ledger rows +
  `InventoryBalance` updates, reusing the *existing* `TransactionJournalPostingService` unchanged
  for GL integration — no new accounting code was needed.
- Full CQRS command/query surface, API controllers, and a first Angular UI slice (Warehouse
  Locations, Inventory Balance, Stock Reservations) — see §C.

## C. Files Added (highlights)

**Domain** (`Modules/Inventory/Inventory.Domain/`): `Enums/{MovementType,TrackingType,
CostingMethod,DocumentStatus,StockTransferStatus,ReservationStatus,BatchStatus,SerialStatus}.cs`,
`Enums/SourceDocumentType.cs`; `Entities/{WarehouseLocation,InventoryBalance,InventoryReceipt(+Line),
InventoryIssue(+Line),StockTransfer(+Line),StockAdjustment(+Line),StockAdjustmentReason,
StockReservation,InventoryBatch,InventorySerial,InventoryCostLayer}.cs`;
`Exceptions/InventoryDomainException.cs` (24 sealed exception types); `Events/*.cs` (11 domain
events); `Repositories/IInventoryBalanceRepository.cs`.

**Tests**: `Tests/Inventory.Domain.Tests/` (new project, 66 tests — did not exist before).

**Application** (`Modules/Inventory/Inventory.Application/`): `Postings/{InventoryLedgerPoster,
StockReservationService}.cs`; full CQRS folders `InventoryReceipts/`, `InventoryIssues/`,
`StockTransfers/`, `StockAdjustments/`, `StockAdjustmentReasons/`, `StockReservations/`,
`InventoryBatches/`, `InventorySerials/`, `InventoryBalances/`, `StockCard/`, `WarehouseLocations/`,
`Availability/` (Commands + Queries + DTOs + MappingProfile per folder).

**Contracts** (`Modules/Inventory/Inventory.Contracts/Availability/`):
`GetInventoryAvailabilityQuery`, `ReserveInventoryCommand` (with a Contracts-local
`ReservationSourceType` enum, deliberately not reusing the Domain enum — see brief §43).

**Infrastructure**: `Persistence/InventoryBalanceRepository.cs`.

**EF Migration**: `BuildingBlocks/OrgSys.DatabaseMigrator/Migrations/
20260914111400_AddInventoryHardening.cs` — additive only (new tables + new nullable/defaulted
columns on `Transaction`/`Stock`/`Product`). Generated, reviewed by hand, and corrected twice
before acceptance (see §H).

**API** (`API/Controllers/Org/Inventory/`): `InventoryReceiptsController`,
`InventoryIssuesController`, `StockTransfersController`, `StockAdjustmentsController`,
`StockReservationsController`, `InventoryBatchesController`, `InventorySerialsController`,
`InventoryBalancesController`, `WarehouseLocationsController`, `StockAdjustmentReasonsController`.

**Angular** (`OrgSys.Angular/src/app/features/warehouse/`): `models/warehouse.model.ts` (every new
DTO/enum, including for screens not yet built — see §H); `services/{warehouse-location,
stock-balance,stock-reservation}.service.ts`; `pages/{locations-list,locations-form,balance,
reservations}/`; `warehouse.routes.ts`. Wired into `app.routes.ts` (`/warehouse`) and
`core/config/menu.config.ts` (new "Warehouse" menu group, alongside the existing "Inventory" group
— not replacing it).

## D. Files Modified (all additive — nothing removed)

`Product.cs` (+`TrackingType`, `CostingMethod`, `ExpiryTracking`, `IsActive`), `Stock.cs`
(+`AllowNegativeStock`, `DefaultReceivingLocationId`, `DefaultIssueLocationId`, `IsActive`,
`Create`/`EnsureActiveForPosting`/`Activate`/`Deactivate`), `Transaction.cs` (+`SourceDocumentType`,
`SourceDocumentId`, `SourceDocumentLineId`, `IdempotencyKey`, `ReversalOfMovementId`,
`CreateReversal`), `OrgContext.cs` (new DbSets + `ConfigureInventoryHardening`),
`CreateTransactionByInvoiceCommandHandler.cs` (now syncs `InventoryBalance` on first-time
transaction creation — see the known gap in §G), `Inventory.Application.csproj` (added
`Microsoft.EntityFrameworkCore` package reference, for the reservation retry loop's
`DbUpdateConcurrencyException` catch), `GlobalUsings.cs` (both Domain and Application).

## E. Files Removed

None. Per the decisions in `docs/ddd/inventory-target-architecture.md` §2 and the project owner's
own instruction, the legacy Razor MVC inventory screens and the existing `Product`/`Stock`/
`Transaction`/`Inventory` entities were kept in place. `docs/ddd/inventory-current-state.md` §1
already found the root legacy `Domain`/`Application` projects empty (0 files) — a prior pass had
already fully extracted Inventory's legacy code, so there was nothing left to remove this pass.

## F. Legacy Mapping

| Old concept | New concept |
|---|---|
| `Transaction`/`TransactionProduct` (kept) | `InventoryMovement` (ledger — unchanged identity, hardened with `MovementType`, `SourceDocumentType`/`Id`/`LineId`, `IdempotencyKey`, `ReversalOfMovementId`) |
| `Stock` (kept) | `Warehouse` aggregate (same class/table, hardened) |
| `Inventory`+`InventoryProduct` (kept) | `StockCount` concept (same class/table, workflow unchanged) |
| *(none existed)* | `InventoryBalance` — new materialized projection |
| *(none existed)* | `InventoryReceipt`/`InventoryIssue`/`StockTransfer`/`StockAdjustment` — new standalone documents, coexisting with the existing raw-`Transaction`-via-Invoice path |
| *(none existed)* | `StockReservation`, `InventoryBatch`, `InventorySerial`, `InventoryCostLayer` |

## G. Integration Map

```
CommercialDocuments (Invoice) --CreateTransactionByInvoiceCommand--> Inventory.Contracts
Inventory (Receipt/Issue/Transfer/Adjustment) --InventoryLedgerPoster--> Transaction/TransactionProduct + InventoryBalance
Inventory --TransactionJournalPostingService (unchanged)--> Accounting.Contracts
Inventory.Contracts.Availability (GetInventoryAvailabilityQuery / ReserveInventoryCommand) --> exposed for Sales/Purchasing, not yet called by either (see §H)
```

No new `Domain -> Domain` cross-module exceptions were introduced (`Tests/Architecture.Tests` —
1213 tests, all still passing, including every `AcceptedDomainExceptions` assertion).

## H. Remaining Technical Debt / Explicitly Deferred Work

1. **`CreateTransactionByInvoiceCommandHandler`'s edit-resync branch** (re-invoicing an
   already-linked transaction) does not update `InventoryBalance` — see the doc comment on that
   class. First-time creation is covered; correcting an edit needs a dedicated delta-reversal pass.
2. **Sales/Purchasing don't call the new availability/reservation contracts yet** — `Sales.Domain`
   has no wired Order/Quotation confirmation flow today (`docs/modular-monolith-target-architecture.md`
   already notes Order is "future — currently unwired"), so there is nothing live to hook
   `ReserveInventoryCommand` into yet. The contract exists and is tested at the Inventory end;
   wiring a caller is follow-up work once Sales Order itself is built.
3. **FIFO costing service (`FifoCostingStrategy`) is not implemented as an orchestrating service** —
   `InventoryCostLayer.Consume` exists and is unit-tested, but nothing in `InventoryLedgerPoster`
   creates/consumes cost layers yet; every posting path currently uses weighted-average semantics
   (`InventoryBalance.Receive`/`IssueOut`) regardless of `Product.CostingMethod`. Wiring FIFO
   selection into the poster is scoped but not done.
4. **Migration not applied** — `AddInventoryHardening` is a local file only; the live
   `OrgConnection` database has not been touched. See [[feedback_db_migration_caution]] memory —
   needs a fresh, specific confirmation before running `dotnet ef database update`.
5. ~~No DB-backed concurrency integration test~~ **Closed** — `Tests/Inventory.Integration.Tests/`
   (new project, SQLite file-backed, never touches `OrgConnection`) proves the real
   `StockReservationService.ReserveAsync` cannot oversell `InventoryBalance` under a genuine
   two-context race: a direct EF-level conflict test, an end-to-end "two requests exceed Available"
   test (only one succeeds), and a "retry succeeds when stock remains" test. 3/3 passing.
6. **Angular UI**: Warehouse Locations, Inventory Balance, Stock Reservations, and Inventory
   Receipts (list + create-draft form + post/cancel) are built. Issues, Transfers, Adjustments,
   Batches, Serials, Stock Card, Aging, and Expiry screens are not — but every backend endpoint and
   the shared `warehouse.model.ts` DTOs already exist, so each remaining screen is a
   services-plus-component increment, not new plumbing. (Receipts needed `GetInventoryReceiptById/
   ListQuery` added to the backend first — the original Commands-only slice had no way to view or
   post a draft; the same gap exists for Issues/Transfers/Adjustments and needs the same fix before
   their screens can be built.)
7. **No permission gating on the new screens/menu items** — the new "Warehouse" menu group and its
   three screens don't check `HasPermissionDirective` (unlike every other menu entry), because the
   corresponding `Inventory.Warehouses.*`/`Inventory.Reservations.*` permissions (brief §53) were
   never seeded into Administration. Left open rather than gated on a permission that would make
   the screens invisible to everyone.
8. **Aging/valuation-by-date reports** (brief §40/§23 "Stock as of Date") not built — `GetStockCard`
   gives historically-correct running balance/value for one item; a cross-item "as of date" snapshot
   query is a natural extension of the same pattern, not yet written.

## I. Build / Test Result (as of last full run this session)

```
dotnet build OrgSys.sln  → Build succeeded, 0 Error(s)
dotnet test OrgSys.sln   → 1636 tests, 0 failed, 0 skipped
  Accounting.Domain.Tests        54 passed
  Payables.Domain.Tests          35 passed
  Advances.Domain.Tests          39 passed
  Receivables.Domain.Tests       35 passed
  Inventory.Domain.Tests         66 passed  (new)
  Inventory.Integration.Tests     3 passed  (new — SQLite-backed, real concurrency proof)
  Purchasing.Domain.Tests        45 passed
  Sales.Domain.Tests             53 passed
  Application.Tests              93 passed
  Architecture.Tests           1213 passed  (no new AcceptedDomainExceptions entries required)

ng build --configuration development → 0 errors; every new component (locations-list,
  locations-form, balance, reservations, receipts-list, receipts-form) confirmed as its own lazy
  chunk. Not visually verified post-login (no test credentials available this session).
```

`dotnet ef migrations has-pending-model-changes` → clean.
