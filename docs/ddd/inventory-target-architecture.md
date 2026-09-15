# Inventory / Warehouse — Target Architecture (Phase 2)

Scope decided with the project owner: full brief scope (Locations, Batch/Lot, Serial, FIFO,
reservations with concurrency safety, full report suite). UI: Angular gets all new screens; the
legacy Razor MVC inventory screens are untouched. This document is the blueprint the remaining
phases (3-15) implement against. See `docs/ddd/inventory-current-state.md` for the Phase 1
findings this builds on.

## 1. Bounded context boundary

Inventory owns: item inventory policy (tracking type), warehouses, warehouse locations, stock
availability, the stock ledger, receipts, issues, transfers, adjustments, reservations, costing,
batch/lot tracking, serial tracking, inventory valuation, inventory accounting facts, and inventory
inquiries. It is the **only** writer of stock quantity. Sales/Purchasing/CommercialDocuments reach
it only through `Inventory.Contracts`.

**Item ownership** (brief §3): already resolved by a prior pass — `Product` stays owned by
`Inventory.Domain` (`docs/module-ownership.md`, `docs/modular-monolith-target-architecture.md:519`
explicitly rejected a separate Catalog module: no second owner, no ProductCategory/PriceList to
justify one). Not revisited here.

## 2. Ubiquitous language — old name → DDD concept

To avoid a high-risk blanket rename across EF tables, the legacy Razor UI, the API, and Angular
(all of which use "Stock"/"Product"/"Inventory" today), **C# class and table names are kept**
where a rename would only be cosmetic. The bounded-context vocabulary is documented here instead:

| Existing class/table | DDD concept | Action |
|---|---|---|
| `Stock` | **Warehouse** aggregate root | Kept as `Stock` (class + table); hardened with invariants |
| `Transaction` + `TransactionProduct` | **InventoryMovement** — canonical ledger | Kept as `Transaction`/`TransactionProduct`; hardened |
| `Inventory` + `InventoryProduct` | **StockCount** (physical count document) | Kept as `Inventory`/`InventoryProduct` — renaming this would collide with the *new* `Inventory.Domain` `InventoryReceipt`/`InventoryIssue` naming below, so it stays as-is; XML-doc comments clarify the concept |
| `Product` | **Item** (inventory-specific policy owner) | Kept as `Product`; gains `TrackingType`, `ExpiryTracking` |
| `TransactionType` | Seeded lookup, becomes backed by `MovementType` value object | Kept as reference table; a `MovementType` enum/VO wraps its `Id` in domain code, replacing magic numbers |

New concepts get new, brief-aligned names since there's no existing convention to collide with:
`WarehouseLocation`, `InventoryBalance`, `InventoryReceipt`/`InventoryReceiptLine`,
`InventoryIssue`/`InventoryIssueLine`, `StockTransfer`/`StockTransferLine`,
`StockAdjustment`/`StockAdjustmentLine`/`StockAdjustmentReason`, `StockReservation`,
`InventoryBatch`, `InventorySerial`, `InventoryCostLayer`.

## 3. Aggregate roots

| Aggregate | Root entity | Owns | Invariants |
|---|---|---|---|
| Warehouse | `Stock` | — | Code unique per company; inactive warehouse rejects new transactional documents; cannot delete with stock or posted movements (deactivate instead) |
| WarehouseLocation | `WarehouseLocation` (new) | — | belongs to exactly one `Stock`; optional `ParentLocationId` hierarchy; `IsReceivable`/`IsPickable` gate document line assignment |
| Item | `Product` | `ProductUnit`, `ProductRecipe`, `ProductPropertyElement` | Base unit required; `TrackingType` (`None`/`Batch`/`Serial`/`BatchAndSerial`) fixed once movements exist |
| InventoryReceipt (new) | `InventoryReceipt` | `InventoryReceiptLine` | Draft→Confirmed→Posted→Cancelled; posted lines immutable; posting emits `InventoryMovement` rows |
| InventoryIssue (new) | `InventoryIssue` | `InventoryIssueLine` | Same lifecycle; posting requires `Available ≥ Requested` unless `Stock.AllowNegativeStock` |
| StockTransfer (new) | `StockTransfer` | `StockTransferLine` | Draft→Confirmed→Shipped→Received→Completed / Cancelled; posting always emits a paired issue-at-source + receipt-at-destination `InventoryMovement`, never a `WarehouseId` edit |
| StockAdjustment (new) | `StockAdjustment` | `StockAdjustmentLine` | Requires `StockAdjustmentReason`; posting emits `InventoryMovement`; distinct from `StockCount`-triggered adjustment (both post through the same movement pipeline) |
| StockCount | `Inventory` (existing) | `InventoryProduct` (existing) | Unchanged workflow (capture system qty → counted qty → variance → post); variance posts via the same movement pipeline as `StockAdjustment`, not a separate code path |
| StockReservation (new) | `StockReservation` | — | Never mutates `Stock`-on-hand; reduces `Available` only; concurrency-safe (see §8) |
| InventoryBatch (new) | `InventoryBatch` | — | Required iff `Product.TrackingType` includes Batch; expired batch cannot be issued unless explicitly overridden |
| InventorySerial (new) | `InventorySerial` | — | Required iff `Product.TrackingType` includes Serial; quantity is always 1; cannot be issued twice |
| InventoryMovement | `Transaction`/`TransactionProduct` (existing) | — | Append-only ledger; never edited/deleted once posted — corrections are compensating (reversal) movements |
| InventoryBalance (new, projection) | `InventoryBalance` | — | Reconstructable from `InventoryMovement`; only Inventory application code writes it |
| InventoryCostLayer (new) | `InventoryCostLayer` | — | Only populated/consumed when an item's costing method is FIFO |

`InventoryMovement` is **not** loaded as a collection off `Stock`/`Product` (brief §34) — always
queried directly, filtered, paged.

## 4. Value objects

- `MovementType` — wraps the existing `TransactionType.Id` values (1 Receipt, 2 Issue, 3 TransferIssue,
  4 TransferReceipt, 5 AdjustmentIncrease, 6 AdjustmentDecrease, 7 OpeningBalance, 8 Damage/Loss) and
  exposes `Direction` (`In`/`Out`) — replaces every `TypeId == n` magic-number check.
- `TrackingType` — `None | Batch | Serial | BatchAndSerial` (replaces scattered booleans, brief §18).
- `Quantity` — decimal + validation (non-negative where required); used on movement/balance/line types.
- `InventoryReference` — `(SourceDocumentType, SourceDocumentId, SourceDocumentLineId)` — replaces the
  current string-based `Notes = "InventoryId: {id}"` correlation.
- `BatchNumber`, `SerialNumber` — thin validated wrappers, not full VOs beyond format validation.

## 5. Domain services

- `InventoryAvailabilityService` — computes OnHand/Reserved/Available for an Item+Warehouse(+Location)(+Batch),
  backing both the reservation check and the public availability contract.
- `InventoryCostingService` — abstraction with `WeightedAverageCostingStrategy` (default, production-ready
  first) and `FifoCostingStrategy` (cost-layer based) implementations selected per `Product` costing method.
- `StockAllocationService` — FEFO/FIFO picking suggestion for batch-controlled issues.
- `TransferPostingService` — creates the paired issue/receipt `InventoryMovement` pair for a `StockTransfer`,
  replacing/absorbing today's `TransferReceivedIntegration` logic once `StockTransfer` exists (the existing
  `Transaction TypeId 3/4` auto-pairing behavior is preserved, just moved behind the new aggregate's Post
  operation instead of being triggered ad hoc from a raw `Transaction` create).

## 6. New tables (schema sketch; exact EF configs land in Phase 5)

All new tables live in the same shared `OrgContext`/database (brief §45 — one physical DB, logical
ownership only) and follow existing precision conventions (`decimal(18,2)` for money, matched
precision for quantities per brief §60-61 — `decimal(18,3)` for quantities that need three
decimals, e.g. weight/length units, confirmed against `Unit`/`ProductUnit.Rate` precision in Phase 5).

```
WarehouseLocation(Id, StockId FK, Code, Name, ParentLocationId, LocationType, IsReceivable, IsPickable, IsActive, ...BaseModel)
InventoryBalance(Id, ProductId FK, StockId FK, LocationId FK NULL, BatchId FK NULL,
                 QuantityOnHand, QuantityReserved, AverageCost, RowVersion, ...)
  unique index (ProductId, StockId, LocationId, BatchId)
InventoryReceipt(Id, StockId FK, LocationId FK NULL, DealerId FK NULL, Status, Date, Notes, ...MovementModel)
InventoryReceiptLine(Id, InventoryReceiptId FK, ProductId, UnitId, Quantity, UnitCost, BatchId NULL, ...)
InventoryIssue(Id, StockId FK, LocationId FK NULL, DealerId FK NULL, Status, Date, Notes, ...MovementModel)
InventoryIssueLine(Id, InventoryIssueId FK, ProductId, UnitId, Quantity, BatchId NULL, SerialId NULL, ...)
StockTransfer(Id, FromStockId FK, ToStockId FK, Status, Date, Notes, ...MovementModel)
StockTransferLine(Id, StockTransferId FK, ProductId, UnitId, Quantity, BatchId NULL, ...)
StockAdjustment(Id, StockId FK, ReasonId FK, Status, Date, Notes, ...MovementModel)
StockAdjustmentLine(Id, StockAdjustmentId FK, ProductId, UnitId, Direction, Quantity, BatchId NULL, ...)
StockAdjustmentReason(Id, Code, Name, IsActive)
StockReservation(Id, ProductId FK, StockId FK, LocationId FK NULL, BatchId FK NULL, SerialId FK NULL,
                 Quantity, SourceType, SourceId, SourceLineId, Status, ExpiresAt NULL, RowVersion, ...)
InventoryBatch(Id, ProductId FK, BatchNumber, ManufacturingDate NULL, ExpiryDate NULL,
               SupplierBatchNumber NULL, Status, ...)
  unique index (ProductId, BatchNumber)
InventorySerial(Id, ProductId FK, SerialNumber, BatchId NULL, CurrentStockId FK NULL,
                CurrentLocationId FK NULL, Status, ReceivedDate, IssuedDate NULL, ...)
  unique index (ProductId, SerialNumber)
InventoryCostLayer(Id, ProductId FK, StockId FK, BatchId NULL, ReceiptMovementId FK,
                   ReceiptDate, OriginalQuantity, RemainingQuantity, UnitCost, ...)
```

`Transaction`/`TransactionProduct` gain (additive, non-breaking): `SourceDocumentType`,
`SourceDocumentId`, `SourceDocumentLineId`, `IdempotencyKey` (unique index), `ReversalOfMovementId`
NULL FK to self (brief §56/§31). `TransactionType` gains no schema change — `MovementType` VO wraps
its existing `Id`/`InOut` columns.

## 7. Domain & integration events

Domain events (in-process, brief §29): `StockReceivedDomainEvent`, `StockIssuedDomainEvent`,
`StockTransferShippedDomainEvent`, `StockTransferReceivedDomainEvent`, `StockAdjustedDomainEvent`,
`StockReservedDomainEvent`, `StockReservationReleasedDomainEvent`,
`StockReservationFulfilledDomainEvent`, `BatchReceivedDomainEvent`, `SerialReceivedDomainEvent`,
`InventoryCostChangedDomainEvent`.

Integration events (`Inventory.Contracts.Events`, DTO payloads only, no domain entities), following
the existing `SalesInvoicePostedIntegrationEvent` pattern already established by CommercialDocuments/
Receivables/Payables: `InventoryReceiptPostedIntegrationEvent`, `InventoryIssuePostedIntegrationEvent`,
`InventoryTransferCompletedIntegrationEvent`, `InventoryReservationCreatedIntegrationEvent`,
`InventoryReservationFailedIntegrationEvent`, `InventoryValuationPostedIntegrationEvent`. Published
via the same `IIntegrationEventPublisher`/same-transaction pattern CommercialDocuments already uses
(see `CreateCommandHandler.cs` in `CommercialDocuments.Application.Invoices.Commands`) — this
codebase has not yet introduced an Outbox (grepped: none found), so integration events stay
in-process/same-transaction like every other module's today, not a new eventual-consistency
mechanism (brief §33: don't introduce an incompatible second event system — there isn't a first one
to be compatible with yet, so this matches the established in-process convention).

## 8. Reservation concurrency strategy

New pattern for this codebase (none exists today — checked). `InventoryBalance.RowVersion`
(SQL Server `rowversion`/EF `IsConcurrencyToken`) plus a single retry-on-`DbUpdateConcurrencyException`
loop in `ReserveStockCommandHandler`: read balance, compute `Available`, if sufficient attempt an
update incrementing `QuantityReserved` guarded by the read `RowVersion`; on conflict, re-read and
retry (bounded retries) or fail with `InsufficientStock`. This is a standard EF optimistic-concurrency
pattern, introduced once, reused by every write to `InventoryBalance` (receipt/issue/adjustment/
transfer posting and reservation).

## 9. Costing

`Product` gains a `CostingMethod` (WeightedAverage default; FIFO opt-in per item — current behavior,
a flat manually-set `Product.Cost` with per-line manual `Cost` on `TransactionProduct`, becomes the
seed value / fallback). `InventoryCostingService.WeightedAverageCostingStrategy` recomputes
`InventoryBalance.AverageCost` on every receipt (`((OldQty×OldCost)+(RecvQty×RecvCost))/(OldQty+RecvQty)`)
and stamps issues at the balance's cost-at-posting-time (never retroactively rewritten — brief §21).
`FifoCostingStrategy` consumes `InventoryCostLayer` rows oldest-first. Existing manually-entered
`TransactionProduct.Cost` values are preserved as historical fact; only newly-posted movements use
the costing service.

## 10. Application layer additions (Inventory.Application)

New command/query folders mirroring the existing `Products/Stocks/Transactions/...` convention:
`WarehouseLocations`, `InventoryReceipts`, `InventoryIssues`, `StockTransfers`, `StockAdjustments`,
`StockAdjustmentReasons`, `StockReservations`, `InventoryBatches`, `InventorySerials`. Commands/
queries follow the brief's §37 naming almost verbatim, adapted to the existing `ICommand`/
`ICommandHandler`/`ICreateCommand<Result>` CQRS shapes already used throughout Inventory.Application
(no new CQRS infrastructure — reuse `OrgSys.SharedKernel`).

Stock Card / Aging / Expiry / Serial History / Valuation queries are **read models**, implemented as
query handlers reading `InventoryMovement`/`InventoryBalance`/`InventoryBatch` directly — never via
domain repositories loading full aggregates (brief §38).

## 11. Contracts additions (Inventory.Contracts)

`IInventoryAvailabilityService` (`GetAvailability(ItemId, WarehouseId, Quantity) → OnHand/Reserved/
Available/CanFulfill`), `ReserveInventoryRequest`/`ReserveInventoryResult`,
`ProductLookupDto`/`StockLookupDto`/`TransactionLookupDto` (per the target doc's
pre-existing plan in `docs/modular-monolith-target-architecture.md:209-212` — implemented now), plus
the integration events from §7. No `InventoryDbContext`/entities/repositories are ever exposed
(unchanged rule, already respected today).

## 12. Dependency rules (unchanged, reconfirmed)

`Inventory.Domain` depends on `SharedKernel` only. No new `Domain→Domain` cross-module exceptions are
introduced by this plan (brief §9's "do not add new exceptions unless unavoidable" — the existing 9
`AcceptedDomainExceptions` entries for Inventory are inherited as-is, not grown). All new
Sales/Purchasing/CommercialDocuments-facing surface goes through `Inventory.Contracts`, verified by
the existing `Tests/Architecture.Tests/{ModuleDependencyTests,ModuleLayerDependencyTests}.cs`, which
gain new assertions for the new projects.

## 13. Migration & reconciliation plan (brief §46-47)

No legacy inventory tables are being replaced — `Transaction`/`TransactionProduct`/`Stock`/`Product`/
`Inventory`/`InventoryProduct` are **kept in place** (classification: **KEEP**, not MOVE/REPLACE).
Only additive tables are introduced. `InventoryBalance` is populated by a one-time backfill job that
folds existing `Transaction`/`TransactionProduct` history using the `MovementType` direction table
from §2, then reconciled per Item+Warehouse: `Σ(In) − Σ(Out) == InventoryBalance.QuantityOnHand`,
enforced by an integration test before `InventoryBalance` is trusted as authoritative (brief §47).
No batch/serial backfill is needed (no existing data uses these concepts).

## 14. Execution roadmap (Phases 3-15)

1. **Phase 3 (Domain)**: `MovementType`/`TrackingType`/`InventoryReference` VOs; harden `Stock`/
   `Product`/`Transaction`/`Inventory` with invariants; new aggregates (`WarehouseLocation`,
   `InventoryReceipt`, `InventoryIssue`, `StockTransfer`, `StockAdjustment`+`StockAdjustmentReason`,
   `StockReservation`, `InventoryBatch`, `InventorySerial`, `InventoryBalance`, `InventoryCostLayer`);
   business errors (brief §52).
2. **Phase 4 (Application)**: commands/queries/handlers/validators per §10; `Inventory.Domain.Tests`
   project created (parity with other modules).
3. **Phase 5 (Infrastructure)**: EF configs (`IEntityTypeConfiguration<T>`, precision, indexes,
   concurrency tokens), migration, repositories.
4. **Phase 6 (Ledger & Balance)**: `InventoryBalance` projection + backfill + reconciliation test;
   `MovementType` replaces magic numbers in existing handlers.
5. **Phase 7 (Receipts/Issues/Transfers/Adjustments)**: new aggregates posting through the shared
   movement pipeline; existing raw-Transaction creation paths migrate to go through them.
6. **Phase 8 (Reservations)**: `StockReservation` + concurrency-safe reserve/release/fulfill.
7. **Phase 9 (Batch/Serial)**: tracking rules, FEFO issuing, traceability.
8. **Phase 10 (Costing)**: weighted-average first, FIFO cost layers second.
9. **Phases 11-13 (Purchasing/Sales/GL integration)**: extend existing `CreateTransactionByInvoiceCommand`
   path; no new coupling patterns needed — the existing Contracts-based integration is preserved.
10. **Phase 14 (Legacy)**: no removal (decision: MVC untouched); only ensure no new code bypasses
    Inventory.Contracts.
11. **Phase 15 (Tests)**: domain, concurrency, accounting-integration, architecture tests;
    `dotnet build` / `dotnet test` gate.

Angular: new `features/inventory` sub-areas for Locations, Receipts, Issues, Transfers, Adjustments,
Reservations, Batches, Serials, and the report suite (Stock Card, Aging, Expiry, Serial History,
Valuation), added incrementally alongside each backend phase rather than as one final pass.
