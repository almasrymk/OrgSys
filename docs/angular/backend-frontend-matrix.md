# Backend–frontend matrix

Generated from live `API/Controllers` and `OrgSys.Angular` routes (Stage 15, 2026-09-17).
Angular stays on feature folders; legacy `/administration/*`, `/financial/*`, `/invoices/*`, `/warehouse/*`, `/transactions/*`, `/customers-suppliers/*`, `/reports/*` remain **redirects only**.

Coverage: **Covered** = dedicated Angular screen. **Workspace** = shared `CrudSearchList` / `QueryWorkspace`. **Redirect** = legacy URL. **API-only** = no UI yet (command-only or lookup without list).

| Controller | HTTP surface | Angular route | Status |
|------------|--------------|---------------|--------|
| AuthController | `api/Auth/login`, CheckEmail, CheckPassword, HavePassword | `/login` | Covered (login only) |
| AccountController | Base Search/GetList/CRUD | `/accounting/accounts` | Covered |
| JournalController | Base + Post/Reverse | `/accounting/journal-entries` | Covered |
| FiscalYearController | Base | `/accounting/fiscal-years` | Covered |
| AccountTypeController / JournalTypeController / OutlayController | Base | — | API-only |
| CashBoxController | Base | `/treasury/financial-accounts/cash-boxes` | Covered |
| BankAccount (FinancialAccount) | Financial/Accounts* | `/treasury/financial-accounts/bank-accounts` | Covered |
| BankController / BankBranchController | Base | `/treasury/banks`, `/treasury/bank-branches` | Covered |
| FinancialController | Receipt/Payment/Deposit/… | `/treasury/receipts` … `/treasury/adjustments` | Covered |
| FinancialTransferController | GetList/GetById/Create | `/treasury/transfers` | Covered |
| FinancialTypeController / FinancialAccountController | Base + Balance | treasury accounts | Covered (via financial-accounts) |
| ReferenceTypeController | Base | — | API-only |
| DealerController / DealerGroupController | Base + Balance | `/parties/dealers/*`, `/parties/dealer-groups/*` | Covered |
| CustomerProfileController / SupplierProfileController | GetByDealerId | parties profiles | Covered |
| PartyContactController / PartyAddressController | GetListByDealer | parties | Covered |
| InvoiceController | Search/CRUD + returns | `/commercial-documents/*` | Covered |
| InvoiceTypeController | Base | — | API-only |
| ProductController / Unit / Classification / ProductUnitModelView | Base + GetAllByBalance | `/catalog/products` | Covered |
| BrandController / PriceListController / PropertyController | Base | `/catalog/brands`, `price-lists`, `properties` | Covered (price-list entries FormArray) |
| PricingController | ResolvePrice | used by invoice/order forms | Covered (service) |
| Country/City/District/Currency | Base | `/master-data/*` | Covered |
| PaymentTypeController | Base | — | API-only |
| BranchController | Base | `/organization/branches` | Covered |
| CompanyController | Base | `/organization/companies` | Covered |
| OrganizationSettingsController | GetByCompanyId/Update | `/organization/settings` | Covered |
| DepartmentController | Base | `/organization/departments` | Workspace |
| UserController / RoleController | Base | `/administration/users`, `/administration/roles` | Covered + `permissionGuard` |
| AccessController | HasPermission / GetCurrentUserAccess | interceptor / permission service | Covered |
| Preference / Shift / Table | Base | — | API-only |
| StockController | Base | inventory count / receipts | Covered (stock lookup) |
| InventoryController / TransactionController / TransactionTypeController | movements | `/inventory/movements/*` (typeIds 1,2,3,5–8) | Covered |
| InventoryReceiptsController | GET/POST/Post | `/inventory/receipts` | Covered (optional `purchaseOrderId`) |
| WarehouseLocationsController | ByStock | `/inventory/locations` | Covered |
| InventoryBalancesController | GET/List/StockCard | `/inventory/balances` | Covered |
| StockReservationsController | GET | `/inventory/reservations` | Covered |
| InventoryIssuesController / StockTransfersController / StockAdjustmentsController | command-only (no list) | `/inventory/movements/issue`, `transfer`, `adjustment-*` | Covered via movement screens |
| StockAdjustmentReasonsController | GET | — | API-only |
| InventoryBatchesController | Expiring | `/inventory/batches` | Workspace |
| InventorySerialsController | History | `/inventory/serials` | Workspace |
| PurchaseRequisitionController | Base + Submit/Reject/Cancel/Convert | `/purchasing/requisitions` | Workspace |
| PurchaseOrderController | Base + LinkInvoice/Cancel/Remaining/ThreeWayMatch | `/purchasing/orders`, `/purchasing/match` | Workspace |
| SalesOrderController | GET list, Confirm/Cancel | `/sales/orders` | Workspace |
| QuotationController | GET list, Send/Accept/Reject/Convert | `/sales/quotations` | Workspace |
| ReceivableController | Outstanding/Overdue/Aging | `/receivables/outstanding` | Workspace |
| PayableController | Outstanding/Overdue/Aging + OpeningBalance | `/payables/outstanding` | Workspace |
| CustodyController | GET list / by id | `/advances/custodies` | Covered |
| TenantController / PlanController / FeatureController | Base Search | `/saas/tenants`, `plans`, `features` | Workspace |
| SubscriptionController | Search/GetList/Subscribe | `/saas/subscriptions` | Workspace |
| BudgetController | Create / VsActual | `/budgeting` | Workspace |
| ApprovalController | Start/Decide/GetByDocument | `/workflow` | Workspace |
| TaxController | Snapshot | `/tax` | Workspace |
| FixedAssetController | Category/Create, Create, Depreciation/Post, Get | `/fixed-assets` | Workspace |
| WarehouseReportController | Movement/Balance | `/reporting/warehouse/*` | Covered |
| DealerReportController | Balance/Statement | `/reporting/dealers/:type/*` | Covered |
| FinancialReportController | SafeMovement/SafeBalance | `/reporting/finance/*` | Covered |
| SalesReportController | Balance | `/reporting/sales/balance` | Covered |
| ProjectionReportController | Aging/SalesSummary | `/reporting/projections/*` | Workspace |
| MainController | unused (Compile Remove) | — | N/A |

## Intentionally not a 1:1 screen per controller

- **InventoryIssues / StockTransfers / StockAdjustments** post via dedicated APIs; the user-facing list/edit remains the existing movement TypeId screens.
- **Tax / Budget / Approval / FixedAsset** expose lookup or command APIs, not BaseController Search; UI is a query workspace against those endpoints.
- **Auth CheckPassword / HavePassword** are not used by Angular login.
