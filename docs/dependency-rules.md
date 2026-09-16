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

## 3. Accepted exceptions

Architecture.Tests allow-lists are **empty** (Stage 1, 2026-09-17):

| List | File | Count |
|------|------|-------|
| Domain → Domain | `ModuleDependencyTests.AcceptedDomainExceptions` | 0 |
| Application → other Domain | `ModuleLayerDependencyTests.AcceptedApplicationDomainExceptions` | 0 |
| Application → other Application | `ModuleLayerDependencyTests.AcceptedApplicationApplicationExceptions` | 0 |

Cross-module communication is Contracts-only (`ICommand` / `IQuery` / integration events). SQL FKs remain via Fluent `HasOne(typeof(X)).WithMany().HasForeignKey("XId")` without Domain navigations.

Do **not** add an allow-list entry to make a test green. Add a Contracts query/command or drop the ProjectReference.

### Historical notes (closed)

Former Domain navigations (Dealer, Branch, Invoice, KeeperUser, Unit) were replaced with scalar IDs. Preference reads go through `GetPreferenceValueQuery`. Accounting posting goes through `Accounting.Contracts`. Reporting Application is Domain-free; live SQL lives in `Reporting.Infrastructure`.

There is no remaining legacy root `Domain`/`Application` project.

## 4. Definition of "violation" for CI purposes

A dependency is a violation if and only if:
1. It is a `ProjectReference` (or transitively-reachable namespace usage) from a `.Domain` or
   `.Application` project to another module's `.Domain`, `.Application`, or `.Infrastructure`
   project, **and**
2. It is not listed in §3 above with a corresponding `Tests/Architecture.Tests` allow-list entry.

`.Contracts` references are never violations. References to `OrgSys.SharedKernel` /
`OrgSys.EventBus` are never violations. The legacy root `Domain`/`Application` projects no longer exist.
