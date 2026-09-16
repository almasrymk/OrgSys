# OrgSys Context Map (as-is, 2026-09-17)

**Canonical current map:** [`current-context-map.md`](current-context-map.md)  
(that file is the Stage 0 prompt deliverable and includes the live ProjectReference graph).

This copy remains as a shorter relationship catalog. If the two disagree, **`current-context-map.md` plus current source win**.

It is **not** the same document as `docs/architecture/context-map.md`, which describes Workflow, Budgeting, IAM extraction, and `ICurrentTenant` as if they already exist. That file is a **target sketch**. This file is the **current graph**.

Relationship types:

| Type | Meaning |
|------|---------|
| Contracts | Caller depends on `Target.Contracts` only |
| Integration Event | Publisher → MediatR notification → handler in another module |
| Scalar ID | Identity field only (`DealerId`, `CurrencyId`, …); no object navigation in Domain |
| Direct Domain reference | `From.Domain` project/type reference to `To.Domain` (navigation or using) |
| Direct Application reference | `From.Application` → `To.Application` |
| Application → foreign Domain | `From.Application` uses `To.Domain` types (`IRepository<Invoice>`, validators) |
| Shared database navigation | Fluent FK in `OrgContext` and/or EF navigation; one database |
| Reporting read access | Reporting.Application queries another module’s Domain entities |
| Host composition | API registers the module; not a business dependency |

---

## 1. Module graph (current)

```text
                         API (JWT host) + OrgSys MVC
                                    │
        ┌───────────────┬───────────┼────────────┬──────────────┐
        │               │           │            │              │
      SaaS         Organization  Administration  MasterData   Catalog
        │               │           │            │              │
        │ TenantId      │ BranchId  │ User/Role  │ Currency/    │ Product/Unit
        │ (scalar)      │           │ Preference │ Country      │
        └───────┬───────┴─────┬─────┴──────┬─────┴──────┬───────┘
                │             │            │            │
         Parties.Dealer  Accounting.Journal  Treasury.Financial
                │             │            │
                │             │            │
         CommercialDocuments.Invoice
                │
        ┌───────┴────────┐
        │                │
   Receivables      Payables
        ▲                ▲
        │ events         │ events
        └──── Treasury ──┘
                │
           Inventory (legacy Transaction + hardened ledger)
                │
           Purchasing (PR / PO) ──link── Invoice
                │
           Sales (Quotation/SalesOrder — not persisted)
           Advances (Custody — not persisted)
           Reporting ──reads Domain of Invoice/Dealer/Treasury/Inventory/Catalog──
```

---

## 2. Relationship catalog

### 2.1 SaaS

| From | To | Type | Evidence |
|------|-----|------|----------|
| Organization.Domain | SaaS | Scalar ID + Shared database FK | `Company.TenantId`; OrgContext `HasOne(typeof(Tenant))` |
| Organization.Application | SaaS.Domain | Application → foreign Domain | `CreateCompanyCommandValidator` / `UpdateCompanyCommandValidator` `IRepository<Tenant>` |
| Host | SaaS | Host composition | `AddSaaSModule()` |
| Other modules | SaaS | **None** | `EnsureTenantActiveQuery` and `ITenantFeatureService` unused outside SaaS |

SaaS does not currently supply a tenant execution context to the rest of the ERP.

### 2.2 Organization

| From | To | Type | Evidence |
|------|-----|------|----------|
| Organization.Application | MasterData.Domain | Application → foreign Domain | Company validators `IRepository<Country>`, `IRepository<Currency>` |
| Organization.Domain | MasterData | Scalar ID + Shared database FK | `Company.CountryId`, `Company.DefaultCurrencyId` (no Domain nav) |
| Administration.Domain | Organization | Scalar ID + Shared database FK | `User.BranchId` |
| Administration.Application | Organization.Contracts | Contracts | Organization.Contracts project reference |
| MovementModel derivatives | Organization | Scalar ID + Shared database FK | `BranchId` on every movement document via OrgContext loop |
| CashBox / BankAccount | Organization | Scalar ID + Shared database FK | `BranchId` Fluent in OrgContext |
| Inventory.Application | Organization.Contracts | Contracts | `GetBranchNamesQuery` / names |
| Host | Organization | Host composition | `AddOrganizationModule()` |

No Department, CostCenter, or Project types.

### 2.3 Administration (IAM)

| From | To | Type | Evidence |
|------|-----|------|----------|
| API | Administration | Host + JWT | `LoginCommand`, `JwtTokenService` |
| CommercialDocuments.Application | Administration.Contracts | Contracts | `GetPreferenceValueQuery` (tax/accounts prefs) |
| Treasury.Application | Administration.Contracts | Contracts | Preferences |
| Parties.Application | Administration.Contracts | Contracts | Preferences |
| Inventory.Application | Administration.Contracts | Contracts | Preferences |
| Accounting / others | Administration.Domain | Shared database FK (no Domain nav) | MovementModel `CreateUserId` / `ModifyUserId` Fluent `HasOne(typeof(User))` |
| CashBox | Administration | Scalar ID + Shared database FK | `KeeperUserId` (navigation dropped) |

No `HasPermissionQuery`. Permission evaluation is JWT claim list + Angular `PermissionService`.

### 2.4 MasterData

| From | To | Type | Evidence |
|------|-----|------|----------|
| Treasury.Domain | MasterData.Domain | Direct Domain reference | Bank/BankBranch geo; Currency; PaymentType |
| CommercialDocuments.Domain | MasterData.Domain | Direct Domain reference | `Invoice.Currency`, `Invoice.PaymentType` |
| Parties.Domain | MasterData.Domain | Direct Domain reference | Dealer / PartyAddress geo |
| Inventory.Domain | MasterData.Domain | Direct Domain reference (stale) | csproj + global using, no types |
| Accounting.Application | MasterData.Contracts | Contracts | `GetDefaultCurrencyQuery`, `GetCurrencyNamesQuery` |
| Payables.Application | MasterData.Contracts | Contracts | default currency |
| Receivables.Application | MasterData.Contracts | Contracts | default currency |
| Parties.Application | MasterData.Domain | Application → foreign Domain (via mapping) | Dealer MappingProfile names |
| Treasury.Application | MasterData.Domain | Application → foreign Domain | Currency mapping / paid invoice |
| Reporting.Application | MasterData.Domain | Reporting read access | Currency on reports |
| Organization.Application | MasterData.Domain | Application → foreign Domain | validators |

Missing Contracts that would retire several edges: `GetCountryQuery`, `GetCityQuery`, `GetDistrictQuery`, `GetPaymentTypeQuery`, `GetCurrencyQuery` (existence), not only names/default.

### 2.5 Catalog

| From | To | Type | Evidence |
|------|-----|------|----------|
| Catalog.Application | Parties.Contracts | Contracts | dealer-related pricing/product if used |
| CommercialDocuments.Domain | Catalog.Domain | Direct Domain reference | `InvoiceProduct.Unit` |
| CommercialDocuments.Application | Catalog.Application | Direct Application reference | UnitDto mapping |
| CommercialDocuments.Application | Catalog.Contracts | Contracts | `GetProductNamesQuery` |
| Inventory.Domain | Catalog.Domain | Direct Domain reference | Product/Unit on lines, balances, batches, serials, reservations |
| Inventory.Application | Catalog.Application | Direct Application reference | ProductDto / UnitDto |
| Inventory.Application | Catalog.Domain | Application → foreign Domain | `GetListByBalanceQueryHandler` |
| Purchasing.Domain | Catalog.Domain | Direct Domain reference | PR/PO line `Unit` |
| Reporting.Application | Catalog.Domain | Reporting read access | Product/Classification via TransactionProduct |

Catalog is the product/unit owner. Inventory still treats Product as a navigation, not an ID + contract lookup.

### 2.6 Parties

| From | To | Type | Evidence |
|------|-----|------|----------|
| Parties.Application | Accounting.Contracts | Contracts | `ProvisionSubAccountCommand` on dealer create |
| Parties.Application | Administration.Contracts | Contracts | preferences |
| CommercialDocuments.Domain | Parties | Scalar ID + Shared database FK | `Invoice.DealerId` (navigation dropped) |
| Purchasing.Domain | Parties | Scalar ID | `PurchaseOrder.DealerId` / requisition dealer |
| Receivables.Domain | Parties | Scalar ID | `Receivable.CustomerId` |
| Payables.Domain | Parties | Scalar ID | `Payable.SupplierId` |
| Reporting.Application | Parties.Domain | Reporting read access | `IRepository<Dealer>` |
| Catalog.Application | Parties.Contracts | Contracts | dealer lookups |

### 2.7 Accounting (General Ledger)

| From | To | Type | Evidence |
|------|-----|------|----------|
| Accounting.Application | Parties.Contracts | Contracts | dealer for AR/AP account validators |
| Accounting.Application | MasterData.Contracts | Contracts | currency |
| Accounting.Domain | MasterData | Scalar ID + Shared database FK | `Journal.CurrencyId` — **no Domain navigation** |
| Parties / Inventory / Treasury Domain | Accounting | Scalar ID + Shared database FK | `AccountId` / `JournalId` Fluent `HasOne(typeof(Account/Journal))` |
| CommercialDocuments.Application | Accounting.Contracts | Contracts | `InvoiceJournalPostingService` posting commands |
| Treasury.Application | Accounting.Contracts | Contracts | financial posting / reverse |
| Inventory.Application | Accounting.Contracts | Contracts | `TransactionJournalPostingService` |
| Receivables.Application | Accounting.Contracts | Contracts | opening balance / activity |
| Payables.Application | Accounting.Contracts | Contracts | opening balance / activity |
| Accounting | others | Integration Event | Journal posted/reversed/cancelled — **no handlers** |

Accounting is the isolation reference: everyone who posts uses Contracts, not Accounting.Domain.

### 2.8 CommercialDocuments

| From | To | Type | Evidence |
|------|-----|------|----------|
| CommercialDocuments.Application | Accounting.Contracts | Contracts | journal posting |
| CommercialDocuments.Application | Inventory.Contracts | Contracts | `CreateTransactionByInvoiceCommand`, status/delete |
| CommercialDocuments.Application | Treasury.Contracts | Contracts | `DeleteFinancialsByInvoiceCommand` |
| CommercialDocuments.Application | Parties.Contracts | Contracts | dealer names |
| CommercialDocuments.Application | Administration.Contracts | Contracts | preferences |
| CommercialDocuments | Receivables | Integration Event | `SalesInvoicePostedIntegrationEvent` |
| CommercialDocuments | Payables | Integration Event | `PurchaseInvoicePostedIntegrationEvent` |
| Inventory.Application | CommercialDocuments.Domain | Application → foreign Domain | `IRepository<Invoice>` |
| Treasury.Application | CommercialDocuments.Domain | Application → foreign Domain | `IRepository<Invoice>` |
| Reporting.Application | CommercialDocuments.Domain | Reporting read access | Invoice reports |
| Purchasing.Application | CommercialDocuments.Contracts | Contracts | `LinkInvoiceCommand` uses invoice reference |

Invoice remains the commercial document aggregate. Sales must not recreate it.

### 2.9 Treasury

| From | To | Type | Evidence |
|------|-----|------|----------|
| Treasury.Application | Accounting.Contracts | Contracts | post/reverse journal |
| Treasury.Application | CommercialDocuments.Contracts | Contracts | invoice nets/reference |
| Treasury.Application | Parties.Contracts | Contracts | dealer names |
| Treasury.Application | Administration.Contracts | Contracts | preferences |
| Treasury | Receivables | Integration Event | `CustomerPaymentPostedIntegrationEvent` |
| Treasury | Payables | Integration Event | `SupplierPaymentPostedIntegrationEvent` |
| Advances (intended) | Treasury | **Not implemented** | Custody holds `IssuingFinancialTransactionId` scalar only; no Application call yet |

### 2.10 Receivables / Payables

| From | To | Type | Evidence |
|------|-----|------|----------|
| Receivables.Application | CommercialDocuments.Contracts | Integration Event + Contracts | sales invoice posted |
| Receivables.Application | Treasury.Contracts | Integration Event | customer payment posted |
| Receivables.Application | Accounting.Contracts | Contracts | opening balance / GL activity |
| Payables.Application | CommercialDocuments.Contracts | Integration Event | purchase invoice posted |
| Payables.Application | Treasury.Contracts | Integration Event | supplier payment posted |
| Payables.Application | Accounting.Contracts | Contracts | opening balance / GL activity |

These two modules **already follow the target interaction rule**. Do not add Domain references here.

### 2.11 Purchasing

| From | To | Type | Evidence |
|------|-----|------|----------|
| Purchasing.Application | Parties.Contracts | Contracts | supplier |
| Purchasing.Application | CommercialDocuments.Contracts | Contracts | link invoice |
| Purchasing.Domain | Catalog.Domain | Direct Domain reference | line Unit |
| Purchasing | Inventory | **Not wired** | PO has quantity methods (`RecordReceipt` etc.) but InventoryReceipt does not reference PO as a first-class integration |
| Purchasing | Payables | Indirect | Invoice posted event, not PO |

No Purchasing.Contracts types. Inventory/Reporting cannot ask Purchasing anything through Contracts today.

### 2.12 Inventory

| From | To | Type | Evidence |
|------|-----|------|----------|
| Inventory.Application | Accounting.Contracts | Contracts | transaction journal posting |
| Inventory.Application | Parties.Contracts | Contracts | dealer names |
| Inventory.Application | Organization.Contracts | Contracts | branch names |
| Inventory.Application | Administration.Contracts | Contracts | preferences |
| Inventory.Application | Catalog.* | Direct Application + foreign Domain | Product balance |
| Inventory.Application | CommercialDocuments.Domain | Application → foreign Domain | linked Invoice |
| Sales (intended) | Inventory.Contracts | Contracts exist | `ReserveInventoryCommand`, `GetInventoryAvailabilityQuery` — **no Sales.Application caller** |

### 2.13 Sales

| From | To | Type | Evidence |
|------|-----|------|----------|
| Sales.Application | MasterData.Application | Direct Application reference | **unused** empty MappingProfile |
| Sales | CommercialDocuments | **None in Application** | Invoice path is independent today |
| Sales | Inventory | **None** | Reservation contracts unused |

Sales is an island: rich Domain, no persistence, no outbound Contracts.

### 2.14 Advances

| From | To | Type | Evidence |
|------|-----|------|----------|
| Advances.Domain | Treasury | Scalar ID (intended) | `IssuingFinancialTransactionId`, `ReturnFinancialTransactionId` |
| Advances.Application | anything | **None** | no handlers |

### 2.15 Reporting

| From | To | Type | Evidence |
|------|-----|------|----------|
| Reporting.Application | CommercialDocuments.Domain | Reporting read access | Invoice |
| Reporting.Application | Parties.Domain | Reporting read access | Dealer |
| Reporting.Application | Treasury.Domain | Reporting read access | Financial, CashBox, FinancialType |
| Reporting.Application | Inventory.Domain | Reporting read access | TransactionProduct, TransactionType |
| Reporting.Application | Catalog.Domain | Reporting read access (transitive/include) | Product, Classification |
| Reporting.Application | MasterData.Domain | Reporting read access | Currency |

No Reporting.Contracts. No projection tables. No tenant/company scoping in queries beyond whatever filters the generic search already has.

### 2.16 Host / Shared persistence

Every module writes through the **same** `OrgContext` / generic `IRepository<T>`. Table ownership is conventional, not technically enforced. Cross-module `IRepository<Invoice>` in Treasury and Inventory is a **cross-module table read** (and potentially write, depending on handler).

---

## 3. Target edges that do not exist yet

Do not implement these in Stage 0. Listed so later stages do not invent a second map.

```text
Sales            -> CommercialDocuments.Contracts   (invoice from order)
Sales            -> Inventory.Contracts             (reserve / deliver)
Purchasing       -> Inventory.Contracts             (GRN / remaining qty)
Inventory        -> Purchasing.Contracts            (receipt against PO)
Advances         -> Treasury.Contracts              (disburse / return cash)
Workflow         -> *                               (does not exist)
Budgeting        -> Accounting.Contracts            (does not exist)
Tax              -> CommercialDocuments             (does not exist)
FixedAssets      -> Accounting.Contracts            (does not exist)
Identity         -> extracted from Administration   (not extracted)
SaaS             -> every module via ICurrentTenant (does not exist)
Reporting        -> read models                     (does not exist)
```

---

## 4. Allowed vs illegal (current tests)

Legal today: Contracts and Integration Events.

Illegal unless allow-listed: Domain→Domain, Application→foreign Domain, Application→foreign Application.

Infrastructure→Infrastructure: illegal, currently zero exceptions.

Shared-database Fluent FKs **without** Domain navigations are the accepted isolation pattern (Journal.CurrencyId, Invoice.DealerId, Company.TenantId, CashBox.KeeperUserId).
