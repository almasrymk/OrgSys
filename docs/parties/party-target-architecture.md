# CRM / Parties Bounded Context — Target Architecture

Companion to [party-current-state.md](party-current-state.md) (Phase 0 discovery). Records the
actual Phase 1 design decisions and — per the brief's own rule ("if existing OrgSys code conflicts
with assumptions in this prompt: inspect, preserve correct existing behavior, apply the DDD
boundary, document the decision") — explains every deliberate deviation from the brief's literal
Part 2 scope, and why.

---

## 1. Central decision: extend `Dealer` additively, do not rename/restructure it

The brief's suggested shape is `Party` + `PersonProfile`/`OrganizationProfile` identity, with
`CustomerProfile`/`SupplierProfile` as roles. Physically renaming `Dealer` → `Party` (or splitting
its identity/role concerns into new tables that replace it) would require touching **seven modules
that hold real, required EF navigations to `Dealer`** (Purchasing, CommercialDocuments, Treasury,
Inventory, Catalog — plus the two Domain-level scalar references in Receivables/Payables) — a much
larger, much riskier change than this pass's budget or the brief's own "do not blindly rewrite
stable functionality" guidance supports, especially with zero existing Parties-specific test
coverage as a regression safety net beforehand.

**Decision**: `Dealer` keeps its name, table, and every existing consumer's navigation exactly as
they are. The brief's actual, load-bearing requirement — brief §2.9's "one real company can be both
Customer and Supplier without a duplicate record" — is met by adding **`CustomerProfile`** and
**`SupplierProfile`** as new, optional 1:1 child tables keyed by `DealerId`, assigned via two new
explicit commands (`AssignCustomerRoleCommand`/`AssignSupplierRoleCommand`) rather than a
restructure. This satisfies the *substance* of brief §2.2-§2.9 (shared identity, non-exclusive
roles, explicit role assignment as a business action) without the blast radius of a rename. The
`Dealer` class itself is the de facto "Party" identity table going forward; `Party` as a literal
type name was not introduced (see §7 for the naming decision).

## 2. `Dealer.TypeId` (Client/Supplier) — kept as the primary/original role, not removed

`TypeId` continues to drive every pre-existing screen, query, and uniqueness rule exactly as before
(Create/Update, List/Search filtering, the Dealer/DealerGroup Angular screens, permission-key
namespaces) — **zero behavior change** for anything that already works. `CustomerProfile`/
`SupplierProfile` are the *additive* mechanism through which a Dealer gains the *other* role:

- A Dealer created with `TypeId = Client` already has an implicit Customer role (unchanged, exactly
  as before) and can now *additionally* be given a `SupplierProfile` via
  `AssignSupplierRoleCommand`, without becoming two rows.
- Symmetrically, a Dealer with `TypeId = Supplier` can gain a `CustomerProfile` via
  `AssignCustomerRoleCommand`.
- `TypeId` was **not** removed or reinterpreted as a bitmask/multi-value field — doing so would
  have forced changes across every one of the query filters, validators, and Angular routes listed
  in the discovery report, all of which are working, tested-by-usage code this pass does not touch.

This is a deliberate, documented deviation from a literal reading of brief §2.2 ("do not use
CustomerType/SupplierType... those are roles") — `TypeId`/`DealerType` still functions as the
Dealer's primary role for everything that predates this pass. The two new profile tables are what
actually deliver "roles, not identity types" for the *dual*-role case; a full removal of `TypeId`'s
role semantics is left to a future phase once Angular and every existing query are ready to move
off of it.

## 3. `CustomerProfile`/`SupplierProfile` — deliberately thin

Fields: `DealerId` (unique FK), `AccountId` (scalar-only, no navigation — see §5), plus the minimal
commercial flags the brief itself lists with nowhere else to live: `CreditLimit`/`IsCreditAllowed`/
`IsOnHold` for Customer, `IsApproved`/`IsOnHold` for Supplier. Explicitly **not** included, per
brief §2.5/§2.6's own caution: `PaymentTermsId`, `DefaultPriceListId`, `DefaultCurrencyId` (AR/AP
configuration, not Party's concern), `LeadTime`/`MOQ`/`SupplierProductCode`/`SupplierProductPrice`
(Purchasing's supplier/product relationship, not Party's concern).

## 4. GL account provisioning — reused, not duplicated, with one necessary fix

`Dealer`'s own Create/Update already had a sophisticated, transactional GL-account-provisioning flow
(`DealerReceivableAccountProvisioning`/`DealerPayableAccountProvisioning`, calling
`Accounting.Contracts.Accounts.ProvisionSubAccountCommand` to auto-create a sub-account under a
configured parent, or validating an explicitly-chosen one). Per the brief's "do not recreate
infrastructure OrgSys already provides," `AssignCustomerRoleCommand`/`AssignSupplierRoleCommand`
call these exact same helpers rather than reimplementing account resolution.

**One necessary, minimal fix**: both helpers had an internal `if (dealer.TypeId != ...)` guard that
silently skipped all validation/auto-create logic. This guard was **redundant** at Dealer's own
Create/Update call site (the caller already branches on `TypeId` before choosing which helper to
call) but would have been **actively wrong** for the new dual-role commands — a Dealer whose primary
`TypeId` is Supplier calling `AssignCustomerRoleCommand` would have had its receivable-account
validation silently skipped. The guard was removed from both helpers (confirmed via the full test
suite, including new dual-role tests, that Create/Update's existing behavior is unchanged — the
guard was dead code at that call site) rather than duplicating a second copy of the provisioning
logic with the guard removed.

## 5. `PartyContact`/`PartyAddress` — new, repeatable child entities

Both are plain `BaseModel` entities, generic CQRS (Create/Update/Delete/DeleteList/GetById, plus a
dedicated `GetListByDealer` query rather than the generic paginated List/Search, since contacts and
addresses are always viewed in the context of one Dealer, mirroring
`MasterData.Application.Cities.Queries.GetListByCountryQueryHandler`'s existing "list scoped to a
parent" precedent). `PartyAddress.CountryId`/`CityId`/`DistrictId` are real EF navigations into
`MasterData.Domain` — `Parties.Domain` already has the accepted architecture exception for exactly
this (`Dealer.Country`/`City`/`District`), so this adds no new `Architecture.Tests` entry.

Both are additive alongside `Dealer.Address`/`Country`/`City`/`District`, which stay exactly as they
are — every pre-existing screen/report/query that reads the Dealer's single flat address keeps
working unchanged. `PartyContact`/`PartyAddress` are the *new* way to record multiple, typed
contacts/addresses; nothing forces existing consumers to adopt them.

## 6. `PartyType` (Person/Organization) and tax identity fields

Added directly to `Dealer` as nullable, additive columns: `PartyType` (enum, Person/Organization —
distinct from the role-oriented `DealerType`, per brief §2.2), `TaxRegistrationNumber`,
`CommercialRegistrationNumber` (both nullable strings, naming borrowed from
`Organization.Domain.Company`'s equivalent fields for cross-codebase consistency, added this
session). Nullable and not backfilled — no existing Dealer row is forced to declare a `PartyType`
retroactively.

## 7. Naming: `Dealer`, not `Party`

The brief's own ubiquitous language prefers "Party." This pass keeps the physical/C# name `Dealer`
unchanged (§1) — introducing a literal `Party` class would either (a) be a second, parallel identity
concept alongside `Dealer` (duplicating exactly what this bounded context exists to prevent), or (b)
require the same large rename this pass deliberately avoided. Matches the precedent already set by
this repo's own Catalog effort ("keep the physical name, consider a new class name only if it aids
clarity — needs a one-line decision, not a re-litigation" — `docs/catalog/catalog-current-state.md`
§10.1). If a literal `Party` rename is wanted later, it is its own, separately-scoped phase.

## 8. What was deliberately deferred (brief rule #43)

1. **Historical snapshot gap** (discovery §3): `Invoice`/`PurchaseOrder`/`Financial` still store only
   `DealerId`, no point-in-time name/address snapshot — unchanged this pass. Fixing it means adding
   columns to three other modules' tables, out of scope for a Parties-focused pass; flagged for a
   dedicated follow-up.
2. **FK integrity gap** (discovery §3): `Payable.SupplierId`/`Receivable.CustomerId`/
   `PaymentApplication.CustomerId`/`SupplierPaymentApplication.SupplierId` remain bare, un-FK'd
   scalars — not wired up this pass (would touch Receivables/Payables' own migrations and models,
   outside this bounded context's ownership). Flagged, not fixed.
3. **CRM Lead/Opportunity** — brief §2.13/§2.14 explicitly frames this as conditional ("if
   implementing CRM functionality now"). Confirmed fully greenfield in discovery, genuinely large
   scope on its own (lifecycle states, qualification workflow, Opportunity pipeline stages) — not
   built this pass. A future phase, not a gap in this one.
4. **Angular** — no new screens for CustomerProfile/SupplierProfile/PartyContact/PartyAddress role
   assignment or the new Dealer fields. Same reasoning as the Organization pass: designing the UX
   for role assignment, multi-contact/address management deserves its own focused effort rather than
   a rushed addition here. Flagged as follow-up.
5. **`TypeId`'s full removal/reinterpretation** (§2) — deliberately not attempted; `TypeId` remains
   authoritative for everything that predates this pass.
6. **Duplicate-detection / legacy-Customer-Supplier migration mapping** (brief §2.16) — not
   applicable this pass: discovery confirmed there is no *legacy* Customer/Supplier data to migrate
   (everything already funnels through the single `Dealer` table; the only "duplication" this
   context solves is the *same-company-two-roles* case via §1-§4, not merging pre-existing duplicate
   records).
7. **Pre-existing seeder bugs found during discovery** (`PartiesDataSeeder`'s duplicate `DealerGroup`
   rows, `TypeId=0` placeholder Dealer) — flagged in the discovery report, not fixed here (unrelated
   to this pass's actual changes, per "do not modify unrelated code").

## 9. Cross-context dependency policy — what changed

No new `Domain → Domain` or `Application → Domain` exception was needed. `PartyAddress`'s
`Country`/`City`/`District` navigations reuse `Parties.Domain`'s existing accepted exception into
`MasterData.Domain`. `CustomerProfile.AccountId`/`SupplierProfile.AccountId` are scalar-only FKs
into `Accounting.Domain.Account`, configured centrally in `OrgContext.ConfigureParties` (the
`OrgSys.DatabaseMigrator` project, not `Parties.Domain` itself) — identical to how
`Dealer.AccountId` already avoids the same reference. `AssignCustomerRoleCommandHandler`/
`AssignSupplierRoleCommandHandler` reuse `Parties.Application`'s existing dependencies
(`Accounting.Contracts`, `Administration.Domain` for `Preference`) — no new project references were
required beyond `Tests/Application.Tests` gaining `Parties.Application`/`Parties.Domain` references
for its new tests.

## 10. API

New controllers under `API/Controllers/Org/Parties/` (the newer per-module-area convention, matching
Organization/Catalog/Sales/Payables/Receivables/Advances — existing `DealerController`/
`DealerGroupController` under `API/Controllers/Org/Setting/` are untouched):
`CustomerProfileController` (`GetByDealerId`, `Assign`), `SupplierProfileController` (mirror),
`PartyContactController` (`GetById`, `GetListByDealer`, `Create`, `Update`, `Delete`, `DeleteList`),
`PartyAddressController` (mirror). None extend the generic `BaseController<>` — role-assignment is
an explicit action, not generic CRUD, and contact/address listing is scoped to a Dealer rather than
globally searchable, so neither fits the 8-type-parameter generic shape (same reasoning as
`OrganizationSettingsController` in the Organization pass).
