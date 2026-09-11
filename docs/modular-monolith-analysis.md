# OrgSys — Modular Monolith Analysis

Status: **Analysis only — no source code has been moved.** This document is Step 1 of the
modular-monolith refactor. Step 2 (`modular-monolith-target-architecture.md`) defines the target
structure and dependency rules. Phase 1 implementation (foundation scaffolding only) begins only
after both documents are reviewed.

Scope note: this analysis covers the active backend (`Domain`, `Application`, `Infrastructure`,
`API`) plus the parts of `OrgSys.Angular` needed to judge frontend/backend alignment. The legacy
`OrgSys` Razor/MVC project is inventoried but treated as out-of-scope for the refactor (see §12).

---

## 1. Current Architecture

Classic 4-layer Clean Architecture, single solution, single database, CQRS via MediatR:

```text
API  ──depends on──▶ Application, Domain, Infrastructure
Infrastructure ──depends on──▶ Application
Application ──depends on──▶ Domain
Domain ──depends on──▶ (nothing internal)
```

- **Domain**: entities, enums, abstractions (`IRepository<T>`, `IUnitOfWork`, `IOrgContext`,
  `IAdminContext`), shared `Result`/`Error` types. No business logic beyond data annotations and
  a couple of FK-navigation-only relationships.
- **Application**: CQRS commands/queries/handlers (MediatR), DTOs, AutoMapper profiles,
  FluentValidation validators, and a small set of hand-written domain services
  (`AccountingPeriodService`, `ReceivableAccountValidator`, `PayableAccountValidator`).
- **Infrastructure**: two `DbContext`s (`OrgContext` active, `AdminContext` dormant), a generic
  `Repository<T>`/`UnitOfWork` implementation tied to `IOrgContext`, EF Core migrations, and one
  large `InitialData.cs` seed file.
- **API**: ASP.NET Core minimal-hosting Web API (`Program.cs`, no `Startup.cs`), JWT auth,
  Swagger, one global exception middleware, and ~40 controllers under `Controllers/Org/**`
  grouped by folder (Financials, Invoices, Reports, Setting, Transaction) plus Auth.
- **OrgSys.Angular**: the live SPA frontend, organized by technical/menu concerns
  (Settings, Accounting, Financial, Customers & Suppliers, Invoices, Inventory, Reports) — not
  yet aligned to the 11 target business modules.
- **OrgSys** (legacy): a pre-existing Razor/MVC app with its own `Startup.cs`, `Repository`,
  `Service`, and `Areas/` folders, still present in `OrgSys.sln`. Per the recent commit history
  ("add OrgSys.Angular and convert from MVC to Angular Version 1"), this is being superseded by
  `API` + `OrgSys.Angular`. **Do not refactor or touch this project** — confirm with the team
  whether it is still deployed before deciding to retire it from the solution (see §21 open
  questions).
- **Tests/Application.Tests**: exists in the solution; not inventoried in depth here, but any
  module split must keep it (or its successor per-module test projects) green.

### Dead / abandoned scaffolding found

Two prior, unfinished refactor attempts are present as inert code and should be treated as
housekeeping, not as a foundation to build on:

- `Domain/Shared/{LockupTypeEntity, LockupTreeEntity, LockupTypeTreeEntity, TransactionTypeEntity, TransactionTreeEntity, TransactionTypeTreeEntity}.cs` — a typed entity base-class hierarchy that **no actual entity inherits from** (all real entities inherit `BaseModel`/`MovementModel`). Confirmed by grep: the only matches for `: LockupTypeEntity` etc. are the six files' own internal inheritance chain.
- `Domain.csproj` reserves empty `Aggregate/`, `Interfaces/`, `Events/`, `Exceptions/`, `ValueObjects/` folders (no files in them) — likely intended for a DDD pass that never happened.
- `Application.csproj` and `Infrastructure.csproj` still exclude source paths (`Contracts\**`, `Features\**`, `Profiles\**`, `Repositories\**`, `UnitOfwork\**`, `Validators\ValidationFilter.cs`) that **no longer exist on disk** — stale `.csproj` entries from deleted folders.

These are candidates for cleanup during Phase 1 (deleting stale `.csproj` exclusions, deciding
whether to delete or repurpose the unused `Domain/Shared` type hierarchy) but are not part of the
module boundaries themselves.

---

## 2. Current Projects

| Project | Path | Target | Key packages | References |
|---|---|---|---|---|
| Domain | `Domain/Domain.csproj` | net10.0 | AutoMapper, MediatR, EFCore (abstractions only), CorePagination | — |
| Application | `Application/Application.csproj` | net10.0 | AutoMapper, FluentValidation, MediatR | Domain |
| Infrastructure | `Infrastructure/Infrastructure.csproj` | net10.0 | AutoMapper, EFCore.SqlServer, Configuration | Application |
| API | `API/API.csproj` | net10.0 | AutoMapper, FluentValidation(+DI ext), MediatR, JwtBearer, Swashbuckle, EFCore.Design | Application, Domain, Infrastructure |
| OrgSys (legacy MVC) | `OrgSys/OrgSys.csproj` | — (not inventoried) | — | own copies of Domain/Repository/Service |
| Application.Tests | `Tests/Application.Tests/Application.Tests.csproj` | net10.0 | — | Application |

All four active projects share `net10.0`, MediatR 14.1.0, AutoMapper 16.1.1, FluentValidation
12.1.1 — no version drift between layers, which simplifies extraction into `BuildingBlocks/`.

---

## 3. Current Domain Entities

118 `.cs` files under `Domain/`, of which ~90 are entities. Two entity families:

- **`Domain/Entities/OrgDb/*`** (55 entities) — the **active**, per-tenant business schema behind
  `OrgContext`. This is effectively the entire live application.
- **`Domain/Entities/AdminDb/*`** (16 entities: `Client`, `ClientPlan`, `Plan`, `PlanElement`,
  `PlanType`, `Request`, `LoginUser`, `Nationality`, `TypeActivity`, `GeneralCity`,
  `GeneralClassification`, `GeneralCountry`, `GeneralDistrict`, `GeneralProduct`,
  `GeneralProductPropertyElement`, `GeneralProductRecipe`, `GeneralProductUnit`,
  `GeneralProperty`, `GeneralPropertyElement`, `GeneralUnit`) — a **dormant** multi-tenant
  SaaS/provisioning schema behind `AdminContext`. See §7 for why this is dormant.

### Base classes

```text
BaseModel            (Id, CodeNumber, Code, MaskText, ParentId, TypeId, Hide, ImgPath, Status)
 └─ MovementModel     (+ Date, CreateUser/Date, ModifyUser/Date, Shift, Branch, HasJournal, Review, Posted)
```

`MovementModel` is the shared base for every "document" entity: `Invoice`, `Transaction`,
`Inventory`, `Financial`, `FinancialTransfer`, `Journal`, `Order`. This is the closest thing the
codebase has to a shared "transactional document" concept and is a strong `SharedKernel`
candidate (see target architecture doc) — **but only the shape, not the FKs to `User`/`Shift`/`Branch`**, which are module-owned concepts.

### Confirmed unified, type-driven aggregates (must be preserved per refactor rules)

| Aggregate | Type discriminator table | Enum mirror | Business area |
|---|---|---|---|
| `Invoice` (+`InvoiceProduct`) | `InvoiceType` (Sales Invoice, Purchase Invoice, Sales Return, Purchase Return) | — | Sales + Purchasing share one entity |
| `Transaction` (+`TransactionProduct`) | `TransactionType` (Addition, Issue, Transfer, Received, Adjustment In/Out, Opening Balance, Damaged) | — | Inventory |
| `Financial` | `FinancialType` (11 rows) | `Domain.Enums.FinancialTransactionType` (OpeningBalance, Receipt, Payment, TransferIn, Deposit, Withdrawal, Fee, Interest, Cheque, Adjustment, TransferOut) | Treasury |
| `Journal` (+`JournalItem`) | `JournalType` (Opening Balance, Journal) | — | Accounting |
| `Dealer` | — | `Domain.Enums.DealerType` (Client=1, Supplier=2) | **Shared** Sales (Customer) / Purchasing (Supplier) |

The `Financial`/`FinancialTransactionType` pairing is explicitly documented in code comments as a
single Id space shared between the enum and the seeded `FinancialType` table (migration
`MergeFinancialTypeWithTransactionTypeEnum` formalized this). **This is exactly the pattern
section 23 of the brief says to preserve — do not split it.**

`Dealer` is the one entity that is genuinely owned by two future modules at once (Sales owns
"Customer" semantics, Purchasing owns "Supplier" semantics, same table, `DealerType`
discriminator). This needs an explicit decision in the target architecture (see that document,
§ Dealer ownership) rather than a default "pick one module" mapping.

### Entities with no dedicated CQRS/API surface yet (see §11, §18)

`Order`, `OrderProduct`, `OrderType` exist as entities and are mapped in `MovementModel`/FK
relations, but their `DbSet`s are **commented out** in `OrgContext` and there is no
`Commands/Org/Orders` folder — Quotations/Sales Orders are modeled in the schema but not wired
up anywhere. Likewise `LogSys`, `Notification`, `ProductRecipe`, `PropertyElement`,
`ProductPropertyElement` have commented-out `DbSet`s.

---

## 4. DTOs

- `Application/DTOs/AdminDb/` — 19 files, mirror the dormant `AdminDb` entities. **Zero
  references anywhere in `Application/Commands`, `Mappings`, or `Validators`** (confirmed by
  grep) — these DTOs are orphaned.
- `Application/DTOs/OrgDb/` — 56 files, flat (no subfolders), one per active `OrgDb` entity/view
  concept (includes composite DTOs like `AccountTreeNodeDto`).
- `Application/DTOs/Report/` — 25 files, read-model DTOs for the Reports feature (`DealerBalance`,
  `DealerStatment` [sic], `SafeBalance`, `SalesBalance`, `StockStatment` [sic], `ProductCost`,
  etc.).

---

## 5. CQRS Commands/Queries

`Application/Commands/Org/` has six top-level feature folders. Full detail (per-handler
cross-references) is in the working notes; the summary that matters for module boundaries:

| Feature folder | Sub-features | Files | Notes |
|---|---|---|---|
| Auth | (flat) | 4 | Login/CheckEmail/CheckPassword/HavePassword — entirely against `OrgDb` (`User`, `Role`, `Permission`). No link to `AdminDb`/`LoginUser` at all — see §7. |
| Financials | Financial, FinancialTransfer, Integration, Journal, Payable, Receivable | 36 | The GL-posting core. `Integration/` (`InvoiceJournalIntegration`, `TransactionJournalIntegration`) is the deliberate cross-module posting bridge from Invoices/Transactions into the ledger. |
| Invoices | Invoice | 15 | Injects `Financial`, `Journal`, `Transaction`, `Preference` repositories **directly** in handlers (e.g. `CreateJournalByInvoiceCommandHandler`) rather than through a facade. |
| Reports | Dealer, Financial, Sales, Warehouse | 7 | Inherently cross-cutting read models; touch Financial/Invoice/CashBox/TransactionProduct directly. |
| Setting | 31 sub-features (Account, AccountType, Bank, BankBranch, Branch, CashBox, City, Classification, Country, Currency, Dealer, DealerGroup, District, FinancialAccount, FinancialType, FiscalYear, InvoiceType, JournalType, Outlay, PaymentType, Preference, Product, ProductUnit, Property, ReferenceType, Role, Shift, Stock, Table, Unit, User) | 281 | Bulk of the codebase; mostly plain CRUD. **This is a catch-all folder that mixes at least 6 future modules** (Administration, Organization, MasterData, Accounting, Treasury, Sales/Purchasing-adjacent Dealer). |
| Transactions | Inventory, Transaction, TransactionType | 37 | Mirrors Invoices: `Transaction` handlers inject `Invoice`, `Journal`, `Preference` directly. |

### Confirmed cross-boundary logic living in the wrong place today

1. **`Setting/Dealer`** directly provisions GL `Account` rows (`DealerPayableAccountProvisioning`,
   `DealerReceivableAccountProvisioning`) and reads `JournalItem` to compute dealer balance
   (`GetBalanceQueryHandler`) — this is Accounting/Treasury-owned logic sitting inside a
   Sales/Purchasing-adjacent feature folder.
2. **`Common/Services`** (`AccountingPeriodService`, `ReceivableAccountValidator`,
   `PayableAccountValidator`) is not generic infrastructure — it is Financials/Accounting domain
   logic reused by `Setting/Dealer` and `Financials/Journal`. `PayableAccountValidator` and
   `ReceivableAccountValidator` independently re-implement near-identical
   "validate supplier dealer + account" logic — a duplication worth resolving when Treasury/
   Accounting/Receivables/Payables boundaries are drawn.
3. **`Setting/Product`**'s `GetListByBalanceQueryHandler` reads `TransactionProduct` directly
   (Inventory-owned data) to compute stock balance from a Setting/MasterData-adjacent feature.
4. **Invoice and Transaction command handlers** inject Financial/Journal repositories directly for
   GL posting instead of calling a single posting service — today this is "fine" because
   everything is one assembly, but it is the #1 thing that must become a Contracts/event boundary
   before any module can be extracted as a microservice.

### CQRS infrastructure

- `Interfaces/Command` + `Interfaces/Query` (namespace `Application.Abstraction.Command/Query`,
  folder name mismatch) — thin wrappers over MediatR `IRequest`/`IRequestHandler` returning the
  project's own `Result`/`Result<T>`/`ResultPagination<T>` types.
- `Interfaces/CQRS` — marker interfaces (`ICreateCommand`, `IUpdateCommand`, `IGetByIdQuery`, etc.).
- `Common/Commands` + `Common/Queries` — generic base handlers
  (`CreateCommandHandler<TDto,TModel>`, `GetQuery`, `ListQuery`, `SearchCommandHandler`, …) that
  nearly every feature's handlers subclass, via `IRepository<T>` + `IMapper`.
- **One** MediatR pipeline behavior: `FluentValidationFilter<TRequest,TResponse>`. No
  logging/transaction/auditing behavior exists at the pipeline level.

### AutoMapper & FluentValidation organization

- AutoMapper: one logical `MappingProfile` class, physically split via `partial class` across
  ~37 per-feature files (~372 `CreateMap` calls total), aggregated by a single constructor that
  calls each feature's `XxxMappingProfile()` method. **Registered as one profile, not genuinely
  per-module** — will need to become real separate `Profile` classes per module.
- FluentValidation: base `Validator<TCommand,TEntity>` + concrete validators in
  `Commands/Org/.../Validators/` (27 files, only 13 of 37 Setting sub-features have one).
  **Many validator files are misnamed** — e.g. `CreateUserCommandValidator.cs` actually contains
  `CreateBankCommandValidator`, `CreateCurrencyCommandValidator`, etc. (copy-paste-from-User
  leftovers in filenames only; the classes inside are correctly named). Cosmetic, but confusing
  during a module split — worth a rename pass.

---

## 6. Repositories / Unit of Work

- `Domain/Abstraction/IRepository<TEntity>` — generic, `TEntity : BaseModel`, filter/pagination
  methods via `Expression<Func<TEntity,bool>>`, soft-delete convention baked in
  (`Status != Deleted && Hide != true`).
- `Infrastructure/Persistence/UnitOfWork/Repository.cs` — implements `IRepository<T>`, but its
  constructor takes `IOrgContext` **directly**, not a generic `DbContext` — it cannot be pointed
  at `AdminContext`. `Delete`/`Update` do manual `Entry(...).CurrentValues.SetValues(...)`
  (soft-delete-by-flag, not a true relational delete).
- `Infrastructure/Persistence/UnitOfWork/UnitOfWork.cs` — thin wrapper forwarding to `IOrgContext`
  (`SaveChangeAsync`, `Begin/Commit/RollbackTransactionAsync`, `ResetDbContextState`).
- **No tenant/schema-switching logic anywhere.** `Client.DbSchema` (AdminDb) and a commented-out
  `OrgContext.Schema` property suggest an original schema-per-tenant design intent that was never
  finished (see §7).

This generic repository is injected via `AddScoped(typeof(IRepository<>), typeof(Repository<>))`
and used by virtually every handler in the project — it is infrastructure every module will keep
depending on during Phase 1–9; it should **not** be forked per module until module-specific
`DbContext`s are introduced (explicitly deferred by the brief, §17).

---

## 7. DbContext and DbSets

Two contexts exist; only one is live.

### `OrgContext` (active — `IOrgContext`, ~44 `DbSet`s)

`Unit, Property, DealerGroup, Dealer, Classification, Product, ProductUnit, Branch, Stock, Role,
Shift, User, Permission, Invoice, InvoiceProduct, InvoiceType, PaymentType, ReferenceType,
Preference, TransactionType, Transaction, TransactionProduct, Inventory, InventoryProduct, Table,
CashBox, Financial, FinancialAccount, FinancialTransfer, FinancialInvoice, FinancialType, Outlay,
Currency, RolePermission, Account, BankAccount, AccountType, Bank, BankBranch, City, Country,
District, JournalType, Journal, JournalItem, FiscalYear, FiscalPeriod`.

Commented-out (not currently mapped): `Order, OrderProduct, OrderType, LogSys, ProductRecipe,
PropertyElement, ProductPropertyElement, Notification`, plus a block of `[NotMapped]` report
view-models that were apparently once EF-mapped and are now handled purely as query DTOs.

`OnModelCreating` has a handful of explicit fluent rules: `Journal.Rate` precision,
`CashBox`/`BankAccount` 1:1 with `FinancialAccount`, `Financial` → `FinancialAccount` /
`ContraFinancialAccount` restrict-delete, `FinancialTransfer` → From/To `FinancialAccount`
restrict-delete, `Journal` → `FiscalYear`/`FiscalPeriod` restrict-delete, a `Journal.Date` index,
and a **unique filtered index** enforcing at most one reversal journal per original
(`OriginalJournalId` unique where not null) — this is a real business invariant enforced at the
DB level, not just in a handler; it must survive the refactor unchanged.

### `AdminContext` (dormant — `IAdminContext`, ~17 `DbSet`s)

`Client, ClientPlan, Plan, PlanElement, PlanType, Request, LoginUser, Nationality, TypeActivity,
GeneralCity, GeneralClassification, GeneralCountry, GeneralDistrict, GeneralProduct,
GeneralProductPropertyElement, GeneralProductRecipe, GeneralProductUnit, GeneralProperty,
GeneralPropertyElement, GeneralUnit`.

**This context is explicitly excluded from compilation**:
`Infrastructure.csproj` has `<Compile Remove="Persistence\Data\AdminContext.cs" />` and
`Domain.csproj` has `<Compile Remove="Abstraction\IAdminContext.cs" />`. It has **no EF Core
migrations at all** (only `OrgContext` has a `Migrations/` folder), and it is **not registered in
DI anywhere** in `API/Program.cs`. It is source-present but dead at runtime.

`OnModelCreating` for `AdminContext` is entirely `HasData(...)` seed calls (166 `Nationality` rows
plus a handful of `TypeActivity`/`Plan`/`Request`/`Client`/`ClientPlan`/`LoginUser` rows) — this
looks like an early proof-of-concept for a SaaS-provisioning layer that was parked. **Conclusion:
the "Administration" business module target should be built around the live `User`/`Role`/
`Permission`/`RolePermission` entities in `OrgDb`, not around the dormant `AdminDb` SaaS layer.**
Whether to revive, delete, or keep `AdminDb` dormant is a decision for the team (§21).

### No `IEntityTypeConfiguration<T>` anywhere

Confirmed by repo-wide grep: zero implementations in `Infrastructure` or `Domain`. All EF
configuration is inline in the two `OnModelCreating` methods above. A module split will need to
extract this into per-module configuration classes (`Treasury.Infrastructure/Persistence/
Configurations/*`, etc., per the target architecture) — there is no existing per-entity config to
carry over mechanically; each rule above needs to be manually relocated to its owning module's
Infrastructure project and re-verified against `dotnet ef migrations has-no-changes` (or
equivalent no-op-migration check) after the move.

---

## 8. EF Core Migrations

Single shared folder `Infrastructure/Migrations/` (OrgContext only), 16 migrations in
chronological order:

`CreateDatabase → AddJournalType → AddAccountToStock → AddInventoryToTransaction →
AddInventoryAdjustmentFlag → UnifyFinancialAccountsAndTransactions → ConsolidateFinancialTypes →
AddFiscalYearAndPeriod → AddJournalDateIndex → AddJournalReversalLink →
AddAccountIsPostableAndArConfig → RenameAccountBankToBankAccount → AddCountryToBank →
AddBranchToBankAccount → RenameSafeToCashBox → MergeFinancialTypeWithTransactionTypeEnum →
AddReferenceType`.

The trend is unmistakable: **General Ledger / Accounting features (fiscal year/period, journal
reversal, account postability, the Financial/TransactionType unification) are the newest and most
actively evolving part of the schema.** This matches the brief's Phase 2 = Accounting priority —
Accounting is where the team has been doing the most recent, highest-risk schema work, so it
benefits most from getting clean module boundaries early, but for the same reason it is also the
**highest migration-risk** module to touch first (see §13 risk column).

---

## 9. Controllers / API Endpoints

`API/Controllers/` — `BaseController.cs` defines the real hierarchy (generic `CoreController<...>`
→ `BaseController<...>`, reflection-driven CRUD actions built via `Activator.CreateInstance` off
generic type parameters); `MainController.cs` is a dead VS-scaffold leftover
(`<Compile Remove>`'d... actually still compiled but unused route, verify before deleting).

| Folder | Controllers | Pattern |
|---|---|---|
| `Org/` (root) | `AuthenticationController` (class `AuthController`, route `api/Auth`) | `[AllowAnonymous]`: Login, CheckEmail, CheckPassword, HavePassword |
| `Org/Financials/` | `FinancialAccountController`, `FinancialController`, `FinancialTransferController`, `FinancialTypeController`, `JournalController`, `PayableController` | CRUD + domain actions (`Redo`, `Cancel`, `Post`, `Reverse`, `SetCustomerOpeningBalance`, `PostTransaction`) |
| `Org/Invoices/` | `InvoiceController` | CRUD + `Cancel`, `Redo`, `CollectPaidInvoice`, `CreateTransactionInvoice`, `CreateJournal`, `GetInvoicesNotReturn`, `SearchInvoice`, `GetProductInvoicesNotReturn` |
| `Org/Reports/` | `DealerReportController`, `FinancialReportController`, `SalesReportController`, `WarehouseReportController` | read-only |
| `Org/Setting/` | 25 controllers (Account, AccountType, BankBranch, Bank, Branch, CashBox, City, Classification, Country, Currency, Dealer, DealerGroup, District, FiscalYear, InvoiceType, JournalType, Outlay, PaymentType, Preference, Product, ProductUnitModelView, Property, ReferenceType, Role, Shift, Stock, Table, Unit, User) | mostly plain CRUD via `BaseController<...>` |
| `Org/Transaction/` | `TransactionController`, `InventoryController`, `TransactionTypeController` | CRUD + `Cancel`, `Redo`, `CreateReceived`, `CreateJournal`, `CreateAdjustment` |

**Findings that matter for the refactor:**

1. `FinancialTransferController` and `PayableController` have **no `[Authorize]` and no
   `[AllowAnonymous]`** at all — every other controller gets `[Authorize]` either directly or by
   inheriting `BaseController`. This looks like a pre-existing bug, not an architectural decision.
   Flag to the team; fixing it is a one-line, low-risk change but is **out of scope for the
   modularization itself** unless the team asks for it to be bundled in.
2. `Setting/` is today's biggest catch-all: it holds `Account`/`AccountType` (→ Accounting),
   `Bank`/`BankBranch`/`CashBox` (→ Treasury), `Dealer`/`DealerGroup` (→ Sales/Purchasing-shared),
   and genuine reference data (`City`/`Country`/`District`/`Currency`/`Unit`/`Classification` →
   MasterData) all in one folder. Angular's own menu already splits these across different menu
   sections (Settings, Accounting, Financial, Customers & Suppliers) — the backend folder
   structure lags the frontend's own mental model.
3. No custom permission attribute or `AuthorizationHandler`/policy exists anywhere — authorization
   is binary `[Authorize]`/`[AllowAnonymous]`. The fine-grained permission codes seeded into JWT
   claims (`Countrys.All`, `Financial.View`, etc., see §10) are **only checked client-side** in
   `menu.config.ts`. There is no server-side enforcement of these permission codes on any
   controller action today. This is a pre-existing security gap, not something introduced by
   modularization — call it out to the team as its own item (§21), do not silently "fix" it as a
   side effect of a module-boundary PR.
4. `GlobalExceptionMiddleware` is registered **after** `app.MapControllers()` in `Program.cs`,
   which is unconventional placement (usually right after routing, before auth). It appears to
   still work under ASP.NET Core's minimal-hosting terminal-middleware semantics, but should be
   double-checked, not "fixed" incidentally during modularization.
5. `Program.cs` has duplicate registrations: `AddSwaggerGen` called twice, `AddControllers` called
   twice, `AddMediatR` called twice (once anchored on `MappingProfile`'s assembly, once on
   `FluentValidationFilter<,>`'s assembly — both resolve to `Application`, so functionally
   harmless but redundant). No `AddInfrastructure`/`AddApplication` extension methods exist
   anywhere — all registration lives directly in `Program.cs` (and is **duplicated** in the legacy
   `OrgSys/Startup.cs`). This duplication is precisely what section 20 of the brief asks the
   target architecture to replace with per-module `AddXModule()` extensions.

---

## 10. Permissions

- `Permission` (Id, Key, Name) + `RolePermission` (RoleId, PermissionId) + `Role` — classic RBAC,
  seeded with ~150+ permission rows in a hierarchical tree (`Organizer → Data/Orders/Invoices/
  Transactions/Financials modules`, e.g. `Branchs.View`, `SalesInvoices.Add`) by
  `InitialData.InitialPermission()`.
- JWT (`API/Authentication/JwtTokenService`) embeds one `permission` claim per entry in the user's
  resolved permission set, plus `NameIdentifier`, `Name`, `displayName`, `Role`/`roleId`,
  `roleName`, optional `branchId`. **No tenant claim** — consistent with the app being
  single-tenant per deployment today (see §7 multi-tenancy finding below).
- `menu.config.ts` reads these same permission keys (`permissionKeys: 'Countrys.All,Countrys.View'`)
  to show/hide menu entries — this is the *only* place permission keys are actually enforced.
- **No server-side authorization policy/attribute maps a permission key to a controller action.**
  This is the single biggest "must not regress" item for the refactor: the permission *data model*
  (Permission/RolePermission/seed tree) must move to Administration intact, and whatever session
  later closes the server-side-enforcement gap must be able to find it without redesigning it.

### Multi-tenancy (bears directly on Administration/Organization module boundaries)

Not implemented at runtime. `Client.DbSchema` (seeded `"org"`) and a commented-out
`OrgContext.Schema` property plus commented-out `ReplaceService<IModelCacheKeyFactory,...>` /
`ReplaceService<IMigrationsAssembly,...>` calls in `OnConfiguring` show an abandoned
schema-per-tenant design. Both `OrgContext` and `AdminContext` today read one connection string
from `appsettings.json` — one deployment = one tenant. **If real multi-tenancy is wanted later, it
is greenfield work, not something this refactor needs to preserve.**

---

## 11. Angular-Facing API Contracts

Controllers use `[Route("[controller]")]` (not `api/[controller]`, except `Auth`), so today's
routes are e.g. `/FinancialAccount`, `/Invoice`, `/Dealer`, `/TransactionType`. **Section 21 of the
brief requires these to stay unchanged initially.** Cross-checked against
`OrgSys.Angular/src/app/core/config/menu.config.ts`:

| Angular menu group | Routes | Backend controllers | Alignment |
|---|---|---|---|
| Settings | `/administration/{countries,cities,districts,currencies,fiscal-years,branches,banks,bank-branches,products}` | `Setting/CountryController`, `CityController`, `DistrictController`, `CurrencyController`, `FiscalYearController`, `BranchController`, `BankController`, `BankBranchController`, `ProductController` | good |
| Accounting | `/accounting/{accounts,journal-entries}` | `Setting/AccountController` + `Financials/JournalController` | **split across two backend folders** for one Angular menu group |
| Financial | `/financial/{financial-accounts/*, transfers, opening-balances, transactions/2-11}` | `Financials/FinancialController`, `FinancialTransferController`, `FinancialAccountController`, plus `Setting/CashBoxController` | **CashBox lives in `Setting/`**, not `Financials/` |
| Customers & Suppliers | `/customers-suppliers/{dealers,dealer-groups}/*` | `Setting/DealerController`, `DealerGroupController` | Dealer is under `Setting/`, not its own folder, despite being a first-class Angular menu section |
| Invoices | `/invoices/{1-4}` | `Invoices/InvoiceController` (single controller, `InvoiceType` discriminator) | good |
| Inventory | `/transactions/{1,2,3,5,6,7,8}`, `/inventory` | `Transaction/TransactionController`, `InventoryController`, `TransactionTypeController` | good |
| Reports | `/reports/{warehouse,dealers/{1,2},finance,sales}/*` | `Reports/*` | good |

**Takeaway:** the Angular frontend already thinks in terms closer to the target modules
(Accounting vs Financial vs Customers&Suppliers as separate concepts) than the current backend
folder structure does. `Setting/` is the main offender — pulling `AccountController`,
`CashBoxController`, `DealerController`/`DealerGroupController` out of `Setting/` into their
correct future modules is low-regression-risk *because the frontend already treats them as
separate concerns*, it's a backend-only reorganization with no route changes required if the
route pattern (`[Route("[controller]")]`, independent of physical folder) is preserved.

---

## 12. Existing Module Candidates (bottom-up, from what the code already suggests)

| Candidate module | Evidence in current code |
|---|---|
| **Administration** | `User`, `Role`, `Permission`, `RolePermission` (OrgDb, live) + `Commands/Org/Auth` + `Setting/Role`, `Setting/User`. Dormant `AdminDb` (Client/Plan/LoginUser) is a *separate*, currently-unused SaaS-provisioning concept — do not conflate the two. |
| **Organization** | `Branch`, `CompanyProfile`, `Shift` — currently scattered in `Setting/`, small. |
| **MasterData** | `Country`, `City`, `District`, `Unit`, `Classification`, `Currency`, `ReferenceType`, `PaymentType` — self-contained CRUD under `Setting/`, no cross-references beyond `City→Country`. |
| **Sales / Purchasing** | `Dealer`(+`DealerType`), `DealerGroup`, `Invoice`(+`InvoiceType`), `InvoiceProduct` — one shared `Dealer`/`Invoice` aggregate serving both; `Order`/`OrderType` scaffolded but unwired (future Quotations/Sales Orders). |
| **Inventory** | `Product`, `ProductUnit`, `Stock`, `Transaction`(+`TransactionType`), `TransactionProduct`, `Inventory`, `InventoryProduct`. `Product.DealerId` (supplier link) and `Stock.AccountId` (GL link) are the two cross-module edges to watch. |
| **Treasury** | `CashBox`, `Bank`, `BankAccount`, `BankBranch`, `FinancialAccount`, `Financial`(+`FinancialType`/`FinancialTransactionType`), `FinancialTransfer`, `FinancialInvoice`, `Outlay`. Already unified via the `FinancialType` discriminator exactly as the brief expects. |
| **Accounting** | `Account`, `AccountType`, `Journal`(+`JournalType`/`JournalItem`), `FiscalYear`, `FiscalPeriod`. Newest, fastest-moving part of the schema (see §8). |
| **Receivables / Payables** | No dedicated entities yet — currently expressed as `Financials/Payable`, `Financials/Receivable` (single `SetXOpeningBalanceCommand` each) plus `Common/Services` validators, plus `Setting/Dealer`'s balance/account-provisioning logic. This is a genuinely **thin/partial** module today (see §18). |
| **Reporting** | `Commands/Org/Reports/*` — already a separate top-level folder, already read-only, already cross-cutting by nature. Easiest module to formalize first as a Contracts consumer once the modules it reads from exist. |

---

## 13. Proposed Module Ownership Mapping

Legend for **Migration Risk**: Low = self-contained, no incoming cross-module references found;
Medium = some cross-module references exist but are one-directional and easy to contract-ize;
High = bidirectional/tightly-coupled today (posting integrations, shared aggregates, DB-level
constraints) — do not move until Contracts/events are designed.

### Domain entities

| Current Class | Current Location | Target Module | Target Layer | Key Dependencies | Migration Risk |
|---|---|---|---|---|---|
| User | Domain/Entities/OrgDb | Administration | Domain | Role, Branch | Medium (Branch = Organization) |
| Role | Domain/Entities/OrgDb | Administration | Domain | — | Low |
| Permission | Domain/Entities/OrgDb | Administration | Domain | — | Low |
| RolePermission | Domain/Entities/OrgDb | Administration | Domain | Role, Permission | Low |
| Client, ClientPlan, Plan, PlanElement, PlanType, Request, LoginUser | Domain/Entities/AdminDb | Administration (dormant/future tenant-provisioning) | Domain | — | Low (dormant — do not wire up as part of this refactor) |
| Nationality, TypeActivity | Domain/Entities/AdminDb | Administration (dormant) | Domain | — | Low (dormant) |
| General{City,Classification,Country,District,Product,ProductUnit,ProductPropertyElement,ProductRecipe,Property,PropertyElement,Unit} | Domain/Entities/AdminDb | MasterData (dormant duplicate of OrgDb equivalents) | Domain | — | Low (dormant) |
| Branch | Domain/Entities/OrgDb | Organization | Domain | — | Medium (referenced by User, Stock, CashBox, BankAccount, MovementModel) |
| CompanyProfile | Domain/Entities/OrgDb | Organization | Domain | — | Low |
| Shift | Domain/Entities/OrgDb | Organization | Domain | — | Low (referenced by MovementModel/User) |
| Country | Domain/Entities/OrgDb | MasterData | Domain | — | Low |
| City | Domain/Entities/OrgDb | MasterData | Domain | Country | Low |
| District | Domain/Entities/OrgDb | MasterData | Domain | City | Low |
| Unit | Domain/Entities/OrgDb | MasterData | Domain | — | Low |
| Classification | Domain/Entities/OrgDb | MasterData | Domain | — | Low |
| Currency | Domain/Entities/OrgDb | MasterData | Domain | — | Medium (referenced by Invoice, Financial, Journal, FinancialTransfer, FinancialAccount) |
| ReferenceType | Domain/Entities/OrgDb | MasterData | Domain | — | Low |
| PaymentType | Domain/Entities/OrgDb | MasterData | Domain | — | Medium (referenced by Invoice, Financial) |
| Property, PropertyElement, ProductPropertyElement, ProductRecipe | Domain/Entities/OrgDb | MasterData or Inventory (currently unmapped `DbSet`s — confirm intent before assigning) | Domain | Product | Low (inactive) |
| Dealer | Domain/Entities/OrgDb | **Shared: Sales owns "Customer" view, Purchasing owns "Supplier" view** — see target-architecture doc for the resolution pattern | Domain | DealerGroup, Country, City, District, Account | **High** — single table serving two future modules |
| DealerGroup | Domain/Entities/OrgDb | Sales/Purchasing (same shared-ownership question as Dealer) | Domain | — | Medium |
| Invoice, InvoiceProduct | Domain/Entities/OrgDb | Sales + Purchasing (shared via `InvoiceType`, preserve unified aggregate per brief §4/§5) | Domain | Dealer, PaymentType, Stock, Transaction, Currency | High (Financials/Transactions post directly against it) |
| InvoiceType | Domain/Entities/OrgDb | Sales/Purchasing (shared reference) | Domain | — | Low |
| Product | Domain/Entities/OrgDb | Inventory (with a supplier-linkage question — `DealerId`) | Domain | Classification, Dealer | Medium |
| ProductUnit | Domain/Entities/OrgDb | Inventory | Domain | Product, Unit | Low |
| Stock | Domain/Entities/OrgDb | Inventory | Domain | Branch, Account | Medium (Account = Accounting) |
| Transaction, TransactionProduct | Domain/Entities/OrgDb | Inventory | Domain | Dealer, Stock, Order, Inventory(entity), Invoice | High (Invoices/Financials post against it) |
| TransactionType | Domain/Entities/OrgDb | Inventory | Domain | — | Low |
| Inventory (entity) | Domain/Entities/OrgDb | Inventory | Domain | — | Low |
| InventoryProduct | Domain/Entities/OrgDb | Inventory | Domain | Inventory(entity), Product | Low |
| Order, OrderProduct, OrderType | Domain/Entities/OrgDb | Sales (future Quotations/Sales Orders) | Domain | Dealer, Product | Low (currently unmapped `DbSet`s, inactive) |
| CashBox | Domain/Entities/OrgDb | Treasury | Domain | Account, FinancialAccount, Branch, User(Keeper) | Medium |
| Bank, BankBranch | Domain/Entities/OrgDb | Treasury | Domain | Country | Low |
| BankAccount | Domain/Entities/OrgDb | Treasury | Domain | Bank, BankBranch, Account, FinancialAccount, Branch | Medium |
| FinancialAccount | Domain/Entities/OrgDb | Treasury | Domain | Account, Currency, CashBox, BankAccount | Medium |
| Financial | Domain/Entities/OrgDb | Treasury | Domain | Dealer, PaymentType, Outlay, Currency, FinancialAccount, FinancialType, FinancialTransfer, Journal | **High** — the busiest cross-module posting node |
| FinancialTransfer | Domain/Entities/OrgDb | Treasury | Domain | FinancialAccount ×2, Currency | Medium |
| FinancialInvoice | Domain/Entities/OrgDb | Treasury (settlement link) | Domain | Financial, Invoice | High (bridges Treasury ↔ Sales/Purchasing settlement) |
| FinancialType | Domain/Entities/OrgDb | Treasury | Domain | — | Low (keep in lockstep with `FinancialTransactionType` enum, see §3) |
| Outlay | Domain/Entities/OrgDb | Treasury | Domain | — | Low |
| Account | Domain/Entities/OrgDb | Accounting | Domain | AccountType | **High** — referenced by Dealer, Stock, CashBox, BankAccount, FinancialAccount (i.e., every module needs a read-only reference to Accounting) |
| AccountType | Domain/Entities/OrgDb | Accounting | Domain | — | Low |
| Journal, JournalItem | Domain/Entities/OrgDb | Accounting | Domain | JournalType, Currency, FiscalYear, FiscalPeriod, self (reversal link) | High — DB-enforced unique-reversal constraint must move intact |
| JournalType | Domain/Entities/OrgDb | Accounting | Domain | — | Low |
| FiscalYear, FiscalPeriod | Domain/Entities/OrgDb | Accounting | Domain | — | Low |
| Preference | Domain/Entities/OrgDb | Cross-cutting config (see target-architecture doc — likely stays generic/shared, keyed rows include Sales/Purchasing/Accounting-integration flags) | Domain | User | Medium |
| Table | Domain/Entities/OrgDb | Organization or a future POS/Sales-floor concept — low-value entity, confirm actual usage before assigning | Domain | — | Low |
| LogSys, Notification | Domain/Entities/OrgDb | Administration/cross-cutting (currently unmapped `DbSet`s, inactive) | Domain | — | Low (inactive) |

### Application feature folders (by folder, not by individual handler — see §5/§12 for detail)

| Current Folder | Target Module | Migration Risk | Notes |
|---|---|---|---|
| Commands/Org/Auth | Administration | Low | Self-contained against User/Role/Permission |
| Commands/Org/Financials/Financial, FinancialTransfer | Treasury | High | Integration/ subfolder is the cross-module posting bridge — becomes an event/contract boundary, not a lift-and-shift |
| Commands/Org/Financials/Journal | Accounting | High | Shares `Common/Services.AccountingPeriodService` |
| Commands/Org/Financials/Payable, Receivable | Payables / Receivables | Medium | Currently thin (one command each); expect to grow, not just move |
| Commands/Org/Invoices/Invoice | Sales + Purchasing (shared) | High | Directly injects Financial/Journal/Transaction repos |
| Commands/Org/Reports/* | Reporting | Medium | Must be re-pointed at Contracts/read models of every module it queries |
| Commands/Org/Setting/{Account,AccountType} | Accounting | Low | |
| Commands/Org/Setting/{Bank,BankBranch,CashBox} | Treasury | Low | |
| Commands/Org/Setting/{Dealer,DealerGroup} | Sales/Purchasing (shared) | High | Contains GL account-provisioning logic that belongs in Treasury/Accounting — must be redesigned as a contract call, not moved as-is |
| Commands/Org/Setting/{City,Country,District,Currency,Classification,Unit,ReferenceType,PaymentType} | MasterData | Low | |
| Commands/Org/Setting/{Branch,Shift} | Organization | Low | |
| Commands/Org/Setting/{Role,User} | Administration | Low | |
| Commands/Org/Setting/{FiscalYear,InvoiceType,JournalType} | Accounting / Sales-Purchasing shared (InvoiceType) | Low | |
| Commands/Org/Setting/{Product,ProductUnit,Property,Stock} | Inventory | Medium | Product's balance query reads TransactionProduct directly |
| Commands/Org/Setting/{Outlay,Preference,Table} | Treasury (Outlay) / cross-cutting (Preference) / Organization (Table) | Low | |
| Commands/Org/Transactions/{Inventory,Transaction,TransactionType} | Inventory | High | Integration/ subfolder posts into Financials/Journal |
| Common/Services | Split: AccountingPeriodService → Accounting; Receivable/PayableAccountValidator → Receivables/Payables (consuming Accounting/Treasury contracts) | High | Currently shared/duplicated logic; needs consolidation, not a straight move |
| Common/Commands, Common/Queries | BuildingBlocks (generic CQRS base classes) | Low | Truly generic, no business meaning |
| Interfaces/CQRS, Command, Query | BuildingBlocks | Low | Generic MediatR wrappers |
| Mappings/MappingProfile.cs + per-feature partials | Split into one AutoMapper Profile per module | Medium | Mechanical but must be done per-module, not centrally |
| Validators/* | Split into one FluentValidation validator set per module | Low | Rename misnamed files while moving (see §5) |
| DTOs/OrgDb/* | Split per module (or module Contracts, for DTOs another module needs) | Medium | |
| DTOs/AdminDb/* | Administration (dormant) | Low | Orphaned, zero current references — safe to move whole, low priority |
| DTOs/Report/* | Reporting | Low | |

### Infrastructure / API

| Current Class | Current Location | Target | Migration Risk |
|---|---|---|---|
| OrgContext | Infrastructure/Persistence/Data | Stays a single shared DbContext during Phase 1–9 (per brief §17); EF configuration split by module folder within it later | High — touches everything |
| AdminContext, IAdminContext | Infrastructure/Persistence/Data, Domain/Abstraction | Leave dormant/untouched until the team decides its fate (§21) | Low (do not move) |
| Repository<T>, UnitOfWork | Infrastructure/Persistence/UnitOfWork | Stays shared generic infrastructure (BuildingBlocks) until module-specific persistence is introduced | High — touches everything |
| InitialData.cs | Infrastructure/Seed | Split seed methods by target module ownership once modules exist; until then, leave as one file | Medium |
| Controllers/Org/Setting/{Account,AccountType}Controller | Move folder only (route unchanged) to Accounting-owned controller area | Low | |
| Controllers/Org/Setting/{Bank,BankBranch,CashBox}Controller | Move folder only to Treasury-owned controller area | Low | |
| Controllers/Org/Setting/{Dealer,DealerGroup}Controller | Move folder only to Sales/Purchasing-owned controller area | Low | |
| API/Program.cs | Host/OrgSys.Api composition root | Refactor into per-module `AddXModule()` calls | Medium — mechanical but must preserve registration order/behavior |

---

## 14. Cross-Module Dependencies (as they exist today)

Directional, derived from actual constructor injections and FK relationships found during this
analysis:

```text
Sales/Purchasing (Invoice, Dealer)
   │  directly injects
   ▼
Treasury (Financial), Accounting (Journal), Inventory (Transaction), MasterData (Preference)

Inventory (Transaction)
   │  directly injects
   ▼
Sales/Purchasing (Invoice), Accounting (Journal), MasterData (Preference)

Sales/Purchasing (Setting/Dealer)
   │  directly provisions/reads
   ▼
Accounting (Account), Accounting (JournalItem)

Treasury (Financial)
   │  FK
   ▼
Accounting (Journal)

Inventory (Stock)
   │  FK
   ▼
Accounting (Account)

Treasury (CashBox, BankAccount, FinancialAccount)
   │  FK
   ▼
Accounting (Account)

Reporting (all report handlers)
   │  reads
   ▼
Sales/Purchasing, Treasury, Inventory (everything — inherent to reporting)
```

Accounting is the most-depended-upon module (Account is referenced by Dealer, Stock, CashBox,
BankAccount, FinancialAccount) — consistent with the brief's Phase 2 = Accounting-first migration
order: get Accounting's Contracts right early since almost everything else will consume them.

---

## 15. Circular Dependency Risks

1. **Sales/Purchasing ↔ Treasury ↔ Accounting triangle.** `Invoice` handlers call into Financial/
   Journal posting; `Financial.JournalId` FKs back into `Journal`; `FinancialInvoice` bridges
   `Financial` and `Invoice` directly (both FKs on one join entity). Today this is fine (one
   assembly, one transaction). Once separated, this **must** become one-directional via events
   (`SalesInvoicePosted` → Treasury/Accounting react) rather than each module calling the others'
   repositories — otherwise Sales→Treasury and Treasury→Sales (via `FinancialInvoice.InvoiceId`)
   become a literal assembly-reference cycle.
2. **Inventory ↔ Sales/Purchasing.** `Invoice.TransactionId` and `Transaction` creation *from* an
   invoice (`CreateTransactionByInvoiceCommandHandler`) run in both directions depending on
   workflow (an invoice can spawn a stock transaction; a transfer's `Received` transaction can
   feed back into invoicing-adjacent flows). Needs the same event-based resolution.
3. **Setting/Dealer → Accounting → (back to) Sales/Purchasing balance display.** Dealer's balance
   query reads `JournalItem` directly; if Accounting later needs dealer-aware validation
   (`ReceivableAccountValidator` already does `ValidateCustomerAsync`), that's Accounting reaching
   back into Sales/Purchasing's `Dealer` — another candidate cycle unless resolved via a
   `Sales.Contracts.IDealerLookup`-style interface consumed by Accounting/Receivables instead of a
   direct entity reference.

None of these are blockers — they are exactly the kind of coupling the brief's Contracts +
Integration Events model (§13–§14 of the brief) is designed to resolve — but they are the reason
Phase 1 must finish the dependency-rule/Contracts design **before** Phase 2 (Accounting) starts
moving code, per the brief's own sequencing.

---

## 16. Database Ownership Map (code ownership only — no physical schema changes now)

Per brief §16: keep one physical database, one connection string, do **not** rename tables. This
is a logical ownership map for future `module.*` schema prefixes, not an action plan.

| Target schema (future) | Tables |
|---|---|
| `administration.*` | User, Role, Permission, RolePermission (+ dormant Client, ClientPlan, Plan, PlanElement, PlanType, Request, LoginUser, Nationality, TypeActivity) |
| `organization.*` | Branch, CompanyProfile, Shift, Table |
| `masterdata.*` | Country, City, District, Unit, Classification, Currency, ReferenceType, PaymentType (+ dormant General* AdminDb equivalents) |
| `sales.*` / `purchasing.*` | Dealer, DealerGroup (shared), Invoice, InvoiceProduct, InvoiceType (shared via type), Order/OrderProduct/OrderType (future, Sales) |
| `inventory.*` | Product, ProductUnit, Stock, Transaction, TransactionProduct, TransactionType, Inventory, InventoryProduct |
| `treasury.*` | CashBox, Bank, BankBranch, BankAccount, FinancialAccount, Financial, FinancialTransfer, FinancialInvoice, FinancialType, Outlay |
| `accounting.*` | Account, AccountType, Journal, JournalItem, JournalType, FiscalYear, FiscalPeriod |
| `receivables.*` / `payables.*` | No dedicated tables today — logically a view/projection over Dealer + Journal/Financial data (see §18) |

---

## 17. API Compatibility Risks

1. **Route stability is achievable with zero frontend changes** as long as each controller keeps
   its `[Route("[controller]")]` attribute regardless of which physical project/folder it lives
   in — folder reorganization (§9 finding #2) is safe; do it early since it's low risk and
   immediately improves code-ownership clarity.
2. **DTO/contract stability**: `BaseController<TGetById,TSearch,TList,TResponse,...>`'s generic
   CRUD actions serialize the same `TResponse` DTOs regardless of module boundaries — as long as
   DTO types keep their names/shapes when relocated to per-module `Contracts` projects (using
   `global using` aliases or namespace-only moves), Angular's generated/typed clients won't break.
   Watch AutoMapper carefully here: splitting `MappingProfile` into per-module profiles is
   mechanical but must preserve every one of the ~372 `CreateMap` calls' source/destination pairs.
3. **Pre-existing gaps that will otherwise get blamed on the refactor** — fix-or-flag *before*
   moving code so they aren't misattributed later: missing `[Authorize]` on
   `FinancialTransferController`/`PayableController` (§9), no server-side permission enforcement
   (§10), duplicate DI registrations in `Program.cs` (§9), `GlobalExceptionMiddleware` pipeline
   ordering (§9).
4. **Swagger/OpenAPI generation** depends on all controllers being discoverable via
   `AddControllers()` assembly scanning — once controllers live in separate module assemblies
   (if/when that split happens; the brief allows folders-first), `AddControllers()` needs explicit
   `AddApplicationPart()` calls per module assembly, or Swagger silently drops module endpoints.
   Not a Phase 1 concern (folders-first, one API assembly) but flag for later phases.

---

## 18. Missing ERP Capabilities Report

| Capability | Status | Evidence |
|---|---|---|
| Accounts Receivable (balances/settlement) | **Partial** | `Financials/Receivable` has one `SetCustomerOpeningBalanceCommand`; balance is computed ad hoc in `Setting/Dealer/GetBalanceQueryHandler` by summing `JournalItem`; no dedicated aging/settlement workflow |
| Accounts Payable | **Partial** | Same shape as Receivable — one `SetSupplierOpeningBalanceCommand`, `PayableAccountValidator` largely duplicates `ReceivableAccountValidator` |
| Customer/Supplier Aging | **Missing** | No aging bucket logic found anywhere |
| Bank Reconciliation | **Missing** | `BankAccount` entity exists; no reconciliation entity/workflow |
| Cheque Lifecycle | **Partial (data model only)** | `FinancialTransactionType.Cheque` and `FinancialReferenceType.Cheque` exist as enum values / seeded `FinancialType`/`ReferenceType` rows; no dedicated Cheque entity, no collection/deposit/endorsement/return/cancellation workflow, no status history |
| Advances / Custody | **Missing** | No entity or workflow found (`FinancialReferenceType.Employee` exists as a reference-type value only) |
| Period Closing | **Partial** | `FiscalYear.FiscalYearStatus`, `FiscalYear`/`FiscalPeriod` entities exist; Journal posting validates against them (`AccountingPeriodService`); no explicit "close period" workflow/command found |
| General Ledger | **Existing** | `Journal`/`JournalItem` + posting, reversal (with DB-enforced uniqueness), fiscal year/period validation — this is the most mature accounting feature in the codebase |
| Trial Balance | **Missing** | No trial-balance query/report found (Reports/ has Dealer/Financial/Sales/Warehouse reports only, no GL-level trial balance) |
| Financial Statements | **Missing** | No P&L/Balance Sheet generation found |
| Financial Instruments (Notes/Bills of Exchange) | **Missing** | Not represented anywhere |
| Purchase Requisitions | **Missing** | No entity/workflow |
| Quotations / Sales Orders | **Missing (scaffolded only)** | `Order`/`OrderProduct`/`OrderType` entities exist but `DbSet`s are commented out in `OrgContext` and no CQRS/API surface exists |

Per brief §24, **do not implement any of these now** — this table exists to inform the migration
order (Accounting-first is right: it's both the most mature and the most depended-upon) and to
set expectations that Receivables/Payables (Phase 7–8) will involve real new development, not
just code relocation.

---

## 19. Migration Order

Adopting the brief's Phase 1–10 order as-is; this analysis found nothing that argues for
reordering it. Rationale from this analysis for each phase:

| Phase | Module | Why this position is right, per findings |
|---|---|---|
| 1 | Foundation (SharedKernel, Contracts, registration, dependency rules) | Contracts must exist before Accounting moves, given the Accounting-is-most-depended-upon finding (§14) and the circular-dependency risks (§15) |
| 2 | Accounting | Most mature, newest, most-depended-upon (§8, §14) — get its Contracts right first |
| 3 | Treasury | Second-most mature; `Financial`↔`Journal` coupling (§15) means it should follow immediately after Accounting has stable Contracts |
| 4 | Inventory | `Transaction`↔`Invoice`↔`Journal` coupling (§15) — needs Accounting/Treasury contracts in place first |
| 5 | Sales | `Invoice`/`Dealer` — highest fan-in from Financials/Transactions (§13); benefits from Accounting/Treasury/Inventory contracts existing first |
| 6 | Purchasing | Shares `Invoice`/`Dealer` with Sales — do together or immediately after, per brief §5's explicit "must remain separate even if they reuse document infrastructure" |
| 7 | Receivables | Currently thin (§18) — this phase is as much new development as extraction |
| 8 | Payables | Same as Receivables; resolve the `PayableAccountValidator`/`ReceivableAccountValidator` duplication (§5 finding 2) as part of this phase |
| 9 | Reporting | Already structurally separate (§12); becomes mechanical once modules 2–8 expose read Contracts |
| 10 | Administration / MasterData cleanup | Lowest risk, most self-contained (§13); also the phase to decide AdminDb's fate (§21) |

---

## 20. Files/Classes That Should Move in Phase 1

Per the brief, Phase 1 is **foundation only** — no business entities, handlers, or controllers
move yet. What Phase 1 should actually touch:

- **New, empty scaffolding**: `BuildingBlocks/{OrgSys.SharedKernel, OrgSys.Contracts,
  OrgSys.Infrastructure, OrgSys.EventBus}` projects (or folders-first, per brief §2, if the team
  prefers to defer new `.csproj`s).
- **Relocate truly generic, business-meaning-free code into SharedKernel** (safe, mechanical,
  zero behavior change): `Domain/Shared/Result.cs` (Result/Error/Pagination types),
  `Application/Common/Commands` + `Application/Common/Queries` generic base handlers,
  `Application/Interfaces/{CQRS,Command,Query}` (fixing the folder/namespace mismatch found in
  §5 while moving), `BaseModel`/`MovementModel` shape (careful: keep the FK properties out of
  SharedKernel — only the pure shape belongs there; see target-architecture doc).
- **Housekeeping** identified in §1: remove the dead `Domain/Shared/{LockupType*,
  TransactionType*}Entity.cs` hierarchy (confirm zero usages, already verified) and the empty
  `Aggregate/Interfaces/Events/Exceptions/ValueObjects` folder declarations; clean the stale
  `.csproj` `<Compile Remove>` entries pointing at deleted `Contracts/Features/Profiles/
  Repositories/UnitOfwork` paths.
- **Non-invasive `Program.cs` cleanup**: collapse the duplicate `AddSwaggerGen`/`AddControllers`/
  `AddMediatR` calls (§9 finding #5) as a mechanical, zero-behavior-change simplification — good
  practice before layering module registration extension methods on top.
- **Write architecture tests** (per brief §29) asserting the *target* forbidden-dependency rules,
  even though nothing has moved yet — they'll simply pass trivially today and start being useful
  the moment Phase 2 begins.

## 21. Files/Classes That Must NOT Move Yet

- **`AdminContext`, `IAdminContext`, `Domain/Entities/AdminDb/*`, `Application/DTOs/AdminDb/*`** —
  dormant, unwired, zero live references. Do not fold into Administration's live scaffolding until
  the team decides whether to revive, delete, or permanently park the SaaS/multi-tenant concept
  (see open question below).
- **`Financial`, `Invoice`, `Transaction`, `Journal` and their `*Integration` handler classes**
  (`InvoiceJournalIntegration`, `TransactionJournalIntegration`, `TransferReceivedIntegration`,
  `InventoryAdjustmentIntegration`) — these are exactly the future integration-event boundaries
  (§14 of the brief). Moving them before the event contracts are designed would just relocate the
  tight coupling instead of resolving it.
- **`Dealer`, `DealerGroup`** — shared-ownership question (Sales vs Purchasing) must be resolved
  in the target-architecture doc before this entity moves anywhere.
- **`Common/Services` (`AccountingPeriodService`, `ReceivableAccountValidator`,
  `PayableAccountValidator`)** — contains the identified duplication (§5); resolve the design
  (which module owns validation, how Receivables/Payables call into it) before relocating, not
  after.
- **Generic `Repository<T>`/`UnitOfWork`/`BaseController<...>`** — depended on by literally every
  handler and controller; per brief §17, stays as shared infrastructure until module-specific
  persistence is deliberately introduced, which is explicitly out of scope for the early phases.
- **`OrgSys` legacy MVC project** — do not modify; confirm its deployment status first (see below).
- **`OrgContext` itself** — stays one shared `DbContext` per brief §17; only its
  `OnModelCreating` configuration gets logically reorganized (later phases), not the context.

## 22. Open Questions for the Team

1. **Legacy `OrgSys` MVC project**: still deployed anywhere, or fully superseded by `API` +
   `OrgSys.Angular`? Affects whether it should stay in `OrgSys.sln` at all.
2. **`AdminDb`/`AdminContext`**: revive as the real multi-tenant provisioning layer for
   Administration, delete as abandoned scaffolding, or leave dormant indefinitely? Affects whether
   Administration's target module includes tenant/plan concepts or only RBAC.
3. **Security gaps found during this analysis** (missing `[Authorize]` on two controllers, no
   server-side permission-code enforcement) — fix now as a fast-follow, or defer and track
   separately from the modularization work? These are pre-existing issues, not introduced by this
   analysis, and are called out here only because a module boundary change is a natural time to
   also close them if the team wants that bundled.
