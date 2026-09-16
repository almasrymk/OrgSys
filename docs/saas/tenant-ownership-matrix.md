# Tenant ownership matrix

OrgSys does **not** stamp `TenantId` on every table. Isolation is company-rooted: `Company.TenantId` is the only operational TenantId on transactional master data. Users, branches, and warehouses resolve tenant through that graph. `ICurrentTenant` is request-scoped from JWT (`tenantId` / `companyId` / `branchId`).

Generated from `OrgContextModelSnapshot` (2026-09-17, after Fixed Assets).

| Table | TenantId column | Isolation path | Notes |
|-------|-----------------|----------------|-------|
| Tenant | PK | SaaS owner | Plan/subscription root |
| Plan | none | SaaS catalog | Shared catalog |
| Feature | none | SaaS catalog | Shared catalog |
| PlanFeature | none | SaaS catalog | Shared catalog |
| Subscription | TenantId | direct | SaaS |
| Company | TenantId | **root** | Stage 1 retrofit; nullable then backfilled |
| Branch | none | CompanyId → Company.TenantId | Required CompanyId |
| Department | none | CompanyId → Company.TenantId | Organization |
| OrganizationSettings | none | CompanyId | Organization |
| User | none | BranchId → Branch.CompanyId → Company.TenantId | Administration IAM |
| Role | none | shared / company practice | Not tenant-stamped |
| Permission | none | shared catalog | |
| RolePermission | none | via Role | |
| Preference | none | shared / type | |
| Shift | none | typically Branch | |
| Table | none | typically Branch | |
| Stock | none | BranchId → Company.TenantId | Warehouse limit uses this path |
| WarehouseLocation | none | Stock | Inventory |
| Inventory / InventoryBalance / InventoryBatch / InventoryCostLayer / InventorySerial / InventoryProduct | none | Stock | Inventory |
| InventoryReceipt / Issue / StockTransfer / StockAdjustment (+ lines) / StockReservation | none | Stock / Branch | Inventory |
| StockAdjustmentReason | none | catalog | |
| Product / ProductUnit / ProductPropertyElement / ProductRecipe | none | catalog (company practice) | Catalog |
| Brand / Classification / PriceList / PriceListEntry / Property / PropertyElement / Unit | none | catalog | Catalog |
| Dealer / DealerGroup / PartyContact / PartyAddress / CustomerProfile / SupplierProfile | none | typically Company/Branch practice | Parties |
| Invoice / InvoiceProduct / InvoiceType | none | BranchId on movement | CommercialDocuments |
| InvoiceTaxSnapshot / InvoiceTaxSnapshotLine | none | InvoiceId | Tax |
| SalesQuotation / SalesQuotationLine / SalesOrder / SalesOrderLine | none | BranchId | Sales |
| PurchaseRequisition / PurchaseOrder (+ products) | none | BranchId | Purchasing |
| Receivable / PaymentApplication (+ lines) | none | via invoice/customer | Receivables |
| Payable / SupplierPaymentApplication (+ lines) | none | via invoice/supplier | Payables |
| Custody / CustodyHandover | none | holder / branch | Advances |
| FinancialAccount / Financial / FinancialTransfer / FinancialInvoice / CashBox / BankAccount | none | BranchId / FinancialAccount | Treasury |
| Bank / BankBranch | none | master | Treasury |
| FinancialType / Outlay | none | lookup | Treasury |
| Account / AccountType / Journal / JournalItem / JournalType / FiscalYear / FiscalPeriod | none | company GL practice | Accounting — not stamped |
| Budget / BudgetLine | none | DepartmentId / FiscalYearId | Budgeting |
| FixedAsset / FixedAssetCategory / DepreciationEntry | none | BranchId / Category | FixedAssets |
| ApprovalRequest / ApprovalDecision | none | document id | Workflow |
| Currency / Country / City / District / PaymentType / ReferenceType / TransactionType / InvoiceType | none | MasterData shared | |
| Transaction / TransactionProduct | none | Stock / Branch | Inventory movements |

## Enforcement (Stage 12)

| Create | Limit | Cross-tenant guard |
|--------|-------|--------------------|
| Company | `TenantLimit.Companies` | `ICurrentTenant.TenantId` overwrites DTO TenantId |
| Branch | `TenantLimit.Branches` | Company must belong to current tenant |
| User | `TenantLimit.Users` | Branch must belong to current tenant |
| Stock | `TenantLimit.Warehouses` | Branch must belong to current tenant |

Company Get/Update/Delete: Tenant A cannot read, update, or delete Tenant B's company (NotFound / tenant-scoped delete filter).

---

## Stage 12 — SaaS / multi-tenant enforcement

**Date:** 2026-09-17  
**Status:** complete  
**HEAD at start:** post-Stage 11 working tree

### 1. What was found

`Company.TenantId` exists. `ITenantFeatureService` unused outside SaaS. No `ICurrentTenant`. JWT had branchId only.

### 2. What was changed

`ICurrentTenant` from JWT `tenantId`/`companyId`/`branchId`. Login resolves ownership via `GetBranchOwnershipQuery`. Plan limits on Create Company/Branch/User/Stock. Company Get/Update/Delete isolated by tenant. Matrix: `docs/saas/tenant-ownership-matrix.md`. No TenantId stamp on every table.

### 9. Tests

`TenantIsolationTests` (4). Application.Tests 140. Architecture 2595.

### 13. Remaining

Stage 13 Outbox/Inbox.

### 14. Architecture exceptions remaining

0 / 0 / 0.

---