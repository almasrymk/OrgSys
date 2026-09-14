# Accounts Receivable — Phase 0 Analysis (AS-IS Inventory & Migration Plan)

Analysis only — no production code changed by this document. Gathered by direct inspection
(Read/Grep) of `D:\Work\MK\Source\OrgSys` on branch `Latest`, 2026-09-14, on top of commits through
`8e41267a` (Accounting bounded-context isolation).

## 0. Executive summary — the one fact that changes everything below

**`Modules/Receivables` is not a half-built subledger. It is a deliberately thin, already-shipped
read/write facade over the General Ledger.** There is no per-invoice open-item table anywhere in
the system. Today, "Accounts Receivable" *is*:

- **Postings**: `CommercialDocuments.Application.Invoices.Integration.InvoiceJournalPostingService`
  posts one balanced `Journal` (via `Accounting.Contracts.Postings.PostAccountingDocumentCommand`)
  straight to the customer's `Dealer.AccountId` GL account when an invoice is created/updated.
- **Settlement**: `Treasury.Domain.Entities.Financial` (a customer receipt) +
  `Treasury.Domain.Entities.FinancialInvoice` (a `Financial` ↔ `Invoice` join row with its own
  `Amount`) already **is** a working payment-allocation mechanism — one receipt can be split across
  many invoices — and it posts its own `Journal` via the same `Accounting.Contracts.Postings`
  bridge.
- **Balance** (`Receivables.Application/Balances/Queries/GetCustomerBalanceQueryHandler.cs`):
  `Sum(Debit − Credit)` of the customer's `Journal`/`JournalItem` history, read live through
  `Accounting.Contracts.Postings.GetAccountActivityQuery`.
- **Aging** (`GetCustomerAgingQueryHandler.cs`): FIFO-matches each GL debit (a charge) against
  later GL credits (a receipt) on the same account, live, on every call — no stored aging bucket,
  no stored "outstanding amount" column anywhere.
- **Opening balance** (`SetCustomerOpeningBalanceCommandHandler.cs`): delegates entirely to
  `Accounting.Contracts.Postings.SetOpeningBalanceLineCommand` — one shared per-fiscal-year Journal
  line per customer, no Receivables-owned row.

`Receivables.Domain` contains a single `AssemblyMarker` class. `Receivables.Infrastructure`
contains only DI registration — no `DbContext`, no `DbSet`, no EF configuration, no migration, no
repository. This is **the accepted, working design**, not an oversight: it is called out explicitly
in code comments (`GetCustomerAgingQueryHandler.cs:10-18`) and confirmed as an intentional,
previously-scoped deferral in `docs/shared-business-capabilities-review.md` §4 ("Receivables/
Payables' emptiness is a feature gap, not a shared-concept-in-the-wrong-place problem... building
real Receivables/Payables domain models is a separate initiative, outside this decomposition
task's scope"). **This task is that separate initiative.**

### Why this matters for the brief that requested this work

The brief this document was commissioned under assumes AR starts from either a naive CRUD
implementation or nothing, and mandates (§14, §16, §51) that AR maintain its **own** open-item
ledger, independent of the GL, as the subledger source of truth, reconciled against a GL control
account. The current system does the opposite by design: **the GL *is* the subledger**, and
Receivables is a query/command facade in front of it. Per the brief's own §45 instruction
("When this specification conflicts with actual repository facts: 1. Prefer repository facts. 2.
Preserve DDD boundaries. 3. Document the discrepancy and chosen solution here"), that is what the
rest of this document does — and it recommends a resolution in §9 that keeps the GL as the single
posting authority while giving Receivables genuine aggregate-owned state, rather than replacing or
duplicating the two production systems (Journal posting, Financial/FinancialInvoice allocation)
that already move real money correctly today.

---

## 1. Current State Inventory — `Modules/Receivables`

| File | Layer | Purpose | Depends on | Belongs in AR? | DDD violation? | Action |
|---|---|---|---|---|---|---|
| `Receivables.Domain/AssemblyMarker.cs` | Domain | Reflection anchor only; no entities | SharedKernel | Yes (anchor stays) | No — Domain is simply empty | KEEP |
| `Receivables.Contracts/Balances/GetCustomerBalanceQuery.cs` | Contracts | Public query: `(DealerId, AsOfDate?) -> decimal` | SharedKernel | Yes | No | KEEP (signature reusable once a subledger exists) |
| `Receivables.Contracts/Balances/GetCustomerAgingQuery.cs` | Contracts | Public query: `(DealerId, AsOfDate?) -> AgingBucketDto` | SharedKernel | Yes | No | KEEP |
| `Receivables.Contracts/Balances/AgingBucketDto.cs` | Contracts | 4-bucket aging DTO (Current/31-60/61-90/90+) | none | Yes | No | KEEP — matches brief §17 bucket scheme already |
| `Receivables.Application/Balances/Queries/GetCustomerBalanceQueryHandler.cs` | Application | Computes balance from `GetAccountActivityQuery` | `Accounting.Contracts`, `IReceivableAccountValidator` | Yes | No (correct Contracts-only boundary) | KEEP as legacy/compat read path during migration; REPLACE as primary source once Receivable aggregate exists (§9) |
| `Receivables.Application/Balances/Queries/GetCustomerAgingQueryHandler.cs` | Application | FIFO-ages GL activity live | same | Yes | No | Same as above — KEEP short-term, REPLACE long-term |
| `Receivables.Application/OpeningBalance/Commands/SetCustomerOpeningBalanceCommandHandler.cs` | Application | Sets one shared opening-balance Journal line per customer | `Accounting.Contracts`, `Administration.Domain` (Preference), `Parties.Contracts` (Dealer) | Yes | **Yes** — direct `Administration.Domain` reference (brief §27) | REFACTOR (remove Administration.Domain dependency — see §7 below); keep the GL-delegation behavior |
| `Receivables.Infrastructure/DependencyInjection/ServiceCollectionExtensions.cs` | Infrastructure | MediatR + FluentValidation registration only | `Receivables.Application` | Yes | No | KEEP, EXTEND (register new EF persistence in Phase 3) |
| `Receivables.Application/GlobalUsings.cs` | Application | `global using Administration.Domain;` | — | — | Symptom of the same §27 issue | REMOVE once §7 resolved |

**No other files exist under `Modules/Receivables`.** There is no `ReceivablesDbContext`, no
repository interface, no EF configuration, no controller, no Angular AR screen dedicated to
open items (verified: no `receivable`-named component under `OrgSys.Angular/src`).

## 2. Relevant AR behavior living *outside* `Modules/Receivables`

| Concept | Current Location | Correct Owner (target) | Action |
|---|---|---|---|
| Customer identity (`Dealer`, `TypeId=1`) | `Parties.Domain.Dealer` | Parties (unchanged) | KEEP — Receivables references `CustomerId`/`DealerId` (a `long`) only, never `Dealer` |
| `Dealer.AccountId` (link to the customer's GL receivable account) | `Parties.Domain.Dealer` | Parties (unchanged) | KEEP — this is how `IReceivableAccountValidator` resolves "the customer's receivable account" today; still needed under the target design |
| Invoice commercial content | `CommercialDocuments.Domain.Invoice`/`InvoiceProduct`/`InvoiceType` | CommercialDocuments (unchanged; moved out of `Sales.Domain` on 2026-09-13) | KEEP — Receivables must reference `SourceDocumentId`/`SourceDocumentType`/`SourceDocumentNumber` only, never `Invoice` |
| Invoice → GL posting | `CommercialDocuments.Application.Invoices.Integration.InvoiceJournalPostingService`, called from `CreateJournalByInvoiceCommandHandler` | CommercialDocuments → Accounting.Contracts (unchanged) | KEEP as the **sole** canonical AR posting path (brief §13's "one canonical accounting posting path" requirement). Receivables must **listen**, never re-post |
| Customer receipt + payment allocation | `Treasury.Domain.Entities.{Financial,FinancialInvoice}`, `Treasury.Application.Financials.Command.CreateFinancialPaidInvoiceHandler`/`PostTransactionCommandHandler` | Treasury (unchanged) | KEEP as the **sole** canonical settlement posting path. Receivables must **listen**, never re-post or re-allocate |
| GL account validation for a customer | `Accounting.Application.ReceivablesPayablesAccountValidation.ReceivableAccountValidator` behind `Accounting.Contracts.Accounts.IReceivableAccountValidator` | Accounting.Contracts (unchanged) | KEEP — already the correct Contracts-only boundary |
| Opening-balance journal mechanics | `Accounting.Application.Postings.SetOpeningBalanceLineCommandHandler` behind `Accounting.Contracts.Postings.SetOpeningBalanceLineCommand` | Accounting.Contracts (unchanged) | KEEP |
| Account activity read (`GetAccountActivityQuery`) | `Accounting.Application.Postings.GetAccountActivityQueryHandler` behind `Accounting.Contracts.Postings.GetAccountActivityQuery` | Accounting.Contracts (unchanged) | KEEP — remains the source for the legacy/compat balance & aging handlers during migration |
| Legacy root AR code (`OrgSys/` MVC project) | none found | n/a | No `Application/Commands/**/Receivable*` or AR-specific legacy code exists — the root `Application`/`Domain`/`Infrastructure` projects were already deleted (commits `2bf55f57`, `9aa50f81`, `41ddba90`) |

**No duplicate Customer, Invoice, Payment, or JournalEntry concept exists anywhere.** The brief's
"do not create duplicate X" guardrails (§0, §43) are, today, fully satisfied — the risk is entirely
on the *implementation* side: building a naive open-item ledger per the brief's literal instructions
would be the thing that creates the first duplicate (a second, competing source of "what does this
customer owe").

## 3. Bounded-context ownership map (verified, not assumed)

| Concept | Owner today | Owner in target design | Notes |
|---|---|---|---|
| Customer master data | Parties | Parties | No change |
| Sales/credit invoice content | CommercialDocuments | CommercialDocuments | No change |
| Invoice → GL posting | CommercialDocuments + Accounting.Contracts | unchanged | Sole canonical posting path |
| Cash/bank/receipt movement | Treasury | Treasury | No change |
| Receipt → invoice allocation | Treasury (`FinancialInvoice`) | Treasury (unchanged) | Sole canonical allocation *posting* path — see §9 for how Receivables mirrors this without re-implementing it |
| Chart of accounts / Journal / FiscalYear / FiscalPeriod | Accounting | Accounting | No change — `Accounting.Domain` untouched by this work |
| Customer AR **subledger read model** (balance, aging, statement, overdue) | Receivables (GL-derived, live) | Receivables (**own** open-item projection, event-fed) | The actual scope of this initiative |
| Customer AR **open-item lifecycle** (create/settle/cancel/write-off as first-class state) | Does not exist | Receivables (new) | Net-new — see §5 |

## 4. Dependency graph — current vs. target

**Current (verified from the 3 non-empty `.csproj` files):**

```
Receivables.Domain        -> SharedKernel                                         [CLEAN]
Receivables.Contracts     -> SharedKernel                                         [CLEAN]
Receivables.Application   -> Receivables.Domain, Receivables.Contracts,
                              OrgSys.EventBus, Accounting.Contracts,
                              MasterData.Contracts, Administration.Domain         [1 VIOLATION]
Receivables.Infrastructure-> Receivables.Application                              [CLEAN]
```

**Target:**

```
Receivables.Domain        -> SharedKernel only                                    (already true)
Receivables.Contracts     -> SharedKernel only                                    (already true)
Receivables.Application   -> Receivables.Domain, Receivables.Contracts,
                              OrgSys.EventBus, Accounting.Contracts,
                              MasterData.Contracts, Administration.Contracts*      (*new, see §7)
Receivables.Infrastructure-> Receivables.Application, Receivables.Domain (EF only)
```

The only project-reference change required is **Administration.Domain → Administration.Contracts**
(§7). Everything else in the current dependency graph already matches the brief's target shape.

### Architecture.Tests — already covers Receivables generically

`Tests/Architecture.Tests/{ModuleDependencyTests,ModuleLayerDependencyTests,
ModuleInfrastructureDependencyTests}.cs` already run their `NotHaveDependencyOn` assertions against
**every** module pair, `Receivables` included, via `NetArchTest`. The brief's §41 ask ("add
enforceable rules... automate them") is **already done** for the generic cross-module boundary —
the only currently-registered exception for Receivables is:

```csharp
("Receivables", "Administration", "SetCustomerOpeningBalanceCommandHandler reads Preference
directly — same relocation-not-rewrite as above.")
```

in `ModuleLayerDependencyTests.AcceptedApplicationDomainExceptions`. Closing §7 below removes this
one line and requires no new NetArchTest infrastructure — only new Domain-purity/aggregate tests
for the new `Receivable` aggregate itself (brief §41's "Domain must not depend on EF Core/MediatR"
rule already runs against `Receivables.Domain` today and will continue to pass once entities are
added, since it is instantiated as a blanket assertion, not scoped to existing types).

## 5. Sources searched, confirmed empty/not-present

A repository-wide search (respecting the note that one `Grep` timed out on an unscoped run — all
searches below were re-run scoped to `Modules/`, `API/`, `docs/`, and the legacy `OrgSys/` project)
found no matches for, i.e. none of these exist anywhere in the codebase today:

`CustomerBalance` (as a type), `CustomerOpeningBalance` (as a type — only the Command class),
`CustomerAccount`, `Allocation` (as a type), `CreditNote`, `DebitNote`, `FinancialTransaction` (as
a type name — the actual entity is `Financial`), `IReceivableAccountValidator` implementers beyond
`Accounting.Application.ReceivablesPayablesAccountValidation.ReceivableAccountValidator`,
`SalesInvoicePostedIntegrationEvent`, `CustomerPaymentReceivedIntegrationEvent`, any
inbox/outbox/idempotency-key infrastructure in `OrgSys.EventBus` (it is 3 files: `IIntegrationEvent`,
`IIntegrationEventPublisher`, `MediatrIntegrationEventPublisher` — an in-process MediatR
notification bridge, no persistence, no delivery guarantee, no dedup).

**Implication**: idempotency for the new inbound integration (§9 Phase 4/6) must be enforced by a
database unique constraint on the new `Receivable` table (`SourceDocumentType + SourceDocumentId`
+ tenant scope), exactly as the brief's §32 fallback describes — there is no existing
outbox/inbox to reuse, and building a generic one is out of scope (would be exactly the kind of
speculative infrastructure the brief's own anti-over-engineering rule, and this repo's established
"§20 anti-over-split rule," warn against).

## 6. Multi-tenancy / audit / money conventions to reuse (not invent)

- IDs are plain `long` (EF identity), not `Guid`, not strongly-typed ID wrappers — confirmed
  consistently across `Accounting.Domain.{Account,Journal,JournalItem}`, `Treasury.Domain.*`,
  `Parties.Domain.Dealer`. **Decision**: `Receivable` uses a plain `long Id` via `BaseModel`, not a
  new `ReceivableId` value type — introducing strongly-typed IDs now would be the one inconsistent
  aggregate in the whole solution for no behavioral gain (brief §6 explicitly permits this call).
- Aggregates inherit `OrgSys.SharedKernel.BaseModel` (`Id, CodeNumber, Code, TypeId, Status, ...`)
  or `MovementModel` (adds `Date, CreateUserId, CreateDate, ModifyUserId, ModifyDate, ShiftId,
  BranchId, HasJournal, Review, Posted`) — see `Accounting.Domain.Journal` for the canonical
  pattern this task should mirror: `protected` constructor + static factory methods, private
  setters, an internal `List<IDomainEvent>` exposed as `IReadOnlyList`, `Ensure*` guard methods
  throwing a module-specific typed exception (`AccountingDomainException` subtypes).
- No multi-tenant/company-scoping column exists on `MovementModel`/`BaseModel` beyond `BranchId` —
  confirmed the whole solution is single-tenant with branch-level scoping only. The unique
  source-document index therefore scopes on `(SourceDocumentType, SourceDocumentId)` alone, not a
  tenant column that doesn't exist.
- Money columns are `decimal(18,2)` everywhere (`Financial.Amount`, `JournalItem.Debit/Credit`) —
  `Receivable` will match.
- No `Money`/`CurrencyCode` value object exists in `SharedKernel` — currency is always a bare
  `CurrencyId (long)` + `Rate (decimal)` pair (see `Journal.CurrencyId`/`Journal.Rate`,
  `Financial.CurrencyId`/`Financial.Rate`). `Receivable` will match this, not invent a `Money` VO.

## 7. Administration.Domain dependency (brief §27) — root cause and fix

`SetCustomerOpeningBalanceCommandHandler` injects `IRepository<Preference>` from
`Administration.Domain` to read two Preference rows (`Reference="Dealer", TypeId=Client,
Key="OpeningBalanceClearingAccountId"`). `Administration.Contracts` currently has **zero** source
files. Five other modules (CommercialDocuments, Inventory, Treasury, Parties, Payables) have the
exact same accepted, documented exception for the same reason — this is a repo-wide pattern, not a
Receivables-specific mistake, and per `ModuleLayerDependencyTests.AcceptedApplicationDomainExceptions`
was a deliberate "relocate, don't rewrite" call made on 2026-09-13 when `Preference` moved out of
the legacy root project.

**Fix scoped to this task**: add a minimal `Administration.Contracts.Preferences.GetPreferenceQuery`
(or a narrower `GetOpeningBalanceClearingAccountQuery`) + handler in `Administration.Application`,
and switch `SetCustomerOpeningBalanceCommandHandler` to use it via `ISender`, matching the exact
pattern `Accounting.Contracts.Postings` already establishes for cross-module reads. This closes the
one real boundary gap Receivables owns, without touching the other five modules' identical
exception (out of scope — flagged, not fixed, per the brief's "don't rewrite unrelated modules"
instruction).

## 8. Sales vs. cash — confirmed rule

`InvoiceJournalPostingService` posts to `dealerAccountId` for **every** invoice with
`AccountsIntegration` enabled in preferences — there is no branch on payment terms/cash-vs-credit
in the posting bridge itself; whether an invoice is "credit" is a property of whether/when a
`Financial`+`FinancialInvoice` receipt later clears it, not a separate invoice field. **Decision**:
Receivables treats every posted, AR-account-integrated invoice as producing an open item (matching
current GL behavior exactly); a same-day full payment simply arrives as a `FinancialInvoice`
allocation that immediately settles it — this mirrors real-world "cash sale posted through AR then
immediately cleared" accounting, which is what the current Journal history already shows, per
brief §12's own fallback ("If current OrgSys accounting workflow posts cash sale through AR
temporarily, preserve accounting correctness but document the rule").

## 9. Recommended target design (resolves §0's discrepancy)

Keep **Journal posting** (CommercialDocuments → Accounting.Contracts) and **payment allocation**
(Treasury `Financial`/`FinancialInvoice` → Accounting.Contracts) exactly as they are today — they
are correct, tested-by-production-use, and single-owner. Do **not** move GL posting responsibility
into Receivables, and do **not** have Receivables re-derive its own competing Journal lines.

Add a genuinely new, Receivables-owned **projection/subledger** that listens to the *same*
lifecycle moments CommercialDocuments/Treasury already act on, via new integration events raised
alongside (not instead of) the existing GL posting calls:

```
Invoice posted (InvoiceJournalPostingService.SyncAsync succeeds, HasJournal=true)
   -> CommercialDocuments raises ReceivableInvoicePostedIntegrationEvent (new, minimal payload)
   -> Receivables.Application creates Receivable (idempotent on SourceDocumentType+SourceDocumentId)

Receipt allocated (FinancialInvoice row created/posted against an Invoice)
   -> Treasury raises CustomerPaymentAppliedIntegrationEvent (new, minimal payload)
   -> Receivables.Application applies the allocation to the matching Receivable
      (idempotent on a natural key: FinancialInvoice.Id)
```

This gives Receivables a real aggregate with real invariants (brief §5) without duplicating GL
posting or payment-allocation logic — those integration events are raised *from inside* the
existing, unchanged posting/allocation code paths, as a side effect, not a parallel decision-maker.
The **existing GL-derived `GetCustomerBalanceQuery`/`GetCustomerAgingQuery` handlers stay in place
unmodified** as the audited, always-correct-by-construction reconciliation baseline (brief §14's
"AR Control Account GL Balance == AR Subledger Balance" check becomes a real, runnable comparison
between the new subledger-derived balance and this existing GL-derived one — not a hoped-for
invariant).

This is a materially larger, higher-risk change than "add a CRUD entity": it requires emitting new
events from `CommercialDocuments.Application` and `Treasury.Application` (both outside
`Modules/Receivables`), a new EF `DbContext`/migration (the first one Receivables has ever had), and
a backfill strategy for pre-existing invoices/receipts if historical open-item data is required (out
of scope for this pass — flagged as a Phase 9/follow-up item, matching brief §15's "preserve
existing posted opening balances... create a migration strategy rather than silently duplicating
historical rows" guidance: new Receivables rows start from the point this ships forward; the
GL remains the historical source of truth for anything before that).

## 10. Risks

- **Financial correctness risk**: any bug in the new event-driven projection could show a customer
  balance in Receivables' new tables that disagrees with the GL-derived query. Mitigated by keeping
  the GL-derived queries live as the reconciliation baseline (§9) rather than replacing them
  outright in this pass.
- **Scope risk**: full brief compliance (write-offs, credit notes, unapplied-credit workflows,
  statements) is a multi-week effort touching 4 modules. This document recommends a phased subset
  (§11) rather than attempting all of it in one pass.
- **No existing test coverage** for Receivables at all today (`Tests/Architecture.Tests` only
  covers boundary rules; no `Receivables.Domain.Tests`/`Receivables.Application.Tests` project
  exists) — the new aggregate is the first thing in this module that needs one.

## 11. Proposed phase sequencing for this engagement

1. **Phase 1** — close the `Administration.Domain` boundary gap (§7). Build, run
   `Architecture.Tests`.
2. **Phase 2** — `Receivable` aggregate in `Receivables.Domain` (Create/Apply/Unapply/Cancel/
   WriteOff, invariants per brief §5, domain events) + a new `Receivables.Domain.Tests` project
   covering the brief's §40 minimum list. No persistence yet.
3. **Phase 3** — EF persistence: `ReceivablesDbContext`, `ReceivableConfiguration`, unique index on
   `(SourceDocumentType, SourceDocumentId)`, first-ever Receivables migration.
4. **Phase 4** — inbound integration: new `ReceivableInvoicePostedIntegrationEvent` raised from
   `CommercialDocuments.Application` (alongside existing posting, not replacing it), consumed
   idempotently by Receivables. Duplicate-delivery test.
5. **Phase 6** (payment application) and **Phase 7** (read models: outstanding/overdue/statement
   backed by the new table, aging bucket cross-check against the GL-derived query) as fast-follow
   once 1-4 are proven — recommended as separate approval checkpoints given the size, rather than
   committed to in this same pass.

Phases 5 (opening balance reconciliation), 8 (write-offs/credit notes), and the full brief-mandated
9/10 (legacy cleanup, final verification) are **not** started in this pass — there is no legacy AR
implementation to clean up (§2 confirmed), and opening balances / write-offs depend on the Phase 2-4
aggregate existing first.

## 12. Implementation status (as built, this pass)

User-approved scope for this engagement: Phases 1-4 only, with cross-module edits to
`CommercialDocuments.Application` permitted to raise the new integration event. Phases 5-7
(payment application via Treasury, opening-balance reconciliation, read-model replacement) were
**not** attempted — the existing GL-derived `GetCustomerBalanceQuery`/`GetCustomerAgingQuery`
handlers are untouched and remain the only balance/aging source today.

**Phase 1 — boundary cleanup.** Added `Administration.Contracts.Preferences.GetPreferenceValueQuery`
+ handler; `SetCustomerOpeningBalanceCommandHandler` now resolves the opening-balance clearing
account through it instead of `IRepository<Preference>`. `Receivables.Application.csproj`'s
`Administration.Domain` reference replaced with `Administration.Contracts`. Removed the now-closed
exception from `ModuleLayerDependencyTests.AcceptedApplicationDomainExceptions`.

**Phase 2 — Receivable aggregate.** `Receivables.Domain.Receivable` (`Create`/`Apply`/`Unapply`/
`Cancel`/`WriteOff`), `ReceivableStatus`, `SourceDocumentType` (currently `SalesInvoice` only),
5 domain events, `ReceivableDomainException` hierarchy. New `Tests/Receivables.Domain.Tests`
project, 29 tests covering the brief §40 minimum list (creation invariants, full/partial
application, unapply, cancellation, write-off, derived `IsOverdue`).

Design deviations from the brief, forced by this repo's actual conventions (documented inline in
code comments too): `Receivable : MovementModel`, not a bespoke aggregate base — the existing
generic `IRepository<TEntity> where TEntity : BaseModel` is the only persistence abstraction in the
codebase (brief §22/§23 "reuse existing architecture, don't invent a second one" left no other
option once `OrgSys.SharedKernel.AggregateRoot` proved incompatible with that constraint). Its
lifecycle enum is exposed as `LifecycleStatus`, not `Status` (the inherited `BaseModel.Status` is a
generic, unrelated enum — reusing the name would have hidden one or the other). IDs are plain
`long`, matching every other entity in the solution — no `ReceivableId` value type.

**Phase 3 — persistence.** No new `ReceivablesDbContext` — this codebase has exactly one, shared
`OrgContext` (`BuildingBlocks/OrgSys.DatabaseMigrator/Persistence/OrgContext.cs`) for every module;
Accounting/Treasury/every other module follow the same pattern (a `DbSet<T>` + Fluent config there,
plus an aggregate-shaped repository in the module's own `.Infrastructure` composed over the generic
`IRepository<T>` — see `Accounting.Infrastructure.Persistence.JournalRepository` for the precedent
`Receivables.Infrastructure.Persistence.ReceivableRepository` mirrors exactly). Added: `DbSet<Receivable>`,
a unique index on `(SourceDocumentType, SourceDocumentId)` (the idempotency guard), `IReceivableRepository`
in `Receivables.Domain.Repositories`, `ReceivableRepository` in `Receivables.Infrastructure.Persistence`,
registered in `AddReceivablesModule`. Migration `20260914045620_AddReceivablesModule` generated
(additive only — one new table, no changes to any existing table/column) but **not yet applied** to
the configured remote database (`API/appsettings.json`'s `OrgConnection`) — applying a migration
against a live, shared, non-local database is treated as a user-confirmed action, not something to
do unilaterally; see the message accompanying this document.

**Phase 4 — inbound integration.** New `CommercialDocuments.Contracts.IntegrationEvents.
SalesInvoicePostedIntegrationEvent`, published from `CommercialDocuments.Application.Invoices.
Commands.CreateCommandHandler` — only for `InvoiceTypeId.Sales` invoices that reached
`HasJournal = true` — **before** that handler's own `CommitAsync()`, so a failure in Receivables
rolls back the whole invoice-creation transaction rather than leaving GL posting committed with no
AR open item (matches `OrgSys.EventBus.IIntegrationEvent`'s documented same-transaction intent, and
avoids a confusing "invoice succeeded but the request failed" outcome). Consumed by new
`Receivables.Application.Invoices.Integration.SalesInvoicePostedIntegrationEventHandler`:
idempotent on `ExistsForSourceDocumentAsync` (fast path) with the Phase 3 unique index as the DB-level
backstop; skips zero/negative-amount invoices (a fully-discounted invoice creates no AR obligation)
instead of letting `Receivable.Create`'s guard throw mid-pipeline. 3 new tests in
`Tests/Application.Tests/SalesInvoicePostedIntegrationEventHandlerTests.cs` (create, duplicate
delivery, zero-amount) using the same Moq pattern as the existing `JournalPostCommandHandlerTests`.

**Known limitation carried forward, not fixed this pass**: `Invoice` has no payment-terms/due-date
field anywhere in the system (verified), so `SalesInvoicePostedIntegrationEvent.DueDate` defaults to
the invoice date — every Receivable is "due immediately" until payment-terms support is added
(tracked as a follow-up, not invented here per the brief's own anti-speculative-feature rule).

**Verification**: `dotnet build OrgSys.sln` — 0 errors. `dotnet test OrgSys.sln` — 1217/1217 passed
across `Receivables.Domain.Tests` (29), `Accounting.Domain.Tests` (54, unaffected), `Application.Tests`
(72, +3 new), `Architecture.Tests` (1062, unaffected — the generic module-boundary rules already
covered `Receivables` before this pass; only the one `Administration` exception line changed).

**Not done, by design (Phases 5-7, deferred)**: payment application (no `CustomerPayment`/allocation
wiring from Treasury's `Financial`/`FinancialInvoice`), opening-balance reconciliation with the new
subledger, and replacing the GL-derived balance/aging queries with subledger-backed ones (plus the
control-account cross-check that would enable). The existing `GetCustomerBalanceQuery`/
`GetCustomerAgingQuery` behavior is completely unchanged — Receivables today has two independent,
not-yet-reconciled views of AR (the long-standing GL-derived one, and the new open-item table this
pass adds), which is expected and documented, not a bug: §9's reconciliation check has no data to
compare yet because nothing writes to the new table except newly-posted sales invoices going
forward.
