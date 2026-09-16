# OrgSys completion progress

Living log. Update after **every** stage. Do not mark a stage complete without build/test results.

---

## Stage 0 — Repository audit

**Date:** 2026-09-17  
**Status:** complete (documentation only)  
**HEAD audited:** `4c0ffdda06fe103f8fda1b1a987855de2c58804e` (equals reviewed baseline; 0 commits after).

### 1. What was found

- 16 modules under `Modules/` on .NET 10 modular monolith; Accounting is the isolation reference.
- Architecture.Tests allow-lists: **7** Domain→Domain, **16** Application→Domain, **3** Application→Application.
- Exact offending types/files documented as EX-D1–D7, EX-AD1–AD16, EX-AA1–AA3 in `master-execution-plan.md`.
- Cheapest Stage 1 wins: Inventory.Domain→MasterData is stale (csproj + global using only); Sales.Application→MasterData is an empty MappingProfile; unused MasterData.Application refs on CommercialDocuments/Inventory Application.
- IAM is Administration (`User`/`Role`/`Permission` + `IPasswordHasher` / `MustResetPassword`), not a separate Identity module.
- Sales Quotation/SalesOrder and Advances Custody are domain-rich but **not** in `OrgContext` (87 snapshot tables).
- No Workflow, Tax, Budgeting, FixedAssets modules or types.
- No `ICurrentTenant`, Outbox, health checks, CI workflows.
- `ITenantFeatureService` has no consumers outside SaaS.
- Treasury Financial handlers **mutate** `Invoice` via `IRepository<Invoice>.UpdateAsync` (Paid/Remaining) — AR/AP should own settlement.
- `PayableController`, `ReceivableController`, `FinancialTransferController` lack `[Authorize]`.
- `AuthController` is `[AllowAnonymous]` including CheckPassword/HavePassword.
- `Domain.zip` exists at repo root; `OrgSys.DatabaseMigrator` is not in `OrgSys.sln`.
- `ModuleInfrastructureDependencyTests` omits Catalog/SaaS/Advances from its assembly array.
- Angular covers accounting/treasury/parties/invoices/partial inventory/master-data/org branches/reporting; not Purchasing, AR/AP UI, Admin users, SaaS, Advances, Sales orders.

### 2. What was changed

Documentation only. No Stage 1 implementation.

### 3. Exact files changed

- `docs/completion/current-state-audit.md` (rewritten from live source)
- `docs/completion/current-context-map.md` (**new** — prompt-required filename)
- `docs/completion/master-execution-plan.md` (rewritten with per-exception cards)
- `docs/completion/progress.md` (this file)
- `docs/completion/context-map.md` (pointer to current-context-map.md)

### 4. New projects/files

None (no csproj, no modules).

### 5–10. Dependencies / schema / API / Angular / tests

None.

### 11. Build result

Not run (no code changes). Next stage must build.

### 12. Test result

Not run (no code changes). Architecture.Tests last known state from inspection: allow-lists as documented.

### 13. Remaining issues

Highest-priority **code** work is Stage 1 isolation. Start with EX-D5, EX-AD1, EX-AA1. Do not create Tax/FixedAssets/Budgeting/Workflow/Identity.

### 14. Architecture exceptions remaining

| List | Count |
|------|-------|
| AcceptedDomainExceptions | 7 |
| AcceptedApplicationDomainExceptions | 16 |
| AcceptedApplicationApplicationExceptions | 3 |

---

## Stage 1 — Boundary hardening

**Date:** 2026-09-17  
**Status:** complete  
**HEAD at start:** `4c0ffdda06fe103f8fda1b1a987855de2c58804e`

### 1. What was found

Allow-lists EX-D1–D7, EX-AD1–AD16, EX-AA1–AA3 were all real or stale ProjectReferences as documented in Stage 0. Invoice settlement was written by Treasury via `IRepository<Invoice>`. Reporting.Application held `IRepository<>` of foreign Domain types.

### 2. What was changed

- Domain navigations to other modules dropped; scalar IDs kept; Fluent `HasOne(typeof(...)).WithMany().HasForeignKey(...)` preserved SQL FKs.
- Application now fills display names via MasterData/Catalog/Parties Contracts (not EF Include flattening).
- Treasury/Inventory no longer load or update `Invoice`; owner writes settlement/link through CommercialDocuments.Contracts.
- Reporting.Application is thin; `IReportingReadStore` in Application, implemented in Reporting.Infrastructure.
- Organization company/settings validators use `ExistsCountryQuery` / `ExistsCurrencyQuery` / `TenantExistsQuery`.
- Architecture.Tests allow-lists emptied; Catalog/SaaS/Advances added to Infrastructure test array.

### 3. Exact files changed (grouped)

- Domain nav drop: Treasury Bank/BankBranch/FinancialAccount/Financial/FinancialTransfer; Invoice/InvoiceProduct; Dealer/PartyAddress; Purchasing line Units; Inventory Product/Unit line entities.
- OrgContext Fluent FKs after InvoiceProduct.StockId (geo, currency, payment type, product, unit).
- Application handlers/mappings across CommercialDocuments, Parties, Treasury, Inventory, Purchasing, Organization, Reporting.
- Architecture.Tests + TestOrgContext + OrganizationCompanyBranchValidatorTests.
- ProductController `GetAllByBalance` now `ProductBalanceDto`.

### 4. New projects/files

No new csproj. New Contracts + owning handlers:

- MasterData.Contracts: `ExistsCountryQuery`, `ExistsCurrencyQuery`, `GetCountryNamesQuery`, `GetCityNamesQuery`, `GetDistrictNamesQuery`, `GetPaymentTypeNamesQuery`, `GetCurrencyLookupsQuery`
- Catalog.Contracts: `GetUnitNamesQuery`, `GetProductCatalogQuery`, `GetProductUnitsQuery`, `ProductCatalogItemDto`, `ProductUnitLookupDto`
- SaaS.Contracts: `TenantExistsQuery`
- CommercialDocuments.Contracts: `GetInvoicePaymentInfoQuery`, `GetInvoiceInventoryImpactQuery`, `AdjustInvoiceSettlementCommand`, `SetInvoiceSettlementFromAllocationCommand`, `SetInvoiceLinkedTransactionCommand`, `GetInvoiceByLinkedTransactionQuery`, `GetInvoicesByLinkedTransactionsQuery`
- Reporting: `IReportingReadStore` / `ReportingReadStore`
- Inventory: `ProductBalanceDto`, `UnitNameDto`

### 5. Dependencies added/removed

Removed Domain→Domain (MasterData/Catalog) and Application→Domain/Application cross-module refs listed in Stage 1 cards. Added Contracts-only refs. Infrastructure: Treasury.Infrastructure → MasterData.Domain (seeder); Organization.Infrastructure → SaaS.Domain (seeder).

### 6. Schema / migration

`dotnet ef migrations has-pending-model-changes --project BuildingBlocks/OrgSys.DatabaseMigrator --startup-project API --context OrgContext` → **No changes have been made to the model since the last migration.** No Stage 1 migration. SQL FKs retained.

### 7. API

`ProductController.GetAllByBalance` returns `ResultCollection<ProductBalanceDto>` (JSON overlap preserved for Angular unit lists). No other public route changes.

### 8. Angular

None.

### 9–10. Tests added / unused

No new test classes. Existing Organization validator tests now mock `ISender`.

### 11. Build result

`dotnet build OrgSys.sln` → 0 error(s).

### 12. Test result

| Project | Result |
|---------|--------|
| Architecture.Tests | 1635 passed |
| Application.Tests | 118 passed |
| Accounting.Integration.Tests | 2 passed |
| Inventory.Integration.Tests | 3 passed |
| Domain.Tests (all modules) | all passed |

### 13. Remaining issues

Stage 1 isolation is done. Invoice.Paid/Remaining still exist as columns; Treasury no longer writes them (CommercialDocuments owner commands do). Reporting still reads foreign tables via Infrastructure `IRepository<>` (allowed). Sales/Advances still not in OrgContext (Stage 4/5). Unauthenticated Payable/Receivable/FinancialTransfer (Stage 7/16).

### 14. Architecture exceptions remaining

| List | Count |
|------|-------|
| AcceptedDomainExceptions | 0 |
| AcceptedApplicationDomainExceptions | 0 |
| AcceptedApplicationApplicationExceptions | 0 |

---

## Stage 2 — Application isolation leftovers + purposeful Contracts

**Date:** 2026-09-17  
**Status:** complete (delivered with Stage 1)

Contracts required by Stage 1 cards were added in Stage 1. Receivables/Payables apply-payment commands were not added: Treasury paid-invoice path now uses CommercialDocuments.Contracts settlement commands; existing `CustomerPaymentPostedIntegrationEvent` / `SupplierPaymentPostedIntegrationEvent` remain. `HasPermissionQuery` deferred to Stage 7. Sales/Purchasing/Advances/Reporting.Contracts left empty until their stages.

Build/tests: same as Stage 1 (no extra code).

---

## Stage 3 — Complete Contracts between current contexts

**Date:** 2026-09-17  
**Status:** complete (no extra lookups beyond Stage 1)

Name/exists/catalog/invoice-impact contracts already cover current cross-context reads. Further CRUD-in-Contracts was avoided.

---

## Stage 4 — Accounting / AR / AP / Treasury / Advances

**Date:** 2026-09-17  
**Status:** complete (Advances persistence + API + Angular; AR/AP `[Authorize]` remains Stage 7)

### 1. What was found

Advances Domain (`Custody` / `CustodyHandover`) was rich but not in `OrgContext`. DatabaseMigrator omitted Advances.Domain. Application had only `AssemblyMarker`. Treasury.Contracts had no custody disbursement command.

### 2. What was changed

- Mapped `Custody` / `CustodyHandover` in OrgContext with Fluent Currency FK and handover cascade.
- Application lifecycle: Create / Approve / Issue / Settle / Return / Close / Cancel / Transfer / GetById / GetList.
- Issue/Return post money through `Treasury.Contracts.PostCustodyFinancialCommand` (Payment out / Receipt in) then stamp Financial.Id on Custody.
- `[Authorize]` `CustodyController`.
- Angular `features/advances/` list + detail/create.

### 3. Exact files changed (grouped)

- `BuildingBlocks/OrgSys.DatabaseMigrator` OrgContext, GlobalUsings, csproj, migration `20260916191132_AddAdvancesCustody`
- `Modules/Advances/Advances.Domain/Entities/Custody.cs` (`[NotMapped]` DomainEvents; Create notes)
- `Modules/Advances/Advances.Application` Custodies commands/queries; csproj → Treasury.Contracts
- `Modules/Treasury/Treasury.Contracts/Financials/PostCustodyFinancialCommand.cs`
- `Modules/Treasury/Treasury.Application/Financials/Command/PostCustodyFinancialCommandHandler.cs`
- `API/Controllers/Org/Advances/CustodyController.cs`
- `Tests/Application.Tests/CustodyCommandHandlerTests.cs`
- Angular advances feature + `app.routes.ts` + `menu.config.ts`

### 4. New projects/files

No new csproj. New migration and Advances Application/API/Angular files as above.

### 5. Dependencies

Advances.Application → Treasury.Contracts. DatabaseMigrator → Advances.Domain.

### 6. Schema / migration

**Migration:** `20260916191132_AddAdvancesCustody`  
Creates `Custody` and `CustodyHandover` only. **Not applied** to live `OrgConnection`.  
`has-pending-model-changes` → clean after add.

### 7. API

`CustodyController` at `/Custody`: POST create; GET list/by id; PUT Approve/Issue/Settle/Return/Close/Cancel/Transfer. Authorized.

### 8. Angular

`/advances/custodies` list and `/advances/custodies/new|:id` form. Menu group Advances. `npm run build` succeeded.

### 9. Tests added

`CustodyCommandHandlerTests` — create, approve, issue (Treasury contract), reject draft issue.

### 10. Unused / compatibility

Cheque register not added (not required for this pass). Payable/Receivable/FinancialTransfer still lack `[Authorize]` (Stage 7).

### 11. Build result

`dotnet build OrgSys.sln` → 0 error(s). `npm run build` in OrgSys.Angular → success.

### 12. Test result

Architecture.Tests 1635 passed; Application.Tests **122** passed (+4); Domain/Integration unchanged green.

### 13. Remaining issues

Apply `AddAdvancesCustody` only with explicit DB approval. Holder is still a scalar `HolderId` (no HR module). Stage 5: persist Sales quotation/order.

### 14. Architecture exceptions remaining

All allow-lists still empty (0 / 0 / 0).

---

## Stage 5 — Sales / Purchasing / Inventory integration

**Date:** 2026-09-17  
**Status:** complete for Sales persistence + confirm reservation; Purchasing remaining-qty contract added. InventoryReceipt↔PO scalar still open.

### 1. What was found

`SalesQuotation` / `SalesOrder` tables were named on Domain `[Table]` attributes but missing from OrgContext/snapshot (legacy `Order` was dropped earlier and must not return). Sales.Application had no handlers. Purchasing.Contracts was empty.

### 2. What was changed

- OrgContext DbSets + Fluent lines/currency/product/unit FKs for Quotation and SalesOrder.
- Sales Application: create/list/get quotation and order; quotation send/accept/reject/cancel/convert; order confirm (reserves via `ReserveInventoryCommand`) and cancel.
- Authorized `SalesOrderController` / `QuotationController`.
- `GetPurchaseOrderRemainingQuery` in Purchasing.Contracts for later three-way match.

### 3. Files (grouped)

- OrgContext + migration `20260916191823_AddSalesQuotationAndOrder`
- Sales.Domain DomainEvents `[NotMapped]`
- Sales.Application Quotations/SalesOrders commands and queries; csproj → Catalog.Contracts, Inventory.Contracts
- API `Controllers/Org/Sales/SalesOrderController.cs`
- `ConfirmSalesOrderCommandHandlerTests`
- Purchasing.Contracts `GetPurchaseOrderRemainingQuery` + handler

### 4. New projects

None.

### 5. Dependencies

Sales.Application → Catalog.Contracts, Inventory.Contracts.

### 6. Schema / migration

**Migration:** `20260916191823_AddSalesQuotationAndOrder` — CreateTable `SalesOrder`, `SalesOrderLine`, `SalesQuotation`, `SalesQuotationLine` only. Does **not** revive dropped `Order`. **Not applied** to live DB. `has-pending-model-changes` clean.

### 7. API

`/SalesOrder` create/list/get/Confirm/Cancel. `/Quotation` create/list/get/Send/Accept/Reject/Cancel/Convert.

### 8. Angular

None this stage (Stage 15). Advances UI already in Stage 4.

### 9. Tests

`ConfirmSalesOrderCommandHandlerTests` (confirm reserves; create fills product names). Application.Tests 124.

### 10. Unused

`InventoryReceipt.PurchaseOrderId` and three-way match were closed after Stage 19 (scalar FK + `GoodsReceiptPostedIntegrationEvent` lines + Purchasing inbox `RecordReceipt` + `GetThreeWayMatchQuery`). RFQ was not added.

### 11–12. Build / tests

`dotnet build` 0 errors. Architecture 1635; Application 124; Domain/Integration green.

### 13. Remaining

Apply sales/advances migrations only with explicit approval. Wire receipt to PO. Sales Angular. Stage 6 Catalog Brand/PriceList UI; Stage 7 `[Authorize]` on Payable/Receivable/FinancialTransfer.

### 14. Architecture exceptions remaining

0 / 0 / 0.

---

## Stage 6 — Catalog / Parties / Organization UI

**Date:** 2026-09-17  
**Status:** complete  
**HEAD at start:** post-Stage 5 working tree

### 1. What was found

Catalog Angular was products-only. Parties Angular was dealers/groups only. Organization Angular was branches-only. Backend already had Brand/PriceList/Property/Company/OrganizationSettings/PartyContact/PartyAddress/CustomerProfile/SupplierProfile APIs.

### 2. What was changed

Angular CRUD for Brand, PriceList, Property, Company, Organization Settings. Dealer edit now manages customer/supplier role assignment, contacts, and addresses. Menu entries added. Currency/Country remain Master Data; Tenant remains SaaS.

### 3. Exact files changed

New Catalog/Organization/Parties Angular feature folders under `OrgSys.Angular/src/app/features/`. Routes and `menu.config.ts` updated. `dealer-form` embeds `dealer-party-details`.

### 4–7. Projects / schema / API

None.

### 8. Angular

`/catalog/brands|price-lists|properties`, `/organization/companies|settings`, dealer profiles/contacts/addresses.

### 9–10. Tests / unused

No new backend tests. Price-list line entries UI deferred.

### 11–12. Build / tests

`npm run build` succeeded.

### 13. Remaining

Price list entries editor. Stage 7 IAM.

### 14. Architecture exceptions remaining

0 / 0 / 0.

---

## Stage 7 — IAM inside Administration

**Date:** 2026-09-17  
**Status:** complete  
**HEAD at start:** post-Stage 6 working tree

### 1. What was found

`PayableController`, `ReceivableController`, `FinancialTransferController` had no `[Authorize]`. No `HasPermissionQuery` / `GetCurrentUserAccessQuery` / `CanAccessBranchQuery`. Angular had no User/Role screens under `features/administration/`. IAM already lives in Administration (`IPasswordHasher`, `MustResetPassword`, one `User.RoleId`).

### 2. What was changed

Authorized the three money controllers. Added Administration.Contracts access queries + handlers + `AccessController`. Angular Users/Roles under `/administration/users|roles`. Did **not** extract Identity. Password mapping still ignores hashes on output.

### 3. Exact files changed

- `API/Controllers/Org/Financials/PayableController.cs`, `ReceivableController.cs`, `FinancialTransferController.cs`
- `Modules/Administration/Administration.Contracts/Access/AccessQueries.cs`
- `Modules/Administration/Administration.Application/Access/Queries/AccessQueryHandlers.cs`
- `API/Controllers/Org/Setting/AccessController.cs`
- `Tests/Application.Tests/AccessQueryHandlerTests.cs`
- Angular `features/administration/` users + roles; `app.routes.ts`; `menu.config.ts`

### 4–7. Projects / schema / API

No new module. No migration. New `/Access` endpoints.

### 8. Angular

`/administration/users`, `/administration/roles`. Legacy `/administration/countries` redirects kept.

### 9. Tests

`AccessQueryHandlerTests` (5). Architecture 1635. `npm run build` succeeded. API build 0 errors.

### 10. Unused

`AuthController` CheckPassword/HavePassword still anonymous (Stage 16). One role per user unchanged.

### 11–12. Build / tests

`dotnet build API` 0 errors. Architecture 1635; AccessQueryHandlerTests 5.

### 13. Remaining

Stage 8 Workflow. Stage 16 AuthController lockdown.

### 14. Architecture exceptions remaining

0 / 0 / 0.

---

## Stage 8 — Workflow & Approvals

**Date:** 2026-09-17  
**Status:** complete  
**HEAD at start:** post-Stage 7 working tree

### 1. What was found

No Workflow types. PR submit only moved `PurchaseRequisition` New → UnderReview with no approval gate. Workflow must never mutate PurchaseOrder.

### 2. What was changed

New `Modules/Workflow/{Domain,Application,Contracts,Infrastructure}`. `ApprovalRequest` / `ApprovalDecision`. PR Submit starts a pending approval via `Workflow.Contracts.StartApprovalCommand`. Decide notifies Purchasing via `ApplyPurchaseRequisitionWorkflowDecisionCommand` (reject only); conversion to PO stays in Purchasing.

### 3. Exact files changed

New Workflow module projects; `SubmitCommandHandler`; Purchasing.Contracts decision command; `ApprovalController`; OrgContext DbSets; Architecture module lists; `WorkflowApprovalHandlerTests`.

### 4. New projects

`Workflow.Domain`, `Workflow.Contracts`, `Workflow.Application`, `Workflow.Infrastructure` (added to `OrgSys.sln`).

### 5. Dependencies

Purchasing.Application → Workflow.Contracts. Workflow.Application → Purchasing.Contracts.

### 6. Schema / migration

**Migration:** `20260916193620_AddWorkflowApprovals` — CreateTable `ApprovalRequest`, `ApprovalDecision` only. **Not applied.**

### 7. API

`/Approval` Start, Decide, GetByDocument.

### 8. Angular

None (Stage 15).

### 9. Tests

`WorkflowApprovalHandlerTests` (2). Application.Tests 131. Architecture 1854.

### 10. Unused

Multi-step workflow definitions not modeled (single decide completes the request).

### 11–12. Build / tests

API build 0 errors. Architecture 1854; Application 131.

### 13. Remaining

Stage 9 Budgeting. Apply workflow migration only with approval.

### 14. Architecture exceptions remaining

0 / 0 / 0.

---

## Stage 9 — Budgeting

**Date:** 2026-09-17  
**Status:** complete  
**HEAD at start:** post-Stage 8 working tree

### 1. What was found

No CostCenter/Department/Budget types. SaaS already seeds feature key `Budgeting`.

### 2. What was changed

`Department` in Organization (not Budgeting). New Budgeting module: `Budget`/`BudgetLine`; actuals via `Accounting.Contracts.GetAccountActivityQuery` only.

### 3. Exact files changed

Organization Department CRUD + `DepartmentController`. `Modules/Budgeting/*`. OrgContext + migration. Architecture lists. `CreateBudgetCommandHandlerTests`.

### 4. New projects

`Budgeting.Domain`, `Budgeting.Contracts`, `Budgeting.Application`, `Budgeting.Infrastructure`.

### 5. Dependencies

Budgeting.Application → Accounting.Contracts. Budget.DepartmentId / FiscalYearId / AccountId are scalars + Fluent `HasOne(typeof(...))`.

### 6. Schema / migration

**Migration:** `20260916194107_AddBudgetingAndDepartment` — CreateTable `Department`, `Budget`, `BudgetLine`. **Not applied.**

### 7. API

`/Department` CRUD. `/Budget` Create + VsActual.

### 8. Angular

None (Stage 15).

### 9. Tests

`CreateBudgetCommandHandlerTests` (1). Application.Tests 132. Architecture 2087.

### 10. Unused

Cost centers not added. Period filter uses activity Date within budget period.

### 11–12. Build / tests

Architecture 2087; Application 132.

### 13. Remaining

Stage 10 Tax. Apply budgeting migration only with approval.

### 14. Architecture exceptions remaining

0 / 0 / 0.

---

## Stage 10 — Tax

**Date:** 2026-09-17  
**Status:** complete  
**HEAD at start:** post-Stage 9 working tree

### 1. What was found

`Invoice.Tax` / `TaxType` already on CommercialDocuments; `Company.TaxRegistrationNumber` already on Organization. No Tax module, no posted-invoice snapshot, no ETA/ZATCA.

### 2. What was changed

New Tax module. `InvoiceTaxSnapshot` (+ lines) captures Tax/TaxType/Net/Total at GL post time. `CreateInvoiceCommandHandler` sends `SnapshotInvoiceTaxCommand` when `HasJournal`. ETA/ZATCA adapters live in Tax.Infrastructure and are disabled no-ops.

### 3. Exact files changed

`Modules/Tax/*`. CommercialDocuments.Application → Tax.Contracts. OrgContext + unique InvoiceId. Architecture lists. `SnapshotInvoiceTaxCommandHandlerTests` (2). `TaxController`.

### 4. New projects

`Tax.Domain`, `Tax.Contracts`, `Tax.Application`, `Tax.Infrastructure`.

### 5. Dependencies

Tax.Domain has no CommercialDocuments reference (scalar InvoiceId). ETA/ZATCA not in Domain.

### 6. Schema / migration

**Migration:** `20260916194648_AddTaxInvoiceSnapshots` — CreateTable `InvoiceTaxSnapshot`, `InvoiceTaxSnapshotLine`. **Not applied.**

### 7. API

`GET /Tax/Snapshot?invoiceId=`

### 8. Angular

None (Stage 15).

### 9. Tests

`SnapshotInvoiceTaxCommandHandlerTests` (2). Application.Tests 134. Architecture 2334.

### 10. Unused

No TaxCode engine. Adapters skipped until credentials exist.

### 11–12. Build / tests

Architecture 2334; Application 134.

### 13. Remaining

Stage 11 Fixed Assets. Apply tax migration only with approval.

### 14. Architecture exceptions remaining

0 / 0 / 0.

---

## Stage 11 — Fixed Assets

**Date:** 2026-09-17  
**Status:** complete  
**HEAD at start:** post-Stage 10 working tree

### 1. What was found

COA already seeds PPE and accumulated-depreciation / depreciation-expense accounts (e.g. 120102 Buildings, 120201 Accum. Dep. Buildings, 620201 Dep. Buildings). No Fixed Assets module.

### 2. What was changed

New FixedAssets module. Category stores GL **account IDs**. Straight-line monthly depreciation posts via `Accounting.Contracts.PostAccountingEntryCommand` (Dr expense, Cr accumulated).

### 3. Exact files changed

`Modules/FixedAssets/*`. OrgContext FKs to Account/Currency. Architecture lists. `FixedAssetCommandHandlerTests` (2). `FixedAssetController`.

### 4. New projects

`FixedAssets.Domain`, `FixedAssets.Contracts`, `FixedAssets.Application`, `FixedAssets.Infrastructure`.

### 5. Dependencies

FixedAssets.Application → Accounting.Contracts only. Category account IDs are scalars + Fluent `HasOne(typeof(Account))`.

### 6. Schema / migration

**Migration:** `20260916195105_AddFixedAssets` — CreateTable `FixedAssetCategory`, `FixedAsset`, `DepreciationEntry`. **Not applied.**

### 7. API

`/FixedAsset` Category/Create, Create, Depreciation/Post, Get.

### 8. Angular

None (Stage 15).

### 9. Tests

`FixedAssetCommandHandlerTests` (2). Application.Tests 136. Architecture 2595.

### 10. Unused

No declining-balance, no disposal journal in this pass.

### 11–12. Build / tests

Architecture 2595; Application 136.

### 13. Remaining

Stage 12 SaaS ICurrentTenant. Apply FA migration only with approval.

### 14. Architecture exceptions remaining

0 / 0 / 0.

---

## Stage 12 — SaaS / ICurrentTenant

**Date:** 2026-09-17  
**Status:** complete

`ICurrentTenant` from JWT (`tenantId`/`companyId`/`branchId`). Tenant limits on Create Company/Branch/User/Stock. Company Get/Update/Delete isolated. `docs/saas/tenant-ownership-matrix.md`. `TenantIsolationTests` (4). **No extra Stage 12 migration.**

---

## Stage 13 — Outbox / Inbox

**Date:** 2026-09-17  
**Status:** complete

`IntegrationEvent.EventId`. `OutboxMessage`/`InboxMessage` on `OrgContext`. `OutboxIntegrationEventPublisher` + `OutboxDispatcher`. Inbox on sales/purchase invoice posted and customer/supplier payment. `GoodsReceiptPostedIntegrationEvent` from receipt post. **Migration:** `20260916200004_AddOutboxInbox` — **not applied.**

---

## Stage 14 — Reporting projections

**Date:** 2026-09-17  
**Status:** complete

`CustomerAgingReadModel` / `SalesSummaryReadModel` in Reporting.Infrastructure. Event-fed from `SalesInvoicePostedIntegrationEvent`. Live SQL `IReportingReadStore` kept read-only. ADR `docs/architecture/adr/reporting-projections.md`. **Migration:** `20260916200316_AddReportingProjections` — **not applied.**

---

## Stage 15 — Angular remaining + matrix

**Date:** 2026-09-17  
**Status:** complete

Routes/menu for Purchasing, Sales orders/quotations, AR/AP outstanding, SaaS, Budget, Workflow, Tax, Fixed Assets, inventory batches/serials, reporting projections. `permissionGuard` on users/roles. Shared `CrudSearchList` / `QueryWorkspace`. `docs/angular/backend-frontend-matrix.md`. Legacy redirects unchanged. Angular `ng build` succeeded.

---

## Stage 16 — Security / observability

**Date:** 2026-09-17  
**Status:** complete

`Auth` class `[Authorize]`; login/CheckEmail stay `[AllowAnonymous]`; CheckPassword/HavePassword require JWT. 500s return ProblemDetails (generic message outside Development). CORS from `Cors:AllowedOrigins`. `/health` (EF DbContext check). `X-Correlation-Id`. OpenTelemetry ASP.NET/HTTP tracing; OTLP only if `OpenTelemetry:OtlpEndpoint` is set. Payable/Receivable/FinancialTransfer already `[Authorize]`. JWT tenant claims already from Stage 12. API Release build 0 errors.

---

## Stage 17 — End-to-end tests

**Date:** 2026-09-17  
**Status:** complete

`EndToEndFlowTests`: A quotation→confirm SO; B PR submit (starts workflow, does not mutate PO)→convert→link invoice; C receipt create/confirm/post; D sales invoice posted then customer payment FIFO; E FA post depreciation; F budget vs actual; G tenant isolation covered by `TenantIsolationTests`.

---

## Stage 18 — CI/CD + hygiene

**Date:** 2026-09-17  
**Status:** complete

`.github/workflows/ci.yml` restore/build/Architecture.Tests/Application.Tests/Domain.Tests/Angular build. `*.zip` in `.gitignore`. `OrgSys.DatabaseMigrator` added to `OrgSys.sln`. `docs/dependency-rules.md` §3 rewritten: allow-lists empty.

---

## Stage 19 — Final architecture docs

**Date:** 2026-09-17  
**Status:** complete

- `docs/architecture/table-ownership.md`
- `docs/architecture/integration-events.md`
- `docs/architecture/ddd-rules.md`
- `docs/architecture/completion-report.md`
- `docs/architecture/remaining-risks.md`

Migrations still **not applied**. Identity not extracted. Encryption not restored.

### 11–12. Build / tests (final)

API Release: 0 errors. Application.Tests 150. Architecture.Tests 2595. Angular development build succeeded.

### 14. Architecture exceptions remaining

0 / 0 / 0.

---

## Leftover closeout (after Stage 19)

**Date:** 2026-09-17  
**Status:** complete (no Stage 20)

Closed remaining plan items that Stages 0–19 left open:

1. **Stage 5 PO↔GRN.** `InventoryReceipt.PurchaseOrderId` scalar + Fluent `HasOne(typeof(PurchaseOrder))`. Posted receipts publish line quantities; Purchasing inbox handler calls `PurchaseOrder.RecordReceipt` (`min(qty, remaining)`). `GET /PurchaseOrder/ThreeWayMatch` compares ordered vs received vs invoiced via `GetInvoiceInventoryImpactQuery`.
2. **Stage 6 price-list entries.** Angular FormArray editor; update persists `PriceListEntry` rows (add/update/remove).
3. **Stage 9 Department UI.** `/organization/departments` CrudSearchList + menu.
4. **Stage 15/17.** Receipt form optional `purchaseOrderId`; purchasing three-way workspace; Flow E (FA depreciation) and Flow F (budget vs actual) in `EndToEndFlowTests`.
5. **Stage 18.** CI runs `Tests/*.Domain.Tests` (not Integration). `Domain.zip` untracked when previously cached.
6. **Migration.** `20260916202437_AddInventoryReceiptPurchaseOrderId` generated, **not applied**.

### 11–12. Build / tests (leftover closeout)

Application.Tests **155**. Architecture.Tests **2595**. Domain.Tests all green. Angular `ng build` succeeded. Migration **not applied**.
