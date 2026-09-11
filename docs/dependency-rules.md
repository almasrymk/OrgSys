# OrgSys Dependency Rules

These rules are enforced mechanically by `Tests/Architecture.Tests` (see
`ModuleDependencyTests.cs` and `ModuleLayerDependencyTests.cs`). A violation fails the build/CI,
not just a code review.

## 1. Layer rules (within a single module)

| From | May depend on | May NOT depend on |
|---|---|---|
| `<Module>.Domain` | `OrgSys.SharedKernel` only | EF Core, ASP.NET Core, MediatR, its own `Application`/`Infrastructure`, **any** other module's `Domain`/`Application`/`Infrastructure` (except documented exceptions, §3) |
| `<Module>.Application` | its own `Domain`, its own `Contracts`, `OrgSys.SharedKernel`, `OrgSys.EventBus`, MediatR/AutoMapper/FluentValidation | its own or any module's `Infrastructure`; **any other module's `Domain`** (except documented exceptions, §3) |
| `<Module>.Contracts` | `OrgSys.SharedKernel` only | its own `Domain`/`Application`/`Infrastructure`, any other project — Contracts must be a leaf so every module can safely depend on every other module's Contracts without cycles |
| `<Module>.Infrastructure` | its own `Domain`, `Application`, `Contracts`; EF Core; other modules' `Infrastructure` only for cross-module EF `HasOne(typeof(...))` "no navigation" FK configuration (pre-existing pattern, unrelated to this brief) | another module's `Application` |
| `API` | any module's `Infrastructure` (for DI wiring) and, transitively, `Application`/`Contracts` | must not contain business rules, must not touch `DbContext`/repositories directly |

## 2. Cross-module communication (the actual point of this brief)

**Allowed:**
```
Sales.Application  ──▶  Inventory.Contracts   (MediatR command/query record)
Sales.Application  ──▶  Treasury.Contracts
Sales.Application  ──▶  Accounting.Contracts   (once Phase 4 introduces it)
```
The record types are message contracts (`ICommand`, `IQuery<T>`, etc. from
`OrgSys.SharedKernel`) — never EF entities. The *handler* for a contract lives in the owning
module's `Application` project and is discovered by MediatR's assembly scan at startup
(`Add<Module>Module()` in each module's `Infrastructure/DependencyInjection`); the caller needs no
compile-time reference to the handler's assembly, only to the `Contracts` project the message type
lives in.

**Forbidden:**
```
Sales.Application  ──▶  Inventory.Domain          (direct entity/repository access)
Sales.Application  ──▶  Inventory.Application     (reaching past Contracts into the handler layer)
Sales.Application  ──▶  Inventory.Infrastructure
<any>.Domain        ──▶  <any other>.Domain/Application/Infrastructure
```

## 3. Accepted exceptions (documented, tested, not silently allowed)

Every exception below is enforced as an explicit allow-list entry in
`Tests/Architecture.Tests` (`ModuleDependencyTests.AcceptedDomainExceptions` for Domain→Domain,
`ModuleLayerDependencyTests.AcceptedApplicationDomainExceptions` /
`AcceptedApplicationApplicationExceptions` for the Application-layer rules added in Phase 1) —
remove the entity/logic and the corresponding test entry should be deleted in the same change,
not left stale. Running the full Phase 1 sweep surfaced substantially more Application→Domain
coupling than the original (pre-Phase-1) audit had caught — the table below reflects what
actually exists today, not just what Phase 2 touched.

### Domain→Domain

| From | To | Reason |
|---|---|---|
| `Treasury.Domain` | `Sales.Domain` | `Financial.Dealer`, `FinancialInvoice.Invoice` EF navigations |
| `Inventory.Domain` | `Sales.Domain` | `Product.Dealer`, `Transaction.Dealer`, `Transaction.Order` EF navigations (the resolved Sales/Inventory circular coupling) |
| `Treasury.Domain`, `Accounting.Domain`, `Sales.Domain`, `Inventory.Domain` | `MasterData.Domain`, `Organization.Domain`, `Administration.Domain`, `Accounting.Domain` | Various EF navigations kept from the original schema (Branch, Shift, Currency, Account, etc.) |

### Application→Domain

| From | To | Reason |
|---|---|---|
| `Accounting.Application` | `Sales.Domain` | `IReceivableAccountValidator`/`IPayableAccountValidator` need `Dealer`/`DealerType`; kept in Accounting because Sales/Treasury/Payables/Receivables Application *and* the two validators mutually depend on it — moving it would add new edges for no benefit (see `Accounting.Application.csproj` comment) |
| `Sales.Application` | `Accounting.Domain` | Invoice/Dealer Get/Search handlers populate `JournalId`/`JournalCode` and GL-account provisioning reads `Account`/`Journal` directly — **Phase 4** ("Fix Sales -> Accounting integration") replaces this with `Accounting.Contracts` |
| `Sales.Application` | `MasterData.Domain` | `MappingProfile` `Unit`/`UnitDto` mapping |
| `Inventory.Application` | `Sales.Domain` | Transaction handlers/`MappingProfile` read `Dealer`/`Order` — mirrors the existing Domain-level exception, now visible at the Application layer too |
| `Inventory.Application` | `Accounting.Domain` | Transaction Get/Search handlers populate `JournalId`/`JournalCode`, same pattern as Sales — Phase-4-class debt |
| `Inventory.Application` | `Organization.Domain`, `MasterData.Domain` | `MappingProfile` Branch/Shift/Unit/Classification mapping |
| `Treasury.Application` | `Sales.Domain` | Financial handlers/`MappingProfile` read `Dealer` directly (mirrors `Treasury.Domain -> Sales.Domain`) |
| `Treasury.Application` | `Accounting.Domain` | FinancialTransfer/Financial Post/Reverse handlers and validators read `Account`/`Journal` directly — Phase-4-class debt |
| `Treasury.Application` | `MasterData.Domain` | `MappingProfile` and `CreateFinancialPaidInvoiceCommandHandler` read `Currency` |
| `Receivables.Application`, `Payables.Application` | `Accounting.Domain`, `Sales.Domain`, `MasterData.Domain` | The opening-balance handlers read `Journal`/`JournalType`/`Currency`/`DealerType` directly |
| `Administration.Application` | `Organization.Domain` | `MappingProfile` reads `Branch` (`User.BranchId`) — mirrors the existing Domain-level exception |
| `Reporting.Application` | `Sales.Domain`, `Treasury.Domain`, `Inventory.Domain`, `MasterData.Domain` | Reporting is a read-only cross-module aggregator by design (no `Reporting.Domain`) — reads every module's entities directly rather than duplicating read models yet |

### Application→Application

| From | To | Reason |
|---|---|---|
| `Sales.Application` | `MasterData.Application` | `UnitDto` mapping support |
| `Sales.Application` | `Accounting.Application` | Dealer create/update GL-account provisioning (`DealerPayableAccountProvisioning`/`DealerReceivableAccountProvisioning`) uses `IPayableAccountValidator`/`IReceivableAccountValidator` — Phase-4-class debt |
| `Inventory.Application` | `MasterData.Application` | `UnitDto`/`ProductDto` mapping support |
| `Payables.Application` | `Accounting.Application`, `Receivables.Application` | Opening-balance validators |
| `Receivables.Application`, `Treasury.Application` | `Accounting.Application` | Same validators / posting-period checks |

### Legacy root

| From | To | Reason |
|---|---|---|
| `Sales.Application`, `Inventory.Application`, `Receivables.Application`, `Payables.Application`, `Reporting.Application` | root `Domain`/`Application` (legacy monolith) | `Preference`, `IOrgContext`, the Journal-posting integration bridges — not yet extracted (Phase 8) |

**Cleared by this pass (Phase 2):** `Sales.Application -> Inventory.Domain`,
`Sales.Application -> Inventory.Application`, `Sales.Application -> Treasury.Domain` — all three
removed; replaced with `Sales.Application -> Inventory.Contracts` /
`Sales.Application -> Treasury.Contracts`. These three are **not** in the accepted-exceptions
list above — Architecture.Tests will fail immediately if they regress.

**Not touched by this pass, listed above only to keep the exception table honest:** every
Application→Domain/Application edge involving Accounting, Treasury, Inventory-to-Sales,
Receivables, Payables, Administration, or Reporting. These are real, pre-existing coupling that
Phase 1's new tests newly caught — fixing them is Phases 3-8 (per the brief's own phase
ordering), not this pass.

## 4. Definition of "violation" for CI purposes

A dependency is a violation if and only if:
1. It is a `ProjectReference` (or transitively-reachable namespace usage) from a `.Domain` or
   `.Application` project to another module's `.Domain`, `.Application`, or `.Infrastructure`
   project, **and**
2. It is not listed in §3 above with a corresponding `Tests/Architecture.Tests` allow-list entry.

`.Contracts` references are never violations. References to `OrgSys.SharedKernel` /
`OrgSys.EventBus` are never violations. References to the legacy root `Domain`/`Application`
projects are tracked (§3, last row) but not failed by Architecture.Tests today — they will start
being enforced once Phase 8 begins moving that code out, at which point each newly-empty
dependency should be removed from the exception list in the same change.
