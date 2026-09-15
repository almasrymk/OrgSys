# CRM / Parties Bounded Context — Migration Result

Phase 1 (Discover → Design → Migration Plan → Domain → Application → Infrastructure → API →
Consumer Migration → Tests → Architecture Validation → Legacy Cleanup → Build) is complete for the
scope defined in [party-target-architecture.md](party-target-architecture.md).

---

## 1. What was built

**Domain** (`Modules/Parties/Parties.Domain/`):
- `Entities/Dealer.cs` — extended with `PartyType` (nullable enum), `TaxRegistrationNumber`,
  `CommercialRegistrationNumber` (nullable strings), and navigations to the four new entities below.
- `Entities/CustomerProfile.cs`, `Entities/SupplierProfile.cs` — new, one-per-Dealer role records
  enabling brief §2.9's dual-role scenario without a duplicate identity row.
- `Entities/PartyContact.cs`, `Entities/PartyAddress.cs` — new, repeatable child entities.
- `Enums/PartyType.cs` (Person/Organization), `Enums/PartyAddressType.cs` (Billing/Shipping/
  Registered/Office/Other) — new.

**Application** (`Modules/Parties/Parties.Application/`):
- `CustomerProfiles/` — `AssignCustomerRoleCommand`/Handler (explicit business action, reuses
  `DealerReceivableAccountProvisioning`), `GetCustomerProfileByDealerIdQuery`/Handler,
  `CustomerProfileDto`, `MappingProfile`.
- `SupplierProfiles/` — mirror of the above for the Supplier role.
- `PartyContacts/` — full CQRS (Create/Update/Delete/DeleteList/GetById/GetListByDealer),
  validators (Dealer-existence check, email format), `MappingProfile`.
- `PartyAddresses/` — mirror of the above.
- `Dealers/DealerDto.cs` — extended with the three new `Dealer` fields (AutoMapper picks them up by
  convention, no `MappingProfile` change needed).
- `Dealers/Commands/DealerReceivableAccountProvisioning.cs` /
  `DealerPayableAccountProvisioning.cs` — internal `TypeId` guard removed (redundant at the existing
  Create/Update call site, would have been wrong for the new dual-role commands — see target
  architecture §4). Confirmed via the full test suite that Create/Update's own behavior is
  unchanged.

**EF Core / Database** (`BuildingBlocks/OrgSys.DatabaseMigrator/`):
- `OrgContext.cs` — four new `DbSet`s, new `ConfigureParties` Fluent-config helper (Dealer→Contacts/
  Addresses cascade, CustomerProfile/SupplierProfile unique-per-Dealer + cascade + scalar-only
  Account FK).
- Migration `20260915101242_AddPartyRolesContactsAddresses` — purely additive, no hand-editing
  needed (see [party-data-migration-plan.md](party-data-migration-plan.md)). **Not applied to the
  live database** — generated and verified only.

**API** (`API/Controllers/Org/Parties/`):
- `CustomerProfileController`, `SupplierProfileController` — `GetByDealerId`/`Assign`.
- `PartyContactController`, `PartyAddressController` — `GetById`/`GetListByDealer`/`Create`/
  `Update`/`Delete`/`DeleteList`.

**Tests** (`Tests/Application.Tests/PartyRoleAssignmentTests.cs`):
- 7 new tests, directly covering the brief's own PARTIES TEST EXAMPLES this pass's scope
  implements: **"Party may be Customer and Supplier simultaneously"** (the two dual-role tests —
  assigning the Customer role to a Dealer whose primary `TypeId` is Supplier, and vice versa — the
  central proof this design actually closes the gap discovery identified), **"Customer role cannot
  be assigned twice"**, **"Supplier role cannot be assigned twice"**, plus account-validation
  error-path coverage (invalid explicit account, no account + auto-create not requested, Dealer not
  found). (Person/Organization rules, primary-address invariants, accidental-merge prevention, and
  deactivation-preserves-history from the brief's list are either not applicable to this pass's
  additive-only scope or have no invariant to test yet — e.g. `PartyType` carries no business rule
  beyond being a plain nullable classification.)

**Documentation** (`docs/parties/`):
- `party-current-state.md` (Phase 0), `party-target-architecture.md`,
  `party-data-migration-plan.md`, this file.

---

## 2. Cross-context dependencies

No new `Domain → Domain` or `Application → Domain` exception was required — see
[party-target-architecture.md](party-target-architecture.md) §9. `Tests/Architecture.Tests` (1376
tests, both `ModuleDependencyTests.cs` and `ModuleLayerDependencyTests.cs`) pass unchanged.

## 3. Domain events / integration events

None added — `CustomerProfile`/`SupplierProfile`/`PartyContact`/`PartyAddress` are plain `BaseModel`
entities, consistent with every sibling entity in `Parties`/`Organization`/`MasterData` (only
`Journal`/`FiscalPeriod`/`Receivable`/`Payable`, which have genuine cross-aggregate lifecycle
consequences, raise domain events in this codebase). No `CustomerRoleAssigned`/`SupplierRoleAssigned`
event was introduced without a concrete current consumer, per the brief's own "don't create events
for meaningless property setters" rule and this session's own precedent from the Organization pass.

## 4. DB migrations

One migration, purely additive, no hand-editing required. Full detail in
[party-data-migration-plan.md](party-data-migration-plan.md). **Status: generated and verified
(`dotnet ef migrations has-pending-model-changes` reports clean), not applied.**

## 5. API changes

Four new controllers under `API/Controllers/Org/Parties/` (§1). `DealerController`'s own DTO gained
three new nullable fields (`PartyType`/`TaxRegistrationNumber`/`CommercialRegistrationNumber`) —
additive to the wire contract, not breaking; existing callers omitting them get `null`, which every
new field already tolerates (no validator requires them).

## 6. Angular changes

None. Deliberately deferred — see
[party-target-architecture.md](party-target-architecture.md) §8.4.

## 7. Legacy code removed / compatibility adapters remaining

- **Removed**: nothing — no legacy duplicate Customer/Supplier identity existed to remove (discovery
  confirmed everything already funnels through the single `Dealer` table).
- **Compatibility adapters**: none introduced. No `TODO DDD-MIGRATION-PARTY` markers were needed —
  like the Organization pass, this is net-new capability layered onto an existing, working table,
  not a legacy-data bridge.
- **Fixed as a necessary side-effect, not a bridge**: the internal `TypeId` guard removed from
  `DealerReceivableAccountProvisioning`/`DealerPayableAccountProvisioning` (§1) — a real, minimal
  code change to existing files, not a temporary adapter; verified safe via the full test suite.

## 8. Technical debt / open items carried forward

1. Historical snapshot gap (`Invoice`/`PurchaseOrder`/`Financial` don't snapshot Dealer name/address)
   — pre-existing, not created by this pass, not fixed — see target architecture §8.1.
2. FK integrity gap (`Payable.SupplierId`/`Receivable.CustomerId`/etc. remain un-FK'd scalars) —
   pre-existing, not fixed — see target architecture §8.2.
3. CRM Lead/Opportunity — fully deferred, genuinely large scope on its own — §8.3.
4. Angular: role-assignment UI, contact/address management screens, `PartyType`/tax-number fields on
   the existing Dealer form — §8.4.
5. `Dealer.TypeId`'s role semantics were not removed/reinterpreted — remains the Dealer's primary
   role for every pre-existing consumer — §8.5.
6. Pre-existing `PartiesDataSeeder` bugs found during discovery (duplicate `DealerGroup` seed rows,
   `TypeId=0` placeholder Dealer) — flagged, not fixed (unrelated to this pass).
7. Migration `20260915101242_AddPartyRolesContactsAddresses` is not yet applied to the live database.

## 9. dotnet build result

```
dotnet build OrgSys.sln
0 Error(s)
(warning count unchanged in substance — no new warnings from Parties code; the raw count
 fluctuates between clean/incremental builds due to pre-existing warnings across the whole solution)
```

## 10. dotnet test result

```
Architecture.Tests:            1376 passed, 0 failed
Application.Tests:               114 passed, 0 failed (7 new — Party role assignment / dual-role)
Accounting.Domain.Tests:          54 passed, 0 failed
Sales.Domain.Tests:               53 passed, 0 failed
Advances.Domain.Tests:            39 passed, 0 failed
Payables.Domain.Tests:            35 passed, 0 failed
Purchasing.Domain.Tests:          45 passed, 0 failed
Inventory.Domain.Tests:           66 passed, 0 failed
Receivables.Domain.Tests:         35 passed, 0 failed
Inventory.Integration.Tests:       3 passed, 0 failed
```

Full solution build and full test suite (every test project) were both run clean before considering
this phase done, per the brief's "full solution must compile / all existing relevant tests must
pass" rules — this specifically confirmed the `DealerReceivableAccountProvisioning`/
`DealerPayableAccountProvisioning` guard removal did not regress Dealer's own existing Create/Update
behavior.

---

## Next context in the sequence

Per the brief's EXECUTION ORDER: **CRM / Parties → Tax**. Next up is Tax — Phase 0 discovery first
(`/docs/tax/tax-current-state.md`), before any Tax/VAT production code changes, exactly as this
context and Organization both started.
