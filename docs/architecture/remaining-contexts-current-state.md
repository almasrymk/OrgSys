# Remaining Bounded Contexts — Phase 0 Discovery Report

Scope: Budgeting & Cost Management, Organization/Administration completion, Identity & Access
Management, Workflow & Approvals, Reporting/Analytics, SaaS/Tenant Management. Analysis only — no
source, config, or migration files were modified to produce this report. Findings are backed by
direct inspection of `D:\Work\MK\Source\OrgSys`, branch `Latest`, as of 2026-09-16.

This report builds on, and does not repeat, the existing per-context Phase 0 reports already in
this repo: [`docs/organization/organization-current-state.md`](../organization/organization-current-state.md),
[`docs/catalog/catalog-current-state.md`](../catalog/catalog-current-state.md),
[`docs/parties/party-current-state.md`](../parties/party-current-state.md),
[`docs/ddd/inventory-current-state.md`](../ddd/inventory-current-state.md). Their methodology
(status legend, ownership table, migration risk table) is reused here without modification.

---

## 1. Current architecture discovered

- **Solution shape**: `OrgSys.sln` + `BuildingBlocks/` (`OrgSys.SharedKernel`, `OrgSys.Infrastructure`,
  `OrgSys.EventBus`, `OrgSys.Localization`, `OrgSys.DatabaseMigrator`) + `Modules/*` (14 bounded
  contexts today: Accounting, Administration, Advances, Catalog, CommercialDocuments, Inventory,
  MasterData, Organization, Parties, Payables, Purchasing, Receivables, Reporting, Sales, Treasury —
  Reporting has no `.Domain`, by design) + `Host`/`API` (composition root) + `Tests/`. Legacy root
  `Domain`/`Infrastructure`/`Host` folders are near-empty shells from a prior extraction pass (only
  `obj`/`bin`/`Migrations`/`Persistence` remain) — not a live competing architecture, safe to ignore.
- **Per-module project layout**: exactly 4 csproj — `<Module>.Domain`, `.Application`,
  `.Infrastructure`, `.Contracts` (Reporting: 3, no Domain). Every module has an `AssemblyMarker`
  class used by `Architecture.Tests` reflection.
- **Entity base classes**: `BaseModel` (`Id`, `CodeNumber`, `Code`, `MaskText`, `ParentId`,
  `TypeId`, `Hide`, `ImgPath`, `Status`) for master data; `MovementModel : BaseModel` (adds `Date`,
  `CreateUserId`, `CreateDate`, `ModifyUserId`, `ModifyDate`, `ShiftId`, `BranchId`, `HasJournal`,
  `Review`, `Posted`) for transactional documents. Both live in `OrgSys.SharedKernel`, deliberately
  free of cross-module navigation properties (FK *columns* only, wired via Fluent config per
  consuming entity). A separate `AggregateRoot` (domain-event-collecting) base exists for new
  aggregates that don't need `MovementModel`'s transactional shape (e.g. `Company`, `FiscalYear`).
  **No strongly-typed IDs anywhere** — plain `long Id`.
- **CQRS**: MediatR records per feature, `<Module>.Contracts/<Feature>/` for cross-module-visible
  commands/queries/DTOs, `<Module>.Application/<Feature>/{Commands,Queries}/` for handlers +
  validators + AutoMapper profile. Generic base handlers (`CreateCommand`, `UpdateCommand`,
  `DeleteCommand`, `GetQuery`, `ListQuery`, `SearchQuery`, `GetMaxQuery`) live in `SharedKernel` and
  are reused via inheritance for standard CRUD; only genuinely non-CRUD operations (e.g.
  `Post`/`Reverse` on Journal, `SubmitForApproval`-shaped commands) get hand-written handlers.
- **Result/Error pattern**: `OrgSys.SharedKernel.Result` / `Result<T>` / `ResultCollection<T>` /
  `ResultPagination<T>`, `Error(MessageError, Key)`. Application handlers return `Result`-shaped
  values; **Domain throws `<Entity>DomainException` subclasses** for invariant violations (e.g.
  `AccountingPeriodClosedException`), not `Result` — the two patterns are layered, not mixed.
- **Validation**: FluentValidation, one validator class per command, wired through
  `FluentValidationFilter<TRequest,TResponse>` (a MediatR `IPipelineBehavior`, registered once
  globally in `Program.cs` — not per module).
- **DI wiring**: `<Module>.Infrastructure/DependencyInjection/ServiceCollectionExtensions.cs`
  exposes `Add<Module>Module(this IServiceCollection)`; `API/Program.cs` chains all of them plus the
  shared `AddDbContext<OrgContext>`/`IUnitOfWork`/`IRepository<>`/`FluentValidationFilter`
  registrations. Each module registers its own MediatR assembly scan and AutoMapper profile.
- **EF Core**: **exactly one `DbContext`, `OrgContext`** (`BuildingBlocks/OrgSys.DatabaseMigrator/Persistence/OrgContext.cs`),
  shared by every module — this is a deliberate, already-documented decision
  (`docs/modular-monolith-target-architecture.md` §7), not an oversight; do not introduce
  per-module `DbContext`s. **Zero `IEntityTypeConfiguration<T>` classes anywhere** — all Fluent
  config is centralized in `OnModelCreating`, one private helper method per module effort (e.g.
  `ConfigureOrganization`, `ConfigureCatalog`). Data-Annotations on entities do the rest. No SQL
  schema separation (`dbo` only). Migrations live in one shared history
  (`BuildingBlocks/OrgSys.DatabaseMigrator/Migrations/`), sequential across all modules.
- **Repository/UnitOfWork**: generic `IRepository<T>`/`IUnitOfWork` in `SharedKernel`, one shared
  implementation in `OrgSys.Infrastructure` over `OrgContext`, still in use everywhere — this is the
  intended shared plumbing, **not** the "IRepository&lt;T&gt; as illegal cross-module gateway" the
  brief warns against, because every module only ever injects `IRepository<TOwnEntity>` for its own
  entities (enforced today only by convention, not by a compiler rule — worth an Architecture.Test).
- **Domain events**: `IDomainEvent` marker interface; `AggregateRoot`-derived entities collect them
  via `Raise(...)`; `MovementModel`-derived entities hand-roll the same shape inline (can't
  multiply-inherit). Only entities with genuine cross-aggregate lifecycle consequences raise events
  today (`Journal`, `FiscalPeriod`) — this codebase's explicit convention is "no event for a
  meaningless property setter."
- **Integration events**: `BuildingBlocks/OrgSys.EventBus` — in-process MediatR notifications via
  `IIntegrationEventPublisher`/`MediatrIntegrationEventPublisher`, registered once globally.
  **No Outbox pattern exists anywhere** — do not introduce one for these 6 contexts (brief's own
  rule: reuse what exists, don't add unrequested framework weight); the transactional guarantee
  today comes entirely from everything sharing one `OrgContext`/one `SaveChangesAsync`.
- **Auditing**: no automatic audit-field population — `CreateUserId`/`CreateDate`/`ModifyUserId`/
  `ModifyDate` on `MovementModel` are set explicitly by each command handler, no interceptor/base
  handler does it implicitly. No generic audit-log table exists (`Administration.Domain.LogSys` is
  the closest analogue — a flat activity-log entity, not a structured before/after audit trail).
- **Soft delete**: `BaseModel.Hide` (bool) is the universal "inactive/soft-deleted" convention
  across every module — there is no separate `IsActive`/`IsDeleted` field pattern anywhere. Reuse
  `Hide`, do not introduce a second flag.
- **Concurrency**: no `[ConcurrencyCheck]`/`RowVersion`/`xmin` pattern found anywhere in the
  codebase (grep-confirmed zero hits) — optimistic concurrency is **not** an existing convention to
  "reuse"; introducing it for Budget/Subscription/WorkflowInstance would be new infrastructure, to
  be justified per-aggregate rather than assumed.
- **Pagination/Search**: `SearchQuery`/`ListQuery`/`ResultPagination<T>` in `SharedKernel`, reused
  by every module's `Search`/`GetList` query handlers — no Specification pattern exists (plain
  LINQ predicates inline in handlers).
- **CurrentUser / CurrentTenant / permission enforcement — the most significant gap found**: JWT
  authentication is real and enforced — `API/Controllers/BaseController.cs`'s `CoreController<>`
  base classes carry `[Authorize]` (bare, no policy), so every generic-CRUD endpoint and every
  plain controller that copies the same `[Authorize]` (e.g. `OrganizationSettingsController`)
  already rejects unauthenticated requests. **But no `ICurrentUser`/`ICurrentUserService`
  abstraction exists anywhere** (grep-confirmed zero hits), **no authorization policy is ever
  registered** (`Program.cs` calls `app.UseAuthorization()` with zero preceding
  `AddAuthorization(options => options.AddPolicy(...))`), and **no controller or handler anywhere
  reads `RoleId`/`RolePermission` to gate an action** — i.e. `[Authorize]` today only proves "this
  is a logged-in user," never "this user has permission X." Any authenticated user can call any
  endpoint regardless of their Role/Permission rows. Permission checks, to the extent they exist at
  all today, are Angular-side only (`menu.config.ts` permission keys hiding menu items) — i.e. the
  exact "frontend-only authorization" anti-pattern the brief explicitly forbids is **today's actual
  production state for anything beyond plain authentication**, not a hypothetical risk. This is the
  single highest-priority gap for the IAM phase to close, independent of any Tenant work.
- **Tenant / multi-company**: confirmed zero `TenantId` columns, zero `CompanyId` columns on any
  transactional table, zero EF global query filters anywhere in the solution. `Organization.Domain.Company`
  (built in the just-completed Organization Phase 1 pass, migration generated but **not yet applied
  to the live DB**) is a plain organizational root today, explicitly not a tenant boundary — see
  `docs/organization/organization-target-architecture.md` §4. Per this session's explicit user
  decision (recorded in `docs/architecture/adr/tenant-vs-company.md`), OrgSys is now committed to a
  **full multi-tenant retrofit**: `Tenant` owns 1..N `Company`, and `TenantId` + EF global query
  filters are added across the transactional modules. This is new work, not a relocation of
  existing behavior — see §6 Migration Risks.
- **Angular conventions**: `OrgSys.Angular/src/app/features/<area>/<feature>/` — `models/*.model.ts`,
  `services/*.service.ts extends BaseApiService`, `pages/*-list`, `pages/*-form`, `*.routes.ts`,
  lazy-loaded from `<area>.routes.ts`. Menu config (`core/config/menu.config.ts`) drives the
  sidebar and today's only (client-side) permission gate. No new Angular work is included in this
  pass's Phase 0-6 scope (see `docs/architecture/context-map.md`); Angular is a later, explicitly
  separate phase per the brief's own execution order.

---

## 2. Existing reusable building blocks (use as-is — do not recreate)

| Building block | Location | Reuse for |
|---|---|---|
| `BaseModel`, `MovementModel`, `AggregateRoot` | `OrgSys.SharedKernel` | Every new entity in all 6 contexts |
| `Result`/`Error`/`ResultCollection<T>`/`ResultPagination<T>` | `OrgSys.SharedKernel/Result.cs` | Every Application handler |
| `ICommand`/`IQuery<T>`/`ICommandHandler`/generic CRUD base handlers | `OrgSys.SharedKernel` (`CQRS/`, `CreateCommand.cs`, `UpdateCommand.cs`, `DeleteCommand.cs`, `GetQuery.cs`, `ListQuery.cs`, `SearchQuery.cs`, `GetMaxQuery.cs`) | Standard CRUD slices (Company/Branch-style entities: `CostCenter`, `Department`, `Project`, `Plan`, `Feature`, `ApprovalPolicy`, etc.) |
| `FluentValidationFilter<TRequest,TResponse>` | `OrgSys.SharedKernel` | Already global — nothing to register per module |
| `IRepository<T>`/`IUnitOfWork` + `Repository<T>`/`UnitOfWork` impl | `SharedKernel` (interfaces) / `OrgSys.Infrastructure` (impl) | Every new aggregate's persistence |
| `IIntegrationEventPublisher` / `MediatrIntegrationEventPublisher` | `OrgSys.EventBus` | `BudgetSubmittedIntegrationEvent`, `DocumentApprovedIntegrationEvent`, `TenantCreatedIntegrationEvent`, etc. |
| `DataSeederCoordinator` + per-module `IDataSeeder` | `OrgSys.DatabaseMigrator/Seeding/` | Seeding default `Tenant`/`Plan`/permission catalog/default `ApprovalPolicy` |
| `BaseController<TDto,...>` generic CRUD controller | `API/Controllers/BaseController.cs` | Standard CRUD endpoints |
| `Status`, `TreeView`, `Security.cs` (password hashing) | `OrgSys.SharedKernel` | `Status` for lifecycle enums where it fits; `TreeView` for CostCenter/Department hierarchy display; `Security.cs` for any new credential handling |
| `Accounting.Contracts.Postings` (`GetFiscalYearForDateQuery`, `PostAccountingEntryCommand`, `ReverseAccountingDocumentJournalCommandHandler`, `SetOpeningBalanceLineCommand`) | `Accounting.Contracts` | Budgeting's "Actual" data must read through here / through a Reporting projection — never a direct `Accounting.Domain` reference |
| `DealerLookupDto`/`GetDealerByIdQuery` (`Parties.Contracts`), `CompanyLookupDto`/`GetCompanyNamesQuery`/`GetDefaultCompanyQuery` (`Organization.Contracts`), `CurrencyLookupDto`/`GetDefaultCurrencyQuery` (`MasterData.Contracts`) | respective `.Contracts` | Template for every new `*.Contracts` surface this pass adds (`Budgeting.Contracts`, `IAM.Contracts`, `Workflow.Contracts`, `SaaS.Contracts`) |
| `Architecture.Tests/ModuleDependencyTests.cs` (`AcceptedDomainExceptions` table) + `ModuleLayerDependencyTests.cs` (`AcceptedApplicationDomainExceptions`/`AcceptedApplicationApplicationExceptions`) | `Tests/Architecture.Tests` | Register every new module's assembly in `ModuleDomains`; add exception rows only when unavoidable, each with a one-line reason, exactly like the 17+20+6 existing entries |

---

## 3. Existing entities that intersect with the new contexts

| Concept requested by the brief | Already exists as | Decision needed |
|---|---|---|
| `Company`, `Branch`, `OrganizationSettings` | **Already built** — `Organization.Domain.{Company,Branch,OrganizationSettings}` (see `docs/organization/organization-migration-result.md`) | Extend, don't rebuild. `Company` gains `TenantId` this pass (see ADR). |
| `FiscalYear`, `FiscalPeriod` | **Already built and load-bearing** — `Accounting.Domain.{FiscalYear,FiscalPeriod}`, tightly coupled to `Journal.Post`/`CreateReversal` | Stay owned by Accounting (already an explicit, documented decision — see `organization-target-architecture.md` §2). Organization/Budgeting consume via `Accounting.Contracts.Postings`. **Not revisited by this pass.** |
| `Currency`, `Country`, `City`, `District` | **Already built** — `MasterData.Domain` | Stay owned by MasterData (already an explicit, documented decision). Referenced by scalar FK only. |
| `User`, `Role`, `Permission`, `RolePermission` | **Already built** — `Administration.Domain`. `User` has a **single** `RoleId` (not many-to-many) and a **single, nullable** `BranchId`. `Role.Name` only (no System-vs-Tenant-defined distinction). `Permission.Key`/`Name`, client-assigned `Id` (`[DatabaseGenerated(None)]`) — i.e. permission rows are seeded with fixed, stable IDs today, a real existing convention any new `Budgeting.*`/`Workflow.*`/etc. permission keys must follow. | Extend, don't rebuild: add `TenantMembership`, multi-branch access, and (only if a real need appears) multi-role support. Preserve the existing single-`RoleId` shape for `User` unless the brief's "Tenant-defined Roles" requirement forces a redesign — see IAM target-architecture doc (next phase). |
| `Preference` (generic per-user KV) | **Already built** — `Administration.Domain.Preference` | Stays Administration's; not `OrganizationSettings`, not a new Settings mechanism. Do not reuse for Budget control policy config — that's a typed `BudgetControlPolicy` entity per the brief. |
| `CostCenter`, `Department`, `Project` (analytic), `DimensionDefinition`/`DimensionValue`, `Budget`, `BudgetLine`, `BudgetPeriodAllocation`, `BudgetRevision`, `BudgetControlPolicy` | **Do not exist anywhere** (grep-confirmed) | Entirely new — Budgeting context, greenfield |
| `ApprovalPolicy`, `ApprovalLevel`, `WorkflowInstance`, `ApprovalTask`, `ApprovalDelegation` | **Do not exist anywhere** | Entirely new — Workflow context, greenfield |
| `Tenant`, `Plan`, `Feature`, `PlanFeature`, `Subscription`, `TenantMembership` | **Do not exist anywhere** live. The only prior art is `AdminContext`'s dead, commented-out `Client`/`Plan`/`Nationality`/`TypeActivity` schema-per-tenant design (`OrgSys\Startup.cs`, root legacy app) — confirmed fully inactive, zero live rows, explicitly scoped out by `docs/legacy-migration-map.md`. **Do not revive it** — it predates this repo's current DDD conventions (no `BaseModel`, no CQRS, no `Result` pattern) and its schema-per-tenant approach is a different multi-tenancy strategy than the shared-database-plus-`TenantId`-column approach this pass has committed to. | Entirely new — SaaS context, greenfield. `Plan` name collides with the dead `AdminContext.Plan` table name only, not with anything live — no rename needed. |
| Financial/Trial Balance/Balance Sheet/P&L/AR-AP-Aging reports | **Already built** — `Reporting.Application/{Dealer,Financial,Sales,Warehouse}` (by-design reads every module's Domain directly, documented exception, see `docs/dependency-rules.md`) | Extend with `Budget vs Actual`; do not duplicate existing report handlers |
| `LogSys`, `Notification` | **Already built** — `Administration.Domain` | Candidate audit-trail substrate for Workflow's `ApprovalTask` decision log — evaluate reuse vs. a dedicated `WorkflowAuditEntry` in the Workflow target-architecture doc; do not force-fit if shapes don't match |

---

## 4. Legacy code that must not be reused

1. **`AdminContext`** (root `OrgSys\Startup.cs` legacy MVC app) — dead, commented out, schema-per-tenant
   design predating every current convention (no CQRS, no `Result`, no `BaseModel`). Explicitly out
   of scope per `docs/legacy-migration-map.md`. Do not revive `Client`/`Plan`/`Nationality`/
   `TypeActivity` as a shortcut for the new SaaS `Plan`/`Tenant` entities.
2. **`CompanyProfile`** (`Organization.Domain/Entities/CompanyProfile.cs`) — fully dormant (`DbSet`
   commented out, no migration, no CQRS, no controller). Already superseded by the real `Company`
   aggregate built in the Organization pass. Leave untouched (out of scope to delete it in this
   pass, per `docs/organization/organization-target-architecture.md` §1) — do not reference it from
   any new code.
3. **Root `Domain`/`Infrastructure`/`Host` folders** — empty shells from an earlier, abandoned
   extraction attempt (`obj`/`bin` only, or a stray `Migrations`/`Persistence` folder with no active
   compiled output referenced by `OrgSys.sln`'s real build). Confirm via `OrgSys.sln` before
   touching; do not add new files there.
4. **`Preference`'s generic KV shape** — do not model `BudgetControlPolicy`, `OrganizationSettings`
   extensions, or SaaS feature limits as `Preference` rows. Every typed-settings decision in this
   repo's history (`OrganizationSettings`) has deliberately moved *away* from KV storage toward
   typed entities; new contexts should follow that precedent, not the older KV pattern.

---

## 5. Duplicate concepts found

**None at the data level** for any of the 6 target contexts — every concept the brief asks for
either doesn't exist yet (Budgeting, Workflow, SaaS — clean greenfield) or has exactly one existing
owner already (Company/Branch → Organization, FiscalYear → Accounting, User/Role/Permission →
Administration). The one **ownership-fragmentation** pattern (not duplication) already documented
by the Organization pass — "Organization" as a business concept is split across
`Organization.Domain` (Company/Branch), `Accounting.Domain` (FiscalYear/FiscalPeriod), and
`MasterData.Domain` (Currency/geo) — is **not re-litigated by this pass**; those placements are
already explicit, documented decisions from the prior Organization Phase 1 work, confirmed correct
by `docs/module-ownership.md`.

`Department` is the one new concept with a genuine placement question (Organization vs. Budgeting)
— resolved in `docs/architecture/domain-ownership.md` §Department: **Organization owns it**,
Budgeting references `DepartmentId` only, per the brief's own §13 reasoning.

---

## 6. Migration risks

1. **Live remote database** (`OrgConnection`, per persistent project memory). `dotnet ef migrations
   add` is safe to run and inspect at any time; `dotnet ef database update` requires fresh, explicit
   consent in the session it runs, every time — a prior "continue" never carries forward.
2. **Full multi-tenant retrofit is the largest risk in this entire effort**, larger than any single
   relocation this repo has done to date (Parties/CommercialDocuments/Catalog each moved 1-4
   entities; this retrofit touches `TenantId` across 30+ transactional tables spanning all 11
   transactional modules, plus EF global query filters). Per the ADR
   (`docs/architecture/adr/tenant-vs-company.md`) this is executed as a **staged, per-module
   rollout** — nullable `TenantId` → backfill to one seeded default Tenant → `NOT NULL` +
   query-filter, one module at a time, each verified independently — not a single big-bang
   migration. **No stage applies to the live DB without a fresh, explicit go-ahead at that specific
   step.**
3. **FiscalYear/FiscalPeriod stay in Accounting, unchanged** — this pass does not touch Journal's
   posting invariants. Budgeting's "Actual" figures are read-only against posted `Journal`/
   `JournalItem` data via `Accounting.Contracts`/Reporting projections; Budgeting must never create,
   mutate, or duplicate Journal data.
4. **Permission enforcement is currently unwired server-side** (§1). Introducing real `[Authorize]`/
   policy-based checks for the first time is behavior-changing for every existing controller, not
   just the 6 new contexts' — must be rolled out carefully (a policy that fails open during rollout,
   flipped to fail-closed only once verified, or scoped first to new controllers only) to avoid
   locking out existing users with stale/no permission rows. This is flagged as a real production
   risk, to be sequenced explicitly in the IAM phase, not treated as a drop-in.
5. **In-flight Catalog migration** (`RelocateCatalogEntities`, generated, not applied) already sits
   ahead of this pass's migrations in the shared history — new migrations from this effort are
   sequenced after it, never reordered ahead of it.
6. **No optimistic concurrency convention exists** — `Budget`/`Subscription`/`WorkflowInstance`
   introducing `[ConcurrencyCheck]` for the first time is new infrastructure; must be scoped
   per-aggregate with a stated reason (brief §73), not applied blanket.

---

## 7. Proposed ownership for every concept in scope

See `docs/architecture/domain-ownership.md` for the full, authoritative table (brief §60) and
`docs/architecture/context-map.md` for the supplies/consumes relationships (brief §59). Summary:

| Context | New in this pass | Already built, extended this pass |
|---|---|---|
| **SaaS** | `Tenant`, `Plan`, `Feature`, `PlanFeature`, `Subscription`, `TenantMembership`, `ITenantFeatureService` | — |
| **Organization** | `Department` | `Company` (+`TenantId`), `Branch` (already has `CompanyId`) |
| **IAM** | `TenantMembership` (co-owned surface, physically in Administration — see ownership doc), `UserBranchAccess`, `ICurrentUser`/`ICurrentTenant` abstractions, policy-based `[Authorize]` | `User`, `Role`, `Permission`, `RolePermission` (extended, not replaced) |
| **Budgeting** | `CostCenter`, `Project` (analytic), `DimensionDefinition`, `DimensionValue`, `Budget`, `BudgetLine`, `BudgetPeriodAllocation`, `BudgetRevision`, `BudgetControlPolicy` | — |
| **Workflow** | `ApprovalPolicy`, `ApprovalLevel`, `WorkflowInstance`, `ApprovalTask`, `ApprovalDelegation` | — |
| **Reporting** | `BudgetVsActualProjection`/query handlers | Existing Dealer/Financial/Sales/Warehouse report handlers, untouched |

Next: `docs/architecture/context-map.md`, `docs/architecture/domain-ownership.md`,
`docs/architecture/adr/tenant-vs-company.md`, then per-context target-architecture docs following
the execution order in §93 of the brief.
