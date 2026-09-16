# OrgSys Current Architecture Review

Date: 2026-09-17 (Domain ID-only navigations). Previous snapshot: 2026-09-16.

## 0. 2026-09-17 update

**Architecture score: 8.0 / 10** (was 7.5 after the Preference/KeeperUser pass). Security remains **6.5 / 10** (human-only secret rotation / git history scrub still outstanding).

### Domain ID-only FKs

Cross-module EF navigations for Dealer / Branch / Invoice were dropped. Scalar FKs stay; Fluent `HasOne(typeof(...))` in `OrgContext` preserves the same columns/constraints (`dotnet ef migrations has-pending-model-changes` reports no schema change). Display names are patched through Contracts (`GetDealerNamesQuery`, `GetBranchNamesQuery`, `GetInvoiceNetsQuery`).

Cleared Architecture.Tests Domain exceptions: Administration→Organization, Treasury→Organization/Parties/CommercialDocuments, Inventory→Parties/Organization, CommercialDocuments→Parties, Purchasing→Parties, Catalog→Parties.

Cleared Application→Domain exceptions for those same Dealer/Branch reads. Remaining Application→Domain exceptions are MasterData/Catalog Unit mapping, Inventory/Treasury `IRepository<Invoice>`, Reporting as a read aggregator, and Organization validators.

Still open: MasterData/Catalog Unit/Currency Domain navigations; Treasury.Application still injects `IRepository<Invoice>` (accepted); event-driven GL posting (`PostAccountingDocumentCommand` — `SalesInvoicePostedIntegrationEvent` is reserved for AR).

### OrgSys MVC (`OrgSys/OrgSys.csproj`)

The Razor MVC host **stays in the solution**. Owner instruction: do not delete it. Angular is the replacement UI; MVC remains the original host.

Closed earlier (2026-09-16): Preference Contracts, `CashBox.KeeperUser`, Domain.Tests for Treasury/Parties/Organization/Catalog, Angular leftover folders.

---

## 0b. 2026-09-16 update (previous)

**Architecture score at that date: 7.5 / 10**. Security **6.5 / 10**.

### Security: **6.5 / 10** (new section)

What landed:
- Tracked `appsettings*.json` no longer contain live `OrgConnection` or `Jwt:Key` values. Real secrets come from `dotnet user-secrets`, `appsettings.Local.json` (gitignored), or env vars `ConnectionStrings__OrgConnection` / `Jwt__Key`. See `docs/local-setup.md`.
- User passwords are hashed with ASP.NET Identity `PasswordHasher<T>` (per-hash salt). The reversible AES helper `OrgSys.SharedKernel.Security` was deleted. Login verifies hashes; seed Owner/Admin get Identity hashes. Migration `AddUserPasswordHashing` adds `User.MustResetPassword` and backfills existing rows to `true`.

What still needs a human:
- Rotate the production SQL password and JWT signing key that were previously committed (listed in the Phase 1 changelog).
- Scrub git history (`git filter-repo` / BFG) — requires an owner force-push decision.
- Communicate the forced password reset to existing users. Seed Owner/Admin can log in after migrate+seed; every other stored AES ciphertext cannot be verified.

### Module boundaries

`Sales.Application → Accounting.*` (the 2026-09-11 Phase 4 item) was already closed in later work: invoices live in CommercialDocuments, dealers in Parties, both talk to Accounting only through `Accounting.Contracts`. This pass additionally:
- Routed `InvoiceJournalPostingService` Preference/Dealer reads through `Administration.Contracts` / `Parties.Contracts`.
- Deleted the `CommercialDocuments → Administration` Application→Domain exception.
- Added `Tests/Accounting.Integration.Tests` covering sales-invoice GL posting (balanced debit/credit) and cancel/redo status mirroring against a real SQLite DbContext.

Still open at that date (closed 2026-09-17 for Dealer/Invoice/Branch): remaining Domain→Domain EF navigations (Dealer/Invoice/Currency/Branch); Invoice/Dealer Application reads that still map full entities; event-driven GL posting (kept as `PostAccountingDocumentCommand` because `SalesInvoicePostedIntegrationEvent` is reserved for AR — see `docs/architecture/receivables-ddd-migration.md`).

Closed in this continuation:
- Inventory/Treasury/Parties Preference reads now go through `GetPreferenceValueQuery` / `GetPreferenceValuesQuery`. Architecture.Tests exceptions `Inventory|Treasury|Parties → Administration` (Application→Domain) were deleted.
- `CashBox.KeeperUser` navigation dropped; FK kept via Fluent `HasOne(typeof(User))`. Architecture.Tests exception `Treasury.Domain → Administration.Domain` deleted.
- Domain unit tests added for Treasury, Parties, Organization, Catalog.
- Legacy root `Domain`/`Application` business code is gone (empty leftover folders only). Journal posting lives in `InvoiceJournalPostingService` / `TransactionJournalPostingService`.
- Angular `features/` already mirrors the backend modules that have UI (`docs/angular-backend-alignment.md`). Empty leftover folders `administration`/`transactions`/`warehouse` are removed.

### OrgSys MVC (`OrgSys/OrgSys.csproj`)

The Razor MVC host **stays in the solution**. Owner instruction (2026-09-17): do not delete it. Angular is the replacement; MVC remains the original host.

---

Date: 2026-09-11, branch `Latest`, HEAD `723ad0f9`.

> This review reflects the repository **as it stands right now**, including the Sales.Application
> cleanup already completed in a prior pass (see `git log` — commit `723ad0f9` "Harden module
> boundaries: remove Sales.Application's illegal cross-module Domain refs..."). It supersedes the
> version numbers in the earlier `docs/architecture-review.md`, which was written before that
> commit landed. Read this one as the current source of truth.

## 1. Current architecture score: **6.5 / 10**

Up from an earlier 5.5/10 baseline. What changed: `Sales.Application` no longer references
`Inventory.Domain`, `Inventory.Application`, or `Treasury.Domain` directly — those three call
sites now go through `Inventory.Contracts`/`Treasury.Contracts` MediatR messages instead. Domain
isolation, EF schema integrity, and build/test hygiene remain solid.

What still caps the score below 8-9:
- `Sales.Application` still reaches `Accounting.Domain` directly (Journal lookups, GL-account
  provisioning) and the root `Application` project (the Journal-posting bridge) — both explicitly
  scoped to **Phase 4** of this plan, not yet started.
- The same Application→Domain leakage pattern exists in Treasury, Inventory (toward Sales),
  Receivables, Payables, and Administration — none of those are Sales-specific, so none were
  touched by the Sales-focused pass.
- No Integration Events, no dedicated `SalesInvoicePosted`/`SalesInvoiceReversed` message —
  cross-module effects still happen via direct MediatR command dispatch into the owning module's
  Application layer (a real improvement over direct repository access, but not yet an
  event-driven boundary).
- Legacy root `Domain`/`Application` still hold `Preference`, `LogSys`, `Notification`, and the
  Journal-posting bridge classes.
- Angular's `features/` folders don't yet mirror the 11 backend modules.

## 2. Project reference survey (as of this review)

Full cross-module Application-layer reference survey (every `<Module>.Application.csproj` ->
`<other>.Domain/.Application/.Infrastructure.csproj`):

| Project | Reference | Legal? |
|---|---|---|
| `Sales.Application` | `Sales.Domain`, `Sales.Contracts`, `MasterData.Application`, `Treasury.Contracts`, `Inventory.Contracts`, root `Domain`, root `Application` | Contracts refs legal. `MasterData.Application` and root `Domain`/`Application` are documented exceptions (§4). |
| `Inventory.Application` | `Inventory.Domain`, `Inventory.Contracts`, `MasterData.Application`, root `Domain`, root `Application` | Same pattern — `MasterData.Application` and legacy root are documented exceptions. **No longer has any Sales project reference in its `.csproj`** — its remaining `Sales.Domain` usage (Dealer navigation) resolves through the shared `global::` type visibility left over from the monolith split, tracked as a documented exception, not a `ProjectReference`. |
| `Treasury.Application` | `Treasury.Domain`, `Treasury.Contracts`, `Accounting.Application`, root `Domain` | `Accounting.Application` is a documented exception (`IAccountingPeriodService`). |
| `Accounting.Application` | `Accounting.Domain`, `Accounting.Contracts`, `Sales.Domain` | `Sales.Domain` is a documented exception (AR/AP validators). |
| `Receivables.Application` | `Receivables.Domain`, `Receivables.Contracts`, `Accounting.Application`, root `Domain` | `Accounting.Application` documented exception. |
| `Payables.Application` | `Payables.Domain`, `Payables.Contracts`, `Accounting.Application`, `Receivables.Application`, root `Domain` | Both documented exceptions. |
| `Purchasing.Application` | `Purchasing.Domain`, `Purchasing.Contracts` only | Clean — no business code exists yet (see `docs/legacy-migration-map.md`). |
| `Reporting.Application` | `Sales.Domain`, `Treasury.Domain`, `Inventory.Domain`, root `Domain` | All documented exceptions — Reporting is a deliberate read-only cross-module aggregator with no `Reporting.Domain` of its own. |
| `Administration.Application`, `Organization.Application`, `MasterData.Application` | own `Domain`/`Contracts` only, plus (Administration) `Organization.Domain` | The `Organization.Domain` reference is a documented exception. |

No `Module.Domain` project references another module's `Application`/`Infrastructure`, and no
`Module.Domain` references its own `Application`/`Infrastructure` — verified by
`Tests/Architecture.Tests` (`ModuleDependencyTests` + `ModuleLayerDependencyTests`), all 613
assertions passing.

## 3. Sales.Application violations — before and after

| Dependency | Status |
|---|---|
| `Sales.Application -> Inventory.Domain` | **Fixed** — removed, replaced by `Inventory.Contracts` |
| `Sales.Application -> Inventory.Application` | **Fixed** — `CreateTransactionByInvoiceCommand` moved to `Inventory.Contracts.Transactions` |
| `Sales.Application -> Treasury.Domain` | **Fixed** — removed, replaced by `Treasury.Contracts` |
| `Sales.Application -> Accounting.Domain` | **Not fixed** — Journal lookups (`JournalId`/`JournalCode` on Invoice Get/Search) and GL-account provisioning on Dealer create/update. Target: `Accounting.Contracts`. **Phase 4.** |
| `Sales.Application -> Accounting.Application` | **Not fixed** — `IPayableAccountValidator`/`IReceivableAccountValidator` used by Dealer create/update. **Phase 4.** |
| `Sales.Application -> MasterData.Application` | **Not fixed, accepted** — `UnitDto` mapping. MasterData is treated as safe shared reference data everywhere in the codebase, not routed through Contracts. |
| `Sales.Application -> root Domain/Application` | **Not fixed** — `Preference` entity, `InvoiceJournalIntegration` bridge. **Phase 10** (legacy migration), with the Journal-posting half overlapping Phase 4. |

## 4. `AcceptedDomainExceptions` / layer-exception lists currently present

Full detail with per-entry justification, cause entity, target solution, and phase is in
`docs/module-dependency-map.md` §3-5 (this review only counts them). As of this commit:

- `ModuleDependencyTests.AcceptedDomainExceptions` (Domain→Domain): **7 entries**
- `ModuleLayerDependencyTests.AcceptedApplicationDomainExceptions` (Application→Domain): **16 entries**
- `ModuleLayerDependencyTests.AcceptedApplicationApplicationExceptions` (Application→Application): **3 entries**

None of these were added by this review — they were already present (added when
`ModuleLayerDependencyTests` was written) and reflect real, pre-existing coupling, not new
allowances. **Zero new exceptions were added in this pass.**

## 5. Legacy classes still outside their owning module

See `docs/legacy-migration-map.md` for the full table. Summary: the legacy root footprint is
small — `Domain/Entities/OrgDb/{LogSys,Notification,Preferences}.cs`,
`Application/DTOs/OrgDb/{CheckEmailAndPasswordDto,CompanyProfileDto,LogSysDto,NotificationDto,
OrderTypeDto,PreferenceDto}.cs`, and `Application/Commands/Org/Financials/Integration/
{JournalInvoice,JournalTransaction}/*.cs` plus `Application/Commands/Org/Setting/Preference/**`.

## 6. Target dependency map

See `docs/module-dependency-map.md` §1 for the full graph. Summary of the enforced rule:

```
Module.Domain        -> SharedKernel only
Module.Application   -> own Domain, own Contracts, SharedKernel/EventBus
Module.Infrastructure -> own Application, own Domain, own Contracts
Other modules         -> Module.Contracts only
```

## 7. What Phase 2 (this pass) changes

**Phase 2 in this brief's numbering asks to fix `Sales.Application`'s illegal dependencies.**
That work was already completed (commit `723ad0f9`) before this review was written — verified
fresh in this pass (§2-3 above, plus a live `dotnet build`/`dotnet test` run, §"Build validation"
in the chat response). No additional code changes were made in this pass beyond writing these
three documents and re-verifying the existing state. `Sales.Application -> Accounting.*` and
`Sales.Application -> root Domain/Application` remain, exactly as this brief's own phase ordering
intends (Phase 4 and Phase 10 respectively) — **not** touched here.
