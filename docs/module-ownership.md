# OrgSys Module Ownership Map

Status legend: **Owned** = lives in the module's own Domain/Application, no known violations ·
**Owned (leaking)** = lives in the right module but another module still reaches in directly ·
**Legacy** = still in the root `Domain`/`Application` projects · **Missing** = concept named in
the brief but not yet implemented in OrgSys.

| Business Concept | Owning Module | Current Location | Target Location | Status |
|---|---|---|---|---|
| Account, AccountType | Accounting | `Modules/Accounting/Accounting.Domain/Entities` | same | Owned |
| Journal, JournalItem, JournalType | Accounting | `Modules/Accounting/Accounting.Domain/Entities` | same | Owned (leaking — see below) |
| FiscalYear, FiscalPeriod | Accounting | `Modules/Accounting/Accounting.Domain/Entities` | same | Owned |
| Journal posting/reversal rules | Accounting | `Application/Commands/Org/Financials/Integration/{JournalInvoice,JournalTransaction}` (**legacy root**, not `Accounting.*`) | `Modules/Accounting/Accounting.Application` | Legacy — called directly by Sales/Inventory command handlers instead of through Accounting's own API (Phase 4/8) |
| CashBox, Bank, BankAccount, FinancialAccount | Treasury | `Modules/Treasury/Treasury.Domain/Entities` | same | Owned |
| Financial (unified transaction), FinancialTransfer, FinancialType | Treasury | `Modules/Treasury/Treasury.Domain/Entities` | same | Owned (leaking — `Sales.Application` reaches `IRepository<Financial>` directly; fixed this pass) |
| Cheques, Advances (عهد), Treasury settlements | Treasury | — | `Modules/Treasury/*` | **Missing** — no cheque/advance entities exist anywhere in OrgSys today; not part of this pass |
| Product, ProductUnit, Stock, Property | Inventory | `Modules/Inventory/Inventory.Domain/Entities` | same | Owned (leaking — `Sales.Application` reaches `Product`/`Stock` directly; fixed this pass) |
| Transaction, TransactionProduct, TransactionType (inventory movement) | Inventory | `Modules/Inventory/Inventory.Domain/Entities` | same | Owned (leaking — `Sales.Application` reaches `IRepository<Transaction>` directly; fixed this pass) |
| Inventory (stocktake/adjustment), InventoryProduct | Inventory | `Modules/Inventory/Inventory.Domain/Entities` | same | Owned |
| Dealer, DealerGroup, DealerType | Sales | `Modules/Sales/Sales.Domain/Entities` | same | Owned (leaking outward — `Treasury.Application`, `Accounting.Application` reach `Sales.Domain.Dealer` directly; see `docs/dependency-rules.md` §Accepted exceptions, not fixed this pass) |
| Invoice, InvoiceProduct, InvoiceType (sales *and* purchase invoices — discriminated by `InvoiceType`, not separate entities) | Sales | `Modules/Sales/Sales.Domain/Entities` | same | Owned |
| Order, OrderProduct, OrderType | Sales | `Modules/Sales/Sales.Domain/Entities` | same | Owned |
| Sales quotation lifecycle | Sales | — | `Modules/Sales/*` | **Missing** — no quotation concept exists in OrgSys; not part of this pass |
| Purchase requisition, dedicated Purchase order/invoice/return workflows | Purchasing | — (uses Sales' `Invoice`+`InvoiceType`/`Dealer`+`DealerType` discriminators — see `docs/modular-monolith-analysis.md` §"Dealer/Invoice cross-cutting resolution") | `Modules/Purchasing/*` if/when Purchasing needs behavior genuinely distinct from Sales' invoice lifecycle | Intentionally thin by design; **not a gap to close by default** — creating a parallel Purchasing document model today would duplicate Sales' Invoice/Dealer without a proven business need (brief §"Do not create unnecessary duplicate ... Invoice ... entities") |
| Customer open items, allocations, settlement, aging, unapplied receipt | Receivables | `Modules/Receivables/Receivables.Application/OpeningBalance` (opening balance only) | `Modules/Receivables/*` | **Partially missing** — only the opening-balance workflow exists; day-to-day open-item/allocation/aging logic doesn't exist anywhere yet (balances today are computed ad hoc in Reporting's Dealer-balance queries, not owned by Receivables) |
| Supplier open items, allocations, settlement, aging, unapplied payment | Payables | `Modules/Payables/Payables.Application/OpeningBalance` (opening balance only) | `Modules/Payables/*` | **Partially missing**, same reasoning as Receivables |
| Country, City, District, Unit, Classification, Currency, ReferenceType, PaymentType | MasterData | `Modules/MasterData/MasterData.Domain/Entities` | same | Owned |
| Branch, CompanyProfile, Shift, Table | Organization | `Modules/Organization/Organization.Domain/Entities` | same | Owned. `Application/DTOs/OrgDb/CompanyProfileDto.cs` still sits in the legacy root — likely dead duplicate, needs triage in Phase 8, not touched this pass |
| User, Role, Permission, RolePermission | Administration | `Modules/Administration/Administration.Domain/Entities` | same | Owned |
| Reports (Dealer/Financial/Sales/Warehouse balance & statement queries) | Reporting | `Modules/Reporting/Reporting.Application` | same | Owned (by design, reads every module's Domain directly — see `docs/dependency-rules.md` for why this is an accepted, scoped exception rather than a violation) |
| LogSys, Notification, Preferences | *(cross-cutting technical, not a business module)* | `Domain/Entities/OrgDb`, `Application/DTOs/OrgDb` (**legacy root**) | Stay as shared technical infrastructure, or move to Administration/Observability once Outbox/logging work starts | Legacy — small, deliberately not migrated this pass (Phase 8/20) |
| `OrderTypeDto` | Sales (Order) | `Application/DTOs/OrgDb/OrderTypeDto.cs` (**legacy root**) | `Modules/Sales/Sales.Application` | Legacy — misplaced, needs triage in Phase 8 |
| Integration Events (`SalesInvoicePosted`, etc.) | *(cross-cutting)* | — | `Modules/<Module>/<Module>.Contracts/IntegrationEvents` | **Missing** — not part of Phase 1/2; the brief's Phase 9 |
| Outbox | *(cross-cutting)* | — | `BuildingBlocks/Outbox` | **Missing** — Phase 9 |
| Observability (structured logging, correlation IDs, health checks) | *(cross-cutting)* | — | `BuildingBlocks/Observability` | **Missing** — brief §20, not part of this pass |
