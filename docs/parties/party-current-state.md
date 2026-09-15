# CRM / Parties Bounded Context — Phase 0 Discovery Report

Analysis only — no source, config, or migration files were modified to produce this report.
Findings are backed by direct inspection (two parallel research passes covering backend and
Angular+DB schema, plus spot-checks of the key entities) of `D:\Work\MK\Source\OrgSys`, branch
`Latest`, as of 2026-09-15, on top of commit `31633d64` ("Extract the Catalog module, harden
Inventory, and build the Organization bounded context").

---

## 0. Headline finding: this is NOT greenfield — a `Parties` module already exists and owns `Dealer`

An earlier session (`docs/shared-business-capabilities-review.md`) already recognized that
Customer/Supplier identity was duplicated across `Sales.Domain` and extracted a `Dealer`/
`DealerGroup`/`DealerType` concept into its own `Modules/Parties/` (Domain/Application/
Infrastructure/Contracts), physically relocated out of `Sales.Domain`, with zero schema changes
(same table names/columns). That extraction solved the **module-ownership** half of the "avoid
Customer/Supplier duplication" problem the brief asks for. It did **not** solve the **data-model**
half: `Dealer` is a single flat entity with a generic `TypeId` scalar (`Client=1`/`Supplier=2`)
that makes a Dealer *either* a Customer *or* a Supplier, never both, and representing a real-world
company that is genuinely both today requires two unrelated `Dealer` rows with no link between
them. This report exists to give the actual data (not assumption) needed to decide how to close
that gap — the brief's §2.9 "mandatory scenario" (Party = ABC Company, active in both roles
simultaneously) is not satisfied by the current model.

---

## 1. `Dealer` — the existing Party identity, exact shape

`Modules/Parties/Parties.Domain/Entities/Dealer.cs:1-47`, table `Dealer`, inherits `BaseModel`:

```
Name (required, string)
Phone (string?, 8-25 chars)
Email (string?, 3-30 chars)
Address (string?, 3-500 chars)      — flat free text, not a structured value object
DealerGroupId / DealerGroup nav     — real EF navigation, intra-module
CountryId / Country nav             — real EF navigation into MasterData.Domain
CityId / City nav                   — real EF navigation into MasterData.Domain
DistrictId / District nav           — real EF navigation into MasterData.Domain
AccountId (long?, scalar only)      — deliberately NO navigation to Accounting.Domain.Account
                                       (documented GL bounded-context isolation, Fluent "no
                                       navigation" FK in OrgContext, same pattern as
                                       MovementModel.CreateUser/ModifyUser/Shift/Branch)
```

Plus everything inherited from `BaseModel`: `Id, CodeNumber, Code, MaskText, ParentId, TypeId,
Hide, ImgPath, Status`. **`TypeId` is where Client(1)/Supplier(2) lives** — there is no dedicated
`DealerType` column/table; it reuses the same generic inherited field every other `BaseModel`
entity has for unrelated purposes. No Person/Organization discriminator exists anywhere.

`DealerGroup.cs` (`Modules/Parties/Parties.Domain/Entities/DealerGroup.cs:1-8`): just `Name`
(inherits `BaseModel`, including `TypeId` — groups are Client-groups or Supplier-groups, not
generic categories; `ParentId` is inherited but unused for any group hierarchy today).

`Enums/DealerType.cs`: `Client = 1, Supplier = 2` — mirrored verbatim in
`Parties.Contracts.Dealers.DealerType` specifically so consumers never need a `Parties.Domain`
reference just to compare a role.

## 2. Customer vs Supplier — confirmed mutually exclusive per row today

No `IsCustomer`/`IsSupplier` flags, no role table — `TypeId` is a single scalar. Evidence this is
load-bearing throughout the stack, not incidental:

- `DealerReceivableAccountProvisioning`/`DealerPayableAccountProvisioning`
  (`Modules/Parties/Parties.Application/Dealers/Commands/`) each early-return unless
  `dealer.TypeId` matches their one type — **a Dealer provisions exactly one GL account, AR or AP,
  never both**.
- Every List/Search/GetMax query for Dealer *and* DealerGroup takes an explicit `TypeId` filter —
  there is no "give me this party regardless of role" query anywhere.
- Uniqueness validators (`Dealers/Validators/{Create,Update}UserCommandValidator.cs`) scope
  `(Code, TypeId)` and `(Name, TypeId)` — i.e. the schema was explicitly designed so the *same*
  Name/Code can exist twice, once per `TypeId`, because one row can only ever be one type. **No DB
  index enforces this** (confirmed in the migration snapshot — only FK indexes exist on `Dealer`,
  no unique constraint on Code/Name/TypeId combinations) — it's an application-layer-only rule.
- Opening-balance clearing-account resolution is looked up per-DealerType
  (`Administration.Contracts.Preferences.GetPreferenceValueQuery("Dealer", (long)DealerType, ...)`).
- Angular: one shared `Dealer` list/form component pair (`features/customers-suppliers/dealers/`)
  drives both `/customers-suppliers/dealers/customers` and `.../suppliers` purely via a
  `DealerType` value threaded through route `data` — confirming the UI/UX intent ("Customers" and
  "Suppliers" are one and the same screen, filtered), even though the underlying data model can't
  express "this one party is both."

**Answer to the brief's §2.9 mandatory scenario**: not supported today. Representing one real
company as both Customer and Supplier requires two separate `Dealer` rows, two separate
`AccountId`s, and independently re-entered Name/Address/Phone/Email/Country/City/District — nothing
links them as "the same underlying party." Closing this is the central design decision for Phase 1.

## 3. Every consumer of `Dealer`/`DealerId` — the real, enforced dependency map

Authoritative source: `Tests/Architecture.Tests/ModuleDependencyTests.cs` (Domain→Domain) and
`ModuleLayerDependencyTests.cs` (Application→Domain/Application) — these are enforced by the build,
unlike the prose docs (see §9).

| Consumer | Mechanism | Nullable? | Live-read or snapshot? |
|---|---|---|---|
| `Purchasing.Domain.PurchaseOrder` | Real EF nav `DealerId`/`Dealer` | **Required**, Cascade delete | Live-read, no name snapshot |
| `CommercialDocuments.Domain.Invoice` | Real EF nav `DealerId`/`Dealer` | **Required**, Cascade delete | Live-read; `InvoiceDto.DealerName` is AutoMapper-computed at query time, not persisted |
| `Treasury.Domain.Financial` | Real EF nav `DealerId`/`Dealer` | Nullable | Live-read |
| `Inventory.Domain` (`Transaction`, `InventoryIssue`, `InventoryReceipt`) | Real EF nav `DealerId`/`Dealer` | Nullable | Live-read |
| `Catalog.Domain.Product` | Real EF nav `DealerId`/`Dealer` (the product's **supplier**) | Nullable | Live-read |
| `Sales.Domain.Quotation` | **Scalar-only** `CustomerId`, no navigation (deliberate — entity's own doc comment cites "must not duplicate or navigate to Customer") | — | Opaque reference |
| `Receivables.Domain.Receivable` | **Scalar-only** `CustomerId` — **no FK constraint declared in the EF model at all** (confirmed in snapshot: no `HasOne` relationship block for `Receivable`→anything) | — | Opaque reference, unenforced |
| `Payables.Domain.Payable` | **Scalar-only** `SupplierId` — same, **no FK constraint declared** | — | Opaque reference, unenforced |
| `Receivables` `PaymentApplication.CustomerId` / `Payables` `SupplierPaymentApplication.SupplierId` | Same — bare scalar, no declared FK | — | Opaque reference, unenforced |
| `Reporting` | No Domain of its own; reads `Dealer`/`DealerType` directly by design (documented exception) | — | Live-read, ad hoc |
| `Accounting.Application` | `IReceivableAccountValidator`/`IPayableAccountValidator` call `Parties.Contracts.Dealers.GetDealerByIdQuery` | — | Via Contracts |

**`Receivable.CustomerId`/`Payable.SupplierId`/`PaymentApplication.CustomerId`/
`SupplierPaymentApplication.SupplierId` are not a separate "Customer"/"Supplier" identity concept**
— the opening-balance handlers' own command parameters are literally named `DealerId` and passed
straight through; these are naming aliases for `Dealer.Id`, nothing more.

**Two real, pre-existing gaps this discovery surfaced, independent of any new Party design:**
1. **No FK constraint** on `Payable.SupplierId`/`Receivable.CustomerId`/etc. — these are bare
   `bigint` columns with zero EF-level or DB-level referential integrity to `Dealer` today.
2. **No historical snapshot anywhere** — `Invoice`, `PurchaseOrder`, and `Financial` (the only
   tables with a real, required FK to Dealer) store *only* the FK, no denormalized name/address at
   all. Editing a Dealer's `Name` today silently changes what every historical Invoice/
   PurchaseOrder/Financial record displays. Every "DealerName" seen anywhere (report DTOs, Angular
   report models) is a live join at query time, not a stored point-in-time value.

## 4. Address, Contact, Tax identity — confirmed greenfield

- **No dedicated Address entity/value object exists anywhere.** Every address in the system is a
  single flat string: `Dealer.Address` (roughly), `Organization.Domain.Company.Address`, and the
  dead `CompanyProfile.Address1`/`Address2` (two strings — the closest thing to "multiple
  addresses" anywhere, and even that isn't a real collection). No entity supports more than one
  address, none are typed (billing/shipping/registered/etc.).
- **No dedicated Contact/ContactPerson entity exists anywhere.** `Dealer` has exactly one `Phone`
  and one `Email`; there is no way to record "the AP contact at Acme is Jane, phone X" separately
  from the Dealer's own single phone/email field.
- **No tax identity fields exist on `Dealer` at all** — no TaxRegistrationNumber/VATNumber/
  CommercialRegistrationNumber. The only place these concepts exist anywhere in the repo is
  `Organization.Domain.Company` (`TaxRegistrationNumber`, `CommercialRegistrationNumber` — added
  this session for the Company aggregate) and the dead `CompanyProfile.CommercialRegister`/
  `.TaxCard`. `Company`'s field naming is a reasonable precedent to reuse for Party for consistency
  within the same codebase.
- **CRM Lead/Opportunity: confirmed zero hits, backend and frontend both.** Fully greenfield.

## 5. Angular frontend — current state

Root: `OrgSys.Angular/src/app/`. One shared component pair drives both roles:
`features/customers-suppliers/dealers/{pages/dealer-list,pages/dealer-form,models,services}` and
the equivalent `dealer-groups/` — routed via `customers-suppliers.routes.ts` → `dealers.routes.ts`
(`/customers-suppliers/dealers/{customers|suppliers}`) and `dealer-groups.routes.ts`
(`/customers-suppliers/dealer-groups/{client-groups|supplier-groups}`), each pair distinguished
only by a `DealerType` value threaded through Angular route `data`.

`Dealer` model (`dealers/models/dealer.model.ts`) exposes exactly: name, phone, email, address
(flat string), dealerGroupId, country/city/districtId, accountId (**required** — every Dealer must
have a GL account picked at save time). No tax number, no structured contacts, no multiple
addresses, no Lead/Opportunity fields anywhere in the Angular app.

`Dealer`/`DealerType` are consumed pervasively outside their own feature area — Invoice form,
Financial transaction form, Inventory transaction form, and both "Clients"/"Suppliers"
balance/statement reports — always via the same `DealerService`/`Dealer`/`DealerType`, never a
parallel Customer- or Supplier-specific API.

Menu (`core/config/menu.config.ts:126-135`): a "Customers & Suppliers" group with **Customers**,
**Suppliers**, **Client Groups**, **Supplier Groups** entries, each routing into the same shared
components. Permission keys are already split per role (`Clients.*` vs `Suppliers.*`) even though
it's the same underlying table — i.e. the permission model already treats Client/Supplier as
separate namespaces, something a new design should preserve or deliberately migrate, not silently
break.

## 6. Database schema ground truth

`Dealer` table (`OrgContextModelSnapshot.cs`): `Id, AccountId, Address(500), CityId, Code,
CodeNumber, CountryId, DealerGroupId, DistrictId, Email(30), Hide, ImgPath, MaskText, Name
(required), ParentId, Phone(25), Status, TypeId`. Indexes: one each on `AccountId`, `CityId`,
`CountryId`, `DealerGroupId`, `DistrictId` — **no unique index on Code/Name/TypeId** despite the
application-layer uniqueness rules. `DealerGroup` table: `Id, Code, CodeNumber, Hide, ImgPath,
MaskText, Name(50), ParentId, Status, TypeId` — no indexes besides PK. **`DealerType` is not a
table** — enum-only, stored as a plain `bigint TypeId` column on both tables.

No other table anywhere stores an independent customer/supplier identity (name/address/phone/tax)
— confirmed by inspecting every `ToTable(...)` in the snapshot. Every FK'd consumer (`Product`,
`Invoice`, `Transaction`, `InventoryIssue`, `InventoryReceipt`, `PurchaseOrder`, `Financial`)
references `Dealer.Id` only. `Payable`/`Receivable`/`PaymentApplication`/
`SupplierPaymentApplication`'s Customer/SupplierId columns are un-FK'd bare `bigint`s (§3).

## 7. Seeders

`Modules/Parties/Parties.Infrastructure/Seeding/PartiesDataSeeder.cs`:
- `InitialDealerGroup` seeds **two identical rows** (`CodeNumber=1, Name="Group 1", TypeId=1`
  twice) — an apparent copy-paste bug, flagged, out of scope to fix in this pass.
- `InitialDealer` seeds one placeholder Dealer with **`TypeId=0`** — neither `Client(1)` nor
  `Supplier(2)` per the enum, i.e. the seed data itself doesn't respect the DealerType invariant.
  Also flagged, not fixed here (pre-existing, unrelated to this discovery).

## 8. API controllers exposing Dealer/DealerGroup today

`API/Controllers/Org/Setting/DealerController.cs` (standard CRUD + `GET Dealer/Balance`),
`API/Controllers/Org/Setting/DealerGroupController.cs` (standard CRUD),
`API/Controllers/Org/Reports/DealerReportController.cs` (`GET DealerReport/Balance`,
`GET DealerReport/Statement`, both parameterized by `DealerTypeId`). Indirect consumers:
`InvoiceController.cs`, `PurchaseRequisitionController.cs`, `FinancialReportController.cs`.

## 9. Docs review — several are stale, do not trust them for current Dealer/Parties state

- **`docs/module-ownership.md:20`** still says `Dealer, DealerGroup, DealerType | Sales |
  Modules/Sales/Sales.Domain/Entities` — **wrong**; confirmed live in `Modules/Parties/
  Parties.Domain/Entities`. Never updated after the Parties extraction.
- **`docs/dependency-rules.md`** — zero mentions of "Parties" anywhere in 116 lines; every
  Dealer-related exception is still written against `Sales.Domain`/`Sales.Application`. **Stale.**
- **`docs/module-dependency-map.md`** — zero mentions of "Parties". **Stale.**
- **The actually-enforced, trustworthy source of truth** for cross-module Dealer dependencies is
  `Tests/Architecture.Tests/ModuleDependencyTests.cs` / `ModuleLayerDependencyTests.cs` (quoted in
  full in §3) — these correctly say "Parties" throughout and fail the build on drift.
- **`docs/shared-business-capabilities-review.md`** — the doc that *originated* the Parties
  extraction; historically accurate about *why* Parties was created (module-ownership only), but
  explicitly does not address Person/Organization, multi-role Customer+Supplier, Address-as-entity,
  Contact-as-entity, tax fields, or Lead/Opportunity — none of that was in its scope.
- **`docs/modular-monolith-analysis.md:113,120-123,459`** — pre-Parties-extraction, but its risk
  framing remains directly relevant: it explicitly called out Dealer/DealerType as *"the one entity
  genuinely owned by two future modules at once... single table serving two future modules... High
  risk"* — i.e. the original analysis recognized the shared-identity/dual-role tension but only the
  Parties extraction (module ownership) resolved it, never the underlying data model. This report's
  §2 finding is the concrete, current-code confirmation of that historical risk callout.
- **No `docs/parties/` folder existed before this report.**

---

## Summary — what Phase 1 design must actually decide

1. **How to let one real company be both Customer and Supplier** without two disconnected rows —
   the brief's own preferred shape (`Party` + `CustomerProfile?`/`SupplierProfile?` roles) is a
   direct, well-motivated fit given `TypeId`'s current all-or-nothing behavior.
2. **What happens to `AccountId`** — today it's one scalar per Dealer, provisioning exactly one GL
   account (AR or AP). A dual-role Party needs either two account references (one per role) or a
   role-scoped account concept.
3. **Historical snapshot gap** (§3) — whether/how to add point-in-time name/address snapshots to
   Invoice/PurchaseOrder/Financial as part of this redesign, since nothing captures that today.
4. **FK integrity gap** (§3) — whether to wire real FK constraints onto `Payable.SupplierId`/
   `Receivable.CustomerId`/etc. while touching this area, or leave as a separately-tracked item.
5. **Address/Contact/Tax fields** — all genuinely new; no legacy shape to preserve or migrate.
6. **Angular/permission-key split** (`Clients.*` vs `Suppliers.*`) — any redesign must either
   preserve this permission namespace split or plan a coordinated, explicit rename.
7. **Lead/Opportunity** — fully new, brief's own guidance ("don't pollute Party master with
   unqualified leads") applies with no existing code to reconcile against.
8. **Physical relocation mechanics** — if `Dealer` itself is renamed/restructured into `Party` +
   role tables, this repo has a proven, low-risk playbook for exactly this kind of change (Parties'
   own original extraction, and this session's Catalog extraction): preserve `[Table(...)]` names
   where unchanged, add new tables for new concepts, update the Architecture.Tests accepted-
   exception lists, verify a zero/minimal-diff migration, update `docs/module-ownership.md` and
   `docs/dependency-rules.md` while at it (both already known-stale, per §9) — do not repeat their
   omission.
