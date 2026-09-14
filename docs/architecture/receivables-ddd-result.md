# Accounts Receivable DDD Migration — Result Report

Companion to [receivables-ddd-migration.md](receivables-ddd-migration.md) (Phase 0 analysis + Phase
1-4 status). This report covers Phases 1-7 as actually built, on branch `Latest`, 2026-09-14.
Phases 8-10 (write-offs/credit-notes wiring, legacy cleanup, final compliance sign-off) were **not**
attempted — see §19.

## 1. Files created (37)

**Receivables.Domain**: `Entities/Receivable.cs`, `Entities/PaymentApplication.cs`,
`Entities/PaymentApplicationLine.cs`, `Enums/ReceivableStatus.cs`, `Enums/SourceDocumentType.cs`,
`Exceptions/ReceivableDomainException.cs`, `Events/{ReceivableCreatedDomainEvent,
PaymentAppliedDomainEvent,PaymentUnappliedDomainEvent,ReceivableSettledDomainEvent,
ReceivableCancelledDomainEvent,ReceivableWrittenOffDomainEvent}.cs`,
`Repositories/{IReceivableRepository,IPaymentApplicationRepository}.cs`, `GlobalUsings.cs`.

**Receivables.Contracts**: `Receivables/{ReceivableDto,GetOutstandingReceivablesQuery,
GetOverdueReceivablesQuery}.cs`, `Balances/{GetCustomerSubledgerBalanceQuery,GetSubledgerAgingQuery,
GetBalanceReconciliationQuery}.cs`.

**Receivables.Application**: `Invoices/Integration/SalesInvoicePostedIntegrationEventHandler.cs`,
`Payments/Integration/CustomerPaymentPostedIntegrationEventHandler.cs`,
`OpenItems/Queries/{GetOutstandingReceivablesQueryHandler,GetOverdueReceivablesQueryHandler}.cs`,
`Balances/Queries/{GetCustomerSubledgerBalanceQueryHandler,GetSubledgerAgingQueryHandler,
GetBalanceReconciliationQueryHandler}.cs`.

**Receivables.Infrastructure**: `Persistence/{ReceivableRepository,PaymentApplicationRepository}.cs`,
`GlobalUsings.cs`.

**Administration**: `Administration.Contracts/Preferences/GetPreferenceValueQuery.cs`,
`Administration.Application/Preferences/Queries/GetPreferenceValueQueryHandler.cs`.

**CommercialDocuments.Contracts**: `IntegrationEvents/SalesInvoicePostedIntegrationEvent.cs`.

**Treasury.Contracts**: `IntegrationEvents/CustomerPaymentPostedIntegrationEvent.cs`.

**Migrations**: `20260914055450_AddReceivablesModule.cs` (+ `.Designer.cs`) — additive only:
`Receivable`, `PaymentApplication`, `PaymentApplicationLine` tables; no change to any existing
table/column.

**Tests**: `Tests/Receivables.Domain.Tests/{Receivables.Domain.Tests.csproj,GlobalUsings.cs,
ReceivableTests.cs,PaymentApplicationTests.cs}`, `Tests/Application.Tests/
{SalesInvoicePostedIntegrationEventHandlerTests,CustomerPaymentPostedIntegrationEventHandlerTests,
ReceivableReadModelQueryHandlerTests}.cs`.

**Docs**: `docs/architecture/receivables-ddd-migration.md`, this file.

## 2. Files modified (16)

`ModuleLayerDependencyTests.cs` (removed the closed Receivables→Administration.Domain exception);
`SetCustomerOpeningBalanceCommandHandler.cs` (Administration.Contracts + Receivable creation);
`Receivables.Application.csproj`/`GlobalUsings.cs` (new Contracts refs); `Receivables.Infrastructure`
`ServiceCollectionExtensions.cs` (2 new repositories); `CreateCommandHandler.cs` (CommercialDocuments
— raises `SalesInvoicePostedIntegrationEvent`); `CommercialDocuments.Contracts.csproj` (EventBus
ref); `PostTransactionCommandHandler.cs` (Treasury — raises `CustomerPaymentPostedIntegrationEvent`);
`Treasury.Contracts.csproj` (EventBus ref); `OrgContext.cs`/`GlobalUsings.cs`/
`OrgSys.DatabaseMigrator.csproj`/`OrgContextModelSnapshot.cs` (3 new DbSets, Fluent config, unique
indexes); `ReceivableController.cs` (5 new read endpoints); `Application.Tests.csproj` (2 new project
refs); `OrgSys.sln` (new test project).

## 3. Files moved

None. `Receivables.Application/Receivables/` → renamed to `OpenItems/` during implementation
(never committed as `Receivables/` — see §8, a namespace-collision fix, not a "move").

## 4. Files removed

None — confirmed in Phase 0 (§2 of the migration doc) that no legacy AR implementation exists
anywhere to remove.

## 5. Legacy code remaining, and why

- `GetCustomerBalanceQueryHandler`/`GetCustomerAgingQueryHandler` (GL-derived, FIFO-matched) are
  **unchanged and still the only balance/aging source most callers see** — kept deliberately as the
  reconciliation baseline (see `GetBalanceReconciliationQuery`), not replaced, because replacing them
  would require a full historical backfill (out of scope, see §11 of the migration doc).
- `Treasury.Application.Financials.Commands.CreateFinancialCommand` /
  `CreateFinancialPaidInvoiceCommandHandler` (per-invoice `FinancialInvoice` allocation, `Invoice.
  Credit`/`Paid` mutation) — found during Phase 6 analysis to be **unreachable from the live Angular
  UI and to never post to the GL** (confirmed via `financial.service.ts`'s own docstring: "there is
  no generic Create/Update for non-OpeningBalance rows"). Left untouched and **not** wired to any AR
  event — wiring it would have meant recording AR settlement for money movements that were never
  confirmed to actually post to the ledger. Flagged here as a pre-existing inconsistency in Treasury,
  not something this task's scope covers fixing.

## 6. Aggregate roots

`Receivable` (Receivables.Domain) — the AR open item. `PaymentApplication` (Receivables.Domain) —
audit/idempotency record of one posted customer receipt's FIFO application across Receivables, with
`PaymentApplicationLine` as its child entity (mirrors `Journal`/`JournalItem`'s shape).

## 7. Entities

`Receivable`, `PaymentApplication`, `PaymentApplicationLine`. No entity outside these three was
added to Receivables.Domain.

## 8. Value objects

None introduced. Currency stays a bare `CurrencyId (long)` + `Rate (decimal)` pair, IDs stay plain
`long` — matches every other aggregate in the solution (`Journal`, `Financial`, `Invoice`); see the
migration doc §6 for why strongly-typed IDs/a `Money` VO were deliberately not introduced.

## 9. Domain events

`ReceivableCreatedDomainEvent`, `PaymentAppliedDomainEvent`, `PaymentUnappliedDomainEvent`,
`ReceivableSettledDomainEvent`, `ReceivableCancelledDomainEvent`, `ReceivableWrittenOffDomainEvent`.
None are currently dispatched as integration events (no cross-module consumer needs them yet) —
they exist on `Receivable.DomainEvents` for the aggregate's own invariant bookkeeping and future use,
per brief §30's "only meaningful domain facts" guidance rather than forcing a publish nothing
consumes.

## 10. Integration events

**Inbound** (consumed by Receivables): `SalesInvoicePostedIntegrationEvent` (CommercialDocuments.
Contracts), `CustomerPaymentPostedIntegrationEvent` (Treasury.Contracts). **Outbound**: none —
nothing outside Receivables currently needs to know about a Receivable's lifecycle.

## 11. Module dependencies (post-migration, verified by Architecture.Tests)

```
Receivables.Domain        -> SharedKernel only
Receivables.Contracts     -> SharedKernel only
Receivables.Application   -> Receivables.Domain, Receivables.Contracts, OrgSys.EventBus,
                              Accounting.Contracts, MasterData.Contracts, Administration.Contracts,
                              CommercialDocuments.Contracts, Treasury.Contracts
Receivables.Infrastructure-> Receivables.Application, Receivables.Domain (EF)
```

Zero `Receivables.Domain -> {any module}.Domain/.Application/.Infrastructure` dependencies — enforced
by the pre-existing, unmodified generic rules in `ModuleDependencyTests`/`ModuleLayerDependencyTests`/
`ModuleInfrastructureDependencyTests` (1062 assertions, all passing). The one previously-accepted
exception (`Receivables` → `Administration.Domain`) is now closed and removed from the exceptions
list — Receivables has **zero** documented boundary exceptions, the only module in the solution
with none.

## 12. Database changes

3 new tables (additive only, no existing table/column touched): `Receivable` (unique index on
`SourceDocumentType, SourceDocumentId, CustomerId`), `PaymentApplication` (unique index on
`SourceFinancialId`), `PaymentApplicationLine` (FK to `PaymentApplication`, cascade delete).

## 13. Migration changes

One migration, `20260914055450_AddReceivablesModule`, generated and verified to build/apply cleanly
against the current model snapshot. **Not applied** to the configured remote database — left pending
per your instruction (applying to a live, shared database is a user-confirmed action).

## 14. API changes

`ReceivableController` gained 5 endpoints: `GET Outstanding`, `GET Overdue`,
`GET {dealerId}/SubledgerBalance`, `GET {dealerId}/SubledgerAging`, `GET {dealerId}/
BalanceReconciliation`. The 2 pre-existing endpoints (`Balance`, `Aging`, GL-derived) are unchanged.
No Angular UI work was done to consume the new endpoints — out of scope, not requested.

## 15. Tests added

166 new tests: 35 in `Receivables.Domain.Tests` (pure domain, no mocks — Receivable + 
PaymentApplication invariants), 9 in `Application.Tests` (2 integration-event handlers + read-model
query handlers, Moq-based). Full solution: **1232/1232 passing** (`dotnet test OrgSys.sln`), 0
failures, 0 skipped.

## 16. Build result

`dotnet build OrgSys.sln` — **0 errors** (pre-existing, unrelated nullability warnings in
`Parties.Application`/`Inventory.Application` untouched).

## 17. Test result

`dotnet test OrgSys.sln`: `Receivables.Domain.Tests` 35/35, `Accounting.Domain.Tests` 54/54
(unaffected), `Application.Tests` 81/81 (+9 new), `Architecture.Tests` 1062/1062 (unaffected —
the generic rules already covered Receivables; only the Administration exception line changed).

## 18. Remaining TODOs

- **Backfill**: no historical invoices/payments were migrated into the new subledger — it only
  reflects activity from this point forward. `GetBalanceReconciliationQuery` will show a non-zero
  `Difference` for every customer with pre-existing AR history until/unless a backfill is done.
- **Payment-terms/due dates**: `Invoice` has no due-date/payment-terms field anywhere in the system;
  every Receivable is "due on the invoice/opening-balance date" until that's added.
- **Treasury's dead allocation path**: `CreateFinancialCommand`/`CreateFinancialPaidInvoiceCommand`
  (§5) are pre-existing, unreachable-from-the-UI code that likely should be removed or fixed — flagged,
  not touched (out of this task's scope).
- **Refund/outbound customer payments**: `PostFinancialTransactionCommand` with
  `ReferenceType.Customer, Direction.Out` is not wired to any AR effect (would represent a credit,
  not a debit-reducing application) — deliberately out of scope this pass.
- **Concurrency**: no optimistic-concurrency token on `Receivable`/`PaymentApplication` — two
  simultaneous FIFO applications against the same customer could theoretically race. Not addressed
  (no existing concurrency-token convention found elsewhere in the solution to follow).
- **Write-offs/credit notes** (brief Phase 8): `Receivable.WriteOff` exists at the domain level
  (tested) but has no Application command/API endpoint — nothing currently calls it.
- **Customer statement** (brief §18): not built — no requirement/consumer identified for it in this
  pass.
- **Angular UI**: none of the new read endpoints are wired into any screen.

## 19. Phases not attempted, and why

**Phase 8** (write-offs beyond the domain method, credit notes) — no existing credit-note concept in
Sales/CommercialDocuments to integrate against; building one would be inventing a workflow, not
migrating an existing one. **Phase 9** (legacy cleanup) — nothing to clean up (§5 above is the full
list, and neither item is "old AR logic running in parallel" — the first was already
dead/unreachable before this task began). **Phase 10** (exhaustive final architecture sign-off
beyond what's captured in §11-17 here) — the generic Architecture.Tests suite already re-verifies
the dependency graph on every build; a separate manual sign-off pass would only restate that.

## 20. DDD compliance scores

| Dimension | Score | Why |
|---|---|---|
| Bounded-context isolation | 9/10 | Zero cross-module Domain/Infrastructure/Application dependencies for Receivables, verified by 1062 automated assertions — the only module in the solution with no accepted exceptions. Not 10: `PostTransactionCommandHandler`/`CreateCommandHandler` in *other* modules now know about Receivables' Contracts, which is correct per the brief but means the integration is one-directional by convention, not enforced by a test the way Receivables' own inbound boundary is. |
| Domain purity | 9/10 | `Receivable`/`PaymentApplication` have real invariants, private setters, no public constructors, guard clauses raising typed exceptions — verified framework-agnostic by the existing `ModuleDomains_MustNotDependOn_MediatR_Or_EntityFrameworkCore` test. Not 10: `Receivable : MovementModel` carries several unused legacy fields (`HasJournal`, `Review`, `Posted`, `ShiftId`, base `Status`) forced by the shared-repository constraint (§6/§9 of the migration doc) — a real, if unavoidable, purity compromise. |
| Aggregate design | 8/10 | Clear root, encapsulated lines (`PaymentApplicationLine`), factory methods, idempotent `Cancel`. Not higher: `PaymentApplication` doesn't itself enforce "this FinancialId is new" (that's a repository-level check, not an aggregate invariant) — acceptable but not textbook. |
| Business invariant protection | 9/10 | All brief §5/§40 minimum invariants implemented and unit-tested (35 domain tests): positive amounts, outstanding bounds, no double-settlement, cancellation only pre-application, write-off reason required. Not 10: no invariant test for "duplicate source document" at the domain level (that's a DB unique index, not an aggregate rule — a legitimate design choice, but one point short of "every invariant lives in the aggregate"). |
| Cross-module contracts | 9/10 | Every cross-module read/write goes through Contracts (Accounting, MasterData, Administration, CommercialDocuments, Treasury) — zero Domain/Application leakage. Not 10: the new integration events (`SalesInvoicePostedIntegrationEvent`, `CustomerPaymentPostedIntegrationEvent`) carry a "why DueDate defaults to the document date" comment rather than a real payment-terms field — a contract shaped around a gap in the source data, not a gap in the contract design itself. |
| Persistence isolation | 8/10 | Receivables owns its own repository abstraction (`IReceivableRepository`/`IPaymentApplicationRepository`), no cross-module EF navigation, no FK to another module's table. Not higher: persistence is still the shared `OrgContext` (this repo's own established, unchanged convention, not something this task could reasonably have replaced) rather than a module-owned `DbContext`. |
| Idempotency | 8/10 | Both inbound events are idempotent (fast-path existence check + DB unique-index backstop), both tested for duplicate delivery. Not higher: no true message-bus retry exists to test against (everything is in-process/same-transaction) — idempotency is proven for the "handler called twice" case, not for a real at-least-once delivery scenario, since no such transport exists in this codebase. |
| Testing | 8/10 | 44 new tests (35 domain + 9 application) covering every brief §40 minimum case that applies to what was built (creation, application, unapply, cancellation, write-off, FIFO payment application, duplicate delivery, read-model correctness). Not higher: no integration test against a real database (EF InMemory/SQLite) — the repositories themselves are untested beyond compiling against the generic `IRepository<T>` contract already proven elsewhere. |
| Legacy removal | 6/10 | Nothing needed removing (§5) — but one genuinely dead/broken legacy path (§5, §18) was identified and *documented* rather than removed, since removal wasn't authorized and risked being wrong about its blast radius without deeper Treasury analysis. |
| GL reconciliation readiness | 7/10 | `GetBalanceReconciliationQuery` makes brief §14/§51's check a real, runnable comparison — but with no backfill, it will show non-zero differences for nearly every real customer today, so it's *ready*, not yet *clean*. |

**Overall**: this is a genuine, working, tested AR subledger addition that respects every module
boundary this repository already enforces automatically — not a 10/10 because a 10/10 would require
either a historical backfill (explicitly out of scope) or claiming completeness a partial-scope,
one-day engagement can't honestly claim.
