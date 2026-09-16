# Table ownership

Single owner per table. Cross-module links are scalar IDs + Fluent `HasOne(typeof(X))` FKs. Isolation is company-rooted (`Company.TenantId`); see `docs/saas/tenant-ownership-matrix.md`.

| Table | Owner module |
|-------|----------------|
| Tenant, Plan, Feature, PlanFeature, Subscription | SaaS |
| Company, Branch, Department, OrganizationSettings | Organization |
| User, Role, Permission, RolePermission, Preference, Shift, Table | Administration |
| Country, City, District, Currency, PaymentType, ReferenceType | MasterData |
| Account, AccountType, FiscalYear, FiscalPeriod, Journal, JournalItem, JournalType | Accounting |
| FinancialAccount, Financial, FinancialTransfer, FinancialInvoice, CashBox, BankAccount, Bank, BankBranch, FinancialType | Treasury |
| Product, ProductUnit, ProductPropertyElement, ProductRecipe, Brand, Classification, PriceList, PriceListEntry, Property, PropertyElement, Unit | Catalog |
| Dealer, DealerGroup, PartyContact, PartyAddress, CustomerProfile, SupplierProfile | Parties |
| Invoice, InvoiceProduct, InvoiceType | CommercialDocuments |
| InvoiceTaxSnapshot, InvoiceTaxSnapshotLine | Tax |
| SalesQuotation, SalesQuotationLine, SalesOrder, SalesOrderLine | Sales |
| PurchaseRequisition, PurchaseRequisitionProduct, PurchaseOrder, PurchaseOrderProduct | Purchasing |
| Receivable, PaymentApplication, PaymentApplicationLine | Receivables |
| Payable, SupplierPaymentApplication, SupplierPaymentApplicationLine | Payables |
| Custody, CustodyHandover | Advances |
| Stock, WarehouseLocation, Inventory, InventoryBalance, InventoryBatch, InventoryCostLayer, InventorySerial, InventoryProduct, InventoryReceipt, InventoryIssue, StockTransfer, StockAdjustment (+ lines), StockReservation, StockAdjustmentReason, Transaction, TransactionProduct, TransactionType | Inventory |
| FixedAssetCategory, FixedAsset, DepreciationEntry | FixedAssets |
| Budget, BudgetLine | Budgeting |
| ApprovalRequest | Workflow |
| CustomerAgingReadModel, SalesSummaryReadModel | Reporting (projections) |
| OutboxMessage, InboxMessage | Messaging (OrgSys.EventBus / OrgContext) |

Reporting live SQL reports read owner tables through `IReportingReadStore` in Infrastructure only.
