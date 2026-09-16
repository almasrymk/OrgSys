# OrgSys Master Execution Plan (repository-specific)

This plan is bound to **current files, types, ProjectReferences, and Architecture.Tests allow-lists**.

| Item | Value |
|------|--------|
| HEAD | `4c0ffdda06fe103f8fda1b1a987855de2c58804e` |
| Branch | `Latest` |
| Source of truth | Current `.cs` / `.csproj` / `OrgContext` / `OrgContextModelSnapshot` / Architecture.Tests / Angular `src/app` |
| Stage 0 status | Complete (documentation only — this file) |

Rules:

- Current code + Architecture.Tests win over historical docs.
- Do **not** create Tax, FixedAssets, Budgeting, Workflow, or Identity **projects** until the named stage.
- Do **not** add Architecture.Tests exception rows.
- After every implementation stage: `dotnet restore`, `dotnet build OrgSys.sln`, `dotnet test OrgSys.sln`, Architecture.Tests independently; `dotnet ef migrations has-pending-model-changes` if mappings changed; Angular `npm run build` if UI changed.
- Update `docs/completion/progress.md` after every stage.

Reference isolation pattern: `Accounting.Domain.Journal.CurrencyId` (scalar) + Fluent FK in `BuildingBlocks/OrgSys.DatabaseMigrator/Persistence/OrgContext.cs`.

---

## Stage 0 — Repository audit

**Status:** complete. No code changes.

Delivered:

- `docs/completion/current-state-audit.md`
- `docs/completion/current-context-map.md`
- `docs/completion/master-execution-plan.md` (this file)
- `docs/completion/progress.md`

---

## Stage 1 — Zero-exception cross-context isolation

**Do this next. Do not start Tax/FixedAssets/Budgeting/Workflow/Identity.**

Authoritative allow-lists:

- `Tests/Architecture.Tests/ModuleDependencyTests.cs` → `AcceptedDomainExceptions` (7)
- `Tests/Architecture.Tests/ModuleLayerDependencyTests.cs` → `AcceptedApplicationDomainExceptions` (16), `AcceptedApplicationApplicationExceptions` (3)
- `Tests/Architecture.Tests/ModuleInfrastructureDependencyTests.cs` → empty allow-list (keep it empty)

Also strengthen later: add Catalog/SaaS/Advances to the Infrastructure test array (they are referenced by Architecture.Tests.csproj but omitted from `ModuleInfrastructures`).

### Safe execution order (Stage 1)

1. Cheap stale ProjectReference / global-using deletions (EX-D5, EX-AD1, EX-AA1, stale MasterData.Application refs).
2. MasterData.Contracts existence+name queries (needed by many later cards).
3. Catalog.Contracts product/unit reference queries.
4. Domain navigations → scalar IDs, Fluent FKs preserved (EX-D1..D4, D6, D7).
5. Application mappings/includes rewritten to Contracts (EX-AD2, AD3, AD8).
6. Organization validators (EX-AD15, AD16) — cheapest remaining Application→Domain.
7. Inventory/Treasury Invoice reads (EX-AD4, AD7) — highest regression; do after invoice Contracts are extended.
8. Reporting cluster (EX-AD9..AD14) — **do not fake-fix**. Either temporary owner-module read queries or defer to Stage 14 with an ADR. Prefer not leaving them as casual allow-list rows.
9. Drop allow-list rows only when NetArchTest is green **without** the row.
10. Build, Architecture.Tests, Accounting.Integration, Inventory.Integration, `dotnet ef migrations has-pending-model-changes`.

---

### Stage 1 exception cards

Each card is one Architecture.Tests allow-list row. Do not add new rows.

---

#### EX-D1 — Treasury.Domain → MasterData.Domain

| Field | Value |
|-------|--------|
| From project | `Modules/Treasury/Treasury.Domain/Treasury.Domain.csproj` |
| To project | `Modules/MasterData/MasterData.Domain/MasterData.Domain.csproj` |
| Offending types | `MasterData.Domain.Country`, `City`, `District`, `Currency`, `PaymentType` |
| Exact existing reference | Navigations: `Bank.Country`; `BankBranch.Country/City/District`; `FinancialAccount.Currency`; `Financial.PaymentType`; `Financial.Currency`; `FinancialTransfer.Currency`. Files: `Treasury.Domain/Entities/Bank.cs`, `BankBranch.cs`, `FinancialAccount.cs`, `Financial.cs`, `FinancialTransfer.cs`. |
| Why it exists | Historical EF Include for names (`Currency.Name`, geo names) on cash/bank masters and movements. |
| Correct ownership | MasterData owns geo/currency/payment-type. Treasury stores `CountryId` / `CityId` / `DistrictId` / `CurrencyId` / `PaymentTypeId` only. |
| Proposed Contract/Event | `MasterData.Contracts`: `GetCurrencyNamesQuery` (exists); add `ExistsCurrencyQuery`, `GetCountryNamesQuery`, `GetCityNamesQuery`, `GetDistrictNamesQuery`, `GetPaymentTypeNamesQuery` (or one `GetReferenceNamesQuery`). |
| Files to modify | Domain entities above; `Treasury.Application/FinancialAccounts/MappingProfile.cs`; BankBranch GetList/Search includes `"Bank,Country,City,District"`; Financial GetList/Search/GetById includes `"Currency,..."`; FinancialAccount includes `"...Currency"`; `CreateFinancialPaidInvoiceHandler .cs` (`invoice.Currency?.Rate`); OrgContext Fluent FKs if nav removal requires explicit `HasOne(typeof(...))`; `Treasury.Domain.csproj`; `ModuleDependencyTests.cs` row. |
| Schema risk | **Low** if columns already exist (they do). Prefer no migration. Run pending-model check. |
| Regression risk | Medium — Angular cash/bank/transfer screens show currency/country names. |
| Tests required | Architecture.Tests; Treasury.Domain.Tests; manual/API GetById on FinancialAccount/BankBranch still returns names via DTO (filled from Contracts, not Include). |
| Safe order | After MasterData.Contracts geo/payment-type queries exist. |

---

#### EX-D2 — CommercialDocuments.Domain → MasterData.Domain

| Field | Value |
|-------|--------|
| From project | `Modules/CommercialDocuments/CommercialDocuments.Domain/CommercialDocuments.Domain.csproj` |
| To project | `Modules/MasterData/MasterData.Domain/MasterData.Domain.csproj` |
| Offending types | `PaymentType`, `Currency` |
| Exact existing reference | `Invoice.PaymentType` (`PaymentTypeId` already present); `Invoice.Currency` (`CurrencyId` already present). File: `CommercialDocuments.Domain/Entities/Invoice.cs`. |
| Why it exists | Invoice search/get-by-id historically Included Currency/PaymentType for names; posting uses snapshot `Rate` / `NetByDefaultCurrency` on the invoice itself. |
| Correct ownership | MasterData owns Currency/PaymentType. Invoice keeps `CurrencyId`, `PaymentTypeId`, and **snapshot** `Rate`, `Net`, `NetByDefaultCurrency`, `Tax`, `Total`. Do not re-read today’s currency rate for posted invoices. |
| Proposed Contract/Event | Existing `GetCurrencyNamesQuery`; add PaymentType name/exists queries. Live lookup only for **draft** validation. |
| Files to modify | `Invoice.cs`; invoice MappingProfile/GetById if they flatten names via nav; `CommercialDocuments.Domain.csproj`; OrgContext; Architecture test row. |
| Schema risk | Low. Keep columns. |
| Regression risk | Medium — invoice print/list currency name; accounting totals must remain snapshot-safe (`Invoice.Rate` already stored). |
| Tests required | Architecture.Tests; `Tests/Accounting.Integration.Tests/InvoiceJournalPostingTests.cs`; Application.Tests invoice reference handler. |
| Safe order | With or after EX-D1 MasterData contracts. |

---

#### EX-D3 — CommercialDocuments.Domain → Catalog.Domain

| Field | Value |
|-------|--------|
| From project | `CommercialDocuments.Domain.csproj` |
| To project | `Catalog.Domain.csproj` |
| Offending types | `Catalog.Domain.Unit` |
| Exact existing reference | `InvoiceProduct.Unit` / `UnitId`. File: `CommercialDocuments.Domain/Entities/InvoiceProduct.cs`. |
| Why it exists | Line unit name via EF Include. Product is already identity-only on the line (inspect before denormalizing). |
| Correct ownership | Catalog owns Unit. Invoice line keeps `UnitId` (and existing product identity/price/qty columns). Add `UnitName` snapshot **only if** current schema/UI already depends on historical unit rename safety; otherwise resolve names via Catalog.Contracts for live UI and keep posted prices/qty on the line. |
| Proposed Contract/Event | `Catalog.Contracts.GetUnitNamesQuery` / `GetProductReferenceQuery` (id, code, name, unit). Existing `GetProductNamesQuery`. |
| Files to modify | `InvoiceProduct.cs`; `CommercialDocuments.Application/Invoices/MappingProfile.cs` (`CreateMap<Unit, UnitDto>`); `GetByIdQueryHandler`; Domain csproj; OrgContext; Architecture tests. |
| Schema risk | Low if only nav is dropped. Do **not** blindly add snapshot columns. |
| Regression risk | Medium — invoice form unit dropdown/name. |
| Tests required | Architecture.Tests; invoice GetById still returns unit name on DTO. |
| Safe order | After Catalog unit-name contract. Coupled with EX-AA2. |

---

#### EX-D4 — Parties.Domain → MasterData.Domain

| Field | Value |
|-------|--------|
| From project | `Parties.Domain.csproj` |
| To project | `MasterData.Domain.csproj` |
| Offending types | `Country`, `City`, `District` |
| Exact existing reference | `Dealer.Country/City/District`; `PartyAddress.Country/City/District`. Files: `Parties.Domain/Entities/Dealer.cs`, `PartyAddress.cs`. |
| Why it exists | Dealer list/search DTO names via Include `"DealerGroup,Country,City,District"`. |
| Correct ownership | MasterData owns geography. Parties stores `CountryId`/`CityId`/`DistrictId`. Do not move Country into Parties. |
| Proposed Contract/Event | MasterData geo name queries (same as EX-D1). |
| Files to modify | Dealer.cs, PartyAddress.cs; `Parties.Application/Dealers/MappingProfile.cs` (`src.Country.Name`); `PartyAddresses/MappingProfile.cs`; Dealer GetById/GetList/Search includes; Domain csproj; OrgContext; Architecture tests. |
| Schema risk | Low. |
| Regression risk | Medium — Angular dealer form country/city/district names. |
| Tests required | Architecture.Tests; `Tests/Parties.Domain.Tests/DealerRoleTests.cs`; dealer search still returns names. |
| Safe order | After MasterData geo contracts. Coupled with EX-AD3. |

---

#### EX-D5 — Inventory.Domain → MasterData.Domain (**stale — delete first**)

| Field | Value |
|-------|--------|
| From project | `Inventory.Domain.csproj` + `Inventory.Domain/GlobalUsings.cs` (`global using MasterData.Domain`) |
| To project | `MasterData.Domain.csproj` |
| Offending types | **None remaining.** Grep found no `Country`/`City`/`District`/`Currency`/`PaymentType` members on Inventory.Domain entities. |
| Exact existing reference | ProjectReference + global using only. Allow-list reason text is **false** (“Product/Stock/Transaction keep geo/currency navs”). |
| Why it exists | Leftover after Catalog relocation. |
| Correct ownership | N/A — remove the reference. |
| Proposed Contract/Event | None. |
| Files to modify | `Inventory.Domain.csproj`, `GlobalUsings.cs`, `ModuleDependencyTests.cs` row. |
| Schema risk | None. |
| Regression risk | Very low. |
| Tests required | Architecture.Tests must fail the row then pass after deletion. |
| Safe order | **First Domain item.** |

---

#### EX-D6 — Inventory.Domain → Catalog.Domain

| Field | Value |
|-------|--------|
| From project | `Inventory.Domain.csproj` |
| To project | `Catalog.Domain.csproj` |
| Offending types | `Catalog.Domain.Product`, `Catalog.Domain.Unit` |
| Exact existing reference | Product and/or Unit navigations on: `TransactionProduct`, `InventoryProduct`, `InventoryBalance`, `InventoryReceiptLine`, `InventoryIssueLine`, `StockTransferLine`, `StockAdjustmentLine`, `StockReservation`, `InventoryBatch`, `InventorySerial`, `InventoryCostLayer`. |
| Why it exists | Includes for product/unit names on stock documents; balance query maps `Product` → `ProductDto`. |
| Correct ownership | Catalog owns Product/Unit. Inventory stores `ProductId`/`UnitId` and stock quantities/costs. Never move Product back into Inventory. |
| Proposed Contract/Event | Existing `GetProductNamesQuery`; add `GetProductReferenceQuery` / `GetUnitNamesQuery`. Availability already in Inventory.Contracts. |
| Files to modify | All line/balance entities listed; many Inventory.Application MappingProfiles and GetList includes; Reporting warehouse handlers (they Include Product via TransactionProduct — see EX-AD14); Domain csproj; OrgContext Fluent FKs; Architecture tests. |
| Schema risk | **Highest Domain card.** Many FKs. Prefer keep SQL FKs, drop navs only. |
| Regression risk | High — movements, receipts, balances, reservations, Angular inventory screens, warehouse reports. |
| Tests required | Architecture.Tests; Inventory.Domain.Tests (11 classes); `Tests/Inventory.Integration.Tests/ReservationConcurrencyTests.cs`; warehouse report queries still return product names. |
| Safe order | After Catalog contracts. Do **after** EX-D5. Do **before** dropping Inventory→Catalog Application exception (EX-AA3). |

---

#### EX-D7 — Purchasing.Domain → Catalog.Domain

| Field | Value |
|-------|--------|
| From project | `Purchasing.Domain.csproj` |
| To project | `Catalog.Domain.csproj` |
| Offending types | `Catalog.Domain.Unit` |
| Exact existing reference | `PurchaseRequisitionProduct.Unit`, `PurchaseOrderProduct.Unit`. `ProductId` is already scalar (comments in those files). |
| Why it exists | Same InvoiceProduct convention. |
| Correct ownership | Catalog owns Unit. PR/PO lines keep `UnitId` + commercial qty/price snapshots already on the line. |
| Proposed Contract/Event | `GetUnitNamesQuery`. Fill `Purchasing.Contracts` later (Stage 3/5) for remaining qty — not required to drop this Domain nav. |
| Files to modify | `PurchaseRequisitionProduct.cs`, `PurchaseOrderProduct.cs`; Purchasing Application DTOs if they flatten `Unit.Name`; Domain csproj; OrgContext; Architecture tests. |
| Schema risk | Low. |
| Regression risk | Medium — no Angular purchasing UI yet; API GetById/list may show unit name. |
| Tests required | Architecture.Tests; `PurchaseOrderTests`, `PurchaseRequisitionTests`; `LinkInvoiceCommandHandlerTests`. |
| Safe order | After Catalog unit-name contract. Can run in parallel with EX-D3. |

---

#### EX-AD1 — Sales.Application → MasterData.Domain (**stale**)

| Field | Value |
|-------|--------|
| From project | `Sales.Application.csproj` |
| To project | `MasterData.Domain` (via `MasterData.Application` ProjectReference) |
| Offending types | Allow-list says Unit/UnitDto. **Actual:** `Sales.Application/MappingProfile.cs` is empty. |
| Exact existing reference | `Sales.Application.csproj` → `MasterData.Application.csproj`. |
| Why it exists | Leftover from pre-Catalog relocation. |
| Correct ownership | Sales will later use Catalog.Contracts / MasterData.Contracts when persisted. Not now. |
| Proposed Contract/Event | None for this deletion. |
| Files to modify | `Sales.Application.csproj`; `ModuleLayerDependencyTests.cs` both Sales/MasterData rows (Domain + Application). |
| Schema risk | None. |
| Regression risk | None (Sales has **zero** handlers). |
| Tests required | Architecture.Tests. |
| Safe order | **First Application item**, with EX-AA1. |

---

#### EX-AD2 — CommercialDocuments.Application → Catalog.Domain

| Field | Value |
|-------|--------|
| From project | CommercialDocuments.Application (transitive via Domain + Catalog.Application) |
| To project | Catalog.Domain |
| Offending types | `Unit` used in `Invoices/MappingProfile.cs` `CreateMap<Unit, UnitDto>()` |
| Exact existing reference | MappingProfile; GetById already uses `GetProductNamesQuery` (Catalog.Contracts) per allow-list comment. |
| Why it exists | AutoMapper Unit↔UnitDto. |
| Correct ownership | Map `UnitId` only; fill `UnitName` from `GetUnitNamesQuery`. |
| Proposed Contract/Event | Catalog unit names (EX-D3). |
| Files to modify | MappingProfile; InvoiceProductDto if it embeds UnitDto; remove Catalog.Application ProjectReference after EX-AA2. |
| Schema risk | None. |
| Regression risk | Medium — invoice form units. |
| Tests required | Architecture.Tests; invoice GetById. |
| Safe order | With EX-D3 and EX-AA2. |

---

#### EX-AD3 — Parties.Application → MasterData.Domain

| Field | Value |
|-------|--------|
| From | Parties.Application (no direct MasterData.Domain csproj; uses navs from Parties.Domain) |
| To | MasterData.Domain |
| Offending types | `Country`, `City`, `District` |
| Exact existing reference | `Dealers/MappingProfile.cs` `src.Country.Name` etc.; `PartyAddresses/MappingProfile.cs`; Dealer query Includes. |
| Why it exists | DTO name flattening. |
| Correct ownership | Query MasterData.Contracts by IDs after loading Dealer. |
| Proposed Contract/Event | Geo name queries. |
| Files to modify | MappingProfiles; GetById/GetList/Search handlers (stop Including Country/City/District). |
| Schema risk | None. |
| Regression risk | Medium — dealer Angular screens. |
| Tests required | Architecture.Tests; dealer search. |
| Safe order | After EX-D4. |

---

#### EX-AD4 — Inventory.Application → CommercialDocuments.Domain

| Field | Value |
|-------|--------|
| From project | `Inventory.Application.csproj` → `CommercialDocuments.Domain.csproj` |
| To project | CommercialDocuments.Domain |
| Offending types | `CommercialDocuments.Domain.Invoice` |
| Exact existing reference | `IRepository<Invoice>` in: `Transactions/Commands/CreateTransactionByInvoiceCommandHandler.cs`; `DeleteCommandHandler.cs`; `UpdateCommandHandler.cs`; `DeleteListCommandHandler.cs`; `Queries/GetByIdQueryHandler.cs`; `Queries/SearchQueryHandler.cs`; `Transactions/Integration/TransactionJournalPostingService.cs` (`ResolveInvoiceCounterKeyAsync`); MappingProfile `CreateMap<Invoice, Transaction>()`. |
| Why it exists | Invoice-linked stock: copy lines, stamp `TransactionId` back on Invoice, choose GL accounts from invoice type, show source invoice code on transaction DTO. |
| Correct ownership | CommercialDocuments owns Invoice. Inventory must not load or update Invoice. Use Contracts: existing `GetInvoiceReferenceQuery`; extend with inventory-impact DTO (type, products, stock id, whether auto-create). **Do not** expose Invoice entity. If Invoice.TransactionId must be updated, add `SetInvoiceInventoryTransactionCommand` on CommercialDocuments.Contracts (owner writes its table). |
| Proposed Contract/Event | `GetInvoiceInventoryImpactQuery` (id, typeId, code, stockId, lines: productId/unitId/qty); `SetInvoiceLinkedTransactionCommand`; keep `CreateTransactionByInvoiceCommand` as Inventory.Contracts inbound from CommercialDocuments. |
| Files to modify | All handlers listed; MappingProfile Invoice maps; `Inventory.Application.csproj`; Architecture tests. |
| Schema risk | None if Invoice.TransactionId stays and is written by CommercialDocuments. |
| Regression risk | **High** — sales/purchase invoice auto-create stock; cancel/redo; journal posting accounts. |
| Tests required | Architecture.Tests; Accounting.Integration invoice posting; Application.Tests `GetInvoiceReferenceQueryHandlerTests`; inventory create-by-invoice path. |
| Safe order | After invoice Contracts extended. After cheap deletions. Before claiming Application→Domain = 0. |

---

#### EX-AD5 — Inventory.Application → Catalog.Domain

| Field | Value |
|-------|--------|
| From | Inventory.Application |
| To | Catalog.Domain |
| Offending types | `Product`, `ProductDto` via `GetListByBalanceQueryHandler` (`Inventory.Application/Products/Queries/GetListByBalanceQueryHandler.cs`) `IRepository<Product>` |
| Exact existing reference | That handler + Transaction/Inventory MappingProfiles mapping Product/Unit DTOs. |
| Why it exists | Stock-balance product list built by joining Catalog products with inventory qty. |
| Correct ownership | Catalog.Contracts product reference + Inventory balance. New Inventory query DTO owned by Inventory, populated with names from Contracts. |
| Proposed Contract/Event | `GetProductReferenceQuery` / names; Inventory keeps qty math. |
| Files to modify | `GetListByBalanceQueryHandler.cs`; MappingProfiles; remove Catalog.Application ref with EX-AA3. |
| Schema risk | None. |
| Regression risk | Medium — inventory balance screen. |
| Tests required | Architecture.Tests; Angular `/inventory/balances`. |
| Safe order | With EX-D6 / EX-AA3. |

---

#### EX-AD6 — Inventory.Application → MasterData.Domain

| Field | Value |
|-------|--------|
| From | Inventory.Application |
| To | MasterData.Domain.Currency |
| Offending types | `Currency` |
| Exact existing reference | `TransactionJournalPostingService` `provider.GetRequiredService<IRepository<Currency>>()`. |
| Why it exists | Pick posting currency for inventory journal. |
| Correct ownership | `MasterData.Contracts.GetDefaultCurrencyQuery` / `GetCurrencyNamesQuery` (both exist). Accounting posting already takes `CurrencyId`. |
| Proposed Contract/Event | Existing currency contracts. Also replace the Invoice repository call in the same service (EX-AD4). |
| Files to modify | `TransactionJournalPostingService.cs`; drop MasterData.Application ProjectReference if unused. |
| Schema risk | None. |
| Regression risk | Medium — inventory GL posting currency. |
| Tests required | Architecture.Tests; Accounting.Integration if it covers inventory journals. |
| Safe order | Immediate after confirming GetDefaultCurrencyQuery semantics match. |

---

#### EX-AD7 — Treasury.Application → CommercialDocuments.Domain

| Field | Value |
|-------|--------|
| From project | `Treasury.Application.csproj` → `CommercialDocuments.Domain.csproj` |
| To project | CommercialDocuments.Domain |
| Offending types | `Invoice` |
| Exact existing reference | `IRepository<Invoice>` **and UpdateAsync on Invoice** in: `CancelFinancialCommandHandler.cs`; `CreateFinancialPaidInvoiceHandler .cs`; `DeleteCommandHandler.cs`; `DeleteListCommandHandler.cs`; `PostTransactionCommandHandler.cs`; `RedoInvoiceCommandHandler.cs`; `UpdateCommandHandler.cs`; `CreateCommandHandler.cs`. MappingProfile maps Invoice↔Financial. GetById already uses `GetInvoiceNetsQuery` (legal). |
| Why it exists | Payment allocation: Treasury writes `Invoice.Paid` / `Remaining` / `Credit` when creating/cancelling financials. |
| Correct ownership | **Critical split:** CommercialDocuments owns the invoice document. **Receivables owns customer settlement. Payables owns supplier settlement. Treasury owns cash movement.** Do **not** keep mutating Invoice as the payment ledger. Target: Treasury posts Financial + publishes existing payment events; AR/AP already handle `CustomerPaymentPostedIntegrationEvent` / `SupplierPaymentPostedIntegrationEvent`. Extend CommercialDocuments.Contracts only for **read** (`GetInvoicePaymentInfoQuery`: id, type, dealerId, outstanding, currencyId, rate snapshot). Application of payment goes to Receivables/Payables Contracts (`ApplyPaymentToInvoice` or rely on existing event handlers). Stop `IRepository<Invoice>.UpdateAsync` in Treasury. |
| Proposed Contract/Event | `GetInvoicePaymentInfoQuery`; keep payment posted events; optionally `Receivables.Contracts`/`Payables.Contracts` apply-payment commands if events are insufficient for the paid-invoice UI path. |
| Files to modify | All Financial command handlers listed; `Financials/MappingProfile.cs`; `Treasury.Application.csproj`; possibly Invoice remaining fields become projections. |
| Schema risk | Medium **functionally** — Invoice.Paid/Remaining are still columns. Do not drop them in Stage 1. Stop writing them from Treasury; add a follow-up to treat them as denormalized cache updated by AR/AP or stop displaying them from Invoice. |
| Regression risk | **Highest Stage 1 card.** Customer/supplier receipts, cancel, redo, paid-invoice shortcut. |
| Tests required | Architecture.Tests; `CustomerPaymentPostedIntegrationEventHandlerTests`; `SupplierPaymentPostedIntegrationEventHandlerTests`; Financial post/cancel/redo; Angular treasury receipts/payments. |
| Safe order | After confirming AR/AP event handlers already apply payments. If they do, Treasury Invoice updates may be duplicate writers — remove Treasury writes first, keep reads via Contracts. |

---

#### EX-AD8 — Treasury.Application → MasterData.Domain

| Field | Value |
|-------|--------|
| From | Treasury.Application (transitive Domain navs) |
| To | MasterData.Domain |
| Offending types | `Currency` (MappingProfile `src.Currency.Name`; paid-invoice `invoice.Currency?.Rate`) |
| Exact existing reference | `FinancialAccounts/MappingProfile.cs`; `CreateFinancialPaidInvoiceHandler .cs`; query Includes `"Currency,..."`. |
| Why it exists | DTO currency name; paid-invoice rate fallback. |
| Correct ownership | Names via MasterData.Contracts. Rate for a **new** financial from invoice snapshot (`Invoice.Rate` via payment-info contract), never live Currency.Rate for posted docs. |
| Proposed Contract/Event | Existing currency names; invoice payment-info includes snapshot rate. |
| Files to modify | MappingProfile; paid-invoice handler; Financial/FinancialAccount query Includes. |
| Schema risk | None. |
| Regression risk | Medium. |
| Tests required | Architecture.Tests; financial account GetById currency name. |
| Safe order | After EX-D1; with EX-AD7 for paid-invoice. |

---

#### EX-AD9 — Reporting.Application → CommercialDocuments.Domain

| Field | Value |
|-------|--------|
| From project | `Reporting.Application.csproj` |
| To project | CommercialDocuments.Domain |
| Offending types | `Invoice`, `InvoiceType` |
| Exact existing reference | `IRepository<Invoice>` in `GetDealerBalanceReportQueryHandler`, `GetDealerStatementReportQueryHandler`, `GetSalesBalanceReportQueryHandler`. |
| Why it exists | Reporting is a read aggregator with no Domain. |
| Correct ownership | Long-term: projections (`CustomerAgingReadModel`, `SalesSummaryReadModel`) fed by integration events. Short-term: **do not** add dozens of sync Contracts. Prefer SQL views / read-only EF types in Reporting.Infrastructure mapped to Invoice **table** without referencing CommercialDocuments.Domain assembly, **or** a single CommercialDocuments.Contracts report DTO if a view is too large for Stage 1. |
| Proposed Contract/Event | Stage 14 projections. Stage 1 option A: Reporting.Infrastructure raw SQL/views. Option B: keep this cluster documented as the only foundational exception with ADR + removal plan. **Do not** add more exception rows. |
| Files to modify | Three dealer/sales report handlers; csproj; possibly new views. |
| Schema risk | Views are additive. |
| Regression risk | High for Angular reporting screens if query shape changes. |
| Tests required | Architecture.Tests; Angular `/reporting/dealers/...` and `/reporting/sales/balance`. |
| Safe order | **Last** in Stage 1. Prefer ADR if timeboxed. |

---

#### EX-AD10 — Reporting.Application → Parties.Domain

Same cluster as EX-AD9. Handlers: dealer balance/statement (`IRepository<Dealer>`). Safe-movement also uses `IRepository<Dealer>`. Same treatment.

---

#### EX-AD11 — Reporting.Application → Treasury.Domain

Handlers: `GetSafeBalanceReportQueryHandler`, `GetSafeMovementReportQueryHandler` (`Financial`, `CashBox`, `FinancialType`). Same Reporting cluster.

---

#### EX-AD12 — Reporting.Application → Inventory.Domain

Handlers: `GetBalanceReportQueryHandler`, `GetMovementReportQueryHandler` (`TransactionProduct`, `TransactionType`). Same cluster. Angular `/reporting/warehouse/*`.

---

#### EX-AD13 — Reporting.Application → MasterData.Domain

Allow-list: warehouse/financial reports read Currency. **No direct MasterData ProjectReference** — transitive via Treasury/Inventory Includes. Removing EX-D1/D6 navs may auto-clear this if reports stop touching Currency types. Verify with NetArchTest rather than keeping the row.

---

#### EX-AD14 — Reporting.Application → Catalog.Domain

Allow-list: warehouse reports read Product/Classification via `TransactionProduct.Product`. Same as EX-AD13 — likely falls when EX-D6 drops Product navigation. Replace product names via Catalog.Contracts or a reporting view column.

---

#### EX-AD15 — Organization.Application → MasterData.Domain

| Field | Value |
|-------|--------|
| From project | `Organization.Application.csproj` → `MasterData.Domain.csproj` (**direct**) |
| To project | MasterData.Domain |
| Offending types | `Country`, `Currency` |
| Exact existing reference | `Companies/Validators/CreateCompanyCommandValidator.cs` and `UpdateCompanyCommandValidator.cs`: `IRepository<Country>`, `IRepository<Currency>`. |
| Why it exists | Existence check for `Company.CountryId` / `DefaultCurrencyId` (already scalar on Domain). |
| Correct ownership | MasterData.Contracts existence queries. Prefer one `ValidateOrganizationReferenceSetQuery(CountryId, CurrencyId)` to avoid chatty calls. |
| Proposed Contract/Event | `ExistsCountryQuery` / `ExistsCurrencyQuery` or grouped `ValidateCompanyMasterDataQuery`. |
| Files to modify | Both company validators; `Organization.Application.csproj`; Architecture tests. |
| Schema risk | None. |
| Regression risk | Low. |
| Tests required | Architecture.Tests; `Tests/Application.Tests/OrganizationCompanyBranchValidatorTests.cs`. |
| Safe order | Immediately after MasterData exists-queries. Easy win. |

---

#### EX-AD16 — Organization.Application → SaaS.Domain

| Field | Value |
|-------|--------|
| From project | `Organization.Application.csproj` → `SaaS.Domain.csproj` (**direct**) |
| To project | SaaS.Domain |
| Offending types | `SaaS.Domain.Tenant` |
| Exact existing reference | Same company validators: `IRepository<Tenant>` for `Company.TenantId`. |
| Why it exists | Stage-1 tenant retrofit existence check. |
| Correct ownership | SaaS owns Tenant. Use `SaaS.Contracts.EnsureTenantActiveQuery` (exists) or add `TenantExistsQuery` if inactive tenants may be assigned. Later: `ValidateTenantCanCreateCompanyQuery` when entitlements are enforced. |
| Proposed Contract/Event | `EnsureTenantActiveQuery` and/or `GetTenantReferenceQuery`. |
| Files to modify | Company create/update validators; Organization.Application.csproj; Architecture tests. |
| Schema risk | None. |
| Regression risk | Low. |
| Tests required | Architecture.Tests; OrganizationCompanyBranchValidatorTests. |
| Safe order | With EX-AD15. |

---

#### EX-AA1 — Sales.Application → MasterData.Application (**stale**)

Same as EX-AD1. Remove `MasterData.Application` ProjectReference. Drop allow-list row.

---

#### EX-AA2 — CommercialDocuments.Application → Catalog.Application

| Field | Value |
|-------|--------|
| From project | `CommercialDocuments.Application.csproj` |
| To project | `Catalog.Application.csproj` |
| Offending types | `Catalog.Application` `UnitDto` |
| Exact existing reference | `Invoices/MappingProfile.cs` Unit↔UnitDto. Also unused `MasterData.Application` ProjectReference on the same csproj — remove it even though it is **not** allow-listed (unused IL currently keeps tests green). |
| Why it exists | Unit DTO mapping after Unit moved to Catalog. |
| Correct ownership | Catalog.Contracts. |
| Proposed Contract/Event | Unit names. |
| Files to modify | MappingProfile; csproj (drop Catalog.Application and MasterData.Application). |
| Schema risk | None. |
| Regression risk | Medium. |
| Tests required | Architecture.Tests. |
| Safe order | With EX-D3 / EX-AD2. |

---

#### EX-AA3 — Inventory.Application → Catalog.Application

| Field | Value |
|-------|--------|
| From project | `Inventory.Application.csproj` |
| To project | `Catalog.Application.csproj` |
| Offending types | `ProductDto`, `UnitDto` |
| Exact existing reference | Transaction MappingProfile Product/Unit DTO maps; GetListByBalanceQueryHandler. Also unused `MasterData.Application` ProjectReference — remove with EX-AD6. |
| Why it exists | Catalog CQRS relocated from Inventory. |
| Correct ownership | Catalog.Contracts. |
| Proposed Contract/Event | Product/unit references. |
| Files to modify | MappingProfiles; GetListByBalanceQueryHandler; csproj. |
| Schema risk | None. |
| Regression risk | Medium. |
| Tests required | Architecture.Tests; inventory balance. |
| Safe order | With EX-D6 / EX-AD5. |

---

### Stage 1 goal

| Allow-list | Target |
|------------|--------|
| `AcceptedDomainExceptions` | **empty** |
| `AcceptedApplicationApplicationExceptions` | **empty** |
| `AcceptedApplicationDomainExceptions` | **empty**, or **only** Reporting (EX-AD9–AD14) with an ADR naming owner + Stage 14 removal plan |

Do not keep Sales/Inventory MasterData Domain rows — they are already stale.

Also in Stage 1 (test hardening, not a new exception): add Catalog, SaaS, Advances to `ModuleInfrastructureDependencyTests.ModuleInfrastructures`.

---

## Stage 2 — Application isolation leftovers + purposeful Contracts

Only after Stage 1 Domain navs are gone.

Add Contracts the cards above proved necessary. Do not expose Domain entities.

| Module | Add | Consumers |
|--------|-----|-----------|
| MasterData.Contracts | Country/City/District/PaymentType exists + names; grouped org validation | Organization, Parties, Treasury, Invoice |
| Catalog.Contracts | `GetProductReferenceQuery`, `GetUnitNamesQuery` | Invoice, Purchasing, Inventory, Reporting |
| SaaS.Contracts | `TenantExistsQuery` if distinct from `EnsureTenantActiveQuery` | Organization |
| CommercialDocuments.Contracts | `GetInvoiceInventoryImpactQuery`, `GetInvoicePaymentInfoQuery`, `SetInvoiceLinkedTransactionCommand` | Inventory, Treasury |
| Receivables/Payables.Contracts | apply-payment only if events are not enough | Treasury paid-invoice path |
| Administration.Contracts | `HasPermissionQuery` (Stage 7) | all |

Empty csproj files to leave empty until their stage: `Sales.Contracts`, `Purchasing.Contracts`, `Advances.Contracts`, `Reporting.Contracts`.

---

## Stage 3 — Complete Contracts between current contexts

Fill remaining look-ups used by later ERP completion. Avoid CRUD-in-Contracts.

---

## Stage 4 — Complete existing Accounting / AR / AP / Treasury / Advances

**Accounting** (reference module): verify chart, journal draft/post/reverse/cancel, fiscal period close, trial balance via reports later. External modules must keep using Accounting.Contracts.

**Receivables / Payables:** backend subledger exists; APIs `PayableController` / `ReceivableController` inherit `ControllerBase` **without** `[Authorize]` (unlike `CoreController`). Fix auth in Stage 7/16. No Angular. Treasury must stop being a second writer of Invoice.Paid (EX-AD7).

**Treasury:** unified model live. Remaining: cheque register entity if product needs status; Advances disbursement Contracts; EX-AD7.

**Advances:** map `Custody` / `CustodyHandover` in OrgContext + additive migration; Application commands matching domain methods; disburse via Treasury.Contracts; API + Angular `features/advances/`. DatabaseMigrator currently **omits** Advances.Domain.

---

## Stage 5 — Sales / Purchasing / Inventory integration

**Sales:** add `SalesQuotation`/`SalesOrder` (+ lines) to OrgContext (tables named in `[Table]` but missing from snapshot). Do not revive dropped `Order`. Wire `ReserveInventoryCommand`. Invoice stays CommercialDocuments.

**Purchasing:** fill Purchasing.Contracts; wire `InventoryReceipt` to PO via scalar `PurchaseOrderId` + events; three-way match in Purchasing.Application using CommercialDocuments.Contracts. RFQ only if product requires it.

**Inventory:** keep dual model until Angular movements migrate to hardened receipts/issues; never let Sales/Purchasing handlers mutate `InventoryBalance`.

---

## Stage 6 — Catalog / Parties / Organization

Catalog: Brand/PriceList/Property UI; ProductType already distinguishes stock vs service — do not force services through inventory.

Parties: Dealer remains the partner; profiles/contacts/addresses have API but little Angular.

Organization: Company/Settings Angular; do not swallow Currency/Country/Tenant.

---

## Stage 7 — IAM inside Administration (do not create Identity)

Keep `IPasswordHasher`, `PasswordHasher`, `MustResetPassword`.

Work items:

- `[Authorize]` on `PayableController`, `ReceivableController`, `FinancialTransferController`
- Contracts: `HasPermissionQuery`, `GetCurrentUserAccessQuery`, `CanAccessBranchQuery`
- Angular User/Role screens under a new `features/administration/` (not a new backend module)
- `UserDto.Password` remains on the type for input; mapping already `.Ignore()` on output — keep it that way; never return hashes
- `AuthController` is `[AllowAnonymous]` including `CheckPassword` / `HavePassword` — lock down in Stage 16
- One role per user (`User.RoleId`); do not add UserRole without a migration plan

Extract `Modules/Identity` only if a later audit shows Administration mixed with non-IAM ownership **and** tables can move without rename. Current audit: **do not extract**.

---

## Stage 8 — Workflow & Approvals (new module, later)

No Workflow types exist. PR “approval” is `PurchaseRequisition` status. Create `Modules/Workflow/{Domain,Application,Contracts,Infrastructure}` only after Stage 1–2. First consumer: PurchaseRequisition submit. Workflow never mutates PO.

---

## Stage 9 — Budgeting (new module, later)

No CostCenter/Department/Budget types. SaaS seeds feature key `Budgeting` only. Department, if introduced, belongs in Organization. Actuals via Accounting.Contracts, never Accounting.Domain.

---

## Stage 10 — Tax (new module, later)

`Invoice.Tax` / `TaxType`; `Company.TaxRegistrationNumber`. No TaxCode engine. Snapshot on posted invoices. ETA/ZATCA adapters in infrastructure, not Domain.

---

## Stage 11 — Fixed Assets (new module, later)

Only COA seed strings (e.g. accumulated depreciation accounts). Category stores account **IDs**. Post via Accounting.Contracts. Straight-line first.

---

## Stage 12 — SaaS / multi-tenant enforcement

Existing: Tenant/Plan/Feature/PlanFeature/Subscription; `Company.TenantId`; `ITenantFeatureService` (unused outside SaaS).

Introduce `ICurrentTenant`. Produce `docs/saas/tenant-ownership-matrix.md` from the 87 snapshot tables. Do not stamp TenantId on every table. Enforce limits (`TenantLimit`) on CreateUser/Company/Branch/Stock. Tests: Tenant A cannot read/update/delete/reference Tenant B.

---

## Stage 13 — Outbox / Inbox

Replace in-process-only reliability. `OutboxMessage` in same `OrgContext` transaction. Background dispatcher. Inbox/idempotency for InvoicePosted / PaymentPosted / GoodsReceiptPosted. AR/AP unique indexes already exist — handlers must still be idempotent on EventId.

---

## Stage 14 — Reporting projections

Replace EX-AD9–AD14. Required reports listed in the master prompt §42. Reporting must not mutate transactional tables.

---

## Stage 15 — Angular missing features

Preserve current feature folders and legacy redirects in `OrgSys.Angular/src/app/app.routes.ts`. Do **not** move back to administration/financial/invoices/warehouse/transactions/customers-suppliers/reports.

Produce `docs/angular/backend-frontend-matrix.md` (prompt name) from live controllers.

Priority UI: Administration users/roles; Purchasing; AR/AP; Company/Settings; Advances; Sales orders (after persistence); SaaS operator; Inventory issues/transfers/adjustments/batches/serials; Catalog Brand/PriceList.

Use existing unused `permissionGuard`.

---

## Stage 16 — Security / API / concurrency / observability

| Item | Current file | Action |
|------|--------------|--------|
| Unauthenticated money APIs | Payable/Receivable/FinancialTransfer controllers | `[Authorize]` + permissions |
| Anonymous password probes | `API/Controllers/Org/AuthenticationController.cs` | Authenticate or rate-limit `CheckPassword`/`HavePassword` |
| JWT | `API/Authentication/JwtTokenService.cs` | tenant/company/branch claims |
| 500 leak | `API/Middlewares/GlobalExceptionMiddleware.cs` | ProblemDetails; hide `ex.Message` |
| CORS | `Program.cs` | config-driven origins |
| Health / CorrelationId / OpenTelemetry | missing | API host only |
| Secrets | startup already rejects CHANGE_ME JWT/connection | keep; never commit real secrets |

---

## Stage 17 — End-to-end tests

Extend existing projects. Flows A Sales / B Purchasing / C Inventory / D Treasury / G multi-tenant as specified. Fixed Assets / Budget after those modules exist.

---

## Stage 18 — CI/CD + hygiene

No `.github/workflows` today. Add restore/build/Architecture.Tests/tests/Angular build. Stop tracking `Domain.zip`; ignore `*.zip`. Add DatabaseMigrator to `OrgSys.sln`. Rewrite stale `docs/dependency-rules.md` §3.

---

## Stage 19 — Final architecture documentation

`docs/architecture/table-ownership.md`, integration-events, ddd-rules, completion-report, remaining-risks — **only after code matches**.

---

## Explicit non-goals until the named stage

| Action | Wait until |
|--------|------------|
| New Identity csproj set | Stage 7 extraction decision (default: never) |
| New Workflow module | Stage 8 |
| New Budgeting module | Stage 9 |
| New Tax module | Stage 10 |
| New FixedAssets module | Stage 11 |
| Recreating Invoice in Sales | never |
| Recreating Custody in Treasury | never |
| Dropping Invoice.CurrencyId SQL FK to make tests green | never |
| Kafka | never unless deployment requires it |

---

## Current projects (for implementers)

**Hosts:** `API/API.csproj`, `OrgSys/OrgSys.csproj`, `OrgSys.Angular/OrgSys.Angular.esproj`

**BuildingBlocks:** SharedKernel, EventBus, Infrastructure, Localization, DatabaseMigrator (not in sln)

**Modules (16 × layers as they exist):** Accounting, Administration, Advances, Catalog, CommercialDocuments, Inventory, MasterData, Organization, Parties, Payables, Purchasing, Receivables, Reporting (no Domain), SaaS, Sales, Treasury — each with Domain (except Reporting), Application, Contracts, Infrastructure.

**Tests:** Architecture.Tests; Application.Tests; Accounting.Domain + Integration; Inventory.Domain + Integration; Domain.Tests for Advances, Catalog, Organization, Parties, Payables, Purchasing, Receivables, Sales, Treasury.

**Next implementation stage:** none — Stages 0–19 complete. Post-Stage 19 leftovers (PO↔GRN three-way match, price-list entries, Department UI, Domain CI tests) are closed. Do not invent Stage 20. Do not apply EF migrations without approval.
