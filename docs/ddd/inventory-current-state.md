# Inventory / Warehouse — Current State (Phase 1 Discovery)

Produced by inspecting the actual repository (branch `Latest`) before any code changes. No code
was modified to produce this document.

## 1. What already exists

Inventory is **not greenfield**. It went through the same modular-monolith DDD pass that produced
Accounting, Sales, Receivables, Payables, Treasury, Advances, CommercialDocuments, Parties and
MasterData (see `docs/module-ownership.md`, `docs/module-dependency-map.md`,
`docs/modular-monolith-target-architecture.md`). All four layers exist under `Modules/Inventory/`:

- `Inventory.Domain/Entities/`: `Product`, `ProductUnit`, `ProductRecipe`, `Property`,
  `PropertyElement`, `ProductPropertyElement`, `Stock`, `Transaction`, `TransactionProduct`,
  `TransactionType`, `Inventory` (physical stock-count document — **not** the bounded context),
  `InventoryProduct` (count line).
- `Inventory.Application/`: CQRS handlers per folder (`Products`, `Stocks`, `Transactions`,
  `TransactionTypes`, `Inventories`, `Properties`, `ProductUnits`) + `*/Integration/*` classes for
  cross-module side effects (`TransactionJournalPostingService`, `TransferReceivedIntegration`,
  `InventoryAdjustmentIntegration`).
- `Inventory.Contracts/`: `CreateTransactionByInvoiceCommand`, `DeleteTransactionByInvoiceCommand`,
  `SetTransactionStatusCommand`, `GetProductNamesQuery`, `GetStockNamesQuery`.
- `Inventory.Infrastructure/`: DI registration only (`AddInventoryModule`) + data seeding. **No
  module-owned DbContext** — persistence is the single shared `OrgContext`
  (`BuildingBlocks/OrgSys.DatabaseMigrator/Persistence/OrgContext.cs`), which every module's
  entities are registered into via Fluent API (entities themselves use Data Annotations —
  `[Table]`/`[ForeignKey]`/`[Column]` — not `IEntityTypeConfiguration<T>`).

Root legacy `Domain`/`Application` projects are already **empty** (0 files) — a prior pass fully
extracted Inventory's domain/application code out of the monolith root.

## 2. Deviation from the brief's assumptions

**OrgSys is not multi-tenant SaaS.** No `TenantId` exists anywhere in the codebase (grepped).
Isolation is scoped by `Branch` (`Modules/Organization`), not tenant. Every "multi-tenancy" clause
in the brief is treated in this design as **Branch-scoping** instead, where relevant (mostly:
Warehouse ownership by Branch, which `Stock.BranchId` already provides).

**No optimistic-concurrency pattern exists anywhere in OrgSys** (grepped for `RowVersion` /
`ConcurrencyCheck` / `Timestamp` — zero hits in any module). Reservation-safe concurrency is a
genuinely new pattern for this codebase, not a reuse of an established one.

## 3. Current stock source of truth

No `InventoryBalance`/`StockBalance` table exists. Balance is computed on demand by summing
`TransactionProduct.Quantity`, with movement direction hardcoded as magic numbers
(`GetListByBalanceQueryHandler.cs`: `TypeId ∈ {2,3,6,8}` subtracts, else adds). Decoded semantics:

| TypeId | Meaning | Direction |
|---|---|---|
| 1 | Purchase/addition receipt | + |
| 2 | Issue | − |
| 3 | Transfer issue (source) | − |
| 4 | Transfer received (destination) | + |
| 5 | Adjustment increase | + |
| 6 | Adjustment decrease | − |
| 7 | Opening balance | + |
| 8 | Inventory damage/loss | − |

There is genuinely **one** canonical ledger (`Transaction`/`TransactionProduct`) — no competing
stock truths exist today — but it is unindexed for this purpose (full scan per balance query) and
its semantics live in scattered `switch`/magic-number logic instead of a value object.

## 4. Receipts / Issues / Transfers / Adjustments today

All are `Transaction` rows differentiated by `TypeId`, not separate aggregate types:

- **Receipt/Issue** (`TypeId 1/2`): created directly, or via `Inventory.Contracts.
  CreateTransactionByInvoiceCommand` from CommercialDocuments' Invoice creation.
- **Transfer** (`TypeId 3/4`): `TransferReceivedIntegration` auto-generates the `TypeId 4` receipt
  from the `TypeId 3` issue, correlated by `ParentId` — **already matches the brief's "two
  movements, not a WarehouseId edit" requirement.**
- **Physical count / Adjustment**: the confusingly-named `Inventory` entity is the stock-count
  document (`InventoryProduct.CalcBalance/ActualBalance/DiffQuantity`).
  `InventoryAdjustmentIntegration` posts variance as `TypeId 5/6` `Transaction` rows — **already
  matches the brief's §13 count-then-post-variance workflow.**
- No explicit `Draft/Confirmed/Posted/Cancelled` state machine as named types — a shared
  `OrgSys.SharedKernel.Status` enum plus `Posted`/`Review`/`HasJournal` booleans on `MovementModel`
  serve that role today.

## 5. Sales / Purchasing → Inventory integration

Purchasing is intentionally thin — no dedicated PO-receipt document; it reuses `Invoice`+
`InvoiceType` (owned by `CommercialDocuments`, extracted from `Sales.Domain` 2026-09-13).
`CommercialDocuments.Application.Invoices.Commands.CreateCommandHandler` sends
`CreateTransactionByInvoiceCommand` through `Inventory.Contracts` — **already routed through
Contracts, not `Inventory.Domain` directly.** Prior passes already closed the
`Sales.Application → Inventory.Domain` violations documented in `docs/module-dependency-map.md`.

## 6. Inventory → GeneralLedger integration

Already contract-based: `TransactionJournalPostingService` calls `Accounting.Contracts`
(`PostAccountingDocumentCommand`, `GetAccountingDocumentJournalQuery`,
`DeleteAccountingDocumentJournalCommand`, `SetAccountingDocumentJournalStatusCommand`), resolving
account IDs from a `Preference` table (`StockAccount`, `TransitAccount`, `SalesAccount`,
`PurchaseAccount`, `InventoryDamageExpenseAccount`, ...) — never hardcoded IDs. This already
satisfies brief §24's "no hardcoded account IDs, go through Contracts" requirement.

## 7. Legacy code

- `OrgSys/OrgSys.csproj` (classic ASP.NET MVC/Razor) is still in `OrgSys.sln` and still references
  `Inventory.Application`. Its `Areas/Transactions/Controllers/InventoryController.cs` /
  `TransactionController.cs` are confirmed thin HTTP proxies to the API (`GetListApi`/`GetValueApi`
  calls), **not** a parallel business-logic implementation — so this is legacy *presentation*, not
  a competing domain. Per project decision, it is left untouched; new capability goes into Angular
  only.
- API controllers live under a flat `API/Controllers/Org/{Setting,Transaction,Reports}/`
  structure (`ProductController`, `StockController`, `TransactionController`,
  `InventoryController`, `TransactionTypeController`, `WarehouseReportController`) rather than the
  module-based `/api/inventory/*` convention.
- Angular already has `features/inventory` (stock-count screens), `features/transactions` (stock
  movement form/list), `features/reports/{stock-movement,stock-balance}`, and
  `features/administration/products` (Product master, filed under "administration" not
  "inventory").
- `Tests/` has `Accounting.Domain.Tests`, `Advances.Domain.Tests`, `Payables.Domain.Tests`,
  `Purchasing.Domain.Tests`, `Receivables.Domain.Tests`, `Sales.Domain.Tests` — **no
  `Inventory.Domain.Tests` project exists.**
- A stale `.claude/worktrees/orgsys-source-work-348dce/` directory has old copies of
  `Domain/Entities/OrgDb/{Product,Stock}.cs`. Not part of the active solution; not touched.

## 8. DDD violations / gaps found

- **Anemic domain model**: every Inventory entity is a plain data bag with public `virtual`
  setters, AutoMapper-mapped straight from DTOs — no invariants, no factory methods. Visibly behind
  newer modules (e.g. `Sales.Domain.QuotationLine` uses private setters + internal constructor +
  `RecalculateLineTotal()`).
- `Inventory` entity name collides with the bounded-context name (really `StockCount`/
  `PhysicalInventory`).
- Movement direction as magic numbers instead of a `MovementType` value object.
- No materialized `InventoryBalance` — every balance query is an unindexed full scan.
- String-based correlation for count↔adjustment linkage (`Notes = "InventoryId: {id}"`) instead of
  a structured `SourceDocumentType`/`SourceDocumentId`.
- No optimistic concurrency (new pattern needed, see §2).
- No `Inventory.Domain.Tests` project.
- Missing entirely: Warehouse Locations/bins, Reservations, Batch/Lot, Serial, FIFO cost layers,
  `InventoryBalance` projection, idempotency keys, reversal-as-compensating-movement (today, posted
  `Transaction`/`TransactionProduct` rows are edited/`ShiftDeleteAsync`'d instead of reversed).

## 9. What must be preserved (do not regress)

- `Transaction`/`TransactionProduct` as the single ledger source of truth.
- The transfer issue/receive auto-pairing via `ParentId`.
- The stock-count → variance-adjustment posting flow.
- The `Accounting.Contracts` + `Preference`-driven GL integration.
- The already-enforced module-boundary tests in `Tests/Architecture.Tests/{ModuleDependencyTests,
  ModuleLayerDependencyTests}.cs`, which already list Inventory's accepted cross-module exceptions
  (→Parties for `Dealer`, →Organization for `Branch`/`Shift`, →MasterData for `Unit`/
  `Classification`, →CommercialDocuments for `Invoice`, →Administration for `Preference`).
- The `docs/module-ownership.md` decision that **`Product` stays owned by Inventory** — no separate
  Catalog module (already evaluated and rejected as unjustified). Resolves brief §3: no Item
  ownership migration needed.

## 10. Decisions made with the project owner (this pass)

- **Scope**: full brief scope — Warehouse Locations, Batch/Lot, Serial tracking, FIFO cost layers,
  reservations with concurrency safety, and the full inquiry/report suite.
- **UI**: leave the Razor MVC inventory screens untouched. All new capability gets new Angular
  screens/services. Angular becomes the complete, current UI for Inventory going forward; MVC
  retirement is out of scope for this pass.
