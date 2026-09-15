# Organization / Administration — Data Migration Plan

Covers the one schema change this pass makes to a table with existing live data: `Branch` gaining a
required `CompanyId`. `Company` and `OrganizationSettings` are brand-new tables (nothing to
migrate into them). Currency/Country/City/District/FiscalYear/FiscalPeriod are unchanged — see
[organization-target-architecture.md](organization-target-architecture.md) §2 for why they were not
relocated this pass.

---

## 1. What has to happen, in order

`Branch` is a **live table** (`OrgConnection` is a live remote database, per this session's project
memory) that may already contain rows (at minimum the seeded "Main Branch," possibly more added
since). Adding `CompanyId` as `NOT NULL` cannot be a plain `AddColumn` — EF's default scaffold for a
non-nullable column on a live table used `defaultValue: 0`, which would leave every existing Branch
pointing at `Company.Id = 0`, a row that doesn't exist, and the subsequent `AddForeignKey` would
fail outright against real data. The migration
(`BuildingBlocks/OrgSys.DatabaseMigrator/Migrations/20260915080929_AddOrganizationCompanyAndSettings.cs`)
was hand-edited after generation to sequence it safely:

1. `CreateTable Company`
2. `InsertData` — exactly one row into `Company` (`Id = 1`, `LegalName = "Main Company"`,
   `Code = "MAIN"`) — guaranteed `Id = 1` since this is a brand-new `IDENTITY(1,1)` table.
3. `AddColumn CompanyId` on `Branch` as **nullable** (no default-value hazard).
4. `Sql("UPDATE [Branch] SET [CompanyId] = 1 WHERE [CompanyId] IS NULL;")` — backfills every
   existing Branch row (and any added between steps, none expected in practice for a single
   migration transaction) to the one seeded Company.
5. `AlterColumn CompanyId` on `Branch` to **NOT NULL** — now safe, every row has a value.
6. `CreateTable OrganizationSettings` (net-new, no data concerns).
7. Indexes, then `AddForeignKey Branch → Company` (only after every row is guaranteed valid) with
   `DeleteBehavior.Restrict` — a Company cannot be deleted while it still has Branches, consistent
   with the brief's "avoid cascade deletion for ERP reference/master data" rule.

`Down()` reverses cleanly: drop the FK, drop `OrganizationSettings`, drop `Company` (which also
removes the seeded row — no separate `DeleteData` needed), drop the index, drop the column.

## 2. Fresh-database path

`OrganizationDataSeeder` (`Modules/Organization/Organization.Infrastructure/Seeding/
OrganizationDataSeeder.cs`) gained a new `InitialCompany` step, run before `InitialBranch`, with the
same "add only if none exist" idempotency `InitialBranch` already used. `InitialBranch` now looks up
the first seeded Company's `Id` and assigns it to the default "Main Branch" row. This path is
independent of the migration's own backfill — it only ever runs against a database that already has
the `CompanyId` column and FK in place (either freshly migrated, or an existing database that has
already had this migration applied).

## 3. What was verified before proposing this migration

- `dotnet ef migrations add AddOrganizationCompanyAndSettings` generated successfully against the
  current model (confirms the Domain entities/OrgContext configuration are internally consistent).
- After the hand-edit, `dotnet ef migrations has-pending-model-changes` reports **"No changes have
  been made to the model since the last migration"** — the hand-edited migration's end state matches
  exactly what `OrgContext`'s model declares; nothing was silently left out or double-applied.
- Full solution build: 0 errors. Full test suite (`Architecture.Tests` 1376, `Application.Tests` 107
  including 11 new Organization-specific tests, every `*.Domain.Tests` project, `Inventory.Integration.Tests`):
  **all passing.**
- **`dotnet ef database update` was NOT run.** Per this session's persistent project memory,
  `OrgConnection` is a live remote database and applying a migration requires fresh, explicit user
  consent in this exact session — general task authorization to "implement Organization completely"
  does not constitute that consent for a schema-changing command against a live remote database.
  The migration is generated, reviewed, and ready to apply; applying it is a separate, explicit step
  for the user (or an operator) to take when ready.

## 4. Rollback / safety notes

- The migration is additive except for `Branch.CompanyId` — no column is dropped, no existing
  column's type changes, no existing table is renamed. `Down()` is a clean, tested-by-symmetry
  reversal (EF-generated drop operations, verified to mirror the hand-edited `Up()`'s final state).
- The one Company row this migration inserts (`Id = 1`, code `MAIN`) is intentionally the same shape
  `OrganizationDataSeeder.InitialCompany` would create on a fresh database — applying the migration
  to the live database and then running the seeder again is a no-op for Company (seeder's
  idempotency check: "no Company rows exist yet" will already be false).
- No other module's tables, data, or migrations are touched. The concurrent, uncommitted
  `RelocateCatalogEntities` migration (Product/Classification/Unit/Property → Catalog) is unrelated
  and unaffected — this migration was generated on top of it in sequence, not in place of it.
