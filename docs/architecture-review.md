# OrgSys Architecture Review — Modular Monolith Hardening

Date: 2026-09-11 (branch `Latest`, commit at time of writing: `a20754d3` and successors)

## 1. Current architecture (as of this review)

OrgSys already completed a first-pass Modular Monolith extraction (see
`docs/modular-monolith-analysis.md` / `docs/modular-monolith-target-architecture.md` for that
earlier effort). The physical shape is correct:

```
BuildingBlocks/OrgSys.SharedKernel, OrgSys.EventBus
Modules/{Administration,Organization,MasterData,Sales,Purchasing,Inventory,
         Receivables,Payables,Treasury,Accounting,Reporting}/
    <Module>.Domain / <Module>.Application / <Module>.Contracts / <Module>.Infrastructure
API/, OrgSys/ (legacy MVC), OrgSys.Angular/
Tests/{Application.Tests,Architecture.Tests}
Domain/, Application/, Infrastructure/  (legacy monolith remnants, shrinking)
```

Every module builds, `dotnet ef migrations has-pending-model-changes` is clean, and
`Tests/Architecture.Tests` already enforces **Domain-to-Domain** isolation with a documented
exception list. What it does **not** yet enforce — and what this review is about — is
**Application-to-Domain** and **Application-to-Infrastructure** isolation. The extraction was
done by moving files and satisfying the compiler, not by introducing Contracts. Several
`<Module>.Application` projects reach directly into another module's `<Module>.Domain` (and in
one case another module's `<Module>.Application`) via `IRepository<TEntity>`, which is exactly
the anti-pattern this brief is written against.

`<Module>.Contracts` projects exist for all 11 modules but are **empty shells** (`.csproj` only,
no `.cs` files) — the module boundary "front door" was scaffolded but never used.

## 2. Architecture score: **5.5 / 10**

Rationale:
- Physical module structure, Domain-to-Domain isolation, EF schema integrity, and build/test
  hygiene: strong (would score 8-9 alone).
- Application-layer coupling (the actual point of a modular monolith — letting modules evolve
  independently) is not enforced at all today; several modules reach into siblings' `Domain`
  entities directly.
- No Contracts, no Integration Events, no Outbox — cross-module writes happen via the same
  `IUnitOfWork`/`DbContext` a caller from a different module borrowed via `IServiceProvider`.
- Accounting is not the sole owner of journal posting — `Application.Commands.Org.Financials
  .Integration.JournalInvoice/JournalTransaction` (a *legacy root* bridge, not `Accounting.*`) is
  called directly from Sales/Inventory to write `Journal`/`JournalItem` rows.
- No multi-tenancy, no Outbox, no structured logging/correlation IDs — none of that exists in the
  codebase today (this is a from-scratch build-out, not a hardening).

These gaps are exactly why the score sits at the midpoint rather than higher or lower: the hard
part (getting business code physically located correctly, keeping the schema stable) is done; the
part this brief is asking for (message-based decoupling, Outbox, observability, multi-tenancy) has
not been started.

## 3. Exact violations (project/file level)

### 3.1 `Module.Application -> Module.Domain` (forbidden — brief §2)

| From | To | Why (file/usage) |
|---|---|---|
| `Sales.Application` | `Inventory.Domain` | `Sales.Application.csproj` references `Inventory.Domain.csproj` directly. Used by: `Invoices/Commands/CancelInvoiceCommandHandler.cs:43`, `RedoInvoiceCommandHandler.cs:42` (`IRepository<Inventory.Domain.Transaction>` via `IServiceProvider.GetRequiredService`), `Invoices/Commands/DeleteCommandHandler.cs`, `DeleteListCommandHandler.cs` (same, plus `IRepository<Treasury.Domain.Financial>`), `Invoices/Queries/GetByIdQueryHandler.cs:12` (`IRepository<Inventory.Domain.Product>`), `Invoices/Queries/SearchQueryHandler.cs:11` (`IRepository<Inventory.Domain.Stock>`) |
| `Sales.Application` | `Treasury.Domain` | Same `Sales.Application.csproj`; used by `DeleteCommandHandler.cs` / `DeleteListCommandHandler.cs` (`IRepository<Financial>`) |
| `Accounting.Application` | `Sales.Domain` | `Accounting.Application.csproj` -> `Sales.Domain.csproj`, for `ReceivablesPayablesAccountValidation/*.cs` (`Dealer`, `Sales.Domain.DealerType`) |
| `Reporting.Application` | `Sales.Domain`, `Treasury.Domain`, `Inventory.Domain` | Reporting is the one module the target architecture explicitly designs as a cross-cutting **read** aggregator with no Domain of its own (brief's own Reporting description: "read-oriented ... must not own transactional business logic"). Documented as an accepted, scoped exception — not touched by Phase 2. |

### 3.2 `Module.Application -> another Module.Application` (discouraged — brief's preferred graph is Contracts, not Application)

| From | To | Usage |
|---|---|---|
| `Sales.Application` | `Inventory.Application` | `Invoices/Commands/CreateCommandHandler.cs` — `using Inventory.Application.Transactions.Commands;` to reference `CreateTransactionByInvoiceCommand` — **fixed this pass** |
| `Sales.Application` | `Accounting.Application` | `Dealers/Commands/{CreateCommandHandler,UpdateCommandHandler,DealerPayableAccountProvisioning,DealerReceivableAccountProvisioning}.cs` use `IPayableAccountValidator`/`IReceivableAccountValidator`. Accepted exception — Phase 4 scope, not fixed this pass |
| `Sales.Application`, `Inventory.Application` | `MasterData.Application` | For `UnitDto`/`ProductDto` mapping support. Accepted exception (MasterData is the shared reference-data module every other module already treats as a safe dependency) |
| `Payables.Application` | `Accounting.Application`, `Receivables.Application` | For `IPayableAccountValidator`/`IReceivableAccountValidator`. Accepted exception |
| `Receivables.Application`, `Treasury.Application` | `Accounting.Application` | Same validators / posting-period checks. Accepted exception |

### 3.3 The full Application→Domain picture, once Phase 1's tests actually ran

The initial version of this review (before `Tests/Architecture.Tests` was extended and run) only
listed the violations found by manual `grep`. Running the new tests surfaced substantially more:
Treasury, Inventory (toward Sales), Payables, Receivables, and Administration all have their own
Application→Domain edges into other modules, on top of the ones above. None of these are fixed
by this pass — they are Phases 3-8 material per the brief's own ordering — but they are now all
enumerated and enforced (not silently allowed) in `docs/dependency-rules.md` §3 and
`Tests/Architecture.Tests/ModuleLayerDependencyTests.cs`. Only the three Sales.Application edges
listed in §3.1's first row have actually been removed.

### 3.3 No module Domain references another module's Application/Infrastructure

Checked (see raw survey below) — **zero violations** of this kind exist today. This part of the
target is already met.

### 3.4 Accounting is not the sole owner of journal posting

`InvoiceJournalIntegration` / `TransactionJournalIntegration`
(`Application/Commands/Org/Financials/Integration/JournalInvoice|JournalTransaction/*.cs`, in the
**legacy root** `Application` project, `public sealed class` — made public earlier only so
Sales/Inventory could keep calling it after the module split) write `Journal`/`JournalItem` rows
directly from Sales' and Inventory's own command handlers. This is real technical debt but is
**Phase 4 scope** (`Fix Sales -> Accounting integration`), not Phase 2 — flagged here, not touched
in this pass.

### 3.5 Legacy root business code still present

```
Domain/Entities/OrgDb/{LogSys,Notification,Preferences}.cs
Application/DTOs/OrgDb/{CheckEmailAndPasswordDto,CompanyProfileDto,LogSysDto,
                        NotificationDto,OrderTypeDto,PreferenceDto}.cs
Application/Commands/Org/Financials/Integration/{JournalInvoice,JournalTransaction}/*.cs
Application/Commands/Org/Setting/Preference/**
```

Small and manageable — this is **Phase 8 scope**. `CompanyProfileDto`/`OrderTypeDto` look
misplaced (Organization/Sales respectively) and should be triaged then, not now.

## 4. Target dependency graph (this phase)

```
                         ┌────────────────────┐
                         │   OrgSys.SharedKernel │  (CQRS, Result, IRepository, IUnitOfWork)
                         └──────────┬─────────┘
                                    │
      ┌─────────────────────────────┼─────────────────────────────┐
      │                             │                             │
Sales.Domain                Inventory.Domain               Treasury.Domain  ...
      │                             │                             │
Sales.Application ──────▶ Inventory.Contracts        Treasury.Contracts ◀── Sales.Application
      │                             ▲                             ▲
      │                             │                             │
      └─────────────────────▶ Inventory.Application ───▶ (implements contracts)
                                     │
                          Treasury.Application ───▶ (implements contracts)
```

`Sales.Application` depends only on `Inventory.Contracts` / `Treasury.Contracts` (MediatR message
contracts, not entities). `Inventory.Application` / `Treasury.Application` implement the
contracts' handlers and keep exclusive write access to their own `Domain` entities. MediatR's
`ISender` is resolved from the shared DI container at runtime — no compile-time reference to the
other module's `Application`/`Domain`/`Infrastructure` project is needed for Sales to invoke a
handler that lives in Inventory or Treasury.

## 5. Migration phases (as specified)

1. Architecture analysis + dependency tests — **this document + Architecture.Tests additions**
2. Remove illegal cross-module references from `Sales.Application` — **this pass**
3. Fix Sales -> Inventory integration — folded into Phase 2 (Contracts introduced together)
4. Fix Sales -> Accounting integration — not started (journal-posting bridge stays as-is)
5. Fix Sales -> Receivables integration — not started (no such reference exists today: Sales
   never touches Receivables directly, since AR wasn't wired into Sales yet)
6. Fix Sales -> Treasury integration — folded into Phase 2
7. Apply the same pattern to Purchasing — not started (Purchasing has no business code yet, see
   `docs/module-ownership.md`)
8. Migrate remaining legacy Domain/Application code — not started
9. Outbox + idempotency — not started
10. Angular alignment — not started
11. Freeze/remove MVC — not started

## 6. First safe refactoring step

Introduce four narrow, purpose-built MediatR contracts (not generic CRUD-y "repository facades")
that replace the *exact* five call sites where `Sales.Application` reaches into
`Inventory.Domain`/`Inventory.Application`/`Treasury.Domain`:

1. `Inventory.Contracts.Transactions.CreateTransactionByInvoiceCommand` (moved, unchanged shape)
2. `Inventory.Contracts.Transactions.SetTransactionStatusCommand`
3. `Inventory.Contracts.Transactions.DeleteTransactionByInvoiceCommand`
4. `Inventory.Contracts.Products.GetProductNamesQuery`
5. `Inventory.Contracts.Stocks.GetStockNamesQuery`
6. `Treasury.Contracts.Financials.DeleteFinancialsByInvoiceCommand`

Each is handled by a new/updated handler *inside* the owning module's `Application` project. This
is minimal-surface-area, behavior-preserving (same SQL, same transaction boundaries via the
already-shared `IUnitOfWork`), and lets `Sales.Application.csproj` drop its `Inventory.Domain`,
`Inventory.Application`, and `Treasury.Domain` project references entirely in favor of
`Inventory.Contracts` and `Treasury.Contracts`.

See `docs/dependency-rules.md` for the enforced rule set and `docs/module-ownership.md` for the
full ownership table.
