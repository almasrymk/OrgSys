# OrgSys Phase 0 — Current-State Audit

**Generated from live source**, not historical documentation.

| Item | Value |
|------|--------|
| Repository | `D:\Work\MK\Source\OrgSys` (remote `https://github.com/almasrymk/OrgSys`) |
| Target branch | `Latest` |
| Audit date | 2026-09-17 |
| Source of truth | Current `.cs` / `.csproj` / EF snapshot / Architecture.Tests / Angular `src/app` |
| Code changes in this stage | **None** (documentation only) |

If this document conflicts with older reports under `docs/`, **this audit wins** until a later stage updates it.

Historical documents that **describe modules or abstractions that do not exist in code** (Workflow, Budgeting, `ICurrentTenant`, IAM extraction) must be treated as **target sketches**, not as-is architecture. See §14.

---

## 1. What the repository is today

OrgSys is a **modular monolith ERP** on **.NET 10** with:

- One host API (`API/`) composing 16 modules via `Add*Module()`.
- One legacy MVC host (`OrgSys/`) still in use; Angular (`OrgSys.Angular/`) is the replacement UI, incomplete.
- One shared EF Core `OrgContext` and one SQL Server database.
- CQRS via MediatR + FluentValidation + AutoMapper.
- Cross-module communication that is **partially** Contracts/events (Accounting, Receivables, Payables are the best examples) and **partially** still Domain navigations / Application→Domain reads.

This is **not** a greenfield redesign. The migration from a layered monolith is already far along. Remaining work is isolation, missing ERP contexts, SaaS enforcement, Angular coverage, and production hardening.

---

## 2. Solution layout

**Solution file:** `OrgSys.sln`

### Hosts

| Project | Path | Role |
|---------|------|------|
| API | `API/API.csproj` | JWT API composition root. |
| OrgSys | `OrgSys/OrgSys.csproj` | Cookie-auth MVC; proxies to API. Still operational. |
| OrgSys.Angular | `OrgSys.Angular/OrgSys.Angular.esproj` | Angular 19.2 SPA. |

### BuildingBlocks

| Project | In `.sln`? | Path |
|---------|------------|------|
| OrgSys.SharedKernel | Yes | `BuildingBlocks/OrgSys.SharedKernel/` |
| OrgSys.EventBus | Yes | `BuildingBlocks/OrgSys.EventBus/` |
| OrgSys.Infrastructure | Yes | `BuildingBlocks/OrgSys.Infrastructure/` |
| OrgSys.Localization | Yes | `BuildingBlocks/OrgSys.Localization/` |
| OrgSys.DatabaseMigrator | **No** (referenced by API + MVC) | `BuildingBlocks/OrgSys.DatabaseMigrator/` |

### Modules (16)

Each transactional module has Domain / Application / Contracts / Infrastructure **except Reporting** (no Domain, by design).

```text
Accounting, Administration, Advances, Catalog, CommercialDocuments,
Inventory, MasterData, Organization, Parties, Payables, Purchasing,
Receivables, Reporting, SaaS, Sales, Treasury
```

**Folders that do not exist under `Modules/`:** `Identity`, `Workflow`, `Tax`, `Budgeting`, `FixedAssets`.

### Tests (`Tests/`)

| Project | Focus |
|---------|--------|
| Architecture.Tests | Layer/module isolation (NetArchTest) |
| Application.Tests | Cross-module handler tests |
| Accounting.Domain.Tests / Accounting.Integration.Tests | GL |
| Inventory.Domain.Tests / Inventory.Integration.Tests | Stock |
| Advances, Catalog, Organization, Parties, Payables, Purchasing, Receivables, Sales, Treasury Domain.Tests | Aggregate invariants |

No dedicated Domain/Application/Integration test projects for Administration, CommercialDocuments, MasterData, Reporting, SaaS.

### Root artifacts that do not belong in source control

| Artifact | Status |
|----------|--------|
| `Domain.zip` | Present at repo root; **not** in `.gitignore` |
| `*.zip` | Not ignored |
| `bin/` / `obj/` | Ignored in `.gitignore`; still appear in working tree on Windows |
| `appsettings.Local.json` | Ignored (correct) |

No `.github/workflows` directory. **No CI quality gates.**

---

## 3. Building blocks (actual types)

### OrgSys.SharedKernel

Cross-cutting primitives. **Not a dumping ground of business entities**, but it **does** contain MediatR/AutoMapper/FluentValidation/EF Core package references (CQRS bases + `IOrgContext`). Domain projects reference SharedKernel, so Domain assemblies are not “pure” in the strict hexagonal sense; Architecture.Tests still forbid Domain → MediatR/EF **namespaces**.

Public types:

- Persistence: `IOrgContext`, `IRepository<TEntity>`, `IUnitOfWork`
- Entity bases: `BaseModel`, `MovementModel`, `AggregateRoot`, `Status`, `IDomainEvent`
- Result: `Result`, `Result<T>`, `ResultCollection<T>`, `ResultPagination<T>`, `Error`, `AppValidationException`
- CQRS generics: `CreateCommandHandler`, `UpdateCommandHandler`, `DeleteCommandHandler`, `GetCommandHandler`, `ListCommandHandler`, `SearchCommandHandler`, `GetMaxCommandHandler` + matching interfaces
- Pipeline: `FluentValidationFilter<TRequest,TResponse>`, `Validator<TCommand,TEntity>`
- UI leftover: `TreeView`

`AggregateRoot` is **optional** and unused by most live entities because they already inherit `MovementModel`/`BaseModel` (C# single inheritance). Rich aggregates (`Journal`, `Custody`, `PurchaseOrder`, `SalesOrder`) duplicate a private `_domainEvents` list.

### OrgSys.EventBus

In-process publisher only:

- `IIntegrationEvent` / `IntegrationEvent` (namespace `OrgSys.SharedKernel`, assembly EventBus)
- `IIntegrationEventPublisher`
- `MediatrIntegrationEventPublisher` — MediatR `INotification`

**No Outbox, Inbox, retry, or transport abstraction beyond this facade.**

### OrgSys.Infrastructure

- `Repository<TEntity>` over `IOrgContext.Set<T>()`
- `UnitOfWork`

Generic repository is the default persistence API. Intention-revealing repositories exist only for a few aggregates (`IJournalRepository`, `IAccountRepository`, `IFiscalPeriodRepository`, `IReceivableRepository`, `IPaymentApplicationRepository`, `IPayableRepository`, `ISupplierPaymentApplicationRepository`, `IInventoryBalanceRepository`).

### OrgSys.DatabaseMigrator

Owns:

- `Persistence/OrgContext.cs` — the **only** DbContext
- All EF migrations
- `Seeding/DataSeederCoordinator` constructing module seeders **directly** (no DI)

### OrgSys.Localization

`.resx` + `Translate.GetTranslate`. Used by Administration role-tree strings. Not a bounded context.

---

## 4. Persistence

### OrgContext

Path: `BuildingBlocks/OrgSys.DatabaseMigrator/Persistence/OrgContext.cs`

- Implements `IOrgContext` (`BuildingBlocks/OrgSys.SharedKernel/IOrgContext.cs`).
- SQL Server, connection `OrgConnection`.
- **Zero** `IEntityTypeConfiguration<T>` under module Infrastructure.
- Table names come from Domain `[Table("...")]` plus Fluent FK/index config in `OnModelCreating`.
- Fluent “no navigation” FKs already used for several isolation cleanups (Journal.CurrencyId, CashBox.KeeperUserId, Dealer.AccountId, MovementModel CreateUser/Branch, Company.TenantId).

### Active tables (from `OrgContextModelSnapshot`)

**87 mapped tables.** Advances (`Custody`, `CustodyHandover`) and Sales (`SalesOrder`, `SalesQuotation` and lines) have `[Table]` attributes but are **not** in the snapshot / `OrgContext` DbSets.

| Module | Tables in snapshot |
|--------|--------------------|
| Accounting | Account, AccountType, FiscalYear, FiscalPeriod, Journal, JournalItem, JournalType |
| Administration | User, Role, Permission, RolePermission, Preference |
| Catalog | Brand, Classification, PriceList, PriceListEntry, Product, ProductPropertyElement, ProductUnit, Property, PropertyElement, Unit |
| CommercialDocuments | Invoice, InvoiceProduct, InvoiceType |
| Inventory | Inventory, InventoryBalance, InventoryBatch, InventoryCostLayer, InventoryIssue, InventoryIssueLine, InventoryProduct, InventoryReceipt, InventoryReceiptLine, InventorySerial, ProductRecipe, Stock, StockAdjustment, StockAdjustmentLine, StockAdjustmentReason, StockReservation, StockTransfer, StockTransferLine, Transaction, TransactionProduct, TransactionType, WarehouseLocation |
| MasterData | City, Country, Currency, District, PaymentType, ReferenceType |
| Organization | Branch, Company, OrganizationSettings, Shift, Table |
| Parties | CustomerProfile, Dealer, DealerGroup, PartyAddress, PartyContact, SupplierProfile |
| Payables | Payable, SupplierPaymentApplication, SupplierPaymentApplicationLine |
| Purchasing | PurchaseOrder, PurchaseOrderProduct, PurchaseRequisition, PurchaseRequisitionProduct |
| Receivables | PaymentApplication, PaymentApplicationLine, Receivable |
| SaaS | Feature, Plan, PlanFeature, Subscription, Tenant |
| Treasury | Bank, BankAccount, BankBranch, CashBox, Financial, FinancialAccount, FinancialInvoice, FinancialTransfer, FinancialType, Outlay |
| Sales | **none persisted** (legacy `Order`/`OrderProduct` dropped by `RemoveDeadSalesOrderModel`) |
| Advances | **none persisted** |
| Reporting | **none owned** |

Entities with `[Table]` but **DbSet commented out**: `LogSys`, `Notification`, `CompanyProfile`.

### Latest migrations (28 total)

Newest:

1. `20260916054152_AddSaaSModuleAndTenantRetrofitStage1` — Tenant/Plan/Feature/Subscription + nullable `Company.TenantId`
2. `20260916174551_AddUserPasswordHashing` — `User.MustResetPassword`

Idempotency indexes already exist for:

- `Receivable (SourceDocumentType, SourceDocumentId, CustomerId)` unique
- `PaymentApplication.SourceFinancialId` unique
- `Payable (SourceDocumentType, SourceDocumentId, SupplierId)` unique
- `SupplierPaymentApplication.SourceFinancialId` unique
- `Journal.OriginalJournalId` unique (filtered)

### Tenant columns

**Only** `Company.TenantId` and `Subscription.TenantId`. No global query filters. No `TenantId` on Invoice, Journal, Financial, Dealer, Product, User, etc.

---

## 5. API composition

File: `API/Program.cs`

Registered in this order:

```text
AddSaaSModule
AddMasterDataModule
AddOrganizationModule
AddAdministrationModule
AddAccountingModule
AddTreasuryModule
AddCommercialDocumentsModule
AddSalesModule
AddPartiesModule
AddInventoryModule
AddCatalogModule
AddPurchasingModule
AddReceivablesModule
AddPayablesModule
AddAdvancesModule
AddReportingModule
```

Also: JWT bearer, Swagger (dev), CORS `AngularClient` (`localhost:4200` only), `IOrgContext`/`IUnitOfWork`/`IRepository<>`/`IIntegrationEventPublisher`, `FluentValidationFilter`, `GlobalExceptionMiddleware`.

**Missing in Program.cs:** health checks, OpenTelemetry, correlation id, API versioning, ProblemDetails, authorization policies, tenant middleware.

Auth: `API/Authentication/JwtTokenService.cs`. Login: `POST /api/Auth/login`. Most CRUD controllers inherit `[Authorize]` via `BaseController`. **No `[Authorize]`** on:

- `API/Controllers/Org/Financials/PayableController.cs`
- `API/Controllers/Org/Financials/ReceivableController.cs`
- `API/Controllers/Org/Financials/FinancialTransferController.cs`

Errors: custom `Result` JSON from `GlobalExceptionMiddleware`. Unhandled exceptions return `ex.Message` to the client (information disclosure). Not RFC7807 ProblemDetails.

MVC (`OrgSys/Startup.cs`) uses cookie auth and still hosts Setting/Invoices/Transactions/Financials areas. Reports area controllers exist on disk but are **excluded from compile**.

---

## 6. Module inventory

Legend for completeness: **D**omain behavior, **A**pplication use cases, **C**ontracts, **P**ersistence, **API**, **T**ests, **UI** (Angular).

### 6.1 Accounting — reference DDD module

| Field | Actual |
|-------|--------|
| Responsibility | Chart of accounts, fiscal calendar, journals, posting API for other modules, AR/AP account validation |
| Aggregates | `Journal` (rich: `CreateDraft`, Post/Reverse/Cancel); `FiscalPeriod` (close/reopen events) |
| Entities | Account, AccountType, JournalType, FiscalYear, JournalItem |
| Tables | Account, AccountType, FiscalYear, FiscalPeriod, Journal, JournalItem, JournalType |
| Repositories | `IJournalRepository`, `IAccountRepository`, `IFiscalPeriodRepository` |
| Domain services (Application) | `IAccountingPeriodService`, `IReceivableAccountValidator`, `IPayableAccountValidator` |
| Contracts | Posting commands/queries, account lookups, `ProvisionSubAccountCommand`, journal integration events |
| Integration events out | `JournalPostedIntegrationEvent`, `JournalReversedIntegrationEvent`, `JournalCancelledIntegrationEvent` (no in-module consumers) |
| Illegal deps | None. `Accounting.Application` → Parties.Contracts + MasterData.Contracts only |
| API | `/Account`, `/AccountType`, `/FiscalYear`, `/Journal`, `/JournalType` |
| Tests | Domain (Account, Journal, FiscalPeriod), Integration (`InvoiceJournalPostingTests`), Application.Tests journal handlers |
| Angular | Chart of Accounts, Journal Entries, Fiscal Years |
| Completeness | D A C P API T UI — **strongest module** |

Journal.CurrencyId is scalar-only; FK kept in OrgContext without Domain navigation. This is the pattern Stage 1 should copy.

### 6.2 Administration — IAM lives here

| Field | Actual |
|-------|--------|
| Responsibility | Users, roles, permissions, login, preferences, password hashing |
| Aggregates | None rich. `User`, `Role`, `Permission`, `RolePermission`, `Preference` are `BaseModel` CRUD |
| Notable | `User.RoleId` only — **no UserRole join**. `User.BranchId` scalar (FK to Branch in OrgContext). `User.Password` hashed via `PasswordHasher` (ASP.NET Identity hasher). `MustResetPassword` for AES→hash cutover |
| Contracts | `GetPreferenceValueQuery`, `GetPreferenceValuesQuery` only. **No** `HasPermissionQuery`, `GetCurrentUserAccessQuery` |
| JWT | Host-level, not module Contracts |
| API | `/api/Auth`, `/User`, `/Role`, `/Preference` |
| Angular | Login only. **No User/Role/Preference screens** |
| Tests | `PasswordHasherTests` only |
| Completeness | Partial. IAM exists; not extracted; not a permission-evaluation bounded context |

**Do not create `Modules/Identity` until Administration ownership, `User` table, and login flow are extracted with a data-safe plan.** Current ownership is clear: Administration owns IAM tables.

### 6.3 Advances — domain only

| Field | Actual |
|-------|--------|
| Responsibility | Employee custody / advances (accountability, not cash accounts) |
| Aggregates | `Custody` (rich lifecycle: Create, Approve, Issue, TransferHolder, Settle, Return, Close, Cancel), `CustodyHandover` child |
| Tables | `[Table("Custody")]`, `[Table("CustodyHandover")]` — **not mapped in OrgContext** |
| Application | AssemblyMarker + GlobalUsings only |
| Contracts | `AssemblyMarker` only |
| API / Angular | None |
| Tests | `CustodyTests` |
| Completeness | Domain richness without persistence, application, Treasury integration, or UI |

### 6.4 Catalog

| Field | Actual |
|-------|--------|
| Responsibility | Product, classification, unit, brand, property, price list, price resolution |
| Entities | Product, ProductUnit, Classification, Unit, Brand, Property, PropertyElement, ProductPropertyElement, PriceList, PriceListEntry |
| Contracts | `GetProductNamesQuery`, `ResolvePriceQuery`, `ResolvedPriceDto` |
| API | `/Product`, `/Unit`, `/Classification`, `/Brand`, `/PriceList`, `/Pricing`, `/Property`, `/ProductUnitModelView` |
| Angular | Products + lookup services for Unit/Classification. Missing Brand/PriceList/Property/Pricing UI |
| Tests | `ProductInvariantTests`, `ResolvePriceQueryHandlerTests` |
| Completeness | Core product CRUD live; pricing/brands incomplete in UI |

### 6.5 CommercialDocuments — invoice owner

| Field | Actual |
|-------|--------|
| Responsibility | Sales/purchase invoices and returns (`InvoiceTypeId` 1–4). Does **not** own SalesOrder |
| Aggregates | `Invoice` (`MovementModel`, still anemic public setters + `Tax`/`TaxType` decimal fields), `InvoiceProduct`, `InvoiceType` |
| Tax | Header `Tax`, `TaxType` integers. **No TaxCode/TaxRate engine** |
| Contracts | `InvoiceReferenceDto`, `GetInvoiceReferenceQuery`, `GetInvoiceNetsQuery`, `InvoiceTypeId`, `SalesInvoicePostedIntegrationEvent`, `PurchaseInvoicePostedIntegrationEvent` |
| Events out | Invoice posted events from `CreateCommandHandler` |
| Illegal deps | Domain → MasterData (Currency, PaymentType), Domain → Catalog (Unit); Application → Catalog.Application (UnitDto); Application also has stale MasterData.Application csproj ref |
| API | `/Invoice`, `/InvoiceType` |
| Angular | Sales/Purchase invoice and return screens |
| Completeness | Operational invoice CRUD + GL posting + AR/AP events. Anemic Invoice. Cross-context navigations remain |

### 6.6 Inventory

| Field | Actual |
|-------|--------|
| Responsibility | Warehouses (`Stock`), legacy `Transaction`/`Inventory` cycles, plus hardened receipts/issues/transfers/adjustments/reservations/batches/serials/cost layers |
| Dual model | **Legacy** `Transaction`/`TransactionProduct` still used by Angular movements **and** **new** `InventoryReceipt`/`InventoryIssue`/`StockTransfer`/`StockAdjustment`/`StockReservation` |
| Illegal deps | Domain → Catalog (Product/Unit navigations on many line entities); Domain → MasterData (csproj + `global using` **with no remaining type usage** — stale); Application → CommercialDocuments.Domain (`IRepository<Invoice>`); Application → Catalog.Application/Domain; Application → MasterData.Application (stale csproj) |
| Contracts | `ReserveInventoryCommand`, `GetInventoryAvailabilityQuery`, invoice-linked transaction commands, `GetStockNamesQuery` |
| API | `/Transaction`, `/Inventory`, `/Stock`, plus REST controllers under `API/Controllers/Org/Inventory/` |
| Angular | Legacy movements + count + locations + receipts + balances + reservations. Missing issues/transfers/adjustments/batches/serials screens |
| Tests | Strongest domain suite (11 classes) + reservation concurrency integration tests |
| Completeness | Hardening exists; dual write-path risk; GRN is `InventoryReceipt`, not Purchasing |

### 6.7 MasterData

| Field | Actual |
|-------|--------|
| Responsibility | Geography + Currency + PaymentType + ReferenceType |
| Contracts | Currency lookups only (`GetDefaultCurrencyQuery`, `GetCurrencyNamesQuery`, `CurrencyLookupDto`). **No** Country/City/District/PaymentType contracts |
| That gap is why Domain navigations to Country/City/District/Currency/PaymentType still exist |
| Angular | Countries, Cities, Districts, Currencies. PaymentType as invoice lookup. No ReferenceType UI |

### 6.8 Organization

| Field | Actual |
|-------|--------|
| Responsibility | Company, Branch, Shift, restaurant `Table`, OrganizationSettings |
| `Company.TenantId` | Scalar, nullable, FK in OrgContext, validated via `IRepository<SaaS.Domain.Tenant>` (**Application→SaaS.Domain exception**) |
| CountryId / DefaultCurrencyId | Scalar; validated via `IRepository<Country/Currency>` (**Application→MasterData.Domain exception**) |
| No Department entity | Budgeting/Org docs that mention Department are **aspirational** |
| API | `/Company`, `/Branch`, `/OrganizationSettings`, `/Shift`, `/Table` |
| Angular | Branches only |
| Completeness | Master data live; tenant link is stage-1 only |

### 6.9 Parties

| Field | Actual |
|-------|--------|
| Responsibility | Unified `Dealer` (customer/supplier), groups, profiles, contacts, addresses; GL sub-account provisioning via Accounting.Contracts |
| Illegal deps | Domain → MasterData (Dealer/PartyAddress Country/City/District navigations); Application maps those names (allow-listed) |
| Contracts | `DealerType`, `DealerLookupDto`, `GetDealerByIdQuery`, `GetDealerNamesQuery` |
| API | `/Dealer`, `/DealerGroup`, `/CustomerProfile`, `/SupplierProfile`, `/PartyContact`, `/PartyAddress` |
| Angular | Customers/Suppliers + groups. No profile/contact/address screens |

### 6.10 Payables

| Field | Actual |
|-------|--------|
| Responsibility | Supplier open-item subledger + FIFO payment application |
| Aggregates | `Payable`, `SupplierPaymentApplication` (+ lines) |
| Inbound events | `PurchaseInvoicePostedIntegrationEventHandler`, `SupplierPaymentPostedIntegrationEventHandler` |
| Application refs | Accounting/MasterData/Administration/CommercialDocuments/Treasury **Contracts only** — **clean** |
| API | `/Payable` (OpeningBalance, Balance, Aging, Outstanding, Overdue, Subledger, Reconciliation) — **unauthenticated** |
| Angular | **None** |
| Completeness | Backend subledger exists; UI missing; auth hole |

### 6.11 Purchasing

| Field | Actual |
|-------|--------|
| Responsibility | Purchase Requisition (submit/reject/cancel/convert) and Purchase Order (link invoice, quantity tracking methods on aggregate) |
| Missing | RFQ, supplier quotation, three-way match engine, GRNI accounting |
| GRN | Inventory `InventoryReceipt` — not yet wired as PO receipt |
| Contracts project | **Empty** (csproj only) |
| Illegal deps | Domain → Catalog (`Unit` on PR/PO lines) |
| API | `/PurchaseRequisition`, `/PurchaseOrder` |
| Angular | **None** |
| Completeness | PR→PO→link invoice exists. Full P2P cycle does not |

### 6.12 Receivables

Mirror of Payables for customers. Inbound: `SalesInvoicePostedIntegrationEvent`, `CustomerPaymentPostedIntegrationEvent`. Contracts-only outbound deps. API `/Receivable` **unauthenticated**. No Angular.

### 6.13 Reporting

Read-only aggregator. **No Domain.** Application queries other modules’ **Domain entities** directly (`IRepository<Invoice>`, `Dealer`, `Financial`, `TransactionProduct`). Empty Contracts. Seven report queries. Angular reporting screens exist for warehouse/dealer/safe/sales balance.

**This is the largest Application→Domain exception cluster (6 of 16).**

Missing vs target: Trial Balance, Balance Sheet, P&L, Cash Flow, AR/AP aging UI, budget vs actual, asset register.

### 6.14 SaaS

| Field | Actual |
|-------|--------|
| Aggregates | Tenant (Activate/Suspend/Cancel), Subscription (Activate/ChangePlan/MarkPastDue/Cancel/Expire), Plan, Feature, PlanFeature |
| Seeded features | `GeneralLedger`, `Sales`, `Purchasing`, `Inventory`, `Budgeting`, `AdvancedReporting`, `MultiBranch`, `APIAccess` |
| Limits enum | Users, Companies, Branches, Warehouses, TransactionsPerMonth |
| Enforcement | `ITenantFeatureService` implemented in `TenantFeatureService` — **zero consumers outside SaaS** |
| `EnsureTenantActiveQuery` | Exists; **not sent by other modules** |
| `ICurrentTenant` | **Does not exist** |
| Angular | **None** |
| Tests | **None dedicated** |

`MarkPastDue` / `Expire` are domain methods with no Application handlers.

### 6.15 Sales — domain only, not persisted

| Field | Actual |
|-------|--------|
| Aggregates | `Quotation` (`[Table("SalesQuotation")]`), `SalesOrder` (`[Table("SalesOrder")]`) with rich lifecycle + domain events |
| Persistence | **Removed** from EF (`20260914082704_RemoveDeadSalesOrderModel` dropped legacy `Order`, never added new tables) |
| Application | Empty `MappingProfile`; stale `MasterData.Application` project reference |
| Contracts | Empty csproj |
| API / Angular | None (invoices live under CommercialDocuments) |
| Completeness | Domain model waiting for a persistence/application pass. Do not confuse with Invoice |

### 6.16 Treasury

| Field | Actual |
|-------|--------|
| Responsibility | Unified financial accounts (CashBox + BankAccount → `FinancialAccount`), `Financial` movements, `FinancialTransfer`, banks, outlays |
| Types | `FinancialTransactionType` enum **is** FinancialType Id space: OpeningBalance, Receipt, Payment, TransferIn/Out, Deposit, Withdrawal, Fee, Interest, Cheque, Adjustment |
| Cheque | Enum value + type row, **not** a cheque register aggregate |
| Events out | `CustomerPaymentPostedIntegrationEvent`, `SupplierPaymentPostedIntegrationEvent` from `PostTransactionCommandHandler` |
| Illegal deps | Domain → MasterData (Bank/BankBranch geo, Currency, PaymentType); Application → CommercialDocuments.Domain (`IRepository<Invoice>`) |
| Contracts | Two payment events + `DeleteFinancialsByInvoiceCommand` only. **No** cash/bank lookup contracts |
| Angular | Cash boxes, bank accounts, banks, branches, transfers, opening balances, movement types |
| Completeness | Unified cash/bank model is live. Cheque register, gateway, Advances disbursement not done |

---

## 7. Module matrix

| Module | Aggregate / main entities | Snapshot tables | API | Contracts | Events | Domain tests | Integration tests | Angular |
|--------|---------------------------|-----------------|-----|-----------|--------|--------------|-------------------|---------|
| Accounting | Journal, Account, FiscalYear/Period | 7 | Yes | Rich posting + lookups | Journal posted/reversed/cancelled | Yes | Yes | Partial |
| Administration | User, Role, Permission | 5 | Yes | Preferences only | — | — | — | Login only |
| Advances | Custody | **0** | No | Marker only | — | Yes | — | No |
| Catalog | Product, Unit, PriceList | 10 | Yes | Product names + price resolve | — | Yes | — | Products only |
| CommercialDocuments | Invoice | 3 | Yes | Invoice ref/nets + posted events | Sales/Purchase invoice posted | Arch ownership | Invoice journal (Accounting.Integration) | Invoices/returns |
| Inventory | Stock, Transaction, Receipt/Issue/Transfer/Adjustment/Reservation | 22 | Yes | Reserve + invoice-linked txn | — | Yes (11) | Yes (reservation) | Partial (legacy + some hardened) |
| MasterData | Country, Currency, PaymentType | 6 | Yes | Currency only | — | — | — | Geo + currency |
| Organization | Company, Branch | 5 | Yes | Company/branch names | — | Yes | — | Branches only |
| Parties | Dealer | 6 | Yes | Dealer lookups | — | Yes | — | Dealers + groups |
| Payables | Payable, SupplierPaymentApplication | 3 | Yes (no auth) | Balance/aging/open items | In: purchase invoice, supplier payment | Yes | Handler tests in Application.Tests | **No** |
| Purchasing | PurchaseRequisition, PurchaseOrder | 4 | Yes | **Empty** | — | Yes | LinkInvoice handler test | **No** |
| Receivables | Receivable, PaymentApplication | 3 | Yes (no auth) | Balance/aging/open items | In: sales invoice, customer payment | Yes | Handler tests | **No** |
| Reporting | (none) | 0 | Yes | **Empty** | — | — | — | 7 report screens |
| SaaS | Tenant, Plan, Subscription | 5 | Yes | Tenant lookups + feature service | — | — | — | **No** |
| Sales | Quotation, SalesOrder | **0** | No | **Empty** | Domain events only | Yes | — | **No** |
| Treasury | FinancialAccount, Financial, FinancialTransfer | 10 | Yes | Payment events + delete-by-invoice | Out: customer/supplier payment | Yes | — | Strong |

---

## 8. Architecture.Tests — current allow-lists

Files:

- `Tests/Architecture.Tests/ModuleDependencyTests.cs`
- `Tests/Architecture.Tests/ModuleLayerDependencyTests.cs`
- `Tests/Architecture.Tests/ModuleInfrastructureDependencyTests.cs` (zero exceptions)
- Plus Legacy*EliminationTests, CommercialDocumentsOwnershipTests, GeneralLedgerArchitectureTests

`ModuleDomains` **includes Catalog** (line 22). Older commentary that Catalog is omitted is **false** against current code.

### 8.1 AcceptedDomainExceptions (7)

| From | To | Current reason in test | Actual offending types |
|------|-----|------------------------|------------------------|
| Treasury | MasterData | EF navs on Bank/BankBranch/FinancialAccount/Financial/FinancialTransfer | `Bank.Country`; `BankBranch.Country/City/District`; `FinancialAccount.Currency`; `Financial.PaymentType` (+ Currency); `FinancialTransfer.Currency`; `Treasury.Domain/GlobalUsings.cs` |
| CommercialDocuments | MasterData | Invoice Currency/PaymentType | `Invoice.PaymentType`, `Invoice.Currency` |
| CommercialDocuments | Catalog | InvoiceProduct.Unit | `InvoiceProduct.Unit` |
| Parties | MasterData | Dealer geo navs | `Dealer.Country/City/District`; `PartyAddress.Country/City/District` |
| Inventory | MasterData | Product/Stock/Transaction geo/currency | **Stale.** Only `Inventory.Domain.csproj` + `global using MasterData.Domain` remain. No entity types |
| Inventory | Catalog | Product/Unit navs | `TransactionProduct`, `InventoryProduct`, `InventoryBalance`, line entities, `StockReservation`, `InventoryBatch`, `InventorySerial`, `InventoryCostLayer` |
| Purchasing | Catalog | PR/PO line Unit | `PurchaseRequisitionProduct.Unit`, `PurchaseOrderProduct.Unit` |

### 8.2 AcceptedApplicationDomainExceptions (16)

| From | To | Offending files (representative) |
|------|-----|----------------------------------|
| Sales | MasterData | Assembly ref only — `MappingProfile` empty |
| CommercialDocuments | Catalog | `Invoices/MappingProfile.cs` Unit/UnitDto; `GetByIdQueryHandler` |
| Parties | MasterData | `Dealers/MappingProfile.cs` Country/City/District.Name |
| Inventory | CommercialDocuments | Transaction handlers `IRepository<Invoice>`; `TransactionJournalPostingService` |
| Inventory | Catalog | `GetListByBalanceQueryHandler` `IRepository<Product>`; mapping |
| Inventory | MasterData | `TransactionJournalPostingService` `IRepository<Currency>` |
| Treasury | CommercialDocuments | Financial handlers `IRepository<Invoice>` |
| Treasury | MasterData | `FinancialAccounts/MappingProfile.cs`; paid-invoice handler uses `invoice.Currency` |
| Reporting | CommercialDocuments | Invoice report handlers |
| Reporting | Parties | Dealer report handlers |
| Reporting | Treasury | Safe/financial report handlers |
| Reporting | Inventory | Warehouse report handlers |
| Reporting | MasterData | Currency via includes / transitive |
| Reporting | Catalog | Product/Classification via TransactionProduct |
| Organization | MasterData | `CreateCompanyCommandValidator`, `UpdateCompanyCommandValidator` `IRepository<Country/Currency>` |
| Organization | SaaS | Same validators `IRepository<Tenant>` |

### 8.3 AcceptedApplicationApplicationExceptions (3)

| From | To | Note |
|------|-----|------|
| Sales | MasterData | Unused; empty mapping |
| CommercialDocuments | Catalog | UnitDto mapping |
| Inventory | Catalog | ProductDto/UnitDto |

**Stale csproj refs not in Application→Application allow-list** (currently unused IL, so tests stay green):

- `CommercialDocuments.Application` → `MasterData.Application`
- `Inventory.Application` → `MasterData.Application`

### 8.4 What is already clean (do not regress)

- Accounting.Domain → no other Domain
- Accounting.Application → no other Domain/Application
- Receivables.Application / Payables.Application → Contracts only
- Purchasing.Application → CommercialDocuments.Contracts + Parties.Contracts
- Administration.Application → Organization.Contracts
- Catalog.Application → Parties.Contracts
- Infrastructure ↔ Infrastructure: **zero** exceptions
- Domain → Infrastructure / EF / MediatR: **forbidden and currently passing**

---

## 9. Integration events (as-is)

In-process MediatR notifications. No outbox. Duplicate handling relies on unique indexes for AR/AP only.

| Event | Defined | Published | Handled |
|-------|---------|-----------|---------|
| `JournalPostedIntegrationEvent` | Accounting.Contracts | Journal `PostCommandHandler` | none |
| `JournalReversedIntegrationEvent` | Accounting.Contracts | `ReverseCommandHandler` | none |
| `JournalCancelledIntegrationEvent` | Accounting.Contracts | `CancelCommandHandler` | none |
| `SalesInvoicePostedIntegrationEvent` | CommercialDocuments.Contracts | Invoice `CreateCommandHandler` | Receivables `SalesInvoicePostedIntegrationEventHandler` |
| `PurchaseInvoicePostedIntegrationEvent` | CommercialDocuments.Contracts | Invoice `CreateCommandHandler` | Payables `PurchaseInvoicePostedIntegrationEventHandler` |
| `CustomerPaymentPostedIntegrationEvent` | Treasury.Contracts | `PostTransactionCommandHandler` | Receivables `CustomerPaymentPostedIntegrationEventHandler` |
| `SupplierPaymentPostedIntegrationEvent` | Treasury.Contracts | `PostTransactionCommandHandler` | Payables `SupplierPaymentPostedIntegrationEventHandler` |

Missing events (not in code): goods receipt posted, stock reserved, sales order confirmed, PO received, custody issued, depreciation posted, document approved.

---

## 10. Concepts searched — do they already exist?

| Concept | Exists? | Where | Safe action |
|---------|---------|-------|-------------|
| User / Role / Permission | Yes | Administration Domain + `User`/`Role`/`Permission`/`RolePermission` tables | Keep. Extract Identity only with a table-move plan |
| UserRole many-to-many | No | `User.RoleId` single FK | Do not invent a second membership model without a migration |
| UserTenant/Company/Branch access | Partial | `User.BranchId` only | New tables if IAM extraction happens |
| JWT / password hash | Yes | `API/Authentication/*`, `Administration.Application/Security/PasswordHasher.cs` | Do not change hashing casually |
| Workflow / ApprovalPolicy | **No types** | PR “approval” is `PurchaseRequisition` Status + convert-to-PO | New module later; do not scatter engines |
| TaxCode / VAT engine / ZATCA | **No** | Invoice `Tax`/`TaxType`; Company `TaxRegistrationNumber` | New Tax module later; snapshot on Invoice |
| Budget / CostCenter / Department / Project | **No types** | SaaS seeds feature key `Budgeting` only; GL seed has depreciation *accounts* | New Budgeting module later; Department may belong in Organization |
| Fixed assets / depreciation run | **No** | Chart of accounts seed strings only (`1202` Accumulated Depreciation) | New FixedAssets module later |
| Outbox / Inbox | **No** | Docs only | New persistence later |
| `ICurrentTenant` | **No** | — | Introduce in SaaS/SharedKernel later |
| Custody | Yes, Domain only | Advances | Complete Advances; do not recreate in Treasury |
| RFQ | **No** | Purchasing starts at Requisition | Create only if P2P completion requires it |
| Goods receipt | Yes | `InventoryReceipt` | Wire to PO; do not duplicate as Purchasing GRN entity without ownership decision |
| Sales quotation / sales order | Yes, Domain only, not persisted | Sales | Restore persistence; do not put orders in CommercialDocuments |
| Invoice | Yes | CommercialDocuments | Sales must not recreate invoices |
| Cheque register | Enum only | `FinancialTransactionType.Cheque` | Extend Treasury, do not new module |
| Feature gating | Interface only | Unused by other modules | Call from use cases |

---

## 11. Angular vs backend (summary)

App: `OrgSys.Angular/src/app` — Angular 19.2, standalone, Dore template preserved.

Feature folders: `auth`, `dashboard`, `accounting`, `catalog`, `commercial-documents`, `inventory`, `master-data`, `organization`, `parties`, `reporting`, `treasury`.

Auth: JWT in `localStorage` (`orgsys.token`), bearer interceptor, `authGuard` on shell. `permissionGuard` **exists unused**. No TenantId header.

`package.json` scripts: `start`, `build`, `watch`, `test`. **No lint script.**

Largest UI gaps vs live API: User/Role/Preference, Company/Settings, Purchasing, Payables, Receivables, SaaS, Catalog brand/pricing, Inventory issues/transfers/adjustments/batches/serials, Advances.

Full gap matrix belongs in Stage 15 (`docs/angular/backend-frontend-gap-analysis.md`). Existing `docs/angular-backend-alignment.md` §2 uses **pre-migration feature names**; §11/§14 match current folders.

---

## 12. Tests — coverage honesty

Present and useful:

- Architecture isolation (allow-lists still non-zero)
- Accounting journal lifecycle + invoice posting integration
- Inventory aggregate + reservation concurrency
- AR/AP integration event handlers
- Password hasher
- Several Domain invariant suites

Absent:

- Tenant isolation tests
- Cross-tenant IDOR tests
- Unauthenticated/unauthorized API tests for Payable/Receivable/Transfer
- E2E Flow A–G from the master plan
- SaaS entitlement tests
- Outbox tests
- Angular tests as a quality gate (Karma exists; not in CI)
- CI itself

---

## 13. Security / observability / production gaps (facts)

| Area | Current |
|------|---------|
| AuthN | JWT; MVC cookies |
| AuthZ | Permission keys in JWT claims; **no** ASP.NET policy handlers on API commands |
| Tenant isolation | Not enforced |
| Entitlements | Not called |
| Secrets | Placeholders rejected at startup; `appsettings.Local.json` ignored |
| CORS | localhost:4200 only |
| Swagger | Dev only |
| Logging | Default ASP.NET; no correlation id; 500s leak exception messages |
| Health | None |
| OpenTelemetry | None |
| Concurrency | Journal reversal unique index; AR/AP unique indexes; inventory reservation integration tests. Many other financial paths still handler-level `if` |
| CI | None |

---

## 14. Documentation vs code (conflicts)

| Document | Conflict with current code |
|----------|----------------------------|
| `docs/architecture/context-map.md` | Describes Workflow, Budgeting, IAM extraction, `ICurrentTenant`, Department as if they exist. They do not. **Target sketch.** |
| `docs/dependency-rules.md` §3 | Allow-list tables are **stale** (still mention Sales.Domain, Accounting→Sales, etc.). Tests are the truth. |
| `docs/architecture/remaining-contexts-current-state.md` | Dated 2026-09-16; module count/SaaS wording lag current 16-module composition. |
| Architecture test file header comments | Still say Domain assemblies were empty in Phase 1 — historically true, currently false. |
| `docs/angular-backend-alignment.md` §2 | Pre-migration folder names. |

Do **not** delete those files in Phase 0. Later documentation stages should mark them Historical or rewrite them to match code.

---

## 15. Honest scorecard (not complete)

Scores are 0–10. **Nothing is 10.** “Module exists as four csproj files” is not completeness.

| Dimension | Score | Exact deficiency |
|-----------|-------|------------------|
| Architecture | 6 | Modular layout exists; 7 Domain + 16 App→Domain + 3 App→App exceptions; shared DbContext by design |
| DDD | 5 | Accounting/Advances/Sales/Purchasing/AR/AP rich; Invoice, User, most masters anemic public setters; generic CRUD handlers mutate state |
| Module isolation | 5 | Accounting/AR/AP clean; Reporting/Treasury/Inventory/Invoice navigations leak |
| Domain richness | 5 | Same split |
| Testing | 4 | Architecture + pockets of domain/integration; no E2E, no tenant tests, no CI |
| ERP coverage | 5 | GL/Treasury/Inventory/Catalog/Parties/Invoices/AR/AP/PR-PO live; Sales/Advances not persisted; Tax/FA/Budget/Workflow absent |
| SaaS readiness | 3 | Tables + lifecycle methods; no execution context, no isolation, no limit enforcement |
| Security | 4 | Hashing + JWT exist; unauthenticated money/read APIs; no policy authz; no tenant checks |
| Observability | 1 | No health, tracing, correlation, structured ops logs |
| Frontend alignment | 5 | Feature folders match some modules; Purchasing/AR/AP/Admin/SaaS/Advances missing |
| Production readiness | 3 | Builds locally; no CI; dual UI (MVC+Angular); Domain.zip tracked; no outbox |

**Recommended next action:** Stage 1 isolation (zero cheap exceptions first), not new Tax/FixedAssets/Budgeting/Workflow/Identity projects.

---

## 16. Phase 0 deliverables

| File | Purpose |
|------|---------|
| `docs/completion/current-state-audit.md` | This document |
| `docs/completion/context-map.md` | As-is relationships, classified |
| `docs/completion/master-execution-plan.md` | Repository-specific work items |
| `docs/completion/progress.md` | Stage log |

No business code, schema, or API contracts were changed in Phase 0.
