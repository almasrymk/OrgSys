# Organization / Administration Bounded Context — Target Architecture

Companion to [organization-current-state.md](organization-current-state.md) (Phase 0 discovery).
This document records the actual design decisions taken for Phase 1 and — per the brief's own rule
("if existing OrgSys code conflicts with assumptions in this prompt: inspect, preserve correct
existing behavior, apply the DDD boundary, document the decision") — explains every place this
implementation deliberately diverges from the brief's literal scope list, and why.

---

## 1. Scope actually built this pass

| Concept | Decision | Where |
|---|---|---|
| `Company` | **New.** Clean-slate aggregate — `CompanyProfile` (dormant, unmapped, no live table) was not revived; its shape didn't match what the brief asks for (Code/LegalName/TradeName/DefaultCurrencyId/CountryId) and carried legacy POS-era fields (`Watsapp`, `NationalityId`, `ClientId` referencing the dead `AdminContext`). `CompanyProfile` itself is untouched — still dead code, out of scope to clean up here. | `Modules/Organization/Organization.Domain/Entities/Company.cs` |
| `OrganizationSettings` | **New.** Typed, narrowly-scoped settings (default currency/country/timezone, a fiscal-start display convention) — deliberately not a KV store (that's `Administration.Domain.Preference`, staying user-scoped) and not other contexts' business config. One row per Company, upserted via an explicit `UpdateOrganizationSettingsCommand` rather than generic Create/Update (brief's CQRS RULE — Settings is a singleton-per-Company value, not a listable master-data entity). | `Modules/Organization/Organization.Domain/Entities/OrganizationSettings.cs` |
| `Branch` | **Extended, not rebuilt.** Added a required `CompanyId` FK (+ real intra-module EF navigation to `Company`) and the two invariants the brief calls out explicitly: a Branch must belong to a Company, and Branch Name is unique within a Company (see §3 for why "Name" and not "Code" — Branch never exposed Code as a settable field). Everything else about Branch (its existing CQRS, controller, Angular screen) is unchanged. | `Modules/Organization/Organization.Domain/Entities/Branch.cs` |
| `FiscalYear`, `FiscalPeriod` | **Deliberately NOT relocated.** Stay owned by `Accounting.Domain`. See §2. |  |
| `Currency`, `Country`, `City`, `District` | **Deliberately NOT relocated this pass.** Stay owned by `MasterData.Domain`; Organization references them as scalar-only FKs (no EF navigation), same convention as `Journal.CurrencyId`. See §2. |  |
| Tenant / multi-tenancy | **Not built.** No `TenantId`/multi-company retrofit onto transactional tables. See §4. |  |

---

## 2. Why FiscalYear/FiscalPeriod and Currency/Country/City/District were NOT relocated

The brief's own Data Ownership Matrix lists these under "Organization/Reference Data," and its
PURPOSE section for Part 1 lists them as things Organization should be "responsible for." Phase 0
discovery found real, documented reasons to keep them where they are instead:

1. **FiscalYear/FiscalPeriod** are tightly coupled to `Journal`'s posting/reversal lifecycle
   (`Journal.Post`/`CreateReversal` both call `FiscalYear.EnsureOpenForPosting()` /
   `FiscalPeriod.EnsureOpenForPosting()`), protected by a `DeleteBehavior.Restrict` FK, and resolved
   through `IAccountingPeriodService`/`Accounting.Contracts.Postings` — an already-established,
   working Contracts-based integration surface that Payables/Receivables already depend on.
   `docs/module-ownership.md` independently rates this placement **"Owned"** (correctly placed, not
   a violation). Physically moving these entities would mean re-plumbing Journal's core posting
   invariants and every consumer of `Accounting.Contracts.Postings` — a high-risk change with no
   corresponding benefit, and exactly the kind of "stable functionality" the brief says not to
   blindly rewrite. **Decision: leave in Accounting.** Organization would consume
   `Accounting.Contracts` if it ever needs fiscal-calendar data (it doesn't yet — `Company`/
   `OrganizationSettings` only carry a display-only `FiscalYearStartMonth`/`Day` convention, not the
   authoritative fiscal calendar).
2. **Currency/Country/City/District** are lower-risk to move (no lifecycle coupling — just scalar
   FKs plus two in-context navigations, `Parties.Domain.Dealer` and `Treasury.Domain.Bank`/
   `BankBranch`), and the brief's own ownership matrix does want them under Organization. But an
   earlier, already-executed architectural decision in this same repository
   (`docs/masterdata-decomposition.md`) explicitly recommended these stay in MasterData as reference
   data, with lookup DTOs as the fix for direct-reference fan-out (partially done already —
   `CurrencyLookupDto`/`GetCurrencyNamesQuery`/`GetDefaultCurrencyQuery` exist). Relocating them now
   would also land directly on top of an **in-flight, uncommitted Catalog module extraction**
   touching the exact same shared files this relocation would need (`OrgContext.cs`
   `OnModelCreating`, the migrations folder, `GlobalUsings.cs`) — a second large structural
   relocation stacked on an unapplied one, against a live remote database, is unnecessary risk for
   this pass. **Decision: leave in MasterData this pass.** `Company.CountryId`/`DefaultCurrencyId`
   and `OrganizationSettings.DefaultCountryId`/`DefaultCurrencyId` reference them as scalar-only FKs
   (no EF navigation), wired centrally in `OrgContext.OnModelCreating`'s new
   `ConfigureOrganization` helper — identical to how `Journal.CurrencyId` already avoids an
   `Accounting.Domain` → `MasterData.Domain` navigation. This is recorded as a documented, deliberate
   deviation from the brief's literal ownership matrix, not an oversight — **revisit in a dedicated,
   later phase** once the Catalog relocation has landed and this can be its own isolated migration,
   following the exact Parties/CommercialDocuments/Catalog relocation playbook (preserve
   `[Table(...)]`, update consumers' project references, verify a zero-diff migration).

This means Organization's actual, physical scope this pass is: **Company, OrganizationSettings,
and Branch's Company relationship.** Its *logical* ownership of Currency/Geography (per the brief's
matrix) is satisfied by treating MasterData as OrgSys's reference-data module for those concepts,
consistent with how `docs/dependency-rules.md` already treats `MasterData.Domain`/
`Organization.Domain`/`Administration.Domain` as a de-facto shared reference-data tier that other
Domains may reference directly.

---

## 3. Branch invariants — Code vs. Name

The brief's suggested Branch model includes a `Code` field with "Branch Code unique within
Company." OrgSys's `Branch` entity inherits a generic `Code`/`CodeNumber` (from `BaseModel`) but
**no existing consumer — Application DTO, Create/Update command, Angular form — ever sets or
exposes it**; every other master-data entity in this codebase (`Currency`, `Country`, `City`,
`FiscalYear`) enforces uniqueness on `Name`, not the inherited `Code`. Forcing Branch to suddenly
require and enforce a `Code` would be new, unrequested behavior change beyond this task's scope
(brief rule: "do not blindly rewrite stable functionality"). **Decision: enforce the brief's
invariant on `Name` instead** (`Branch Name unique within Company`) — same spirit, consistent with
every sibling entity's actual uniqueness convention. If a real `Code`-based numbering scheme is
wanted for Branch later, that's a separate, explicit follow-up.

The other Branch invariants (`Branch must belong to a Company`, `An inactive Company cannot receive
new active Branches`) are implemented exactly as specified, in
`CreateBranchCommandValidator`/`UpdateBranchCommandValidator`
(`Modules/Organization/Organization.Application/Branches/Validators/`). "Inactive" maps to the
existing `Hide` flag (from `BaseModel`) — this codebase's established active/inactive convention for
master data (Branch/Currency/etc. never had a separate `IsActive` bool; `Company` follows the same
pattern rather than introducing a redundant field). "Only one head office" and "historical
references remain valid if Branch is deactivated" were in the brief's *possible* model but have no
existing `IsHeadOffice` concept or deactivation workflow anywhere in OrgSys to hang them on — not
built this pass; flagged as a candidate for a later Branch-lifecycle phase if a real business need
surfaces (brief's own anti-speculative-scaffolding guidance, already applied identically in the
Catalog effort's ProductVariant decision).

---

## 4. Tenant vs. Company

Per Phase 0 discovery: **no multi-tenancy exists anywhere in OrgSys today** — zero `TenantId`/
`CompanyId` columns, no EF global query filters, and the one piece of prior art (`AdminContext`'s
schema-per-tenant `Client`/`Plan` design) is entirely dead/commented-out code, never activated.

**Decision: `Company` is a plain organizational root, not a tenant.** `Branch.CompanyId` is the only
schema change this introduces — Branch now transitively scopes every `BranchId`-carrying
transactional entity (Journal, Invoice, User, Payable, etc., all already reference `BranchId`) to a
Company *without* needing to retrofit a `CompanyId` column onto each of those tables directly. A
real multi-tenant retrofit (a `CompanyId`/`TenantId` column plus an EF global query filter on every
transactional table) remains a separate, much larger, schema-breaking decision this pass
deliberately does not make — it would need its own explicit approval and migration plan, per the
brief's own "multi-tenancy must follow existing OrgSys implementation, do not invent a second
multi-tenant abstraction" rule (there is no existing implementation to follow yet). Until that
decision is made, OrgSys remains effectively single-company in practice, with `Company` existing as
a real, extensible aggregate rather than a hardcoded assumption — multiple `Company` rows can be
created today (nothing prevents it), they just aren't yet used to scope every transactional table.

---

## 5. What Organization does NOT own (unchanged from the brief's PURPOSE section)

Users, Authentication, Authorization, Employees, Payroll, GL transactions, Invoices, Customers,
Suppliers, Inventory — none of this pass's changes touch those areas. `Shift`/`Table` (restaurant
POS concepts) remain co-located in `Organization.Domain` as they were before this pass; they are
outside this task's requested scope (Company/Branch/Fiscal/Currency/Geo/Settings) and were left
untouched rather than silently relocated either direction — an explicit "no change" decision, not an
oversight.

---

## 6. Cross-context dependency policy — what changed

- **`Organization.Application` → `MasterData.Domain`** (new, documented exception): Company/Branch/
  OrganizationSettings validators check `CountryId`/`DefaultCurrencyId` existence via
  `IRepository<MasterData.Domain.Country>`/`IRepository<MasterData.Domain.Currency>` directly —
  registered in `Tests/Architecture.Tests/ModuleLayerDependencyTests.cs`
  `AcceptedApplicationDomainExceptions`. This is an Application-layer-only reference (read-only
  existence checks); `Organization.Domain` itself does **not** reference `MasterData.Domain` — no
  change was needed to `ModuleDependencyTests.cs` (the Domain-to-Domain rule set), because
  `Company.CountryId`/`DefaultCurrencyId` are scalar-only, no EF navigation.
- **`Branch` → `Company`**: intra-module (`Organization.Domain` → `Organization.Domain`), not a
  cross-module reference at all.
- No other module's dependency footprint changed. `Administration.Domain → Organization.Domain`
  (User.BranchId) and `Treasury.Domain → Organization.Domain` (CashBox/BankAccount.BranchId) are
  pre-existing, already-documented exceptions, unaffected by this pass.
- New `Organization.Contracts.Companies` surface (`CompanyLookupDto`, `GetCompanyNamesQuery`,
  `GetDefaultCompanyQuery`) mirrors `MasterData.Contracts.Currencies` exactly, so a future module
  needing Company data (e.g. Budgeting/FixedAssets scoping by Company) has a Contracts-based path
  ready, without yet having a consumer.

---

## 7. API / Angular

- New controllers under `API/Controllers/Org/Organization/` (`CompanyController` — standard
  `BaseController<>` CRUD; `OrganizationSettingsController` — a small plain controller with
  `GetByCompanyId`/`Update`, mirroring `API.Controllers.Org.Catalog.PricingController`'s
  plain-controller shape for a non-CRUD-shaped resource). This is the "newer per-module area"
  convention (matching Sales/Payables/Receivables/Advances/Catalog), used here for genuinely new
  resources — existing `API/Controllers/Org/Setting/{BranchController,CurrencyController,
  CountryController,CityController,DistrictController,FiscalYearController}` are untouched and stay
  where they are (zero churn for entities that didn't move).
- **Angular**: not built this pass. `Company`/`OrganizationSettings` have no screens yet — the
  brief's own Angular section says "do not invent... do not duplicate existing screens without a
  migration plan," and there is no existing Company/Settings screen to extend (confirmed zero
  Angular presence in Phase 0). The existing `features/administration/branches/` form needs a
  `CompanyId` field added once a Company picker/Angular slice exists; flagged as follow-up, not done
  here to keep this pass backend-focused and reviewable.

---

## 8. Explicit deviations summary (brief's rule #43 "document deliberate deviations")

1. FiscalYear/FiscalPeriod stay in Accounting (not moved into Organization) — §2.
2. Currency/Country/City/District stay in MasterData this pass (not moved into Organization) — §2.
3. Branch's uniqueness invariant is enforced on `Name`, not `Code` (brief's suggested field) — §3.
4. No multi-tenancy/`TenantId` retrofit — `Company` is a plain root, not a tenant — §4.
5. `Shift`/`Table` were left exactly where they are in `Organization.Domain` — §5.
6. `CompanyProfile` was not revived/reused — a fresh `Company` entity was built instead — §1.
7. Angular screens for Company/Settings were not built this pass — §7.
