# OrgSys Shared Business Capabilities Review

Analysis only — no source code has been moved. This document answers the brief's Phase 1
question: for every business concept currently shared across modules, who really owns it today,
who actually consumes it, and does it justify a new module (Parties / Catalog /
CommercialDocuments / ReferenceData) per the brief's own anti-over-split rule (§20: "If a proposed
module contains only one trivial enum and no real business ownership, do not create it").

Every row below is backed by a real class, file path, and consumer list found by direct
inspection (Read/Grep) of `D:\Work\MK\Source\OrgSys` on branch `Latest`, HEAD `723ad0f9` — nothing
here is guessed or inferred from the brief's own diagrams. This review is consistent with, and
supersedes where more specific, the existing `docs/module-ownership.md` and
`docs/modular-monolith-target-architecture.md` (written for a prior, differently-scoped brief
about layer-boundary hardening) — those documents already independently reached the same
conclusions for Product/Invoice ownership; this review adds the Parties/ReferenceData angle they
didn't cover and confirms/extends the rest with fresh evidence.

---

## 1. Party / Customer / Supplier (`Dealer`)

| | |
|---|---|
| **Concept** | A business partner that can be a customer, a supplier, or (conceptually) both |
| **Current Owner** | `Modules/Sales/Sales.Domain/Entities/Dealer.cs` — `[Table("Dealer")]`, plus `DealerGroup.cs` (name-only category lookup) and `Enums/DealerType.cs` (`Client = 1, Supplier = 2`) |
| **Actual Consumers** | `Accounting.Application` (`ReceivableAccountValidator.cs:6,31-57`, `PayableAccountValidator.cs:19` — `IRepository<Dealer>`, checks `TypeId`); `Receivables.Application/SetCustomerOpeningBalanceCommandHandler.cs:7,53`; `Payables.Application/SetSupplierOpeningBalanceCommandHandler.cs:7,54`; `Treasury.Domain/Entities/Financial.cs:6-9` (`DealerId`/`Dealer` nav) + `Treasury.Application` query handlers; `Inventory.Domain/Entities/{Product.cs,Transaction.cs}` (`DealerId`/`Dealer` nav) + `Inventory.Application` query handlers/MappingProfile; `Reporting.Application/Dealer/Queries/*` (filters `Dealer.TypeId`) — **6 modules outside Sales**, all via direct `Sales.Domain` reference, none via Contracts (`Sales.Contracts` has zero source files) |
| **Recommended Owner** | New **Parties** module (`Parties.Domain/Application/Contracts/Infrastructure`), owning the physically-unchanged `Dealer`/`DealerGroup`/`DealerType` (same `[Table("Dealer")]` mapping) — *or*, as a lower-effort alternative, keep `Dealer` in `Sales.Domain` and build out the currently-empty `Sales.Contracts` (`IDealerModuleApi`, `CustomerLookupDto`, `SupplierLookupDto`) so the 6 consumers stop reaching into `Sales.Domain` directly |
| **Reason** | This is the one concept in the whole review that passes the brief's §20 "genuinely shared and independently owned" test: it is consumed identically by Accounting, Treasury, Inventory, Receivables, Payables, and Reporting — none of those are Sales-specific relationships (AR needs the customer view, AP needs the supplier view, Treasury needs both for payments/receipts). Unlike Invoice/Order, `Dealer` is not itself a Sales *workflow* — it is master data Sales happens to currently host. `Dealer` already implements the brief's "one Party, role-flagged" model in miniature (one table, `TypeId` decides Customer vs Supplier) — it does **not** need to be redesigned into `Party`/`PartyRole`/`Contact`/`Address` sub-entities, since no `Contact`/`Address`/multi-role requirement exists anywhere in the current system (`Address` is a plain `string?` field; there is no "Both" role and no evidence anyone needs one) |
| **Database Impact** | None if physically relocated with the same `[Table("Dealer")]`/`[Table("DealerGroup")]` attributes and column names — this is a namespace/project move, not a schema change (verify with `dotnet ef migrations add VerifyNoOp` producing an empty diff, per this repo's established verification pattern) |
| **Code Impact** | High surface area: 6 consuming modules' `.csproj` references and `using` statements change from `Sales.Domain`/`Sales.Application` to `Parties.Contracts` (or `Sales.Contracts`, if the lower-effort path is chosen); `Sales.Application`'s own Dealer CQRS handlers (`Modules/Sales/Sales.Application/Dealers/**`) move wholesale |
| **Risk** | Medium — mechanical relocation (same pattern already proven for `Sales.Application -> Inventory.Contracts`/`Treasury.Contracts` in commit `723ad0f9`), but touches more files than any other single item in this review. **This is the only concept in this review where creating a brand-new module is evidence-justified.** |

## 2. Product / Product Category / Unit of Measure ("Catalog")

| | |
|---|---|
| **Concept** | Product master data, its category, and its unit of measure |
| **Current Owner** | `Product` → `Modules/Inventory/Inventory.Domain/Entities/Product.cs` (sole owner). Category-equivalent (`ClassificationId`/`Classification`) and `Unit` (`ProductUnit.UnitId`/`Unit`) → `Modules/MasterData/MasterData.Domain/Entities/{Classification,Unit}.cs` |
| **Actual Consumers** | Sales (`InvoiceProduct.ProductId`, `OrderProduct.ProductId` — bare FK, navigation deliberately dropped) and Purchasing (none — Purchasing has zero domain code) reference `Product` only by ID; the one place Sales needs a display name, it already calls `Inventory.Contracts.GetProductNamesQuery` (the correct pattern, already in production). `Reporting.Application` reads `Product`/`Classification` directly via EF `Include` — a documented, accepted, by-design exception (Reporting has no `Reporting.Domain` and reads every module's entities as a read-only aggregator) |
| **Recommended Owner** | No change. `Product` stays in **Inventory**; `Classification`/`Unit` stay in **MasterData** (→ReferenceData, see §5) |
| **Reason** | There is no god-module problem here and no duplication to resolve: exactly one `Product` table, exactly one category concept (`Classification`), exactly one `Unit` table; Sales/Purchasing already reference by ID only, never by navigation; a Contracts query already exists and is used correctly. `ProductCategory`/`ProductGroup`/`Barcode`(-entity)/`PriceList` do not exist anywhere in the codebase — inventing a `Catalog` module to hold concepts that don't exist yet would be pure speculative scaffolding, exactly what brief §20 warns against |
| **Database Impact** | None — no change recommended |
| **Code Impact** | None required. Optional, low-priority polish: expand `Inventory.Contracts` with a proper `ProductLookupDto` so `Reporting.Application`'s direct EF reads could eventually go through Contracts — this is Reporting hygiene, not a Catalog-module justification |
| **Risk** | None (no action recommended) |

## 3. Invoice ("CommercialDocuments")

| | |
|---|---|
| **Concept** | The commercial document — sales invoice, purchase invoice, sales return, purchase return |
| **Current Owner** | `Modules/Sales/Sales.Domain/Entities/{Invoice.cs, InvoiceProduct.cs, InvoiceType.cs}`. `InvoiceType` is a **data-driven lookup table**, not a C# enum — seeded with exactly 4 rows (`Infrastructure/Seed/InitialData.cs:782-799`): Sales Invoice, Purchase Invoice, Sales Return, Purchase Return, distinguished by `Group`/`InOut`. `Invoice.TypeId` (inherited from `BaseModel`) is the discriminator FK |
| **Actual Consumers** | `Treasury.Domain/Entities/FinancialInvoice.cs:14-17` (nav to `Invoice`, a join entity linking `Financial`↔`Invoice`) + `Treasury.Application` handlers/DTOs; `Inventory.Contracts/Transactions/{CreateTransactionByInvoiceCommand,DeleteTransactionByInvoiceCommand,SetTransactionStatusCommand}.cs` (already correctly Contracts-based, FK-only); `Reporting.Application` (direct reads, accepted exception); legacy root `Application/Commands/Org/Financials/Integration/JournalInvoice/InvoiceJournalIntegration.cs:38` — **already branches on `TypeId is 2 or 4` to route Purchase-side GL postings today**, proving the 4-way discriminator is live production logic, not a theoretical design |
| **Recommended Owner** | No change. `Invoice`/`InvoiceProduct`/`InvoiceType` stay in **Sales** |
| **Reason** | Exactly one physical `Invoices`/`InvoiceProducts`/`InvoiceTypes` table set exists (confirmed via `OrgContext` `DbSet<>` declarations and all 20 migration files — no second invoice table anywhere). `Purchasing` has zero domain code to migrate or protect (only `AssemblyMarker.cs` placeholder classes) — there is no duplicate `PurchaseInvoice` to consolidate. No `DebitNote`/`CreditNote`/`InvoiceStatus` entities exist. Moving this proven, working, single-owner model into a new `CommercialDocuments` module would split Sales' own workflow (Quotation → SalesOrder → Invoice) from its own core document for no functional gain — exactly the "do NOT move the complete Sales process into CommercialDocuments" the brief itself warns against, except here it would be the reverse: pulling the shared document *out* of a workflow that already handles it correctly. The real, current gap is that `Sales.Contracts` is completely empty, not that Invoice has the wrong owner |
| **Database Impact** | None — no change recommended |
| **Code Impact** | None required now. When Purchasing grows real workflows (Phase 8/14), build `Sales.Contracts.{ISalesInvoiceModuleApi, IPurchaseInvoiceModuleApi, SalesInvoiceLookupDto}` so Purchasing/Treasury consume Contracts instead of `Sales.Domain` — already scoped in `docs/modular-monolith-target-architecture.md` §4, just not implemented yet |
| **Risk** | None (no action recommended now); Medium-High if/when the GL-posting bridge (`InvoiceJournalIntegration`) is eventually moved into Accounting — flagged as the highest-risk item in `docs/legacy-migration-map.md`, out of scope for this review |

## 4. Accounting / Treasury / Receivables / Payables

| | |
|---|---|
| **Concept** | Chart of accounts & journal posting (Accounting); cash/bank/payment instruments (Treasury); customer/supplier open-item ledgers (Receivables/Payables) |
| **Current Owner** | Accounting: `Account, AccountType, Journal, JournalItem, JournalType, FiscalYear, FiscalPeriod` — all in `Modules/Accounting/Accounting.Domain/Entities`. Treasury: `Bank, BankBranch, BankAccount, CashBox, FinancialAccount, Financial, FinancialInvoice, FinancialTransfer, FinancialType, Outlay` — all in `Modules/Treasury/Treasury.Domain/Entities`. Receivables/Payables: **no entities at all** — `Modules/Receivables/Receivables.Domain` and `Modules/Payables/Payables.Domain` contain only `AssemblyMarker.cs` |
| **Actual Consumers** | Accounting/Treasury are consumed by each other and by Sales/Inventory/Receivables/Payables via a mix of documented "accepted exception" direct Domain/Application references (see `docs/dependency-rules.md` §3) — a real, pre-existing layer-boundary debt, but **not an ownership-ambiguity problem**: every entity has exactly one owning module already, matching the brief's own Accounting/Treasury target lists almost line-for-line. Receivables'/Payables' only features (`SetCustomerOpeningBalanceCommandHandler`, `SetSupplierOpeningBalanceCommandHandler`) build `Journal` rows by reaching directly into `Accounting`/`Sales`/`MasterData` — there is no owned Receivables/Payables data to speak of |
| **Recommended Owner** | No change to Accounting or Treasury ownership. Receivables/Payables need **new development** (OpenItem/Allocation/Settlement/Aging), not a migration of existing shared concepts |
| **Reason** | Accounting and Treasury are already correctly scoped, single-owner modules — nothing to decompose. Receivables/Payables' emptiness is a feature gap, not a "shared business concept living in the wrong place" problem; there is nothing currently duplicated or misowned to fix by module restructuring |
| **Database Impact** | None recommended here |
| **Code Impact** | None recommended here (building real Receivables/Payables domain models is a separate initiative, outside this decomposition task's scope) |
| **Risk** | N/A |

## 5. MasterData → ReferenceData

See `docs/masterdata-decomposition.md` for the full per-entity breakdown. Summary: **all 8**
entities in `MasterData.Domain` (`Country, City, District, Currency, Classification, PaymentType,
ReferenceType, Unit`) are genuinely lightweight reference/lookup data — no `Product`, `Account`,
`Warehouse`, or other business aggregate is hiding there. MasterData is **not** currently a
"god module" in the sense the brief warns about; it already matches the brief's own definition of
`ReferenceData`. The real gap is that `MasterData.Contracts` is completely empty (zero source
files) — every one of the 6 consuming modules (Accounting, Treasury, Sales, Inventory, Payables,
Receivables) has a documented direct reference into `MasterData.Domain`/`MasterData.Application`
instead of a Contracts boundary, and two DTOs (`CountryDto`, `PaymentTypeDto`, `ReferenceTypeDto`)
leak the domain entity by inheriting from it directly instead of being independent shapes.

**Database Impact**: none. **Code Impact**: low-priority rename (`MasterData` → `ReferenceData`,
cosmetic, optional) plus the real fix — building a proper `.Contracts` project with lookup DTOs.
**Risk**: low.

---

## Answers to the brief's 10 required questions

1. **Every shared business concept found**: Party/Dealer, Product/Category/Unit, Invoice/InvoiceType, Account/Journal, CashBox/Bank/FinancialAccount/Financial, Country/City/District/Currency/PaymentType/ReferenceType/Classification/Unit. (Receivables/Payables open-item ledgers do not exist yet, so there is no "shared concept" there to relocate.)
2. **Where each currently lives**: see the "Current Owner" row per concept above; full entity-level detail for MasterData in `docs/masterdata-decomposition.md`.
3. **Which modules use each**: see "Actual Consumers" rows above (file:line evidence gathered directly, not inferred).
4. **Recommended owner**: Dealer/Party → new `Parties` module (or `Sales.Contracts` build-out as lower-effort alternative); Product/Category/Unit → unchanged (Inventory/MasterData); Invoice → unchanged (Sales); Accounting/Treasury → unchanged; MasterData → unchanged content, optional rename to `ReferenceData`.
5. **Is a new module truly necessary?** Only for **Parties** — it is the sole concept that passes the brief's own §20 test (genuinely shared, independently owned, consumed identically by 6 unrelated modules). `Catalog` and `CommercialDocuments` are **not** justified — both concepts already have exactly one clean owner, no duplication, and a working (if incompletely exposed) Contracts pattern.
6. **What should remain in MasterData/ReferenceData**: all 8 current entities (`Country, City, District, Currency, Classification, PaymentType, ReferenceType, Unit`) — none need to move out.
7. **What must not stay in SharedKernel**: nothing currently in `BuildingBlocks/OrgSys.SharedKernel` is a business entity today (verified: it holds only `BaseModel`/`MovementModel` and generic CQRS/Result plumbing) — this rule is already satisfied, no action needed.
8. **Proposed final module dependency diagram**: unchanged from `docs/modular-monolith-target-architecture.md` §11, with one addition — a `Parties` module sitting alongside `MasterData`/`Organization`/`Administration` at the reference-data tier, depended on via `Parties.Contracts` by Sales, Purchasing, Treasury, Inventory, Accounting, Receivables, Payables, and Reporting.
9. **Database impact**: zero schema changes required for any recommendation in this review — every relocation preserves existing table names/columns (`[Table("Dealer")]` etc.); this is an ownership/project-reference change, not a migration.
10. **Migration phases**: see the phase list already defined in the brief (§23) — this review recommends executing only **Phase 3 (Parties)** as a new-module extraction; **Phase 4 (Catalog)** and **Phase 5 (CommercialDocuments)** should be marked "not needed — see this review" rather than attempted; **Phase 6 (MasterData→ReferenceData)** should be scoped down to "build Contracts + optional rename," not a structural split.
