# ADR: Tenant vs. Company — full multi-tenant retrofit

**Status**: Accepted (2026-09-16). **Decision owner**: user, via explicit in-session question —
this is exactly the class of decision Phase 0 discovery flagged as "cannot be derived from the
code" (`docs/organization/organization-current-state.md` §8.1, `organization-target-architecture.md`
§4 both explicitly deferred it pending approval).

## Context

OrgSys is, today, a genuinely single-tenant, single-company system: zero `TenantId`/`CompanyId`
columns on any transactional table, zero EF global query filters, one shared `OrgContext` /
connection string. `Organization.Domain.Company` (built in the immediately-preceding Organization
Phase 1 pass) is a plain organizational root — multiple `Company` rows can exist, but nothing
scopes transactional data by Company today.

The brief's SaaS/Tenant Management and IAM sections assume a real multi-tenant SaaS platform:
`Tenant` (OrgSys's own customer) owns 1..N `Company` (legal entities), each with 1..N `Branch`;
every business record must be provably isolated per Tenant; IAM enforces Tenant + Branch scope on
every request.

Three options were presented:
1. **SaaS skeleton only, no retrofit** — build Tenant/Plan/Subscription/Feature as real aggregates,
   leave existing transactional tables unscoped, revisit the retrofit later.
2. **Full retrofit now** — `TenantId` on `Company`, plus `TenantId` + EF global query filter on
   every transactional table across all 11+ existing modules.
3. **Skip SaaS entirely.**

**Decision: option 2, full retrofit.**

## Decision

- `Tenant` (new, in `SaaS.Domain`) is the top of the hierarchy: `Tenant → Company → Branch`,
  matching the brief's own §3 example (`ABC Holding → ABC Egypt LLC / ABC Saudi LLC → Cairo Branch /
  Alexandria Branch`).
- `Organization.Domain.Company` gains a required `TenantId` (scalar FK only — **no** EF navigation
  to `SaaS.Domain.Tenant`, consistent with the existing "reference by ID across modules" convention
  used everywhere else in this codebase, e.g. `Journal.CurrencyId`). `SaaS.Domain`/`Organization.Domain`
  do not reference each other's Domain assemblies.
- Every transactional table (the `MovementModel`-derived family — `Journal`, `Invoice`, `Order`,
  `Transaction`, `Financial`, `FinancialTransfer`, `Payable`/`Receivable` open items, `Custody`,
  `PurchaseOrder`, `PurchaseRequisition`, etc. — plus `User`) gets a `TenantId` column and an EF
  global query filter keyed off a new `ICurrentTenant` abstraction (`SharedKernel`).
- **This is executed as a staged, per-module rollout, not a single migration**:
  1. Add `TenantId` as **nullable** on a module's tables (non-breaking).
  2. Backfill every existing row to one seeded "Default Tenant" (idempotent data migration, run
     once per module, verified row-count before/after).
  3. Flip `TenantId` to **required** + add the EF global query filter for that module.
  4. Verify `dotnet ef migrations has-pending-model-changes` is clean and the module's existing test
     suite still passes before moving to the next module.
  - Order: SaaS + Organization first (foundation), then Administration/IAM (User needs `TenantId`
    before tenant-scoped permission checks mean anything), then the remaining transactional modules
    in the brief's own dependency order (Accounting → Treasury/Sales/Purchasing/Inventory →
    Receivables/Payables/Advances/Parties/Catalog/CommercialDocuments → Reporting last, since it
    reads everything).
  - **No stage is applied to the live `OrgConnection` database without a fresh, explicit user
    go-ahead at that specific step** — generating and reviewing a migration is not consent to run
    `dotnet ef database update`, per standing project rule.
- `ICurrentTenant`/`ICurrentUser` (new, `SharedKernel` interfaces, implemented in `API`/Infrastructure
  from JWT claims) resolve `TenantId` **server-side only** — never trusted from the request body,
  per the brief's explicit security rule.

## Consequences

- This is materially larger and riskier than any prior relocation in this codebase (Parties/
  CommercialDocuments/Catalog each moved 1-4 entities with no schema-shape change; this retrofit
  adds a real column + filter to 30+ tables across every existing module). It is sequenced as its
  own multi-step program across the phases below, verified module-by-module, exactly like every
  large change this repo has made before — never as one uncheckable mega-migration.
- Every existing module's `Architecture.Tests` allow-lists gain a `TenantId`-related entry only if a
  genuine new cross-module reference is introduced; the `ICurrentTenant` abstraction lives in
  `SharedKernel` specifically so modules don't need to reference `SaaS.Domain`/`Administration.Domain`
  to enforce tenant scoping.
- Until every module's stage 3 lands, the system is in a **mixed state** (some modules
  tenant-scoped, some not) — this must be clearly tracked (a checklist in
  `docs/architecture/domain-ownership.md` or a dedicated tracking doc) so no module is silently
  skipped.
- SaaS billing (`Subscription`/`Plan`) is entirely separate from ERP business data (Sales
  `Invoice`) per the brief's own §57 — the retrofit only adds `TenantId` as a scoping column, it does
  not touch Sales/Purchasing's existing document models.
