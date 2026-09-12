# OrgSys MasterData Decomposition

Every class in `Modules/MasterData/MasterData.Domain/Entities` — **exactly 8, confirmed by
directory listing, no others exist** — classified by real current usage. Companion to
`docs/shared-business-capabilities-review.md`. All entities inherit `OrgSys.SharedKernel.BaseModel`
(`Id, CodeNumber, Code, MaskText, ParentId, TypeId, Hide, ImgPath, Status`); only entity-specific
fields are listed below.

**Headline finding**: MasterData is not a "god module" containing unrelated ERP business
aggregates — every one of its 8 entities is genuinely lightweight reference/lookup data. Nothing
here needs to move to Catalog, Accounting, Treasury, or any other business module. The real
problem is architectural, not content-based: `MasterData.Contracts` has **zero source files** —
every consumer reaches directly into `MasterData.Domain`/`MasterData.Application` instead of a
Contracts boundary, and this is currently allow-listed rather than fixed
(`Tests/Architecture.Tests/{ModuleDependencyTests,ModuleLayerDependencyTests}.cs`).

| Current Concept | Current Location | True Owner | Target Module | Shared By | Migration Required | Notes |
|---|---|---|---|---|---|---|
| `Country` (`Name`) | `Modules/MasterData/MasterData.Domain/Entities/Country.cs` | MasterData | ReferenceData (rename only, or stays MasterData) | Treasury (`Bank.Country`, `BankBranch.Country`), Sales (`Dealer.Country`) — both via EF navigation, both documented accepted exceptions in `ModuleDependencyTests.cs` | No structural move. Optional: expose `CountryLookupDto` via a new `MasterData.Contracts` (or `ReferenceData.Contracts`) project | Simplest entity in the module — no cross-refs of its own |
| `City` (`Name`, `CountryId`→`Country`) | `Modules/MasterData/MasterData.Domain/Entities/City.cs` | MasterData | ReferenceData | Treasury (`BankBranch.City`), Sales (`Dealer.City`) — navigation, accepted exceptions | Same as Country — Contracts DTO only | — |
| `District` (`Name`, `CountryId`, `CityId`→`Country`/`City`) | `Modules/MasterData/MasterData.Domain/Entities/District.cs` | MasterData | ReferenceData | Treasury (`BankBranch.District`), Sales (`Dealer.District`) — navigation, accepted exceptions | Same as Country/City | — |
| `Currency` (`Name`, `Rate`, `IsDefault`) | `Modules/MasterData/MasterData.Domain/Entities/Currency.cs` | MasterData | ReferenceData | Widest fan-out of the 8: Accounting (`Journal.Currency`), Treasury (`Financial.Currency`, `FinancialAccount.Currency`, `FinancialTransfer.Currency`), Sales (`Invoice.Currency`), Receivables/Payables opening-balance handlers (`IRepository<Currency>`), Reporting (`GetSafeMovementReportQueryHandler`) — all documented accepted exceptions at both Domain and Application layers | No structural move — build `CurrencyLookupDto` in Contracts; this is the highest-value Contracts DTO to add first given fan-out | Borderline candidate for "should this be closer to Accounting/Treasury" but it is a pure rate lookup, not a business aggregate — stays reference data |
| `Classification` (`Name`, `BePurchased`, `BeSold`, `BeManufactured`) | `Modules/MasterData/MasterData.Domain/Entities/Classification.cs` | MasterData | ReferenceData (see note) | Inventory (`Product.Classification` navigation — the main product-category link), Reporting (`Product.Classification` in Warehouse report queries) | No structural move | This is the closest thing to a "ProductCategory" in the codebase, which the brief's §8 says should move to Catalog if Catalog existed — since this review does not recommend creating Catalog (§2 of the capabilities review), `Classification` stays in MasterData/ReferenceData as-is. Revisit only if Catalog is created later for other reasons |
| `PaymentType` (`Id` client-assigned, `Name`) | `Modules/MasterData/MasterData.Domain/Entities/PaymentType.cs` | MasterData | ReferenceData | Treasury (`Financial.PaymentType` navigation), Sales (`Invoice.PaymentType` navigation) — accepted exceptions | No structural move | Matches the brief's "PaymentTerms definitions" reference-data example closely |
| `ReferenceType` (`Id` client-assigned, `Name`) | `Modules/MasterData/MasterData.Domain/Entities/ReferenceType.cs` | MasterData | ReferenceData | Only genuine consumer found: seed data/UI dropdown for `Treasury.Domain.Financial.ReferenceType` (a **different, unrelated** `FinancialReferenceType` enum on `Financial`) — `Infrastructure/Seed/InitialData.cs:889-899` seeds labels (Customer/Supplier/Employee/...) that mirror the enum's values | No structural move | **Naming-collision risk**: `MasterData.Domain.ReferenceType` (entity) and `Treasury.Domain.FinancialReferenceType` (enum, exposed as `Financial.ReferenceType` property) are easily confused in code search. Worth a rename note (`ReferenceType` → e.g. `LookupCategory`) if this module is ever touched, but out of scope for this pass — no functional issue today |
| `Unit` (`Name`) | `Modules/MasterData/MasterData.Domain/Entities/Unit.cs` | MasterData | ReferenceData | Inventory (`ProductUnit.Unit`, `TransactionProduct.Unit`, `InventoryProduct.Unit` navigation), Sales (`InvoiceProduct.Unit`, `OrderProduct.Unit` navigation) — accepted exceptions at both Domain and Application layers | No structural move | Simple UoM lookup, exactly matching the brief's own "Unit references where appropriate" ReferenceData example |

## Concepts explicitly checked and confirmed NOT in MasterData

Per the brief's request to classify "every object currently treated as MasterData" — these
business-brief concepts were searched for and **do not exist in `MasterData.Domain`** (they either
live in their correct owning module already, or don't exist anywhere in the codebase yet):

| Concept | Actual status |
|---|---|
| `Product`, `ProductUnit`, `ProductRecipe`, `ProductPropertyElement` | Owned by `Modules/Inventory/Inventory.Domain/Entities` — never lived in MasterData |
| `ProductCategory`, `ProductGroup` | Do not exist anywhere in the codebase as separate types — `Classification` (above) is the only category-like concept, and it's a flag set (`BePurchased`/`BeSold`/`BeManufactured`), not a hierarchical category tree |
| `Barcode` | Not a class anywhere — a plain `string?` field on `Inventory.Domain.Product` |
| `Warehouse` | Not a named entity — `Inventory.Domain.Stock` plays this role, owned by Inventory |
| `Account`, `BankAccount` | Owned by Accounting/Treasury respectively — never lived in MasterData |
| `Language`, `TaxCode`/`Tax`, `Industry`, `PaymentTerms` (as separate concepts) | **Do not exist anywhere in the codebase.** `PaymentType` (above) is the closest existing analog to `PaymentTerms`. These should be flagged as "not yet modeled" if/when the business needs them — not invented speculatively as part of this decomposition |

## Recommendation

No entity needs to move out of MasterData. The decomposition this brief asks for is **already
substantially done** in terms of content — MasterData contains only reference data today, no
business aggregates. What remains is:

1. **Build `MasterData.Contracts`** (currently empty) with one lookup DTO per entity
   (`CountryLookupDto`, `CityLookupDto`, `DistrictLookupDto`, `CurrencyLookupDto`,
   `ClassificationLookupDto`, `PaymentTypeLookupDto`, `ReferenceTypeLookupDto`, `UnitLookupDto`),
   starting with `Currency` (widest fan-out) — this is the real fix for the "every module has an
   accepted-exception direct reference" problem, independent of any rename.
2. **Optional, low-priority**: rename the `MasterData` module/namespace to `ReferenceData` for
   vocabulary alignment with the brief. Purely cosmetic — no entity, table, or consumer behavior
   changes. Can be deferred indefinitely without blocking anything else in this decomposition.
3. **Fix the two DTO leaks** (`CountryDto : Country`, `PaymentTypeDto : PaymentType`,
   `ReferenceTypeDto : ReferenceType` subclass the domain entity directly) when the Contracts
   project is built — give them independent shapes like `City`/`Classification`/`Currency`/
   `District`/`Unit`'s DTOs already have.
