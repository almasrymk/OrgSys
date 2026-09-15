# CRM / Parties — Data Migration Plan

Covers the schema change this pass makes. Unlike Organization's `Branch.CompanyId` addition, this
pass required **no backfill logic** — every change is purely additive against a live table with
existing rows (`Dealer`) or introduces brand-new, empty tables.

---

## 1. What changes and why it's safe as generated (no hand-editing needed)

`Dealer` (existing, live table — `OrgConnection` is a live remote database per persistent project
memory) gains three new columns, **all nullable**:
- `PartyType` (`int`, nullable)
- `TaxRegistrationNumber` (`nvarchar(50)`, nullable)
- `CommercialRegistrationNumber` (`nvarchar(50)`, nullable)

A nullable `AddColumn` against a table with existing rows is always safe — every existing row simply
gets `NULL` for the new columns, no default-value hazard (unlike Organization's `Branch.CompanyId`,
which had to be non-nullable and therefore needed a hand-authored backfill — see
`docs/organization/organization-data-migration-plan.md` for that contrast).

Four brand-new tables, all currently empty (no pre-existing data of any kind):
- `CustomerProfile` (unique index on `DealerId`; FK to `Dealer` with `Cascade` delete — a Customer
  role cannot outlive its Dealer; FK to `Account`, no cascade)
- `SupplierProfile` (mirror of `CustomerProfile`)
- `PartyContact` (FK to `Dealer`, `Cascade` delete)
- `PartyAddress` (FK to `Dealer`, `Cascade` delete; FKs to `Country`/`City`/`District`, no cascade)

Migration: `BuildingBlocks/OrgSys.DatabaseMigrator/Migrations/
20260915101242_AddPartyRolesContactsAddresses.cs` — used exactly as EF generated it, no manual
edits were needed (contrast with the Organization pass's `Branch.CompanyId` migration, which
required hand-authoring an `InsertData`/`Sql` backfill because that column had to be non-nullable
on a live table).

## 2. What was verified before proposing this migration

- `dotnet ef migrations add AddPartyRolesContactsAddresses` generated successfully against the
  current model.
- `dotnet ef migrations has-pending-model-changes` reports **"No changes have been made to the model
  since the last migration"** — the generated migration's end state matches exactly what
  `OrgContext`'s model declares.
- Full solution build: 0 errors. Full test suite (`Architecture.Tests` 1376, `Application.Tests` 114
  including 7 new Party role-assignment tests, every `*.Domain.Tests` project,
  `Inventory.Integration.Tests`): **all passing**, confirming in particular that removing the
  internal `TypeId` guard from `DealerReceivableAccountProvisioning`/
  `DealerPayableAccountProvisioning` (§4 of the target-architecture doc) did not change Dealer's own
  existing Create/Update behavior.
- **`dotnet ef database update` was NOT run.** Same standing rule as the Organization pass:
  `OrgConnection` is a live remote database; applying a migration requires fresh, explicit user
  consent in this exact session, not implied by general task authorization.

## 3. Rollback / safety notes

- `Down()` is a clean EF-generated reversal: drop the four new tables, drop the three new nullable
  `Dealer` columns. No data-loss risk beyond the (currently non-existent, since the tables are new)
  contents of the four new tables themselves.
- This migration stacks after both `20260915070850_RelocateCatalogEntities` (Catalog extraction,
  also pending/not-yet-applied) and `20260915080929_AddOrganizationCompanyAndSettings`
  (Organization's Company/Branch.CompanyId addition, same status) in the migrations folder — none of
  the three touch overlapping tables (`Dealer`/`CustomerProfile`/`SupplierProfile`/`PartyContact`/
  `PartyAddress` here; `Product`/`Classification`/`Unit`/etc. for Catalog; `Company`/`Branch`/
  `OrganizationSettings` for Organization), so applying all three in sequence is safe and does not
  require reordering.
- No other module's tables, data, or migrations are touched.
