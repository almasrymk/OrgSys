# Catalog Bounded Context — Data Ownership Matrix

Companion to `catalog-current-state.md` (Phase 0) and `catalog-target-architecture.md` (Phase 1).
Reflects the actual OrgSys module set discovered in this repo, not a generic template.

| Data | Owner | Notes |
|---|---|---|
| Product master (`Product`, `ProductUnit`) | **Catalog** | Relocated from Inventory.Domain, table names unchanged |
| Category (`Classification`) | **Catalog** | Relocated from MasterData.Domain, table name unchanged; C# class name kept as `Classification` — see target-architecture §1 |
| Unit of Measure (`Unit`) | **Catalog** | Relocated from MasterData.Domain, table name unchanged |
| Brand | **Catalog** | New |
| Product Attributes (`Property`, `PropertyElement`, `ProductPropertyElement`) | **Catalog** | Relocated from Inventory.Domain, table names unchanged |
| Price List (`PriceList`, `PriceListEntry`) | **Catalog** | New |
| Service master | **Catalog** | Not a separate entity — a `Product` row with `ProductType = Service` |
| Bill of Materials (`ProductRecipe`) | **Inventory** | Costing composition, not product definition — brief's own Phase 29 boundary; discovery confirmed no reason to move it |
| Warehouse (`WarehouseLocation`) | **Inventory** | Unchanged |
| Stock balance (`Stock`, `InventoryBalance`, `InventoryCostLayer`, `InventoryBatch`) | **Inventory** | Unchanged |
| Stock movements (`Transaction`, `TransactionProduct`, `Inventory`/`InventoryProduct`, `InventoryReceipt(Line)`, `InventoryIssue(Line)`, `StockAdjustment(Line+Reason)`, `StockTransfer(Line)`) | **Inventory** | Unchanged |
| Reservations (`StockReservation`) | **Inventory** | Unchanged |
| Serial numbers (`InventorySerial`) | **Inventory** | Unchanged |
| Costing method application / valuation | **Inventory** | Unchanged — `Product.CostingMethod` stays on Product as a *classification* of how Inventory should cost it; the actual valuation logic and numbers live in Inventory |
| Supplier / Customer (`Dealer`, `DealerGroup`, `DealerType`) | **Parties** | Unchanged |
| Sales Order / Quotation (`SalesOrder(Line)`, `Quotation(Line)`) | **Sales** | Unchanged; `ProductId` scalar FK + `ProductName` snapshot already follows the brief's snapshot guidance |
| Purchase Order / Requisition (`PurchaseOrder(Product)`, `PurchaseRequisition(Product)`) | **Purchasing** | Unchanged; `ProductId` scalar FK, `.Unit` navigation retargets to `Catalog.Domain.Unit` |
| Invoice — sales & purchase (`Invoice`, `InvoiceProduct`, `InvoiceType`) | **CommercialDocuments** | Unchanged; `InvoiceProduct.ProductId` scalar FK only, no navigation to update |
| Accounting (`Account`, `Journal`, `JournalItem`, `FiscalYear/Period`) | **Accounting** | Unchanged — Catalog never posts journal entries directly |
| Treasury (`Financial`, `Bank*`, `CashBox`, `FinancialAccount`) | **Treasury** | Unchanged |
| Receivables / Payables open items | **Receivables / Payables** | Unchanged |
| Reference data (`Country, City, District, Currency, PaymentType, ReferenceType`) | **MasterData** | Unchanged — genuinely cross-domain, none are Product-specific (confirmed by discovery) |
| Organization (`Branch`, `CompanyProfile`, `Shift`, `Table`) | **Organization** | Unchanged |
| Users/Roles/Permissions | **Administration** | Unchanged |
| Cross-module reports | **Reporting** | Unchanged — continues its by-design direct-read exception, now reading `Catalog.Domain` instead of `Inventory.Domain`/`MasterData.Domain` for Product/Category |

## Deliberately deferred (not owned by anyone yet — documented, not silently dropped)

| Data | Reason deferred |
|---|---|
| `ProductVariant`, `VariantAttributeValue` | No confirmed business need in OrgSys today; user confirmed defer (2026-09-15) |
| Multi-barcode-per-product | Same — `Product.Barcode` (single column) is kept as-is |
| Category hierarchy (`ParentCategoryId`) | No existing consumer needs it; `BaseModel.ParentId` already provides a ready seam if confirmed needed later |
| Global (non-product-specific) UOM conversion / dimension grouping | `ProductUnit.Rate` already covers every real conversion need found in this codebase |
| `Property.IsVariantDefining` | Meaningless without `ProductVariant`; would be pure speculative scaffolding |
