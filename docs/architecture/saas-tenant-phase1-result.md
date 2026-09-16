# SaaS / Tenant Management — Phase 1 Result

Companion to [`adr/tenant-vs-company.md`](adr/tenant-vs-company.md) (the decision this implements)
and [`remaining-contexts-current-state.md`](remaining-contexts-current-state.md) (Phase 0
discovery). This is the first concrete implementation phase of the 6-context effort — SaaS was
built first, ahead of the brief's own suggested Organization→IAM→Budgeting→Workflow→Reporting→SaaS
order, because the "full retrofit now" decision means `Organization.Domain.Company` needs a real
`Tenant` to reference before Organization's own work (already substantially complete from the prior
session) can be extended.

---

## 1. What was built

**New module** `Modules/SaaS/{SaaS.Domain, SaaS.Application, SaaS.Contracts, SaaS.Infrastructure}`,
added to `OrgSys.sln` and wired into `API/Program.cs` (`AddSaaSModule()`).

**Domain** (`SaaS.Domain/Entities/`):
- `Tenant` — `Name`, `TenantLifecycleStatus` (Trial/Active/Suspended/Cancelled), `TrialEndsAt`/
  `ActivatedAt`/`SuspendedAt`. Domain methods `Activate`/`Suspend`/`Cancel`/`EnsureActive()` enforce
  valid transitions (`Cancelled` is terminal, matching `Journal.Cancel`'s precedent). Deliberately
  does **not** carry a `PlanId` (deviates from the brief's suggested shape) — `Subscription` is the
  single source of truth for "what plan is this tenant on," avoiding two competing answers to that
  question; documented inline on the entity.
- `Plan` — `Name`, `Price`, `BillingPeriod`, nullable `MaxUsers`/`MaxCompanies`/`MaxBranches`/
  `MaxWarehouses`/`MaxTransactionsPerMonth` (null = unlimited).
- `Feature` — `Key`/`Name`/`Description`, stable client-facing key (`"GeneralLedger"`,
  `"Budgeting"`, etc.), same convention as `Administration.Domain.Permission.Key`.
- `PlanFeature` — join entity, real intra-module navigation to `Plan`/`Feature`.
- `Subscription` — `TenantId`/`PlanId` (real intra-module navigation), `SubscriptionStatus`
  (Trial/Active/PastDue/Suspended/Cancelled/Expired), `Activate`/`ChangePlan`/`MarkPastDue`/
  `Cancel`/`Expire` domain methods.
- `SaaS.Domain.Exceptions.SaaSDomainException` hierarchy, same shape as
  `Accounting.Domain.Exceptions.AccountingDomainException`.

**Contracts** (`SaaS.Contracts/`):
- `Tenants/`: `TenantLookupDto`, `GetTenantNamesQuery`, `GetDefaultTenantQuery` (used by the tenant
  retrofit's backfill step), `EnsureTenantActiveQuery`.
- `Plans/`: `PlanLookupDto`.
- `Features/`: `ITenantFeatureService` (brief §55/§58's centralized feature/limit-enforcement
  abstraction — every module gates a feature/limit through this, not scattered `if (plan == ...)`
  checks), `TenantLimit` enum.

**Application** (`SaaS.Application/`): full CQRS for Tenants (CRUD + `Activate`/`Suspend`/`Cancel`
commands), Plans (CRUD), Features (CRUD), `PlanFeatures` (`Assign`/`Remove` + `GetFeaturesForPlan`),
Subscriptions (`SubscribeTenant`/`ChangePlan`/`Cancel` + CRUD-shaped queries). `TenantFeatureService`
implements `ITenantFeatureService` by resolving the Tenant's current usable Subscription → Plan →
`PlanFeature`/limit fields.

**Infrastructure**: `AddSaaSModule()` DI extension; `SaaSDataSeeder` — seeds one "Default Tenant"
(Active), an 8-entry Feature catalog matching brief §54's examples, 3 Plans (Starter/Professional/
Enterprise) with graduated feature entitlements, wired into `DataSeederCoordinator` **first**, ahead
of every other module — Organization's `InitialCompany` now reads the seeded Default Tenant's Id to
backfill `Company.TenantId` on a fresh database.

**API**: `API/Controllers/Org/SaaS/{TenantController, PlanController, FeatureController,
SubscriptionController}` — `Tenant`/`Plan`/`Feature` are `BaseController<>` CRUD plus
lifecycle-specific extra actions (`Tenant.Activate/Suspend/Cancel`, `Plan.AssignFeature/
RemoveFeature/GetFeatures`); `Subscription` is a plain controller (`Subscribe`/`ChangePlan`/
`Cancel`/`GetById`/`GetList`/`Search`) since Create/Update don't fit the generic CRUD shape, mirroring
`OrganizationSettingsController`'s precedent.

## 2. Tenant retrofit — stage 1 (Organization only)

Per the ADR, this phase executes **stage 1 for Organization only** (the rest of the 11 transactional
modules are not touched — see §4 "Not done this phase"):

- `Organization.Domain.Company.TenantId` — new, nullable, scalar-only FK into `SaaS.Domain.Tenant`
  (no EF navigation, same "no navigation across module boundary" convention as `Company.CountryId`).
- `Organization.Application`'s `CreateCompanyCommandValidator`/`UpdateCompanyCommandValidator` gained
  a `TenantId` existence check via `IRepository<SaaS.Domain.Tenant>` — same Application-layer-only
  accepted-exception shape as the existing `Organization → MasterData` check.
- `OrganizationDataSeeder.InitialCompany` now reads the seeded Default Tenant's Id and assigns it —
  a fresh database's seed Company is tenant-scoped from creation.
- **No backfill migration was needed for the live database**: `docs/organization/
  organization-migration-result.md` already recorded that Organization's own prior migration
  (`AddOrganizationCompanyAndSettings`) was generated but never applied to the live DB — i.e. the
  `Company` table does not exist in production yet. This phase's migration is sequenced directly
  after it in the same migrations folder; both are still pending a single, explicit, user-approved
  `dotnet ef database update` together. There is no live `Company` data to backfill.

## 3. Migration

`BuildingBlocks/OrgSys.DatabaseMigrator/Migrations/20260916054152_AddSaaSModuleAndTenantRetrofitStage1.cs`
— purely additive: 5 `CreateTable` (`Tenant`, `Plan`, `Feature`, `PlanFeature`, `Subscription`), 1
`AddColumn` (`Company.TenantId`, nullable). No `DropTable`/`DropColumn` in `Up()`. Verified with
`dotnet ef migrations has-pending-model-changes` → clean. **Not applied to the live `OrgConnection`
database** — per standing project rule, this requires a separate, explicit user go-ahead.

## 4. Not done this phase (tracked in `domain-ownership.md`'s retrofit table)

- `TenantId` on `Administration.Domain.User` and every other transactional module (Accounting,
  Treasury, Sales/Parties/CommercialDocuments, Purchasing, Inventory/Catalog, Receivables/Payables,
  Advances, Reporting) — stages 1-3 of the ADR, for each module, not started.
- `ICurrentTenant`/`ICurrentUser` abstractions and the EF global query filter itself — this phase
  added the `TenantId` *column* only; the filter that actually enforces isolation is IAM-phase work
  (needs `ICurrentUser` reading JWT claims first, per `remaining-contexts-current-state.md` §1's
  permission-enforcement gap).
- SaaS Angular screens.
- SaaS-specific tests (Application/Domain unit tests for Tenant/Subscription lifecycle transitions)
  — flagged as an open item, not written this pass; existing Architecture.Tests (1551, all passing)
  and Application.Tests (114, all passing, +7 net counting the Organization TenantId validator
  updates) cover module-boundary and Organization-side regressions only.

## 5. Build/test result

```
dotnet build OrgSys.sln         → 0 Error(s)
dotnet test OrgSys.sln          → 2035 passed, 0 failed, 0 skipped
  Architecture.Tests:            1551 passed (was 1376 before this module existed — SaaS added ~175
                                  new module-pair assertions by virtue of joining the pairwise matrix)
  Application.Tests:              114 passed
  every Domain.Tests project:     all passing, unaffected
dotnet ef migrations has-pending-model-changes → "No changes have been made to the model since the last migration."
```

## 6. Next

Per the brief's execution order (adjusted for the full-retrofit decision): **Organization/
Administration completion** was already done in the prior session; **IAM** is next — building
`ICurrentUser`/`ICurrentTenant`, `TenantMembership`, `UserBranchAccess`, and wiring real
policy-based `[Authorize]` (closing the permission-enforcement gap `remaining-contexts-current-
state.md` §1 identified as the highest-priority finding), then Budgeting master data, Budgets,
Workflow, Reporting's Budget-vs-Actual, and only then the wider tenant-retrofit rollout across the
remaining 10 transactional modules.
