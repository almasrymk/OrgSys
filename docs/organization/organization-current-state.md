# Organization / Administration Bounded Context — Phase 0 Discovery Report

Analysis only — no source, config, or migration files were modified to produce this report.
Findings are backed by direct inspection (Read/Grep/Glob, two parallel research passes covering
backend + Angular + DB schema) of `D:\Work\MK\Source\OrgSys`, branch `Latest`, as of 2026-09-15.
Recent commits on this branch: Sales Quotation/SalesOrder + Purchasing hardening (`48a48637`),
Advances/Custody skeleton (`79df098a`), Accounts Payable (`89f66c01`), Accounts Receivable
(`58630b4e`). The working tree currently has an **in-progress, uncommitted Catalog module
extraction** (Product/Classification/Unit/Property relocating out of Inventory/MasterData into a
new `Modules/Catalog`, plus an `AddInventoryHardening` and `RelocateCatalogEntities` migration,
neither applied to the live DB per project memory). None of that work touches Company, Branch,
FiscalYear, Currency, Country/City/District, or Settings — this report treats it as unrelated
background and does not modify it.

---

## 0. Headline finding: this is mostly a re-scoping exercise, not a greenfield build

Every concept in scope except `Company`/`Tenant`/`Settings` **already exists and is live** —
split across three modules that were never unified under an "Organization" umbrella:

| Concept | Currently owned by |
|---|---|
| `Branch` | `Organization.Domain` (already the right module) |
| `FiscalYear`, `FiscalPeriod` | `Accounting.Domain` |
| `Currency` | `MasterData.Domain` |
| `Country`, `City`, `District` | `MasterData.Domain` |
| `CompanyProfile` (dormant, unmapped) | `Organization.Domain` |
| Generic per-user KV settings (`Preference`) | `Administration.Domain` |
| `Company` / `Tenant` / real Organization-level settings | **nowhere — does not exist** |

`Modules/Organization/` (Domain/Application/Infrastructure/Contracts) **already exists** as a
project and is already wired into `OrgContext`/the API — it currently owns only `Branch` (live),
`CompanyProfile` (dead), `Shift` and `Table` (live, but restaurant-POS concepts, not in this
context's requested scope). This means "build the Organization/Administration bounded context" is
really: **(a) design `Company`/`Tenant`/`Settings` fresh — nothing to migrate — and (b) decide
whether to pull `FiscalYear`/`FiscalPeriod` (from Accounting) and `Currency`/`Country`/`City`/
`District` (from MasterData) into the existing `Organization` module, or leave them where they are
and have Organization be the new home only for Company/Branch/Settings.** That ownership decision
is the single biggest open question this report surfaces (see §9).

---

## 1. Everything that exists today, by concept

### 1.1 Company / Tenant / Multi-tenancy — genuinely does not exist

A repo-wide search for `CompanyId`/`TenantId`/`ITenant*`/EF global query filters returned **zero
hits**. There is no `BaseModel` field, no tenant interface, no query filter, anywhere.

- **`CompanyProfile`** — `Modules/Organization/Organization.Domain/Entities/CompanyProfile.cs:1-60`.
  Fields: `Name, Description, Phone1/2, Mobile1/2, Fax1/2, Email1/2, Address1/2,
  CommercialRegister, TaxCard, Website, Watsapp, DateCreated, TypeActivity, NationalityId,
  SizeOfCompany, ClientId` (+ `BaseModel`). **It is fully dormant**: `OrgContext`'s
  `DbSet<CompanyProfile>` is commented out (`BuildingBlocks/OrgSys.DatabaseMigrator/Persistence/
  OrgContext.cs:496`), it has **no entry at all** in `OrgContextModelSnapshot.cs` (never migrated
  into the live DB), its seeding body is fully commented out
  (`Modules/Organization/Organization.Infrastructure/Seeding/OrganizationDataSeeder.cs:26-41`), it
  has no Application-layer CQRS, and no controller exists. Its former DTO
  (`CompanyProfileDto.cs`) was already deleted during the Catalog pass per `docs/module-ownership.md
  :28` / this session's Catalog report. **Nothing to migrate — a clean slate.**
- **Prior art for multi-tenancy, also dead**: the legacy root `OrgSys\Startup.cs:66` has an
  `AdminContext` (tables `Client`, `Plan`, `Nationality`, `TypeActivity`, with `Client.DbSchema`
  implying a schema-per-tenant SaaS design) **entirely commented out**, never activated, zero live
  rows. `docs/legacy-migration-map.md` explicitly scopes it out as "not part of the 11-module ERP
  business domain."
- **Conclusion**: OrgSys is a genuinely single-tenant, single-company database today. Introducing
  real multi-company/multi-tenant support (a `CompanyId`/`TenantId` column + EF global query
  filter retrofitted onto every transactional table) is a **schema-breaking, additive-everywhere**
  change, not a migration of existing data — this must be called out as its own decision, separate
  from "give Organization a Company entity for org-profile purposes."

### 1.2 Branch — already correctly owned

`Modules/Organization/Organization.Domain/Entities/Branch.cs:1-9`. Fields beyond `BaseModel`: only
`Name` (`[StringLength(50, MinimumLength=3)]`). Table `Branch`, live, matches DB snapshot exactly
(`OrgContextModelSnapshot.cs:3443-3482`) — **no FKs out at all**: no `CompanyId`, no `CurrencyId`,
no default-fiscal-year link. Full CQRS exists (`Modules/Organization/Organization.Application/
Branches/**`), controller exists (`API/Controllers/Org/Setting/BranchController.cs`), Angular
screen exists (`features/administration/branches/`, DTO exposes only `name`). One default row is
seeded (`"Main Branch"`).

`BranchId` (`long?`, always nullable, always indexed when present) is baked directly into
`OrgSys.SharedKernel.MovementModel` (`BuildingBlocks/OrgSys.SharedKernel/MovementModel.cs:35`), the
shared base class for every transactional-document entity (`Financial`, `FinancialTransfer`,
`Invoice`, `Journal`, `Transaction`, `Inventory`, `Custody`, `Payable`, etc.) — so it is already the
consistently-applied "operational scope" dimension across the whole system (57 files reference it).
`Administration.Domain.User.BranchId` is the one place with a *real* EF navigation (`Administration.
Domain.csproj` directly references `Organization.Domain.csproj` for it) rather than a scalar-only
FK — every other consumer uses the scalar-plus-Fluent-no-navigation pattern wired centrally in
`OrgContext.OnModelCreating:178-203`. `BranchId` is **absent** from pure master-data tables (Dealer,
Product, Brand, Classification, Country/City/District, Bank) — it is a transactional-document
dimension, not attached to reference data.

### 1.3 FiscalYear / FiscalPeriod — owned by Accounting, tightly coupled to Journal posting

**This is the highest-risk area in scope** — flagged explicitly by the brief ("review existing
GeneralLedger code first... do NOT break GL") and confirmed by inspection to be justified caution.

- `FiscalYear` — `Modules/Accounting/Accounting.Domain/Entities/FiscalYear.cs:1-30`: `Name,
  StartDate, EndDate, IsCurrent, FiscalYearStatus (Open=1/Closed=2), Periods` (collection). Domain
  method `EnsureOpenForPosting()` throws `AccountingPeriodClosedException` when closed.
- `FiscalPeriod` — `Modules/Accounting/Accounting.Domain/Entities/FiscalPeriod.cs:1-83`:
  `FiscalYearId, FiscalYear (nav), PeriodNumber, Name, StartDate, EndDate, FiscalPeriodStatus
  (Open=1/Closed=2/Locked=3)`. Raises `FiscalPeriodClosedDomainEvent`/`FiscalPeriodReopenedDomainEvent`.
  `Close()`/`Reopen()` (Locked periods cannot reopen), `EnsureOpenForPosting()`.
- **`Journal`'s posting/reversal lifecycle depends on both directly**: `Journal.Post(FiscalYear,
  FiscalPeriod, ...)` and `CreateReversal(...)` both call `EnsureOpenForPosting()` on each before
  allowing the state transition (`Modules/Accounting/Accounting.Domain/Entities/Journal.cs:225-341`).
  `IAccountingPeriodService` (`Accounting.Application/AccountingPeriodService/
  IAccountingPeriodService.cs`) is the single resolver: `ResolveAndValidateAsync` (date → year/
  period), `GetFiscalYearAsync`, `ValidateOpeningBalanceAsync` (at most one Opening Balance journal
  per fiscal year; entry date must equal the fiscal year's `StartDate`).
- **DB-level protection**: `Journal.FiscalYear`/`FiscalPeriod` FKs are `DeleteBehavior.Restrict`
  (`OrgContext.cs:105-121`) — a FiscalYear/FiscalPeriod with journals against it cannot be deleted.
- **No FiscalYear is ever seeded** — `AccountingDataSeeder` seeds `JournalType`/`AccountType`/
  chart-of-accounts only.
- **Established cross-module access pattern**: Payables/Receivables opening-balance handlers are
  the only consumers reaching `FiscalYearId`/`FiscalPeriodId` today, and they do so via
  `Accounting.Contracts.Postings` MediatR queries/commands (`GetFiscalYearForDateQuery`,
  `PostAccountingDocumentCommand`, etc.) — **never** a direct `Accounting.Domain` reference (the one
  documented exception is the opening-balance handlers reading `Journal`/`JournalType` directly,
  tracked in `docs/dependency-rules.md` as an accepted exception, not a pattern to extend).
- `docs/module-ownership.md:12` lists `FiscalYear, FiscalPeriod` as **"Owned"** by Accounting with
  no "leaking" flag — i.e., today's placement is considered architecturally correct, not a
  violation waiting to be fixed.
- Angular: `features/administration/fiscal-years/` exists (date-range validated form, status
  toggle) but **`FiscalPeriod` has no standalone screen, no Commands/Queries, no Controller at
  all** — periods are only ever written as a nested child collection from FiscalYear's own Update
  handler (confirmed independently by `docs/ANGULAR_MIGRATION_INVENTORY.md`).

### 1.4 Currency / ExchangeRate

- `Currency` — `Modules/MasterData/MasterData.Domain/Entities/Currency.cs:1-14`: `Name (3-50
  chars), Rate (decimal 18,2), IsDefault (bool)`. Table `Currency` (`OrgContext.Currencys` — typo'd
  DbSet property name, cosmetic). Seeded with exactly one row (`"Epg"`, likely a truncated/typo'd
  "Egp" — worth fixing during any relocation, not before).
- **No `ExchangeRate`/rate-history entity exists anywhere.** `Rate` is a single mutable field
  directly on `Currency` — no dated/historical rate table. Every consuming module (`Journal.Rate`,
  `Custody.Rate`, etc.) instead snapshots its own copy of the rate at document-creation time. A
  proper historical FX-rate table would be **new functionality**, not a relocation.
- No `ISOCode`/`Symbol`/`DecimalPlaces` columns exist today — just `Name` and `Rate`.
- Some `MasterData.Contracts` DTOs already exist for lookup purposes (`CurrencyLookupDto`,
  `GetCurrencyNamesQuery`, `GetDefaultCurrencyQuery`) — the "reference by contract, not Domain"
  pattern is partially in place for Currency already.
- Referenced (scalar `CurrencyId`, no Domain navigation — explicitly documented in `Journal.cs` as
  a deliberate boundary: "`Accounting.Domain` must not reference `MasterData.Domain`") from
  Accounting (`Journal`), Treasury (`Financial`, `FinancialAccount`, `FinancialTransfer`), Catalog
  (`PriceList`), CommercialDocuments (`Invoice`), Advances (`Custody`), plus Payables/Receivables/
  Reporting Application-layer DTOs — 40+ files total, all via scalar FK.
- Angular: `features/administration/currencies/` — plain CRUD, `rate` as a flat number field,
  matching the backend exactly (no ExchangeRate concept in the UI either).

### 1.5 Country / City / District — no "Region" concept exists

All three owned by `MasterData.Domain`, forming a strict `Country ← City ← District` hierarchy:

- `Country` — `Modules/MasterData/MasterData.Domain/Entities/Country.cs`: just `Name`.
- `City` — `.../City.cs`: `Name, CountryId? (FK + nav)`.
- `District` — `.../District.cs`: `Name, CountryId? (FK+nav), CityId? (FK+nav)`.
- No `Region`/`State`/`Province` exists anywhere (repo-wide grep, zero hits).
- Seeded: 13 countries (EG, SA, AE, KW, QA, BH, OM, SD, LY, TN, DZ, MA, MR) with cities for all 13,
  districts only for EG/SA/AE.
- **Notable Domain→Domain finding**: `Parties.Domain.Dealer` and `Treasury.Domain.Bank`/
  `BankBranch` hold **real EF navigation properties** (not just scalar IDs) to `Country`/`City`/
  `District`, requiring their `.Domain.csproj` to directly reference `MasterData.Domain.csproj`.
  This is **explicitly documented as intentional** in both the `.csproj` comments and
  `docs/dependency-rules.md:57` — `MasterData.Domain` (and `Organization.Domain`,
  `Administration.Domain`) are already treated as **quasi-shared-kernel reference-data modules**
  that other Domains are allow-listed to reference directly, unlike e.g. `Sales.Domain`/
  `Inventory.Domain` which must go through Contracts. Any redesign that folds MasterData's geo
  entities into "Organization" inherits this already-loose boundary rather than establishing a new
  one — worth an explicit decision on whether to tighten it going forward (force Contracts-only
  access) or keep it as-is.
- Angular: `countries`/`cities`/`districts` features exist with cascading Country→City→District
  dropdowns (`district-form.component.ts` calls `cityService.getByCountry()` on Country change).
  `Bank`/`BankBranch` forms reuse the same cascading pattern.

### 1.6 Organization / Company Settings — does not exist as a typed concept

The only settings-like construct is `Administration.Domain.Preference`
(`Modules/Administration/Administration.Domain/Entities/Preference.cs:1-15`): `Key, Value,
Reference, UserId?` — a **user-scoped, generic key/value store** (client-assigned `Id`, no
`[DatabaseGenerated]`), read by ~8 modules for things like `AutoCreateTransaction`,
`OpeningBalanceClearingAccountId`. This is the closest existing analogue to "organization
settings" but it is per-user KV, not a typed company-level settings entity — there is nothing to
migrate; a real `OrganizationSettings`/`CompanySettings` concept would be new.

### 1.7 Seeding

All seeding runs through `BuildingBlocks/OrgSys.DatabaseMigrator/Seeding/DataSeederCoordinator.cs`,
one seeder per module in a fixed order (MasterData before Treasury, since Treasury's Bank seed
reads MasterData's Country/City rows), wired via EF's `UseSeeding`/`UseAsyncSeeding` hooks directly
on `OrgContext`. Relevant seeders: `OrganizationDataSeeder` (Branch only — CompanyProfile/Shift
seed bodies are dead), `MasterDataDataSeeder` (Currency/Country/City/District/PaymentType/
ReferenceType), `AccountingDataSeeder` (JournalType/AccountType/chart of accounts — **no
FiscalYear**). **Reuse this coordinator and its ordering convention for any new Organization
seeding — do not create a parallel seeding mechanism.**

---

## 2. Database schema ground truth

**There is exactly one `DbContext` in the entire solution**: `OrgContext`
(`BuildingBlocks/OrgSys.DatabaseMigrator/Persistence/OrgContext.cs`, ~540 lines). No module has its
own DbContext, and **zero `IEntityTypeConfiguration<T>` classes exist anywhere** — all EF
configuration is Data-Annotations on entities plus centralized Fluent calls in
`OrgContext.OnModelCreating` (including every cross-module "no navigation" FK, e.g. `Branch`/
`Currency` on `Journal`, and per-effort helpers like `ConfigureCatalog(modelBuilder)`). **This means
any new Organization table is added the same way as everything else — a `DbSet` on this same
shared `OrgContext`, configured in this same `OnModelCreating` — regardless of which
`Modules/<Context>` folder its `.Domain` project lives in.** No SQL-schema separation exists
(`OrgContext`'s `Schema` property is commented out; everything is `dbo`).

Tables confirmed live and matching their entities exactly (`OrgContextModelSnapshot.cs`, spot-
checked): `FiscalYear`, `FiscalPeriod`, `Currency`, `Country`, `City`, `District`, `Branch`. **No
`Company`/`CompanyProfile`/`Tenant`/`Setting`/`ExchangeRate` table exists anywhere** (grep-confirmed
zero matches for `ToTable("Company...`, `ToTable("Tenant...`, `ToTable("Setting...`).

`CompanyId`/`TenantId` sampled against unrelated business tables (`Journal`, `Invoice`, `User`,
`Dealer`, `Product`, `Payable`) — **none have it**. `BranchId` appears (nullable, indexed) on
transactional tables (`Journal`, `Invoice`, `User`, `Payable`) but is **absent** from master-data
tables (`Dealer`, `Product`, `Brand`, `Classification`, `Country`/`City`/`District`, `Bank`).

**Schema defect found (unrelated to this task but worth flagging so it isn't propagated)**:
`Treasury.Domain.BankAccount` has a live column `BankBranchd` (missing the trailing "h") wired as
the actual FK column name to `BankBranch` — a real typo baked into the live schema.

**Dead/commented `DbSet` declarations** already sitting in `OrgContext.cs`, harmless but confusing:
`CompanyProfiles` (line 496), a duplicate commented `Country` (line 493), a duplicate commented
`Journal`/`JournalItem` pair (494-495) — all leftover cruft alongside the real active declarations.

---

## 3. Angular frontend — current state

Root: `OrgSys.Angular/src/app/`. The de-facto "Organization/Administration" IA already exists as
`features/administration/**`, routed at `/administration/*`
(`features/administration/administration.routes.ts`), with 9 lazy-loaded children: `countries`,
`cities`, `districts`, `currencies`, `fiscal-years`, `branches`, `banks`, `bank-branches`,
`products`. Every feature follows an identical, established structure (`models/*.model.ts`,
`services/*.service.ts extends BaseApiService`, `pages/*-list`, `pages/*-form`, `*.routes.ts`) —
**this convention must be followed exactly for any new screens**, not reinvented.

- Top-level menu (`core/config/menu.config.ts`) currently groups all of this under a section
  labeled **"Settings"** (not "Administration" or "Organization") — a naming decision to make
  explicitly if the new bounded context wants its own menu identity.
- Permission keys baked into the menu config have legacy pluralization typos (`Branchs`, `Citys`)
  that are tied to backend permission seed data — any IA rename must coordinate with permission
  data, not just relabel the menu.
- `Company`/`CompanyProfile`/`Tenant`/`ExchangeRate`/`FiscalPeriod`(standalone)/any Settings screen
  — **zero Angular presence**, confirmed via glob. Genuinely new frontend work.
- **Stray screen worth relocating**: `features/administration/products/` — Product now
  conceptually belongs to the in-progress `Catalog` backend module (per the concurrent, uncommitted
  Catalog extraction), not Administration/Organization. No `features/catalog/` Angular area exists
  yet. Out of scope to move here, but flagged so the new Organization IA doesn't inherit it.

---

## 4. Established architectural conventions (from Payables/Receivables/Parties/CommercialDocuments/Catalog — apply without modification)

1. **Project layout**: exactly 4 csproj per module — `<Module>.Domain`, `.Application`,
   `.Infrastructure`, `.Contracts`. `Modules/Organization/*` already follows this.
2. **No strongly-typed IDs** — every entity uses plain `long Id` from `BaseModel`. Do not introduce
   `CompanyId`/`FiscalYearId` record structs.
3. **Entity base classes**: `BaseModel` (master data) or `MovementModel : BaseModel`
   (transactional). `Company`, `Branch`, `FiscalYear`, `Currency`, `Country`/`City`/`District` are
   all master data → `BaseModel`. No automatic audit-field population.
4. **No `IEntityTypeConfiguration<T>` anywhere** — all Fluent config stays inline in
   `OrgContext.OnModelCreating`, with a dedicated private helper per module effort (mirror
   `ConfigureCatalog`/`ConfigureInventoryHardening`) rather than per-entity config classes.
5. **No SQL schema separation** — stays in `dbo`, table names via `[Table("...")]`.
6. **Repositories**: per-aggregate interfaces in `<Module>.Domain/Repositories/`, thin
   implementations in `<Module>.Infrastructure/Persistence/` over the generic
   `OrgSys.SharedKernel.IRepository<TEntity>`.
7. **CQRS**: MediatR records + DTOs in `<Module>.Contracts/<Feature>/`, handlers in
   `<Module>.Application/<Feature>/{Commands,Queries}/`, returning `Result<T>`
   (`SharedKernel.Result`). Domain throws `<Entity>DomainException` subclasses, not `Result`.
8. **Domain events**: `IDomainEvent` marker; `MovementModel`-derived types hand-roll the
   `_domainEvents`/`Raise(...)` shape (can't multiply-inherit a shared `AggregateRoot`); pure
   `BaseModel` aggregates (Company, FiscalYear, Currency, Country/City/District, Branch) can use
   the real `AggregateRoot` base directly.
9. **Integration events**: `BuildingBlocks/OrgSys.EventBus` (in-process MediatR, no outbox today —
   "if OrgSys currently has an Outbox pattern, reuse it" does not apply; none exists).
10. **DI wiring**: `<Module>.Infrastructure/DependencyInjection/ServiceCollectionExtensions.cs`,
    `Add<Module>Module(this IServiceCollection)`.
11. **Multi-tenancy does not exist anywhere** — see §1.1. Do not invent tenant columns/scoping
    without an explicit decision; today, uniqueness rules are simply global (e.g. `Currency.Name`
    unique full-stop, no company-scoping).
12. **Architecture tests are real and enforced**: `Tests/Architecture.Tests/
    ModuleDependencyTests.cs` and `ModuleLayerDependencyTests.cs` (NetArchTest + xUnit) assert no
    unauthorized cross-module Domain→Domain/Application→Domain references, via explicit allow-lists
    (`AcceptedDomainExceptions`). `Organization` is presumably already registered (it's an existing
    module); confirm and update its allow-list entries if FiscalYear/Currency/Geography move in.
13. **API controllers**: legacy convention is a flat `API/Controllers/Org/Setting/` folder
    regardless of owning module (where `BranchController`, `FiscalYearController`,
    `CurrencyController`, `CountryController`, `CityController`, `DistrictController` all already
    live). Newer bounded contexts (Sales, Payables, Receivables, Advances, and the in-progress
    Catalog) are establishing per-module area folders instead (e.g. `API/Controllers/Org/
    Catalog/`). A Phase 1 decision is needed: keep Organization's controllers under the existing
    `Setting/` folder (matches current reality, zero churn) or migrate to `API/Controllers/Org/
    Organization/` (matches the newer convention) — recommend the former for controllers that
    aren't moving domain (Branch, Currency, Country/City/District if left in place) and the latter
    only for genuinely new resources (`Company`, `FiscalPeriod` if it gets a real endpoint,
    `Settings`).
14. **Best full-stack templates**: Payables/Receivables for Domain+Contracts+Application+
    Infrastructure+DI end-to-end; Parties/CommercialDocuments/Catalog for the specific "physically
    relocate an existing entity out of another module, preserve `[Table(...)]`, verify zero-diff
    migration" technique — directly applicable if FiscalYear/Currency/Geography are pulled into
    Organization.

---

## 5. Duplications found

**None at the data level.** Every concept in scope (Branch, FiscalYear/FiscalPeriod, Currency,
Country/City/District) has exactly one physical owner and exactly one table today — there is no
"Customer.Address vs Supplier.Address vs Company.Address" style duplication to reconcile, because
Company doesn't exist yet and Branch/geo/currency were never duplicated per-module. The only
"duplication-shaped" finding is **ownership fragmentation**, not data duplication: the same
conceptual "Organization" umbrella is split across `Organization.Domain` (Branch), `Accounting.
Domain` (FiscalYear/FiscalPeriod), and `MasterData.Domain` (Currency/Country/City/District) — a
boundary decision, not a cleanup.

---

## 6. Migration risks

1. **Live remote database.** `OrgConnection` is a live remote DB (per persistent project memory).
   `dotnet ef migrations add` is safe to run and inspect; `dotnet ef database update` must **never**
   run without fresh, explicit user consent in this exact session.
2. **FiscalYear/FiscalPeriod relocation, if pursued, is the highest-risk single change in this
   entire effort.** It requires re-plumbing `Journal.Post`/`CreateReversal`, `
   IAccountingPeriodService`, the `DeleteBehavior.Restrict` FK, and the `Accounting.Contracts.
   Postings` query/command surface that Payables/Receivables already depend on. Given `docs/module-
   ownership.md` explicitly rates this placement "Owned" (correct, not a violation), **the default
   recommendation is to leave FiscalYear/FiscalPeriod in Accounting** and have Organization consume
   them via `Accounting.Contracts` if/when it needs to (e.g. to default a Branch's fiscal calendar) —
   treat physically moving them as a separate, explicitly-approved future phase, not part of
   standing up Organization.
3. **Currency/Country/City/District relocation** is comparatively low-risk (no lifecycle coupling
   like FiscalYear has, only scalar-FK and two documented in-context navigations to accommodate:
   `Parties.Dealer` and `Treasury.Bank`/`BankBranch`), and would follow the exact Parties/
   CommercialDocuments/Catalog relocation playbook (preserve `[Table(...)]`, no-op migration diff,
   update `Architecture.Tests` module registration). Still a real, non-trivial decision — see §9.
4. **In-flight Catalog work.** Uncommitted Product/Classification/Unit relocation and an unapplied
   `RelocateCatalogEntities` migration currently sit in the working tree. It does not intersect
   Organization's scope, but any new migration this task adds will stack on top of it in the same
   migrations folder/history — sequence new Organization migrations after Catalog's, not before.
5. **Multi-tenancy retrofit, if the new `Company` design decides to introduce real `CompanyId`
   scoping, is schema-breaking** — every transactional table (Journal, Invoice, User, Payable, etc.)
   would need a new column plus a global query filter. This should be an explicit, separately-
   scoped decision, not bundled silently into "add a Company entity."
6. **`Currency.Name = "Epg"` seed typo** and **`OrgContext.Currencys` DbSet typo** — safe to fix
   opportunistically during any Currency-touching migration, not urgent on their own.
7. **`docs/module-ownership.md` and `docs/dependency-rules.md` are partially stale** (they still
   describe `Dealer`/`Invoice` as owned by `Sales.Domain`; both have since moved to `Parties`/
   `CommercialDocuments`). Their *methodology* (ownership status legend, accepted-exception
   tracking) is still the right pattern to follow; their specific ownership claims for
   Dealer/Invoice should not be trusted without cross-checking current code — this report already
   did so for everything in its own scope.

---

## 7. Proposed target ownership (input to Phase 1 design doc)

| Concept | Current Owner | Proposed Owner | Action |
|---|---|---|---|
| `Company` (new) | — | **Organization** | New — greenfield design, no legacy data |
| `Branch` | Organization.Domain | **Organization** | Unchanged — already correctly placed |
| `Organization/Company Settings` (new) | — (`Preference` is user-scoped, not this) | **Organization** | New, narrowly scoped (display/default-currency/default-country/fiscal config only — not a generic KV dump) |
| `FiscalYear`, `FiscalPeriod` | Accounting.Domain | **stays Accounting** (recommended) | Leave in place; Organization consumes via `Accounting.Contracts.Postings` if needed. Physically moving is a separate, higher-risk future phase — see §6.2 |
| `Currency` | MasterData.Domain | **Organization** (candidate) | Relocate using the Parties/Catalog playbook — needs Phase 1 sign-off; low technical risk, moderate fan-out (40+ consumer files, all scalar-FK) |
| `ExchangeRate` (new) | — | **Organization** (candidate) or defer | New functionality (no history table exists today) — evaluate real business need before building, per this repo's established over-engineering caution (see Catalog report §10.3 for the same reasoning applied to ProductVariant) |
| `Country`, `City`, `District` | MasterData.Domain | **Organization** (candidate) | Relocate using the same playbook; two in-context-navigation consumers to update (`Parties.Dealer`, `Treasury.Bank`/`BankBranch`) |
| `Shift`, `Table` (currently co-located in `Organization.Domain`) | Organization.Domain | **stays Organization**, or split to a POS-specific module | Out of this task's requested scope (Company/Branch/Fiscal/Currency/Geo/Settings) — flag for a Phase 1 decision on whether they belong in the same bounded context going forward |
| `Preference` (generic per-user KV) | Administration.Domain | **stays Administration** | Not Organization's concern — it's per-user, not per-company |
| Multi-tenancy (`CompanyId`/`TenantId` scoping) | — | Separate decision | Do not bundle into standing up Organization; evaluate as its own phase if/when real multi-company support is required |

This table is provisional input to a Phase 1 ownership/target-architecture doc, not a final
decision — in particular, the Currency/Geography relocation and the FiscalYear "leave in place"
recommendation should be explicitly confirmed (or overridden with a documented reason) before any
Domain code is touched, per this task's own "document deliberate deviations" rule.

---

## 8. Open questions carried into Phase 1 (not blocking, but must be resolved there)

1. **Tenant vs. Company vs. Legal Entity vs. Branch** — since none of Tenant/multi-company exists
   today, this is a pure design choice, not a discovery finding. Recommend: `Company` = single
   root record representing "this installation's organization" (revives/redesigns
   `CompanyProfile`'s intent), `Branch` continues as-is (already company-agnostic — has no
   `CompanyId` today and doesn't need one unless real multi-company is later approved). Do **not**
   introduce a `Tenant` concept unless multi-tenant SaaS is an explicit, approved requirement — the
   dead `AdminContext`/`Client.DbSchema` schema-per-tenant design is prior art to consciously reject
   unless revived on purpose.
2. **Does Currency/Country/City/District physically move into `Organization`, or does `MasterData`
   simply get renamed/kept as the geo+currency reference-data home?** `docs/masterdata-
   decomposition.md` (written before this task) already recommended Currency/Country/City/District
   **stay in MasterData** as reference data, with lookup DTOs as the fix for direct-reference
   fan-out — this conflicts with the brief's assumption that Organization should own them. Needs an
   explicit reconciliation in Phase 1: either follow this task's brief (relocate into Organization)
   or follow the earlier MasterData decision (leave in place, Organization references via
   Contracts) — both are defensible; pick one and document why, per rule #19 precedent already
   established for Catalog vs. Classification/Unit.
3. **FiscalYear/FiscalPeriod ownership** — recommend leaving in Accounting (§6.2, §7); needs
   explicit confirmation since it's the one deviation from the brief's literal Company/Branch/
   FiscalYear/FiscalPeriod/Currency/Geography scope list for Organization.
4. **Menu/IA naming** — current menu label is "Settings", brief suggests "Administration". Decide
   before touching `menu.config.ts` (permission-key typos are coupled to this and need a
   coordinated rename, not a cosmetic relabel).
5. **`Shift`/`Table`'s continued home** — restaurant-POS concepts currently co-located in
   `Organization.Domain` alongside Branch/CompanyProfile; not in this task's requested scope, но
   worth an explicit "leave as-is" decision rather than silent scope creep either direction.
