# OrgSys Phase 0 — Current-State Audit

**Generated from live source**, not historical documentation.

| Item | Value |
|------|--------|
| Repository | `D:\Work\MK\Source\OrgSys` (remote `https://github.com/almasrymk/OrgSys`) |
| Branch | `Latest` |
| HEAD | `4c0ffdda06fe103f8fda1b1a987855de2c58804e` |
| Baseline reviewed commit | `4c0ffdda06fe103f8fda1b1a987855de2c58804e` |
| HEAD vs baseline | **Equal** (0 commits after baseline). Working tree tracked source matches HEAD. |
| Audit date | 2026-09-17 |
| Source of truth | Current `.cs` / `.csproj` / EF snapshot / Architecture.Tests / Angular `src/app` |
| Code changes in this stage | **None** (documentation only) |

Companion documents:

- `docs/completion/current-context-map.md` — as-is relationship graph + ProjectReferences
- `docs/completion/master-execution-plan.md` — exception-by-exception Stage 1 cards
- `docs/completion/progress.md` — stage log

If this document conflicts with older reports under `docs/`, **this audit wins**.

---

## 1. What the repository is today

OrgSys is a **modular monolith ERP** on **.NET 10** with:

- One JWT API host (`API/`) composing **16** modules via `Add*Module()` in `API/Program.cs`.
- One legacy MVC host (`OrgSys/`) still operational.
- Angular 19.2 SPA (`OrgSys.Angular/`) as the replacement UI, incomplete vs API.
- One shared EF Core `OrgContext` and one SQL Server database.
- CQRS via MediatR + FluentValidation + AutoMapper.
- Cross-module communication that is **partially** Contracts/events (Accounting, Receivables, Payables are the best examples) and **partially** still Domain navigations / Application→Domain reads.

This is **not** a greenfield redesign. Do not recreate modules. Do not restore `Administration.Domain.User.Branch` navigation. Do not restore SharedKernel password encryption.

---

## 2. Solution layout

**Solution file:** `OrgSys.sln`

### Hosts

| Project | Path | Role |
|---------|------|------|
| API | `API/API.csproj` | JWT composition root. References all 16 `*.Infrastructure` projects + Shared persistence. |
| OrgSys | `OrgSys/OrgSys.csproj` | Cookie-auth MVC; still references Sales/CommercialDocuments/Parties/Inventory/Catalog Application. |
| OrgSys.Angular | `OrgSys.Angular/OrgSys.Angular.esproj` | Angular 19.2 SPA. |

### BuildingBlocks

| Project | In `.sln`? | Path |
|---------|------------|------|
| OrgSys.SharedKernel | Yes | `BuildingBlocks/OrgSys.SharedKernel/` |
| OrgSys.EventBus | Yes | `BuildingBlocks/OrgSys.EventBus/` |
| OrgSys.Infrastructure | Yes | `BuildingBlocks/OrgSys.Infrastructure/` |
| OrgSys.Localization | Yes | `BuildingBlocks/OrgSys.Localization/` |
| OrgSys.DatabaseMigrator | **No** | `BuildingBlocks/OrgSys.DatabaseMigrator/` — referenced by API + MVC |

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
| Application.Tests | Cross-module handler tests (journal, AR/AP events, password hasher, org validators, price, invoice reference, PO link) |
| Accounting.Domain.Tests / Accounting.Integration.Tests | GL + invoice journal posting |
| Inventory.Domain.Tests / Inventory.Integration.Tests | Stock + reservation concurrency |
| Advances, Catalog, Organization, Parties, Payables, Purchasing, Receivables, Sales, Treasury Domain.Tests | Aggregate invariants |

No dedicated Domain/Application/Integration test projects for Administration, CommercialDocuments, MasterData, Reporting, SaaS.

### Root hygiene

| Artifact | Status |
|----------|--------|
| `Domain.zip` | Present at repo root; **not** in `.gitignore` |
| `*.zip` | Not ignored |
| `bin/` / `obj/` | Ignored; still appear locally |
| `appsettings.Local.json` | Ignored (correct) |
| `.github/workflows` | **Missing — no CI** |
| `.claude/worktrees/` | Old layered-monolith worktree; ignore for architecture |

---

## 3. Building blocks (actual types)

### OrgSys.SharedKernel

Persistence: `IOrgContext`, `IRepository<TEntity>`, `IUnitOfWork`.  
Entity bases: `BaseModel`, `MovementModel`, `AggregateRoot`, `Status`, `IDomainEvent`.  
Result + generic CRUD handlers + FluentValidation pipeline.

Domain projects reference SharedKernel (CQRS/EF package refs live here). Architecture.Tests still forbid Domain → MediatR/EF **namespaces**.

`AggregateRoot` is unused by most live entities (`MovementModel` already occupies the base). Rich aggregates duplicate `_domainEvents`.

### OrgSys.EventBus

`IIntegrationEvent` / `IntegrationEvent` (MediatR `INotification`), `IIntegrationEventPublisher`, `MediatrIntegrationEventPublisher`.  
**No Outbox, Inbox, retry, or bus transport.**

### OrgSys.Infrastructure

`Repository<TEntity>` over `IOrgContext.Set<T>()`, `UnitOfWork`.  
Intention-revealing repos exist only for Journal, Account, FiscalPeriod, Receivable, PaymentApplication, Payable, SupplierPaymentApplication, InventoryBalance.

### OrgSys.DatabaseMigrator

Owns `Persistence/OrgContext.cs` (the **only** DbContext), all migrations, and seeders constructed directly (no DI).  
References every Domain **except Advances and Reporting**.

---

## 4. Persistence

Path: `BuildingBlocks/OrgSys.DatabaseMigrator/Persistence/OrgContext.cs`

- Zero `IEntityTypeConfiguration<T>` under module Infrastructure.
- Table names from Domain `[Table]` + Fluent FK/index in `OnModelCreating`.
- Fluent “no navigation” FKs already used: Journal.CurrencyId, CashBox.KeeperUserId, Dealer/Invoice Account/Dealer FKs, MovementModel CreateUser/Branch, Company.TenantId.

### Snapshot tables (87)

From `OrgContextModelSnapshot` `ToTable`:

Accounting 7: Account, AccountType, FiscalYear, FiscalPeriod, Journal, JournalItem, JournalType  
Administration 5: User, Role, Permission, RolePermission, Preference  
Catalog 10: Brand, Classification, PriceList, PriceListEntry, Product, ProductPropertyElement, ProductUnit, Property, PropertyElement, Unit  
CommercialDocuments 3: Invoice, InvoiceProduct, InvoiceType  
Inventory 22: Inventory, InventoryBalance, InventoryBatch, InventoryCostLayer, InventoryIssue, InventoryIssueLine, InventoryProduct, InventoryReceipt, InventoryReceiptLine, InventorySerial, ProductRecipe, Stock, StockAdjustment, StockAdjustmentLine, StockAdjustmentReason, StockReservation, StockTransfer, StockTransferLine, Transaction, TransactionProduct, TransactionType, WarehouseLocation  
MasterData 6: City, Country, Currency, District, PaymentType, ReferenceType  
Organization 5: Branch, Company, OrganizationSettings, Shift, Table  
Parties 6: CustomerProfile, Dealer, DealerGroup, PartyAddress, PartyContact, SupplierProfile  
Payables 3: Payable, SupplierPaymentApplication, SupplierPaymentApplicationLine  
Purchasing 4: PurchaseOrder, PurchaseOrderProduct, PurchaseRequisition, PurchaseRequisitionProduct  
Receivables 3: PaymentApplication, PaymentApplicationLine, Receivable  
SaaS 5: Feature, Plan, PlanFeature, Subscription, Tenant  
Treasury 10: Bank, BankAccount, BankBranch, CashBox, Financial, FinancialAccount, FinancialInvoice, FinancialTransfer, FinancialType, Outlay  
Sales **0** / Advances **0** / Reporting **0**

`[Table]` present but **not** in snapshot: `Custody`, `CustodyHandover`, `SalesQuotation`, `SalesQuotationLine`, `SalesOrder`, `SalesOrderLine`, `LogSys`, `Notification`, `CompanyProfile`.  
DbSets commented out: LogSys, Notification, CompanyProfile. PropertyElement/ProductPropertyElement have tables in snapshot despite commented DbSets in OrgContext (mapped via model).

### Migrations (28)

Newest:

1. `20260916174551_AddUserPasswordHashing` — `User.MustResetPassword`
2. `20260916054152_AddSaaSModuleAndTenantRetrofitStage1` — Tenant/Plan/Feature/Subscription + nullable `Company.TenantId`

Idempotency unique indexes already exist for Receivable source document, PaymentApplication.SourceFinancialId, Payable source document, SupplierPaymentApplication.SourceFinancialId, Journal.OriginalJournalId (filtered).

### Tenant columns

**Only** `Company.TenantId` and `Subscription.TenantId`. No global query filters. No TenantId on Invoice, Journal, Financial, Dealer, Product, User.

---

## 5. API composition and controllers

`API/Program.cs` module order is listed in `current-context-map.md`.

**Missing in Program.cs:** health checks, OpenTelemetry, correlation id, API versioning, ProblemDetails, authorization policies, tenant middleware.

Auth: `API/Authentication/JwtTokenService.cs`. Login: `POST /api/Auth/login`. `AuthController` is `[AllowAnonymous]` including `CheckPassword` / `HavePassword` / `CheckEmail`.

CRUD controllers inherit `[Authorize]` via `CoreController`. **Not authorized** (plain `ControllerBase`):

- `API/Controllers/Org/Financials/PayableController.cs`
- `API/Controllers/Org/Financials/ReceivableController.cs`
- `API/Controllers/Org/Financials/FinancialTransferController.cs`

### Controllers by backend context

| Context | Controllers under `API/Controllers/` |
|---------|--------------------------------------|
| Administration | `Org/AuthenticationController.cs`, `Org/Setting/UserController.cs`, `RoleController.cs`, `PreferenceController.cs` |
| Organization | `Org/Organization/CompanyController.cs`, `OrganizationSettingsController.cs`, `Org/Setting/BranchController.cs`, `ShiftController.cs`, `TableController.cs` |
| MasterData | Country, City, District, Currency, PaymentType, ReferenceType |
| Catalog | Product, Unit, Classification, Brand, PriceList, Pricing, Property, ProductUnitModelView |
| Parties | Dealer, DealerGroup, CustomerProfile, SupplierProfile, PartyContact, PartyAddress |
| Accounting | Account, AccountType, FiscalYear, Journal, JournalType |
| Treasury | CashBox, Bank, BankBranch, Financial, FinancialAccount, FinancialType, FinancialTransfer, Outlay |
| CommercialDocuments | Invoice, InvoiceType |
| Inventory | Transaction, TransactionType, Inventory, Stock, plus `Org/Inventory/*` receipts/issues/transfers/adjustments/reservations/batches/serials/balances/locations |
| Purchasing | PurchaseOrder, PurchaseRequisition |
| Receivables | Receivable |
| Payables | Payable |
| SaaS | Tenant, Plan, Feature, Subscription |
| Reporting | DealerReport, WarehouseReport, FinancialReport, SalesReport |
| Sales / Advances | **none** |

Errors: custom `Result` JSON from `GlobalExceptionMiddleware`. Unhandled exceptions return `ex.Message`. Not RFC7807 ProblemDetails.

---

## 6. Handler and contract counts (live)

Handlers = `*Handler.cs` under each module Application.

| Module | Handlers | Contracts source files (non-csproj) |
|--------|----------|-------------------------------------|
| Accounting | 52 | 20 (posting + accounts + journal events) |
| Administration | 23 | 2 (preference queries only) |
| Advances | **0** | AssemblyMarker only |
| Catalog | 48 | 3 (product names + price resolve) |
| CommercialDocuments | 23 | 5 (invoice ref/nets + posted events) |
| Inventory | 83 | 6 (reserve, availability, invoice-linked txn, stock names) |
| MasterData | 46 | 3 (currency only) |
| Organization | 33 | 4 (company/branch names, default company) |
| Parties | 35 | 4 (dealer lookups) |
| Payables | 10 | 9 (balance/aging/outstanding) |
| Purchasing | 22 | **empty csproj** |
| Receivables | 10 | 9 (mirror of Payables) |
| Reporting | 7 | **empty csproj** |
| SaaS | 30 | 6 (tenant lookups + `ITenantFeatureService`) |
| Sales | **0** | **empty csproj** |
| Treasury | 62 | 3 (delete-by-invoice + two payment events) |

---

## 7. Completeness matrix

Legend: **Strong** / **Partial** / **Thin** / **None**. “Module exists as four csproj files” is not completeness.

| Context | Domain | Application | Contracts | Infra | API | Angular | Domain tests | Integration tests | Illegal deps | Remaining business gaps |
|---------|--------|-------------|-----------|-------|-----|---------|--------------|-------------------|--------------|-------------------------|
| Accounting | Strong (Journal lifecycle) | Strong (52 handlers, posting API) | Strong | Strong | Yes | Partial (accounts, journals, FY) | Yes | Yes | None | TB/BS/P&L/cash flow live in Reporting, not here |
| Administration | Thin (anemic User/Role) | Partial (CRUD + login + hasher) | Thin (prefs only) | Seed + DI | Yes | Login only | PasswordHasher only | — | None | Permissions evaluation, User/Role UI, password-reset flow, CheckPassword anonymous |
| Advances | Strong (`Custody`) | None | Marker | Marker | None | None | Yes | — | None | Not in OrgContext; no Treasury disbursement |
| Catalog | Partial (ProductType exists; anemic masters) | Strong CRUD + pricing query | Thin | Seed | Yes | Products only | Product invariants | — | None outbound | Brand/PriceList/Property/Pricing UI |
| CommercialDocuments | Thin (anemic Invoice + tax ints) | Partial | Partial | Seed | Yes | Invoices/returns | Ownership arch tests | Via Accounting.Integration | Domain→MD/Catalog; App→Catalog | Settlement fields mutated by Treasury; no Tax engine |
| Inventory | Strong dual model | Strong (83) | Partial | Seed | Yes | Partial (legacy movements + count/locations/receipts/balances/reservations) | Yes (11) | Reservation concurrency | Domain→Catalog (+ stale MD); App→Invoice/Catalog/MD | Dual write path; GRN not wired to PO; issues/transfers/adjustments/batches/serials UI thin |
| MasterData | Thin lookup entities | Strong CRUD | Thin (currency only) | Seed | Yes | Geo + currency | — | — | None | Missing geo/payment-type Contracts (blocks Stage 1) |
| Organization | Thin | CRUD + validators hitting foreign Domain | Partial | Seed | Yes | Branches only | Company/Branch | — | App→MD.Domain, App→SaaS.Domain | Company/Settings UI; tenant link stage-1 only |
| Parties | Partial (Dealer roles) | CRUD + GL provision | Partial | Seed | Yes | Dealers + groups | Dealer roles | — | Domain→MD; App maps geo names | Profile/contact/address UI; Party vs Dealer migration later |
| Payables | Strong | Event handlers + reads | Strong | Yes | Yes **no auth** | **None** | Yes | Handler tests | None | UI; auth; stop Invoice as AP cache |
| Purchasing | Strong PR/PO qty methods | Partial (22) | **Empty** | Yes | Yes | **None** | Yes | LinkInvoice | Domain→Catalog Unit | RFQ, GRN wire-up, 3-way match, GRNI, Angular |
| Receivables | Strong | Event handlers + reads | Strong | Yes | Yes **no auth** | **None** | Yes | Handler tests | None | UI; auth; Invoice.Paid dual writer |
| Reporting | N/A (by design) | 7 queries over foreign Domain | Empty | Thin | Yes | 7 screens | — | — | App→6 Domains | No projections; no TB/BS/P&L/aging UI |
| SaaS | Partial (lifecycle methods) | CRUD + EnsureTenantActive | Partial unused | Seed | Yes | **None** | — | — | None outbound | No ICurrentTenant, no isolation, no entitlement consumers |
| Sales | Strong Quotation/SO | **Empty** | Empty | Marker | None | None | Yes | — | Stale App→MD.Application | Not persisted |
| Treasury | Partial (unified FA; anemic Financial) | Strong (62) | Thin | Seed | Yes | Strong | Financial txn tests | — | Domain→MD; App→Invoice/MD | Cheque register; EX-AD7 Invoice mutation; Advances integration |

---

## 8. Architecture.Tests — current allow-lists

Files:

- `Tests/Architecture.Tests/ModuleDependencyTests.cs` — Catalog **is** in `ModuleDomains`
- `Tests/Architecture.Tests/ModuleLayerDependencyTests.cs`
- `Tests/Architecture.Tests/ModuleInfrastructureDependencyTests.cs` (zero exceptions; **omits Catalog/SaaS/Advances from the array**)
- Plus Legacy*EliminationTests, CommercialDocumentsOwnershipTests, GeneralLedgerArchitectureTests

Counts: **7** Domain, **16** Application→Domain, **3** Application→Application.

Exact offending types and files are in `master-execution-plan.md` Stage 1 cards EX-D1–D7, EX-AD1–AD16, EX-AA1–AA3.

### Already clean (do not regress)

- Accounting.Domain / Accounting.Application isolation
- Receivables.Application / Payables.Application → Contracts only
- Purchasing.Application → CommercialDocuments.Contracts + Parties.Contracts
- Administration.Application → Organization.Contracts
- Catalog.Application → Parties.Contracts
- Infrastructure↔Infrastructure currently passing for the 13 modules in the test array
- Domain → Infrastructure / EF / MediatR: passing

### Stale allow-list / unused refs

- Inventory.Domain → MasterData: **no types left**
- Sales.Application → MasterData: empty MappingProfile
- CommercialDocuments.Application → MasterData.Application: unused ProjectReference (not even allow-listed)
- Inventory.Application → MasterData.Application: unused ProjectReference (not allow-listed)

---

## 9. Integration events

See `current-context-map.md` §5. In-process only. AR/AP unique indexes provide partial idempotency. Journal events have no handlers.

---

## 10. Concepts searched — do they already exist?

| Concept | Exists? | Where | Safe action |
|---------|---------|-------|-------------|
| User / Role / Permission / RolePermission | Yes | Administration | Keep; do not create Identity now |
| UserRole M:N | No | `User.RoleId` | Do not invent without migration |
| Password hash / MustResetPassword | Yes | `Administration.Application/Security/*`, `User` | Preserve; UserDto.Password ignored on map-out |
| Workflow / ApprovalPolicy | **No** | PR status only | New module later |
| TaxCode / VAT engine / ZATCA | **No** | Invoice.Tax/TaxType | New Tax module later |
| Budget / CostCenter / Department / Project | **No** | SaaS feature key `Budgeting` only | New Budgeting later |
| Fixed assets run | **No** | COA seed names | New FixedAssets later |
| Outbox / Inbox | **No** | — | Stage 13 |
| `ICurrentTenant` | **No** | — | Stage 12 |
| Custody | Domain only | Advances | Complete Advances |
| RFQ | **No** | — | Only if P2P needs it |
| Goods receipt | Yes | `InventoryReceipt` | Wire to PO |
| Sales quotation / order | Domain only, not persisted | Sales | Restore persistence |
| Cheque register | Enum only | `FinancialTransactionType.Cheque` | Stay in Treasury |
| Feature gating | Interface unused | `ITenantFeatureService` | Call from use cases |

---

## 11. Angular vs backend

App: `OrgSys.Angular/src/app` — Angular 19.2, standalone.

Root routes in `app.routes.ts`: `login`, `dashboard`, `accounting`, `treasury`, `parties`, `commercial-documents`, `inventory`, `catalog`, `master-data`, `organization`, `reporting`. Legacy redirects from `administration`, `financial`, `customers-suppliers`, `invoices`, `transactions`, `warehouse`, `reports`.

| Feature folder | Routes (actual) | Services (representative) | Backend |
|----------------|-----------------|---------------------------|---------|
| auth | `/login` | login | AuthController |
| accounting | `/accounting/accounts`, `journal-entries`, `fiscal-years` | account, journal, fiscal-year | Account/Journal/FiscalYear |
| treasury | `/treasury/financial-accounts`, `transfers`, `opening-balances`, `banks`, `bank-branches`, `receipts`, `payments`, `deposits`, `withdrawals`, `fees`, `interest`, `cheques`, `adjustments`, `transfer-in/out` | financial, financial-account, bank | CashBox/Bank/Financial* |
| parties | `/parties/dealers`, `dealer-groups` | dealer, dealer-group | Dealer* |
| commercial-documents | `/commercial-documents/sales-invoices`, `purchase-invoices`, `sales-returns`, `purchase-returns` | invoice, invoice-type | Invoice |
| inventory | `/inventory/count`, `locations`, `balances`, `reservations`, `receipts`, `movements/*` | stock, transaction, balance | Transaction/Inventory/Stock/Receipts |
| catalog | `/catalog/products` | product, unit, classification | Product (Brand/PriceList API unused by UI) |
| master-data | countries, cities, districts, currencies | geo/currency services | Country/City/District/Currency |
| organization | `/organization/branches` | branch | Branch (Company/Settings API unused by UI) |
| reporting | warehouse movement/balance, dealers balance/statement, safe movement/balance, sales balance | report.service | *ReportController |

**No Angular features** for: administration users/roles, purchasing, receivables, payables, advances, sales orders, saas, workflow, tax, fixed-assets, budgeting.

Auth: JWT in `localStorage` (`orgsys.token`), bearer interceptor, `authGuard` on shell. `permissionGuard` **exists unused**. No TenantId header. `package.json`: start/build/watch/test; **no lint script**.

Full endpoint matrix is a Stage 15 deliverable (`docs/angular/backend-frontend-matrix.md`).

---

## 12. Security / observability / production (facts)

| Area | Current |
|------|---------|
| AuthN | JWT; MVC cookies |
| AuthZ | Permission keys in JWT; no ASP.NET policy handlers on commands |
| Tenant isolation | Not enforced |
| Entitlements | `ITenantFeatureService` unused outside SaaS |
| Secrets | Placeholders rejected at API startup; Local.json ignored |
| CORS | localhost:4200 only |
| Swagger | Dev only |
| Logging | Default ASP.NET; no correlation id; 500s leak messages |
| Health | None |
| OpenTelemetry | None |
| Concurrency | Journal reversal unique index; AR/AP unique indexes; inventory reservation tests |
| CI | None |
| UserDto.Password | Field exists for input; AutoMapper Ignore on User→Dto. Do not regress |

---

## 13. Documentation vs code

| Document | Conflict |
|----------|----------|
| `docs/architecture/context-map.md` | Describes Workflow, Budgeting, IAM extraction, `ICurrentTenant` as if they exist. **Target sketch.** Use `docs/completion/current-context-map.md`. |
| `docs/dependency-rules.md` §3 | Allow-list tables stale vs Architecture.Tests |
| Architecture test file headers | Still say Domain assemblies were empty in Phase 1 — currently false |

Do not restore removed dependencies because an old `.md` mentions them.

---

## 14. Honest scorecard

Scores 0–10. **Nothing is 10.**

| Dimension | Score | Exact deficiency |
|-----------|-------|------------------|
| Architecture | 6 | 7+16+3 exceptions; shared DbContext |
| DDD | 5 | Rich Journal/Custody/SO/PO/AR/AP; anemic Invoice/User/Financial |
| Module isolation | 5 | Accounting/AR/AP clean; Reporting/Treasury/Inventory/Invoice leak |
| Testing | 4 | Architecture + pockets; no E2E, no tenant tests, no CI |
| ERP coverage | 5 | GL/Treasury/Inventory/Catalog/Parties/Invoices/AR/AP/PR-PO live; Sales/Advances not persisted; Tax/FA/Budget/Workflow absent |
| SaaS readiness | 3 | Tables + methods; no execution context, isolation, or enforcement |
| Security | 4 | Hashing + JWT; unauthenticated money APIs; anonymous password probes |
| Observability | 1 | No health, tracing, correlation |
| Frontend alignment | 5 | Feature folders match some modules; Purchasing/AR/AP/Admin/SaaS/Advances/Sales missing |
| Production readiness | 3 | Builds locally; no CI; dual UI; Domain.zip; no outbox |

**Recommended next action:** Stage 1 isolation (cheap stale refs first), not new Tax/FixedAssets/Budgeting/Workflow/Identity projects.

---

## 15. Phase 0 deliverables

| File | Purpose |
|------|---------|
| `docs/completion/current-state-audit.md` | This document |
| `docs/completion/current-context-map.md` | As-is relationships |
| `docs/completion/master-execution-plan.md` | Repository-specific work items + exception cards |
| `docs/completion/progress.md` | Stage log |

No business code, schema, or API contracts were changed in Phase 0.
