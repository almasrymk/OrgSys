# Accounts Payable DDD Migration — Analysis + Result Report

Companion to [receivables-ddd-migration.md](receivables-ddd-migration.md)/
[receivables-ddd-result.md](receivables-ddd-result.md), which this report follows as its direct
architectural template — Payables was built as AP's structural mirror of AR, adjusted for
Credit-natured balances and outbound settlement, not a renamed copy of AR's business behavior. On
branch `Latest`, 2026-09-14.

## A. Repository analysis before changes

`Modules/Payables` was, before this work, an exact structural twin of pre-migration
`Modules/Receivables`: `Payables.Domain` held only `AssemblyMarker.cs`; `Payables.Application` had 3
handlers — `GetSupplierBalanceQueryHandler`/`GetSupplierAgingQueryHandler` (both GL-derived, live,
via `Accounting.Contracts.Postings.GetAccountActivityQuery`, sign-flipped for Credit-natured
payable balances) and `SetSupplierOpeningBalanceCommandHandler` (delegates to
`Accounting.Contracts.Postings.SetOpeningBalanceLineCommand`, no AP-owned row); `Payables.Infrastructure`
had zero persistence. `PayableController` already existed with `OpeningBalance`/`Balance`/`Aging`
endpoints. Supplier lives at `Parties.Domain.Dealer` (`TypeId = DealerType.Supplier`) — the same
physical entity Customer uses; Purchase Invoice lives at `CommercialDocuments.Domain.Invoice`
(`TypeId = InvoiceTypeId.Purchase`/`PurchaseReturn`) — the same physical entity Sales Invoice uses,
already posted to GL via the existing `InvoiceJournalPostingService` (credit side, since AP is
Credit-natured). `Purchasing.Domain.PurchaseOrder` is a real, separate aggregate (implemented
2026-09-13) manually linked to an already-created Purchase Invoice via `LinkInvoiceCommand` — no
automatic invoice generation, no PO→AP coupling. `Payables.Application` referenced
`Administration.Domain` directly (identical to AR's pre-migration violation). No other legacy AP
code exists anywhere in the repository (confirmed by search — root legacy `Application`/`Domain`/
`Infrastructure` projects were already deleted in prior commits).

## B. Existing problems found

1. `Payables.Application → Administration.Domain` direct dependency (same violation AR had).
2. Two **stale** Architecture.Tests exceptions that no code actually exercised: `("Payables",
   "Administration", ...)` (closed by fixing #1) and `("Payables", "Receivables", "Shared
   IReceivableAccountValidator clearing-account check.")` — the latter referred to
   `Accounting.Contracts.Accounts.IReceivableAccountValidator` (a generic GL-account-validation
   interface, confusingly named after its AR-side original use), not the `Receivables` module at
   all; grep confirmed zero compiled references from `Payables.Application` to `Receivables.*`.
   Removed — verified safe by a full architecture-test re-run (1062/1062 still passing).
3. No AP subledger existed at all — `Payables` could only ever report balance/aging by reading GL
   Journal history live, with no persisted open-item record, no idempotent invoice-to-liability
   creation, and no real payment-allocation model.

## C. Files moved

None — Supplier (`Dealer`) and Purchase Invoice (`Invoice`) already lived in the correct modules
and stay there untouched, exactly as the AR analysis found for Customer/Sales Invoice.

## D. Files created (32)

**Payables.Domain**: `Entities/{Payable,SupplierPaymentApplication,SupplierPaymentApplicationLine}.cs`,
`Enums/{PayableStatus,SourceDocumentType}.cs`, `Exceptions/PayableDomainException.cs`,
`Events/{PaymentAppliedDomainEvent,PaymentUnappliedDomainEvent,PayableSettledDomainEvent,
PayableCancelledDomainEvent,PayableWrittenOffDomainEvent}.cs`,
`Repositories/{IPayableRepository,ISupplierPaymentApplicationRepository}.cs`, `GlobalUsings.cs`.

**Payables.Contracts**: `Payables/{PayableDto,GetOutstandingPayablesQuery,
GetOverduePayablesQuery}.cs`, `Balances/{GetSupplierSubledgerBalanceQuery,GetSubledgerAgingQuery,
GetBalanceReconciliationQuery}.cs`.

**Payables.Application**: `Invoices/Integration/PurchaseInvoicePostedIntegrationEventHandler.cs`,
`Payments/Integration/SupplierPaymentPostedIntegrationEventHandler.cs`,
`OpenItems/Queries/{GetOutstandingPayablesQueryHandler,GetOverduePayablesQueryHandler}.cs`,
`Balances/Queries/{GetSupplierSubledgerBalanceQueryHandler,GetSubledgerAgingQueryHandler,
GetBalanceReconciliationQueryHandler}.cs`.

**Payables.Infrastructure**: `Persistence/{PayableRepository,SupplierPaymentApplicationRepository}.cs`,
`GlobalUsings.cs`.

**CommercialDocuments.Contracts**: `IntegrationEvents/PurchaseInvoicePostedIntegrationEvent.cs`.

**Treasury.Contracts**: `IntegrationEvents/SupplierPaymentPostedIntegrationEvent.cs`.

**Migrations**: `20260914065501_AddPayablesModule.cs` (+ `.Designer.cs`) — additive only: `Payable`,
`SupplierPaymentApplication`, `SupplierPaymentApplicationLine` tables; no existing table/column
touched.

**Tests**: `Tests/Payables.Domain.Tests/{Payables.Domain.Tests.csproj,GlobalUsings.cs,
PayableTests.cs,SupplierPaymentApplicationTests.cs}`,
`Tests/Application.Tests/{PurchaseInvoicePostedIntegrationEventHandlerTests,
SupplierPaymentPostedIntegrationEventHandlerTests,PayableReadModelQueryHandlerTests}.cs`.

**Docs**: this file.

## E. Files modified (14)

`ModuleLayerDependencyTests.cs` (removed 2 stale exceptions — §B.2); `SetSupplierOpeningBalanceCommandHandler.cs`
(Administration.Contracts + Payable creation); `Payables.Application.csproj`/`GlobalUsings.cs` (new
Contracts refs, removed Administration.Domain); `Payables.Infrastructure`
`ServiceCollectionExtensions.cs` (2 new repositories); `CreateCommandHandler.cs`
(CommercialDocuments — now also raises `PurchaseInvoicePostedIntegrationEvent` alongside the
existing AR-side `SalesInvoicePostedIntegrationEvent`); `PostTransactionCommandHandler.cs`
(Treasury — now also raises `SupplierPaymentPostedIntegrationEvent` alongside the existing AR-side
`CustomerPaymentPostedIntegrationEvent`); `OrgContext.cs`/`GlobalUsings.cs`/
`OrgSys.DatabaseMigrator.csproj`/`OrgContextModelSnapshot.cs` (3 new DbSets, Fluent config, unique
indexes); `PayableController.cs` (5 new read endpoints); `Application.Tests.csproj` (1 new project
ref); `OrgSys.sln` (new test project).

## F. Files deleted

None — §A/§B confirmed there was no legacy AP implementation to remove.

## G. Aggregate roots

`Payable` — the AP open item (liability to a supplier). `SupplierPaymentApplication` — audit/
idempotency record of one posted supplier payment's FIFO application across Payables, with
`SupplierPaymentApplicationLine` as its child entity.

## H. Entities

`Payable`, `SupplierPaymentApplication`, `SupplierPaymentApplicationLine`. No entity outside these
three was added.

## I. Value objects

None introduced — matches AR's decision exactly (see [receivables-ddd-result.md](receivables-ddd-result.md)
§8): plain `long` IDs, bare `CurrencyId`/`Rate` pair, no `Money` VO, consistent with every other
aggregate in this solution (`Journal`, `Financial`, `Invoice`, `Receivable`).

## J. Domain events

`PaymentAppliedDomainEvent`, `PaymentUnappliedDomainEvent`, `PayableSettledDomainEvent`,
`PayableCancelledDomainEvent`, `PayableWrittenOffDomainEvent`. No `PayableCreatedDomainEvent` — same
reasoning as AR: `Payable.Id` doesn't exist at `Create()` time (EF-generated on save), so it would
have to be raised post-save by the Application handler, and nothing currently consumes it — deferred
until a real consumer exists, per the anti-CRUD-event guidance (brief §30).

## K. Integration events

**Inbound** (consumed by Payables): `PurchaseInvoicePostedIntegrationEvent` (CommercialDocuments.
Contracts), `SupplierPaymentPostedIntegrationEvent` (Treasury.Contracts). **Outbound**: none —
nothing outside Payables currently needs to know about a Payable's lifecycle.

## L. Repositories created

`IPayableRepository`, `ISupplierPaymentApplicationRepository` (Domain interfaces) +
`PayableRepository`, `SupplierPaymentApplicationRepository` (Infrastructure implementations,
composed over the existing generic `IRepository<T>` — no second persistence abstraction, no
per-aggregate-line repository, matching brief §38's explicit prohibition).

## M. Commands implemented

`SetSupplierOpeningBalanceCommand` (pre-existing, extended to also create a `Payable`). No new
public commands beyond that in this pass — invoice creation stays CommercialDocuments' own
`CreateInvoiceCommand`; payment posting stays Treasury's own `PostFinancialTransactionCommand`. AP
observes both via integration events rather than exposing parallel AP-side create commands, per the
brief's own "AccountsPayable DOES NOT own... FinancialTransaction" ownership rule.

## N. Queries implemented

`GetSupplierBalanceQuery`/`GetSupplierAgingQuery` (pre-existing, GL-derived, unchanged) +
6 new: `GetOutstandingPayablesQuery`, `GetOverduePayablesQuery`, `GetSupplierSubledgerBalanceQuery`,
`GetSubledgerAgingQuery`, `GetBalanceReconciliationQuery` (the GL-vs-subledger check), all backed by
the new `Payable` table.

## O. Database tables/migrations affected

3 new tables (additive only): `Payable` (unique index on `SourceDocumentType, SourceDocumentId,
SupplierId`), `SupplierPaymentApplication` (unique index on `SourceFinancialId`),
`SupplierPaymentApplicationLine` (FK to `SupplierPaymentApplication`, cascade delete). Migration
`20260914065501_AddPayablesModule`. No existing table/column changed.

## P. Legacy code removed

None (§F) — nothing legacy existed to remove. The two stale architecture-test exceptions (§B.2) were
removed as dead documentation, not legacy business logic.

## Q. Remaining technical debt

- **Backfill**: no historical purchase invoices/payments migrated into the subledger — it only
  reflects activity from this point forward, same as AR.
- **Payment-terms/due dates**: `Invoice` has no due-date/payment-terms field; every Payable is "due
  on the invoice/opening-balance date."
- **Refund/inbound supplier payments**: `PostFinancialTransactionCommand` with
  `ReferenceType.Supplier, Direction.In` (a supplier refunding an overpayment) is not wired to any
  AP effect — deliberately out of scope.
- **Concurrency**: no optimistic-concurrency token on `Payable`/`SupplierPaymentApplication` — same
  gap AR has, no existing convention in the codebase to follow.
- **Write-offs/debit-credit notes/PurchaseReturn** (brief's Phase 8/9 equivalents): `Payable.WriteOff`
  exists at the domain level (tested) with no Application command/API endpoint; `PurchaseReturn`
  invoices are not wired to any AP adjustment — no existing credit-note concept in Purchasing/
  CommercialDocuments to integrate against.
- **Supplier statement** (brief §26): not built — no requirement/consumer identified.
- **Three-way match / GRNI** (brief §15/§16): no Goods Receipt / GRNI concept exists anywhere in
  this codebase today (`Purchasing` has `PurchaseRequisition`/`PurchaseOrder` only) — nothing to
  integrate against; the architectural boundary (Payable references only `SourceDocumentId`, never
  a Purchasing/Inventory entity) is in place so this can be added later without breaking AP's
  isolation.
- **Angular UI**: none of the new read endpoints are wired into any screen.

## R. Build result

`dotnet build OrgSys.sln` — **0 errors** (pre-existing, unrelated nullability warnings untouched).

## S. Test result

`dotnet test OrgSys.sln`: `Payables.Domain.Tests` 35/35 (new), `Receivables.Domain.Tests` 35/35
(unaffected), `Accounting.Domain.Tests` 54/54 (unaffected), `Application.Tests` 93/93 (+12 new: 3
`PurchaseInvoicePostedIntegrationEventHandlerTests`, 5 `SupplierPaymentPostedIntegrationEventHandlerTests`,
4 `PayableReadModelQueryHandlerTests`), `Architecture.Tests` 1062/1062 (unaffected — the generic
rules already covered Payables before this pass; two stale exceptions removed, zero new ones added).
**Total: 1279/1279 passing.**

## T. Architecture-test result

All 1062 `Architecture.Tests` assertions pass with **zero** accepted exceptions remaining for
Payables (both prior ones were stale and removed — see §B.2) — the same "cleanest module in the
solution" status AR achieved. Verified manually per brief §64: grepped `Payables.Domain` for `using
*.Domain` from any other module — zero occurrences of `GeneralLedger`(`Accounting`)`.Domain`,
`Receivables.Domain`, `Purchasing.Domain`, `Inventory.Domain`, `Treasury.Domain`.

## U. Bounded-context dependency matrix (verified)

```
Payables.Domain        -> SharedKernel only
Payables.Contracts     -> SharedKernel only
Payables.Application   -> Payables.Domain, Payables.Contracts, OrgSys.EventBus,
                           Accounting.Contracts, MasterData.Contracts, Administration.Contracts,
                           CommercialDocuments.Contracts, Treasury.Contracts
Payables.Infrastructure-> Payables.Application, Payables.Domain (EF)
```

Zero `Payables.Domain -> {any module}.Domain/.Application/.Infrastructure`. Cross-context
communication flows exactly as brief §68 specifies:

```
CommercialDocuments (Purchase Invoice posted)
    -> PurchaseInvoicePostedIntegrationEvent -> Payables

Treasury (Supplier payment posted)
    -> SupplierPaymentPostedIntegrationEvent -> Payables

Payables -> Accounting.Contracts   (account validation, opening-balance GL line, activity read)
Payables -> Parties.Contracts      (Supplier/Dealer identity, transitively via Accounting.Contracts)
Payables -> MasterData.Contracts   (default currency)
Payables -> Administration.Contracts (preference lookup)
```

No `Payables.Domain -> Purchasing.Domain`/`Inventory.Domain` exists or was ever needed — no
Purchasing/Inventory integration was built this pass (§Q), so the boundary was never tested against
a real PO/GRN reference; `SourceDocumentId` stays a bare `long` with no FK, ready for that later.

## V. Assumptions made

1. **Supplier opening balance sign convention**: a Credit opening balance (`SupplierIsDebit ==
   false`) is the AP liability and creates a `Payable`; a Debit opening balance is treated as a
   prepayment/on-account credit and does not create one (mirrors AR's opposite-sign rule exactly —
   verified against `GetSupplierBalanceQueryHandler`'s existing `Credit - Debit` sign convention).
2. **Payment direction**: a supplier settlement is `PostFinancialTransactionCommand` with
   `ReferenceType.Supplier, Direction.Out` (money leaving the company) — the mirror image of AR's
   `ReferenceType.Customer, Direction.In`. Not explicitly documented anywhere in the existing code;
   inferred from `FinancialTransactionDirection`'s two values and the natural accounting meaning of
   paying a supplier.
3. **`Financials.Commands.CreateFinancialCommand`/legacy per-invoice allocation paths are not
   canonical for AP either** — the same dead/unreachable-from-UI code AR's analysis flagged applies
   symmetrically here (it has no ReferenceType-specific behavior; the finding transfers directly).
4. **PurchaseReturn and GRNI are out of scope** — no existing implementation to integrate against;
   inventing one would violate the "do not invent workflows not requested" principle both briefs
   share.

## W. DDD compliance scorecard

| Dimension | Score | Why |
|---|---|---|
| Bounded Context isolation | 9/10 | Zero cross-module Domain/Infrastructure/Application dependencies, verified by 1062 automated assertions with zero remaining accepted exceptions (two stale ones removed). Not 10: like AR, the *other* modules (CommercialDocuments, Treasury) now know about Payables' Contracts — correct per the brief, but one-directional by convention rather than test-enforced from that side. |
| Domain Model quality | 9/10 | `Payable`/`SupplierPaymentApplication` are real, encapsulated, invariant-protected aggregates — not anemic CRUD. Not 10: `Payable : MovementModel` carries unused legacy fields (`HasJournal`, `Review`, `Posted`, base `Status`) forced by the shared-repository constraint, identical to AR's compromise. |
| Aggregate design | 8/10 | Clear roots, encapsulated child lines, factory methods, idempotent `Cancel`. Not higher: `SupplierPaymentApplication` doesn't itself enforce "this FinancialId is new" — that's a repository/DB-index check, not an aggregate invariant. |
| Domain invariants | 9/10 | All brief §12 minimum invariants implemented and unit-tested (35 domain tests): positive amounts, outstanding bounds, no double-settlement, cancellation only pre-application, write-off reason required, over-allocation rejected, wrong-payable-status rejected. Not 10: "duplicate posting must be prevented" (§12.20) is a DB unique index + fast-path check, not an aggregate-level rule — a legitimate, documented design choice, not a gap, but one point short of "every invariant lives in the aggregate." |
| CQRS | 9/10 | Commands mutate through aggregate methods only (`Payable.Apply`/`Cancel`/`WriteOff`, never a public setter); queries return DTOs, never EF entities. Not 10: no dedicated FluentValidation validators were added for the (unchanged) `SetSupplierOpeningBalanceCommand` request shape — validation is inline in the handler, matching the pre-existing AR pattern rather than introducing a new one. |
| Persistence isolation | 8/10 | Own repository abstractions, zero cross-module EF navigation/FK. Not higher: persistence is the shared `OrgContext` (this repo's own unchanged, established convention — every module including the already-refactored GeneralLedger/AR uses it; there is no per-module `DbContext` anywhere in this codebase to match). |
| GL integration | 9/10 | Payables.Domain never references Accounting.Domain; all GL contact goes through `Accounting.Contracts` (`IPayableAccountValidator`, `SetOpeningBalanceLineCommand`, `GetAccountActivityQuery`) — the identical, already-proven AR pattern. Not 10: relies on `CommercialDocuments`' existing posting bridge rather than Payables driving its own posting — correct per brief §17's "do not create JournalEntry directly," but means AP's GL-integration correctness is inherited, not independently verified by an AP-side integration test against a real Journal. |
| Treasury integration | 9/10 | No `FinancialTransaction`/`Financial` duplication — Payables only records *that* and *how much* of a posted payment it applied, via `SupplierPaymentApplication`, referencing `FinancialId` by value only. Not 10: Direction/ReferenceType semantics (§V.2) are inferred, not confirmed against a written spec, since none exists in this codebase. |
| Purchasing integration | 6/10 | Correctly *un*coupled — `Payable.SourceDocumentId` is a bare `long`, no FK, no navigation to `PurchaseOrder` — but no actual PO-to-Payable linkage or three-way-match preparation was built (§Q), because no GRN/receiving concept exists to match against. Isolation is real; integration is absent. |
| Inventory integration | N/A (no GoodsReceipt concept exists in this codebase) | Not scored — nothing to integrate against, confirmed by repository search. |
| AP Subledger | 8/10 | Real, event-fed, DB-backed open-item table (`Payable`), independent of GL, with its own balance/aging/reconciliation queries. Not higher: no historical backfill (§Q), so it's a forward-only subledger today, not yet a complete source of truth for existing suppliers. |
| Payment allocation | 8/10 | Full/partial/multi-payable FIFO allocation implemented and tested (5 handler tests covering the exact scenarios brief §21 describes: partial, spanning multiple payables, exceeding outstanding, no open items, duplicate delivery). Not higher: allocation policy is FIFO-only — brief §21's example (explicit per-invoice allocation amounts from the payer) isn't supported, because the live Treasury UI/API (`PostFinancialTransactionCommand`) has no per-invoice allocation input to consume (confirmed via the same UI-reachability check AR's analysis used). |
| Supplier advances | N/A (not built) | Explicitly deferred (§Q) — no advance/prepayment concept requested or backed by an existing Treasury flow; excess payment stays as `UnappliedAmount` on the `SupplierPaymentApplication` audit row rather than a first-class advance entity. |
| Reversal/auditability | 7/10 | `Unapply` restores a payable correctly (tested); `CreateUserId`/`CreateDate`/`BranchId` audit fields preserved throughout. Not higher: no explicit "reverse a posted Payable" domain method distinct from `Unapply` + manual re-cancellation, and no correlation/reversal-document-id field, unlike GL's `Journal.CreateReversal`/`OriginalJournalId` pattern this brief asks AP to mirror (§47) — not implemented this pass. |
| Idempotency | 8/10 | Both inbound events are idempotent (fast-path existence check + DB unique-index backstop), both tested for duplicate delivery (2 dedicated tests). Not higher: no true message-bus retry exists in this codebase to test against — proven for "handler called twice," not genuine at-least-once delivery, since no such transport exists here (confirmed: `OrgSys.EventBus` is 3 files, in-process MediatR notification only). |
| Multi-tenancy | N/A (this OrgSys instance is single-tenant, branch-scoped) | Confirmed in the AR analysis and re-verified here: no `TenantId` column exists anywhere in the solution, only `BranchId` (present on `Payable`/`SupplierPaymentApplication` via `MovementModel`, respected throughout). Brief §45's multi-tenancy requirements don't apply to this codebase's actual architecture. |
| Architecture tests | 9/10 | 1062 pre-existing, generic, automated assertions already fully cover Payables' Domain/Application/Infrastructure boundaries against every other module — the strongest score category, same as AR. Not 10: no Payables-specific architecture test was *added* (none was needed — the generic suite already parameterizes over every module including Payables). |
| Unit tests | 9/10 | 35 domain tests (creation, apply, unapply, cancel, write-off, overdue) + 12 application-layer tests (2 integration handlers with FIFO/duplicate/zero-amount coverage, read-model correctness). Not 10: no test exercises `SetSupplierOpeningBalanceCommandHandler`'s new Payable-creation branch directly (only indirectly implied by the mirrored AR pattern) — a gap, not a false claim. |
| Integration tests | 5/10 | No test exercises the full path through a real database/DbContext (EF InMemory/SQLite) — all "integration" coverage here is Moq-based at the Application-handler boundary, proving orchestration logic but not actual EF mapping/migration correctness beyond what `dotnet ef migrations add` validates statically. Same gap AR has. |
| Legacy removal | N/A (nothing legacy existed) | §F/§P — no legacy AP implementation existed to remove; two stale test-exception lines were tidied, not "legacy removal" in the sense the brief means. |

**Overall**: a genuine, working, tested AP subledger that mirrors AR's proven pattern faithfully
while getting the AP-specific asymmetries right (Credit-natured balance, outbound Direction, no
supplier-advance entity yet) — not a 10 anywhere a real gap exists (Purchasing/Inventory
integration, advances, reversal-as-first-class-concept, historical backfill), each scored against
what would concretely need to be built to close it, not against a hypothetical ideal.
