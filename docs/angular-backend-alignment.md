# OrgSys Angular ↔ Backend Architecture Alignment

**Source of truth:** current repository source (not prior docs, not generic ERP assumptions).  
**Angular version:** 19.2, standalone components, lazy-loaded feature routes.  
**API style:** attribute-routed controllers at `/{Controller}/...` (no global `/api` prefix except `/api/Auth` and `/api/Main`).  
**Date of this pass:** 2026-09-16.

---

## 0. Safety baseline (before structural change)

| Check | Result |
|--------|--------|
| `dotnet restore` + `dotnet build OrgSys.sln` | **Succeeded** — 0 errors, **677 existing warnings** |
| `npx ng build --configuration=development` | **Succeeded** (~27.5s) |
| Angular unit tests | Only `src/app/app.component.spec.ts` exists; Karma not run as a gate (headless CI not configured as a required script) |
| Backend tests | Present under `Tests/*` — not treated as part of this Angular move; must not regress because of frontend-only changes |

Existing warnings/failures are **pre-migration**. Do not treat them as alignment regressions.

---

## 1. Current backend modules

Filesystem under `Modules/` (authoritative, 16 modules):

| Module | Domain | Application | Contracts | Infrastructure | UI candidate |
|--------|--------|-------------|-----------|----------------|--------------|
| Accounting | yes | yes | yes | yes | `features/accounting` |
| Administration | yes | yes | yes | yes | `features/administration` **only if Users/Roles UI exists** — it does **not** today |
| Advances | yes | stub | stub | yes | **no Angular UI** — domain-only (Custody) |
| Catalog | yes | yes | yes | yes | `features/catalog` |
| CommercialDocuments | yes | yes | yes | yes | `features/commercial-documents` |
| Inventory | yes | yes | yes | yes | `features/inventory` |
| MasterData | yes | yes | yes | yes | `features/master-data` |
| Organization | yes | yes | yes | yes | `features/organization` |
| Parties | yes | yes | yes | yes | `features/parties` |
| Payables | yes | yes | yes | yes | **API exists; no dedicated Angular screens** |
| Purchasing | yes | yes | empty | yes | **API exists; no Angular screens** |
| Receivables | yes | yes | yes | yes | **API exists; no dedicated Angular screens** |
| Reporting | no | yes | empty | yes | `features/reporting` |
| SaaS | yes | yes | yes | yes | **API exists; no Angular screens** |
| Sales | yes | stub (mapping only) | empty | yes | **no Application handlers / no Angular screens** |
| Treasury | yes | yes | yes | yes | `features/treasury` |

Frontend-only technical features (keep): `features/auth`, `features/dashboard`.

---

## 2. Current Angular features (before alignment)

Under `OrgSys.Angular/src/app/features/`:

| Feature | Routes | Meaning |
|---------|--------|---------|
| `auth` | `/login` | Authentication |
| `dashboard` | `/dashboard` | Home placeholder |
| `administration` | `/administration/*` | Mixed master data: geo, currency, fiscal years, branches, banks, products |
| `accounting` | `/accounting/*` | Chart of accounts, journals |
| `financial` | `/financial/*` | Unified financial accounts, treasury movements, transfers, opening balances |
| `customers-suppliers` | `/customers-suppliers/*` | Dealer + dealer group master (customers/suppliers) |
| `invoices` | `/invoices/:typeId` | Sales/purchase invoices & returns (CommercialDocuments) |
| `transactions` | `/transactions/:typeId` | **Inventory stock movements** (not treasury) |
| `inventory` | `/inventory` | Physical inventory count / adjustment document |
| `warehouse` | `/warehouse/*` | Locations, receipts, balances, reservations (Inventory BC) |
| `reports` | `/reports/*` | Read-model reports |

`core/` is technical (auth, guards, interceptors, `BaseApiService`, menu config).  
`shared/` is presentational (page-header, pagination, toast, confirm, tree-view) plus `BaseEntity` / `Status`.  
**No business HTTP services live in `core/` or `shared/`.** Cross-feature coupling is lookup services currently hosted under `invoices` and `reports`.

---

## 3. Backend context map (application capabilities)

### Accounting

**Capabilities:** Chart of accounts CRUD/activate; account types; journals CRUD/post/cancel/reverse/redo; fiscal years; close/reopen fiscal periods; cross-document posting contracts.  
**Entities:** Account, AccountType, FiscalYear, FiscalPeriod, Journal, JournalItem, JournalType.  
**Enums:** `FiscalYearStatus` (Open, Closed); `FiscalPeriodStatus` (Open, Closed, Locked). Shared `OrgSys.SharedKernel.Status` for documents.  
**Endpoints:** `/Account`, `/AccountType`, `/Journal` (+ Post/Cancel/Redo/Reverse), `/JournalType`, `/FiscalYear`.  
**Permissions (seed):** `Accounts.*`, `Journal.*`, `FiscalYears.*`.  
**Frontend:** `features/accounting`.

### Administration

**Capabilities:** Users, roles, preferences, login, permission resolution.  
**Entities:** User, Role, Permission, RolePermission, Preference, Notification, LogSys.  
**Endpoints:** `/api/Auth/*`, `/User`, `/Role`, `/Preference`.  
**Frontend:** **no Users/Roles/Preferences screens today.** After this pass Administration has no remaining Angular pages. Remaining debt, not an empty placeholder.

### Advances

**Capabilities:** Custody aggregate + domain events. **No commands/queries/API.**  
**Frontend:** do **not** create `features/advances`.

### Catalog

**Capabilities:** Products, brands, units, classifications, properties, product units, price lists; `ResolvePriceQuery`.  
**Endpoints:** `/Product`, `/Brand`, `/Unit`, `/Classification`, `/Property`, `/ProductUnitModelView`, `/PriceList`, `/Pricing/ResolvePrice`.  
**Frontend:** `features/catalog` (products UI exists; other catalog screens do not).

### CommercialDocuments

**Capabilities:** Invoice CRUD/cancel/redo; invoice types; journal-by-invoice; integration events `SalesInvoicePosted` / `PurchaseInvoicePosted`.  
**Entities:** Invoice, InvoiceProduct, InvoiceType.  
**Endpoints:** `/Invoice`, `/InvoiceType`.  
**Ownership test:** Invoice aggregate lives in CommercialDocuments.Domain; Sales/Purchasing do **not** own invoice screens.  
**Frontend:** `features/commercial-documents`.

### Inventory

**Capabilities:** Stocks, legacy `Transaction` movements, physical `Inventory` counts, receipts/issues/adjustments/transfers, batches/serials, reservations, warehouse locations, balances/availability.  
**Endpoints:** `/Stock`, `/Transaction`, `/TransactionType`, `/Inventory`, plus REST `/InventoryReceipts`, `/InventoryIssues`, `/StockTransfers`, `/StockAdjustments`, `/StockReservations`, `/InventoryBalances`, `/WarehouseLocations`, `/InventoryBatches`, `/InventorySerials`.  
**Frontend:** `features/inventory` (merge legacy `warehouse` + stock `transactions`).

### MasterData

**Capabilities:** Country, City, District, Currency, PaymentType, ReferenceType.  
**Endpoints:** `/Country`, `/City`, `/District`, `/Currency`, `/PaymentType`, `/ReferenceType`.  
**Frontend:** `features/master-data`.

### Organization

**Capabilities:** Company, Branch, Shift, Table, OrganizationSettings.  
**Endpoints:** `/Company`, `/Branch`, `/Shift`, `/Table`, `/OrganizationSettings`.  
**Frontend:** `features/organization` (branches UI exists; company/settings screens do not).

### Parties

**Capabilities:** Dealer, DealerGroup, PartyAddress, PartyContact, CustomerProfile, SupplierProfile, assign customer/supplier role.  
**Enums:** `DealerType` (Client=1, Supplier=2).  
**Endpoints:** `/Dealer`, `/DealerGroup`, `/PartyContact`, `/PartyAddress`, `/CustomerProfile`, `/SupplierProfile`.  
**Frontend:** `features/parties`. Customer/supplier **identity** stays here — not Receivables/Payables.

### Payables / Receivables

**Capabilities:** Opening balance commands; balance/aging/outstanding/overdue/subledger/reconciliation **queries**. Integration handlers for invoice posted + payment posted events.  
**Endpoints:** `/Payable/*`, `/Receivable/*`, plus `POST /Financial/Receivable/OpeningBalance`.  
**Frontend:** **no dedicated screens.** Dealer balance/statement reports stay in Reporting (read model). Opening-balance **financial account** screen is Treasury (`PostFinancialOpeningBalanceCommand`), not AR/AP.

### Purchasing

**Capabilities:** Purchase requisition + purchase order lifecycle (submit/reject/cancel/convert/link invoice).  
**Endpoints:** `/PurchaseRequisition`, `/PurchaseOrder`.  
**Frontend:** **no screens** — do not invent.

### Reporting

**Capabilities:** Read-only dealer/safe/warehouse/sales reports.  
**Endpoints:** `/DealerReport`, `/FinancialReport`, `/WarehouseReport`, `/SalesReport`.  
**Frontend:** `features/reporting`.

### SaaS

**Capabilities:** Tenant/plan/feature/subscription.  
**Endpoints:** `/Tenant`, `/Plan`, `/Feature`, `/Subscription`.  
**Frontend:** **no screens** — platform admin, not tenant ERP.

### Sales

**Capabilities:** Quotation + SalesOrder **domain only**; Application layer has no handlers.  
**Frontend:** do **not** create `features/sales`. Invoices are CommercialDocuments.

### Treasury

**Capabilities:** FinancialAccount (cash+bank), Financial movements, FinancialTransfer, CashBox, Bank, BankBranch, BankAccount, FinancialType, Outlay; post/cancel/reverse/redo; payment integration events.  
**Enums:** `FinancialTransactionType` 1–11 (OpeningBalance…TransferOut); `FinancialTransactionDirection` In=1, Out=2; `FinancialReferenceType` 0–11; `FinancialAccountType`.  
**Endpoints:** `/FinancialAccount`, `/Financial`, `/FinancialTransfer`, `/CashBox`, `/Bank`, `/BankBranch`, `/FinancialType`, `/Outlay`.  
**Frontend:** `features/treasury`.

---

## 4. API inventory (summary)

Full CRUD controllers inherit `BaseController`: `GetById`, `GetList`, `Search`, `Create`, `Update`, `Delete`, `DeleteList` (+ `GetMax` where declared). Auth is `[Authorize]` on BaseController; **no per-action permission attributes**. Granular keys exist only in Administration seed + login payload; Angular uses them for UX.

Notable custom endpoints used by current Angular:

| Method | Route | Module |
|--------|-------|--------|
| POST | `/api/Auth/login` | Administration |
| PUT | `/Journal/Post\|Cancel\|Redo\|Reverse` | Accounting |
| POST | `/Financial/Transactions/Post` | Treasury |
| PUT | `/Financial/Cancel\|Redo\|Reverse\|Post` | Treasury |
| * | `/FinancialTransfer/*` | Treasury |
| GET | `/Dealer/Balance` | Parties |
| PUT | `/Invoice/Cancel\|Redo` | CommercialDocuments |
| PUT | `/Transaction/Cancel\|Redo` | Inventory |
| POST | `/Transaction/CreateReceived` | Inventory |
| PUT | `/Inventory/Cancel\|Redo` | Inventory |
| POST | `/Inventory/CreateAdjustment` | Inventory |
| GET | `/Product/GetAllByBalance` | Inventory query on Product controller |
| * | `/InventoryReceipts`, `/InventoryBalances`, `/StockReservations`, `/WarehouseLocations` | Inventory |
| GET | `/DealerReport/*`, `/FinancialReport/*`, `/WarehouseReport/*`, `/SalesReport/*` | Reporting |

**Anonymous (no `[Authorize]`):** `PayableController`, `ReceivableController`, `FinancialTransferController` — backend gap / inconsistency; not changed in this Angular pass.

---

## 5. Angular → backend mapping (BEFORE → ACTION)

| CURRENT ANGULAR | BUSINESS MEANING | BACKEND OWNER | TARGET | ACTION |
|-----------------|------------------|---------------|--------|--------|
| `features/auth` | Login | Administration Auth | `features/auth` | KEEP |
| `features/dashboard` | Home | n/a | `features/dashboard` | KEEP |
| `administration/countries\|cities\|districts\|currencies` | Geo + currency | MasterData | `features/master-data` | MOVE |
| `administration/fiscal-years` | Fiscal years | Accounting | `features/accounting/fiscal-years` | MOVE |
| `administration/branches` | Branches | Organization | `features/organization/branches` | MOVE |
| `administration/banks\|bank-branches` | Bank master | Treasury | `features/treasury/banks` | MOVE |
| `administration/products` | Product master | Catalog | `features/catalog/products` | MOVE |
| `features/accounting` | COA + journals | Accounting | same | KEEP + add fiscal years |
| `features/financial` | Cash/bank + movements | Treasury | `features/treasury` | RENAME/MOVE |
| `features/customers-suppliers` | Party master | Parties | `features/parties` | RENAME/MOVE |
| `features/invoices` | Invoice documents | CommercialDocuments | `features/commercial-documents` | RENAME/MOVE |
| `features/transactions` | Stock movements | Inventory | `features/inventory` (stock movements) | MOVE/MERGE |
| `features/inventory` | Stock count | Inventory | `features/inventory` (count) | KEEP (re-route) |
| `features/warehouse` | Locations/receipts/balances/reservations | Inventory | `features/inventory` | MERGE |
| `features/reports` | Read reports | Reporting | `features/reporting` | RENAME/MOVE |
| `invoices/.../invoice-lookups.service` | PaymentType, Stock, Unit, Product, InvoiceType | split owners | owning feature `data-access` | SPLIT |
| `reports/.../report-lookups` Classification | Catalog | `catalog` | MOVE |
| `reports/.../CashBoxService` | Legacy cash-box lookup | Treasury (`CashBox` API still exists) | stay as reporting lookup | KEEP (legacy API) |

**Not created (no UI and/or no Application API):** `advances`, `sales`, `purchasing`, `receivables`, `payables`, `saas`, empty `administration`.

---

## 6. Migration stages

1. **Wave 1 — Foundation:** this document; permission key constants; enum alignment (`FinancialReferenceType`); keep auth/layout/BaseApiService.  
2. **Wave 2 — Easy 1:1:** Accounting stays; reports → reporting; inventory absorbs warehouse + stock transactions.  
3. **Wave 3 — Financial split:** financial → treasury (banks included). Advances/AR/AP have no extra screens to split.  
4. **Wave 4 — Parties:** customers-suppliers → parties.  
5. **Wave 5 — Commercial:** invoices → commercial-documents. Sales/Purchasing remain backend-only.  
6. **Wave 6 — Catalog:** products → catalog; lookup split (Product/Unit/Classification).  
7. **Wave 7 — Platform:** master-data + organization; administration folder removed once empty.  
8. **Wave 8 — Cleanup:** legacy redirects; delete empty folders; build.

---

## 7. Known legacy areas

- Dual inventory stacks: legacy `/Transaction` + `/Inventory` **and** new receipts/reservations/locations. Both are Inventory-owned; both remain in UI.  
- Reports still filter safes via `/CashBox` while transactional UI uses `/FinancialAccount`.  
- Duplicate `ProductService` (admin CRUD vs invoice lookup) — **merged** onto Catalog `ProductService` (`getAllByBalance` remains Inventory query exposed on `/Product`).  
- `permissionGuard` defined but unused on routes (menu + button gating only).  
- Payable/Receivable/FinancialTransfer controllers lack `[Authorize]`.  
- Seeded permission keys keep historical spelling (`Countrys`, `Citys`, `Branchs`, `BankBranchs`). Angular must match seed keys, not “correct” English.

---

## 8. Identified API mismatches

| Angular call | Backend | Notes |
|--------------|---------|-------|
| `CashBox` lookup on safe reports | `/CashBox` still exists | Works; not aligned with unified FinancialAccount UI |
| Invoice lookup `ProductService` vs Catalog `ProductService` | both `/Product` | **Resolved** — single Catalog `ProductService` |
| No Angular client for `/Payable`, `/Receivable` | APIs exist | BACKEND present, **UI gap** |
| No Angular client for `/PurchaseOrder`, `/PurchaseRequisition` | APIs exist | UI gap |
| No Angular client for `/Tenant`, `/Plan`, `/Subscription` | APIs exist | UI gap (SaaS) |
| No Angular client for `/Company`, `/OrganizationSettings` | APIs exist | UI gap |
| Advances custody | no module API | BACKEND GAP |
| Sales quotation/order | no Application API | BACKEND GAP |

Do **not** invent Angular HTTP methods for missing APIs.

---

## 9. Unresolved / deferred ownership

| Topic | Decision from code | Frontend action |
|-------|--------------------|-----------------|
| FiscalYear | Accounting.Domain | Move UI to accounting |
| Bank / BankBranch | Treasury.Domain | Move UI to treasury |
| Invoice | CommercialDocuments | Not Sales, not Purchasing |
| Dealer | Parties | Not Receivables |
| Financial opening balance screen | Treasury `Financial` + `PostFinancialOpeningBalance` | Stay treasury |
| Customer/supplier opening-balance APIs | Receivables/Payables | No Angular screen to move |
| Warehouse | Inventory aggregate/sub-feature | Merge into inventory |
| Users/Roles | Administration | No UI — remaining debt |

---

## 10. Target routes (with compatibility redirects)

| New primary | Legacy redirect |
|-------------|-----------------|
| `/master-data/countries` etc. | `/administration/countries` etc. |
| `/accounting/fiscal-years` | `/administration/fiscal-years` |
| `/organization/branches` | `/administration/branches` |
| `/treasury/banks`, `/treasury/bank-branches` | `/administration/banks` … |
| `/catalog/products` | `/administration/products` |
| `/treasury/...` | `/financial/...` |
| `/parties/...` | `/customers-suppliers/...` |
| `/commercial-documents/sales-invoices` etc. | `/invoices/:typeId` |
| `/inventory/count` | `/inventory` (old count root) |
| `/inventory/movements/{named}` | `/transactions/:typeId` |
| `/inventory/locations\|receipts\|balances\|reservations` | `/warehouse/...` |
| `/reporting/...` | `/reports/...` |

Named treasury movement URLs: `receipts`, `payments`, `transfer-in`, `deposits`, `withdrawals`, `fees`, `interest`, `cheques`, `adjustments`, `transfer-out` (FinancialTransactionType 2–11).

---

## 11. Migration log (updated as groups move)

Status values: Planned → Migrated.

| OLD | NEW | Backend owner | Endpoint | Status |
|-----|-----|---------------|----------|--------|
| `features/financial` | `features/treasury` | Treasury | `/Financial`, `/FinancialAccount`, `/FinancialTransfer` | **Migrated** |
| `features/customers-suppliers` | `features/parties` | Parties | `/Dealer`, `/DealerGroup` | **Migrated** |
| `features/invoices` | `features/commercial-documents` | CommercialDocuments | `/Invoice` | **Migrated** |
| `features/reports` | `features/reporting` | Reporting | `*Report` | **Migrated** |
| `features/warehouse` | `features/inventory` (locations/receipts/balances/reservations) | Inventory | `/WarehouseLocations`, `/InventoryReceipts`, `/InventoryBalances`, `/StockReservations` | **Migrated** |
| `features/transactions` | `features/inventory` (movements) | Inventory | `/Transaction` | **Migrated** |
| `administration/products` | `catalog/products` | Catalog | `/Product` | **Migrated** |
| `administration/countries…currencies` | `master-data` | MasterData | `/Country` … `/Currency` | **Migrated** |
| `administration/branches` | `organization/branches` | Organization | `/Branch` | **Migrated** |
| `administration/fiscal-years` | `accounting/fiscal-years` | Accounting | `/FiscalYear` | **Migrated** |
| `administration/banks` | `treasury/banks` | Treasury | `/Bank`, `/BankBranch` | **Migrated** |

Legacy folders `financial`, `customers-suppliers`, `invoices`, `reports`, `warehouse`, `transactions`, `administration` have no remaining page/service consumers. Compatibility redirects remain in `app.routes.ts`. Empty leftover feature directories were removed 2026-09-16.

---

## 12. Permissions

Backend seed (`AdministrationDataSeeder`) is the key vocabulary. Angular `PermissionService` reads `AuthUser.permissions[].key` from login. Route protection remains `authGuard` on the shell; button/menu gating uses the same seed keys via `core/permissions/permission-keys.ts`. Frontend permissions are UX only. `permissionGuard` remains available but unused on routes (same as before this pass).

---

## 13. Tenant / company / branch

Angular does **not** send `TenantId` on each request. JWT + `[Authorize]` is the current pattern. `branchId` appears on some command DTOs (e.g. post financial transaction) from the form/session, matching existing screens. No new TenantId fields were added.

---

## 14. Post-migration feature tree

Implemented Angular features (no empty placeholders):

```
features/
  auth/
  dashboard/
  accounting/          (+ fiscal-years from Administration)
  catalog/             (products + unit/classification lookups)
  commercial-documents/
  inventory/           (count + movements + locations/receipts/balances/reservations)
  master-data/         (countries, cities, districts, currencies + payment-type lookup)
  organization/        (branches)
  parties/
  reporting/
  treasury/            (financial accounts, movements, transfers, opening balances, banks)
```

Not created (no UI and/or no Application API): `advances`, `sales`, `purchasing`, `receivables`, `payables`, `saas`, empty `administration`.

Primary routes use business language (`/treasury/receipts`, `/inventory/movements/addition`, `/commercial-documents/sales-invoices`, …). Legacy URLs redirect.

Lookups cross context via public barrels (`catalog/index.ts`, `parties/index.ts`, `inventory/index.ts`, `master-data/index.ts`) — not via page internals.

`FinancialReferenceType` in Angular now matches `Treasury.Domain` exactly (0–11). Duplicate `ProductService` clients were merged onto Catalog.

## 15. Post-migration build

| Check | Result |
|--------|--------|
| Angular `ng build --configuration=development` after alignment | **Succeeded** |
| Backend `dotnet build OrgSys.sln` | Baseline succeeded (0 errors, 677 existing warnings); **no backend files modified** |
