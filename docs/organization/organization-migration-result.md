# Organization / Administration Bounded Context — Migration Result

Phase 1 (Discover → Design → Migration Plan → Domain → Application → Infrastructure → API →
Consumer Migration → Tests → Architecture Validation → Legacy Cleanup → Build) is complete for the
scope defined in [organization-target-architecture.md](organization-target-architecture.md).

---

## 1. What was built

**Domain** (`Modules/Organization/Organization.Domain/Entities/`):
- `Company.cs` — new aggregate (LegalName, TradeName, TaxRegistrationNumber,
  CommercialRegistrationNumber, scalar-only DefaultCurrencyId/CountryId, Address, Phone, Email,
  Website, Branches collection).
- `OrganizationSettings.cs` — new, one row per Company (scalar-only DefaultCurrencyId/
  DefaultCountryId, DefaultTimeZone, FiscalYearStartMonth/Day).
- `Branch.cs` — extended with required `CompanyId` + `Company` navigation.

**Contracts** (`Modules/Organization/Organization.Contracts/Companies/`):
- `CompanyLookupDto`, `GetCompanyNamesQuery`, `GetDefaultCompanyQuery` (mirrors
  `MasterData.Contracts.Currencies` exactly).

**Application** (`Modules/Organization/Organization.Application/`):
- `Companies/` — full CQRS: Create/Update/Delete/DeleteList commands, GetById/GetList/Search
  queries, `GetCompanyNamesQueryHandler`/`GetDefaultCompanyQueryHandler` (Contracts handlers),
  `CreateCompanyCommandValidator`/`UpdateCompanyCommandValidator` (legal-name uniqueness, Country/
  DefaultCurrency existence checks), `MappingProfile`.
- `OrganizationSettings/` — `GetOrganizationSettingsQuery` (reuses the generic `GetCommandHandler`,
  filtered by CompanyId — returns an empty/default DTO rather than 404 when unconfigured),
  `UpdateOrganizationSettingsCommand`/Handler (explicit upsert-by-CompanyId, not generic Create/
  Update — brief's CQRS RULE), `UpdateOrganizationSettingsCommandValidator`, `MappingProfile`.
- `Branches/Validators/` — new `CreateBranchCommandValidator`/`UpdateBranchCommandValidator`
  enforcing: Company required and must exist, an inactive (Hidden) Company cannot receive new/
  reassigned Branches, Branch Name unique within Company.

**Infrastructure**:
- `OrganizationDataSeeder.cs` — new `InitialCompany` step (fresh-DB path), `InitialBranch` updated
  to assign the seeded Company's Id.

**API** (`API/Controllers/Org/Organization/`):
- `CompanyController` — standard `BaseController<>` CRUD, route `/Company/*` (`[controller]` token,
  same convention as every other controller — e.g. `BranchController` → `/Branch/*`).
- `OrganizationSettingsController` — plain controller, `/OrganizationSettings/GetByCompanyId` and
  `/OrganizationSettings/Update`.

**EF Core / Database** (`BuildingBlocks/OrgSys.DatabaseMigrator/`):
- `OrgContext.cs` — new `DbSet<Company>`, `DbSet<OrganizationSettings>`; new `ConfigureOrganization`
  Fluent-config helper (Branch→Company FK, Company/OrganizationSettings scalar-only FKs into
  MasterData.Currency/Country, OrganizationSettings unique-per-Company index).
- Migration `20260915080929_AddOrganizationCompanyAndSettings` — generated then hand-edited for a
  safe live-data backfill (see [organization-data-migration-plan.md](organization-data-migration-plan.md)).
  **Not applied to the live database** — generated and verified only.

**Tests** (`Tests/Application.Tests/OrganizationCompanyBranchValidatorTests.cs`):
- 11 new tests covering the brief's own ORGANIZATION TEST EXAMPLES that this pass's scope actually
  implements: Branch requires an existing Company; an inactive Company blocks new/reassigned
  Branches; Branch Name is unique per Company (and correctly *not* unique across different
  Companies); Company legal-name uniqueness; Company's Country/DefaultCurrency existence checks;
  OrganizationSettings' create-on-first-use vs. update-in-place upsert behavior.
  (FiscalYear/Currency-specific examples from the brief's list remain covered wherever Accounting/
  MasterData already test them — neither was relocated into Organization this pass.)

**Architecture** (`Tests/Architecture.Tests/ModuleLayerDependencyTests.cs`):
- One new documented exception: `("Organization", "MasterData", ...)` in
  `AcceptedApplicationDomainExceptions` — Application-layer-only, for the Country/Currency existence
  checks described above. No change to `ModuleDependencyTests.cs` (Domain-to-Domain rules) —
  `Organization.Domain` does not reference `MasterData.Domain`.

**Documentation** (`docs/organization/`):
- `organization-current-state.md` (Phase 0), `organization-target-architecture.md`,
  `organization-data-migration-plan.md`, this file.

---

## 2. Cross-context dependencies

No existing module's dependency footprint changed except the one new, documented
`Organization.Application → MasterData.Domain` exception described above. No `Domain → Domain`
reference was added anywhere. `Branch → Company` is intra-module. See
[organization-target-architecture.md](organization-target-architecture.md) §6 for the full picture.

## 3. Domain events / integration events

None added. Company/Branch/OrganizationSettings are plain `BaseModel` master data, consistent with
every sibling entity in `Organization`/`MasterData`/`Accounting` (only `Journal`/`FiscalPeriod`, which
have genuine cross-aggregate lifecycle consequences, raise domain events in this codebase — see
`docs/organization/organization-current-state.md` §1.3). No event was introduced without a concrete
current consumer, per the brief's own "don't create events for meaningless property setters" rule.

## 4. DB migrations

One migration, hand-edited for a safe live-data backfill. Full detail in
[organization-data-migration-plan.md](organization-data-migration-plan.md). **Status: generated and
verified (`dotnet ef migrations has-pending-model-changes` reports clean), not applied** — applying
it to the live `OrgConnection` database requires a separate, explicit user go-ahead.

## 5. API changes

Two new controllers under `API/Controllers/Org/Organization/` (§1). No existing controller was
modified — `BranchController` continues to work unchanged (its DTO/commands gained a `CompanyId`
field, which is additive to the wire contract, not breaking; existing callers omitting it will get
`CompanyId = 0`, which the new validator now correctly rejects with "The company field is required" —
see §7 for the resulting Angular follow-up this implies).

## 6. Angular changes

None. Deliberately deferred — see
[organization-target-architecture.md](organization-target-architecture.md) §7.

## 7. Legacy code removed / compatibility adapters remaining

- **Removed**: nothing (no legacy Company implementation existed to remove — `CompanyProfile` is
  pre-existing dead code, left as-is, out of scope for this pass).
- **Compatibility adapters**: none introduced. No `TODO DDD-MIGRATION-ORG` markers were needed — this
  is net-new capability with no legacy data path to bridge from (unlike the Party/Tax/Budget/
  FixedAsset contexts still to come, which will be migrating real existing Customer/Supplier/etc.
  data and will likely need them).
- **Known follow-up, not a bridge**: the existing `features/administration/branches/` Angular form
  does not yet send `CompanyId` — until it's updated (§6), creating/editing a Branch through that
  screen will fail the new "company field is required" validation. This is flagged, not silently
  patched around, per the brief's "do not modify unrelated UI" and "do not duplicate existing
  screens without a migration plan" rules — the Angular update is intentionally a separate, explicit
  follow-up task.

## 8. Technical debt / open items carried forward

1. Currency/Country/City/District relocation into Organization — deferred, see
   [organization-target-architecture.md](organization-target-architecture.md) §2.
2. FiscalYear/FiscalPeriod relocation — deferred (recommended to stay in Accounting permanently),
   same section.
3. Branch's `IsHeadOffice`/deactivation-history invariants from the brief's *possible* model — not
   built, no existing workflow to hang them on; candidate for a future Branch-lifecycle phase if a
   real business need surfaces.
4. Angular: Company/OrganizationSettings screens, and adding `CompanyId` to the existing Branch form.
5. Multi-tenancy retrofit (`CompanyId`/`TenantId` on transactional tables) — explicitly not attempted
   this pass; a separate, much larger decision if ever needed.
6. Migration `20260915080929_AddOrganizationCompanyAndSettings` is not yet applied to the live
   database.

## 9. dotnet build result

```
dotnet build OrgSys.sln
0 Warning(s) affecting new code (63 pre-existing warnings, all in legacy/unrelated files)
0 Error(s)
```

## 10. dotnet test result

```
Architecture.Tests:            1376 passed, 0 failed
Application.Tests:              107 passed, 0 failed (11 new — Organization validators/handlers)
Accounting.Domain.Tests:         54 passed, 0 failed
Sales.Domain.Tests:              53 passed, 0 failed
Advances.Domain.Tests:           39 passed, 0 failed
Payables.Domain.Tests:           35 passed, 0 failed
Purchasing.Domain.Tests:         45 passed, 0 failed
Inventory.Domain.Tests:          66 passed, 0 failed
Receivables.Domain.Tests:        35 passed, 0 failed
Inventory.Integration.Tests:      3 passed, 0 failed
```

Full solution build and full test suite (every test project, not just the new one) were both run
clean before considering this phase done, per the brief's "full solution must compile / all existing
relevant tests must pass" rules.

---

## Next context in the sequence

Per the brief's EXECUTION ORDER, the next bounded context is **CRM / Parties** — Phase 0 discovery
first (`/docs/parties/party-current-state.md`), before any Party/Customer/Supplier production code
changes, exactly as this context started.
