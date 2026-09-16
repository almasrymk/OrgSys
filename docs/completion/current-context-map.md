# OrgSys Current Context Map

**Generated from live source on 2026-09-17.**  
HEAD: `4c0ffdda06fe103f8fda1b1a987855de2c58804e` (equals the reviewed baseline).  
Branch: `Latest`.

This is the **as-is** graph. It is not a target sketch.

Related files:

- `docs/completion/current-state-audit.md` — inventories and completeness matrix
- `docs/completion/master-execution-plan.md` — exception-by-exception Stage 1 plan
- `docs/architecture/context-map.md` — **outdated target sketch** (Workflow, Budgeting, `ICurrentTenant` do not exist)

Relationship types used below:

| Type | Meaning |
|------|---------|
| Contracts | `From.Application` ProjectReference to `To.Contracts` only |
| Integration Event | MediatR `IIntegrationEvent` published in-process |
| Scalar ID + Fluent FK | Identity field in Domain; FK configured in `OrgContext` without a Domain navigation |
| Direct Domain | `From.Domain` ProjectReference / type usage of `To.Domain` |
| Application → foreign Domain | `From.Application` uses `To.Domain` types (`IRepository<Invoice>`, validators) |
| Application → Application | `From.Application` ProjectReference to `To.Application` |
| Reporting read | Reporting.Application queries another module’s Domain via `IRepository<T>` |
| Host composition | `API/Program.cs` `Add*Module()` |

---

## 1. Composition root

`API/Program.cs` registration order (do not reorder casually):

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

Shared host wiring in the same file: JWT bearer, CORS `AngularClient` (`localhost:4200`), `IOrgContext`/`IUnitOfWork`/`IRepository<>`, `IIntegrationEventPublisher` → `MediatrIntegrationEventPublisher`, `FluentValidationFilter`, `GlobalExceptionMiddleware`.

MVC host `OrgSys/OrgSys.csproj` still references Application slices of Sales, CommercialDocuments, Parties, Inventory, Catalog plus DatabaseMigrator. It is a second composition root, not a bounded context.

---

## 2. ProjectReference graph (illegal and legal)

### 2.1 Domain → Domain (illegal unless allow-listed)

From current `*.Domain.csproj` files:

| From project | To project | Status |
|--------------|------------|--------|
| `Treasury.Domain` | `MasterData.Domain` | **Allow-listed** |
| `CommercialDocuments.Domain` | `MasterData.Domain`, `Catalog.Domain` | **Allow-listed** |
| `Parties.Domain` | `MasterData.Domain` | **Allow-listed** |
| `Inventory.Domain` | `MasterData.Domain`, `Catalog.Domain` | **Allow-listed** (MasterData is stale — no remaining types) |
| `Purchasing.Domain` | `Catalog.Domain` | **Allow-listed** |
| All other Domain projects | SharedKernel only | Clean |

Clean Domain projects (SharedKernel only): Accounting, Administration, Advances, Catalog, MasterData, Organization, Payables, Receivables, SaaS, Sales.

### 2.2 Application → foreign Domain (illegal unless allow-listed)

| From project | To project | How it is referenced |
|--------------|------------|----------------------|
| `Organization.Application` | `MasterData.Domain`, `SaaS.Domain` | **Direct ProjectReference** |
| `Inventory.Application` | `CommercialDocuments.Domain` | **Direct ProjectReference** |
| `Treasury.Application` | `CommercialDocuments.Domain` | **Direct ProjectReference** |
| `Reporting.Application` | `CommercialDocuments.Domain`, `Treasury.Domain`, `Inventory.Domain`, `Parties.Domain` | **Direct ProjectReference** |
| `Reporting.Application` | `Catalog.Domain`, `MasterData.Domain` | Transitive via Inventory/Treasury navigations (allow-listed) |
| `Parties.Application` | `MasterData.Domain` | Transitive via `Dealer.Country/City/District` used in mapping/includes |
| `Treasury.Application` | `MasterData.Domain` | Transitive via `FinancialAccount.Currency`, `Financial.Currency/PaymentType` |
| `CommercialDocuments.Application` | `Catalog.Domain` | Transitive via `InvoiceProduct.Unit` + `Catalog.Application` UnitDto maps |
| `Inventory.Application` | `Catalog.Domain`, `MasterData.Domain` | Catalog via Product/Unit maps; MasterData via `IRepository<Currency>` |
| `Sales.Application` | `MasterData.Domain` | Allow-listed; **stale** (empty `MappingProfile`) |

### 2.3 Application → Application (illegal unless allow-listed)

| From | To | ProjectReference exists? | Live type usage? |
|------|-----|--------------------------|------------------|
| `Sales.Application` | `MasterData.Application` | Yes | **No** — empty `MappingProfile` |
| `CommercialDocuments.Application` | `Catalog.Application` | Yes | Yes — `Unit`/`UnitDto` maps |
| `CommercialDocuments.Application` | `MasterData.Application` | Yes | **No live types found** (not in allow-list; currently unused IL) |
| `Inventory.Application` | `Catalog.Application` | Yes | Yes — `ProductDto`/`UnitDto` |
| `Inventory.Application` | `MasterData.Application` | Yes | **No live types found** (not in allow-list; currently unused IL) |

### 2.4 Application → Contracts (legal; current)

| From | To Contracts |
|------|----------------|
| Accounting.Application | Parties, MasterData |
| Administration.Application | Organization |
| Catalog.Application | Parties |
| CommercialDocuments.Application | Treasury, Inventory, Accounting, Administration, Parties, own |
| Inventory.Application | Accounting, Administration, Parties, Organization, own |
| Organization.Application | own |
| Parties.Application | Accounting, Administration, own |
| Payables.Application | Accounting, MasterData, Administration, CommercialDocuments, Treasury, own |
| Purchasing.Application | CommercialDocuments, Parties, own (own Contracts empty) |
| Receivables.Application | Accounting, MasterData, Administration, CommercialDocuments, Treasury, own |
| SaaS.Application | own |
| Treasury.Application | Accounting, Administration, CommercialDocuments, Parties, own |
| Reporting.Application | own (empty) |
| Sales.Application | own (empty) |
| Advances.Application | own (marker only) |
| MasterData.Application | own |

### 2.5 Contracts → other Contracts

| From | To | Note |
|------|-----|------|
| `Accounting.Contracts` | `Parties.Contracts` | AR/AP account validators take dealer type. Unusual but not Domain leakage. |

All other Contracts projects reference SharedKernel and optionally EventBus only.

### 2.6 Infrastructure

Every `*.Infrastructure` project references **only** its own Application (which pulls Domain/Contracts).  
`ModuleInfrastructureDependencyTests` currently asserts **zero** Infrastructure→Infrastructure exceptions for 13 of 16 modules.

**Test coverage gap:** the infrastructure test array omits `Catalog.Infrastructure`, `SaaS.Infrastructure`, `Advances.Infrastructure` even though Architecture.Tests.csproj references those assemblies.

### 2.7 DatabaseMigrator (shared persistence composition)

`BuildingBlocks/OrgSys.DatabaseMigrator/OrgSys.DatabaseMigrator.csproj` references **every Domain except Advances and Reporting**, plus most Infrastructure seeders. It is not in `OrgSys.sln`. It owns `OrgContext` and all EF migrations.

Missing Domain refs: `Advances.Domain` (Custody not mapped), Reporting (no Domain).

---

## 3. Module map

```text
                         API (JWT) + OrgSys MVC
                                    │
        ┌───────────────┬───────────┼────────────┬──────────────┐
        │               │           │            │              │
      SaaS         Organization  Administration  MasterData   Catalog
        │ TenantId      │ BranchId  │ User/Role  │ Currency/    │ Product/Unit
        │ (scalar)      │           │ Preference │ Country      │
        └───────┬───────┴─────┬─────┴──────┬─────┴──────┬───────┘
                │             │            │            │
         Parties.Dealer  Accounting.Journal  Treasury.Financial
                │             │            │
         CommercialDocuments.Invoice
                │
        ┌───────┴────────┐
   Receivables      Payables     ← integration events from Invoice + Treasury
                │
           Inventory (legacy Transaction + hardened ledger)
           Purchasing (PR / PO; Unit nav to Catalog)
           Sales (Quotation/SalesOrder — Domain only, not in OrgContext)
           Advances (Custody — Domain only, not in OrgContext)
           Reporting ──reads Domain of Invoice/Dealer/Financial/TransactionProduct──
```

---

## 4. Relationship catalog by owner

### 4.1 SaaS owns Tenant / Plan / Feature / PlanFeature / Subscription

| From | To | Type | Evidence |
|------|-----|------|----------|
| Organization.Domain.Company | SaaS.Tenant | Scalar ID + Fluent FK | `Company.TenantId`; OrgContext `HasOne(typeof(Tenant))` |
| Organization.Application | SaaS.Domain.Tenant | Application → foreign Domain | `CreateCompanyCommandValidator`, `UpdateCompanyCommandValidator` `IRepository<Tenant>` |
| Other modules | SaaS.Contracts | **Almost unused** | `EnsureTenantActiveQuery`, `ITenantFeatureService` have **no consumers outside SaaS** |

No `ICurrentTenant`. No query filters.

### 4.2 Organization owns Company / Branch / Shift / Table / OrganizationSettings

| From | To | Type | Evidence |
|------|-----|------|----------|
| Company / OrganizationSettings | MasterData | Scalar ID + Fluent FK | `CountryId`, `DefaultCurrencyId` — **no Domain nav** |
| Organization.Application | MasterData.Domain | Application → foreign Domain | same validators `IRepository<Country>`, `IRepository<Currency>` |
| Administration.Domain.User | Organization.Branch | Scalar ID + Fluent FK | `User.BranchId` — **do not restore Branch navigation** |
| MovementModel documents | Organization.Branch | Scalar ID + Fluent FK | OrgContext loop on `BranchId` |
| Inventory.Application | Organization.Contracts | Contracts | `GetBranchNamesQuery` |

No Department / CostCenter / Project types.

### 4.3 Administration owns User / Role / Permission / RolePermission / Preference

IAM lives here. Do not create `Modules/Identity` from this audit.

| From | To | Type | Evidence |
|------|-----|------|----------|
| API AuthController | Administration.Application | Host | `LoginCommand`, `CheckPasswordQuery` |
| CommercialDocuments / Treasury / Parties / Inventory Application | Administration.Contracts | Contracts | `GetPreferenceValueQuery` / `GetPreferenceValuesQuery` |
| MovementModel | Administration.User | Fluent FK no Domain nav | `CreateUserId` / `ModifyUserId` |
| CashBox | Administration.User | Scalar ID + Fluent FK | `KeeperUserId` |

Missing Contracts: `HasPermissionQuery`, `GetUserAccessQuery`, tenant/company/branch access queries.

Password hashing: `IPasswordHasher` / `PasswordHasher` / `UserPasswordApplier` in `Administration.Application/Security/`. `User.MustResetPassword` is the AES→hash cutover flag. User→UserDto mapping **ignores** `Password`. DTO still declares `Password` / `NewPassword` fields.

### 4.4 MasterData owns Country / City / District / Currency / PaymentType / ReferenceType

This is the largest remaining Domain leak source.

Live Domain navigations into MasterData:

- Treasury: `Bank.Country`, `BankBranch.Country/City/District`, `FinancialAccount.Currency`, `Financial.PaymentType` + `Financial.Currency`, `FinancialTransfer.Currency`
- CommercialDocuments: `Invoice.PaymentType`, `Invoice.Currency`
- Parties: `Dealer.Country/City/District`, `PartyAddress.Country/City/District`

Contracts today: currency only (`GetDefaultCurrencyQuery`, `GetCurrencyNamesQuery`, `CurrencyLookupDto`).  
**No** Country / City / District / PaymentType contracts — that gap is why navigations remain.

### 4.5 Catalog owns Product / Unit / Classification / Brand / Property / PriceList

Live Domain navigations into Catalog:

- CommercialDocuments: `InvoiceProduct.Unit`
- Purchasing: `PurchaseRequisitionProduct.Unit`, `PurchaseOrderProduct.Unit` (`ProductId` already scalar)
- Inventory: Product and/or Unit on `TransactionProduct`, `InventoryProduct`, `InventoryBalance`, `InventoryReceiptLine`, `InventoryIssueLine`, `StockTransferLine`, `StockAdjustmentLine`, `StockReservation`, `InventoryBatch`, `InventorySerial`, `InventoryCostLayer`

Contracts today: `GetProductNamesQuery`, `ResolvePriceQuery` / `ResolvedPriceDto`. **No** unit-name / product-reference DTO.

### 4.6 Parties owns Dealer / DealerGroup / CustomerProfile / SupplierProfile / PartyContact / PartyAddress

Dealer is the customer/supplier master. Do not recreate Customer/Supplier entities.

| From | To | Type | Evidence |
|------|-----|------|----------|
| Invoice / PO / Receivable / Payable / Financial | Dealer | Scalar ID | `DealerId` / `CustomerId` / `SupplierId` |
| Parties.Application | Accounting.Contracts | Contracts | `ProvisionSubAccountCommand` |
| Reporting.Application | Parties.Domain | Reporting read | `IRepository<Dealer>` |
| Catalog.Application | Parties.Contracts | Contracts | dealer lookups for pricing |

### 4.7 Accounting owns Account / AccountType / FiscalYear / FiscalPeriod / Journal / JournalItem / JournalType

Isolation reference. No foreign Domain ProjectReferences.

Outbound: Contracts posting API + journal integration events (posted/reversed/cancelled) with **no in-module consumers**.

Inbound posting consumers (Contracts, legal): CommercialDocuments, Treasury, Inventory, Receivables, Payables, Parties (provision sub-account).

`Journal.CurrencyId` is scalar-only — **the pattern Stage 1 must copy**.

### 4.8 CommercialDocuments owns Invoice / InvoiceProduct / InvoiceType

Invoice still stores settlement fields (`Paid`, `Remaining`, `Credit`) that Treasury handlers **mutate via `IRepository<Invoice>.UpdateAsync`**. That is a table-ownership leak: AR/AP should own settlement; Treasury should own money movement; Invoice should own the commercial document.

Existing Contracts already used by some callers:

- `GetInvoiceReferenceQuery` / `InvoiceReferenceDto`
- `GetInvoiceNetsQuery`
- `SalesInvoicePostedIntegrationEvent` → Receivables
- `PurchaseInvoicePostedIntegrationEvent` → Payables

Inventory and most Treasury payment handlers still load `Invoice` anyway.

### 4.9 Treasury owns FinancialAccount / CashBox / BankAccount / Bank / BankBranch / Financial / FinancialTransfer / FinancialInvoice / FinancialType / Outlay

Unified cash/bank model is live. Cheque is an enum/`FinancialType` row, not a register aggregate.

Events out: `CustomerPaymentPostedIntegrationEvent`, `SupplierPaymentPostedIntegrationEvent` from `PostTransactionCommandHandler`.

Advances is **not yet** a Treasury consumer (`Custody.IssuingFinancialTransactionId` is a scalar with no Application call).

### 4.10 Receivables / Payables

These two **already follow the target interaction rule** (Contracts + events only).

Inbound events:

- Receivables: `SalesInvoicePostedIntegrationEvent`, `CustomerPaymentPostedIntegrationEvent`
- Payables: `PurchaseInvoicePostedIntegrationEvent`, `SupplierPaymentPostedIntegrationEvent`

Do not add Domain references here during Stage 1.

### 4.11 Purchasing owns PurchaseRequisition / PurchaseOrder (+ line entities)

Application is clean (Parties.Contracts + CommercialDocuments.Contracts).  
Domain still has Catalog `Unit` navigations.  
`Purchasing.Contracts` is an **empty csproj**.  
InventoryReceipt is **not wired** as PO goods receipt.

### 4.12 Inventory owns warehouses, stock movements, hardened ledger

Dual write path:

1. Legacy `Transaction` / `TransactionProduct` (Angular movements + invoice-linked stock)
2. Hardened `InventoryReceipt` / `InventoryIssue` / `StockTransfer` / `StockAdjustment` / `StockReservation` / batch / serial / cost layer

Sales never calls `ReserveInventoryCommand` (contract exists, unused).

### 4.13 Sales owns Quotation / SalesOrder (+ lines)

Rich Domain, `[Table("SalesQuotation"|"SalesOrder"|...)]`, **not in OrgContext**. Migration `20260914082704_RemoveDeadSalesOrderModel` dropped legacy `Order`. Application is empty mapping + stale MasterData.Application reference. Contracts empty.

### 4.14 Advances owns Custody / CustodyHandover

Rich lifecycle Domain. **Not in OrgContext.** Application is AssemblyMarker only. Contracts marker only.

### 4.15 Reporting has no Domain

Seven query handlers read transactional Domain entities directly. Empty Contracts. No projection tables.

---

## 5. Integration events (as-is)

In-process MediatR notifications. No Outbox/Inbox.

| Event | Defined in | Published by | Handled by |
|-------|------------|--------------|------------|
| `JournalPostedIntegrationEvent` | Accounting.Contracts | Journal `PostCommandHandler` | none |
| `JournalReversedIntegrationEvent` | Accounting.Contracts | `ReverseCommandHandler` | none |
| `JournalCancelledIntegrationEvent` | Accounting.Contracts | `CancelCommandHandler` | none |
| `SalesInvoicePostedIntegrationEvent` | CommercialDocuments.Contracts | Invoice `CreateCommandHandler` | Receivables handler |
| `PurchaseInvoicePostedIntegrationEvent` | CommercialDocuments.Contracts | Invoice `CreateCommandHandler` | Payables handler |
| `CustomerPaymentPostedIntegrationEvent` | Treasury.Contracts | `PostTransactionCommandHandler` | Receivables handler |
| `SupplierPaymentPostedIntegrationEvent` | Treasury.Contracts | `PostTransactionCommandHandler` | Payables handler |

Missing (not in code): goods-receipt posted, stock reserved, sales-order confirmed, PO received, custody issued, document approved, depreciation posted.

---

## 6. Target edges that do not exist yet

Do not implement in Stage 0.

```text
Sales            -> CommercialDocuments.Contracts   (invoice from order)
Sales            -> Inventory.Contracts             (reserve / deliver)
Purchasing       -> Inventory.Contracts             (GRN / remaining qty)
Inventory        -> Purchasing.Contracts            (receipt against PO)
Advances         -> Treasury.Contracts              (disburse / return cash)
Treasury         -> Receivables/Payables.Contracts  (apply payment; stop mutating Invoice)
Organization     -> SaaS.Contracts                  (tenant validation)
Organization     -> MasterData.Contracts            (country/currency validation)
Reporting        -> read models                     (stop Domain IRepository)
SaaS             -> every module via ICurrentTenant (does not exist)
Workflow / Tax / Budgeting / FixedAssets / Identity modules — do not exist
```

---

## 7. Isolation pattern already proven

Shared-database Fluent FKs **without** Domain navigations:

- `Journal.CurrencyId`
- `Invoice.DealerId`
- `Company.TenantId` / `Company.CountryId` / `Company.DefaultCurrencyId`
- `CashBox.KeeperUserId`
- `User.BranchId`
- `Financial.DealerId`

Stage 1 must copy this pattern, not drop SQL FKs.
