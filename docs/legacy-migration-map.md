# OrgSys Legacy Migration Map

**Update (2026-09-12)**: `LogSys`, `Notification` (+ their `LogType`/`LogStatus`/`LogAccessLevel`
enums), and `CheckEmailAndPasswordDto` have been relocated to `Modules/Administration` — all three
had zero CQRS handlers/API surface (confirmed dead-but-schema-present tables), so this was a pure,
zero-risk namespace move (verified: `dotnet build`/`dotnet test` green, `dotnet ef
migrations has-pending-model-changes` reports no schema change). `CompanyProfileDto.cs` and
`OrderTypeDto.cs` — flagged below as likely dead duplicates — were confirmed to have zero
references anywhere in the codebase and were deleted outright rather than moved. **Not done**:
`Preferences.cs`/`PreferenceDto.cs`/the Preference CRUD slice (used by 8 modules — this needs its
own ownership decision, SharedKernel vs. a settings-service Contracts API, not a quick move) and
the two Journal-posting integration bridges (explicitly separate, high-risk items tied to Phase 4/8
of the modular-monolith work, not "legacy relocation").


Every business-relevant class still living in the root `Domain`/`Application` projects, classified
by real owner. Excludes the root `AdminContext`/`Application/DTOs/AdminDb/*` family — that's a
**separate DbContext** for SaaS tenant/plan provisioning (Client, Plan, PlanElement, Nationality,
etc.), not part of the 11-module ERP business domain at all, so it has no "owning module" in this
map and is out of scope for this migration.

| Current File | Business Concept | Owning Module | Target Project | Dependencies | Migration Status |
|---|---|---|---|---|---|
| `Domain/Entities/OrgDb/Preferences.cs` | Generic per-(Reference,TypeId,Key) settings store, read by nearly every module (`AutoCreateTransaction`, `OpeningBalanceClearingAccountId`, etc.) | *Cross-cutting technical concept, not a single business module's data* | Candidate: stays as a shared kernel-level concept (e.g. `BuildingBlocks/OrgSys.SharedKernel` or a new thin `Administration.Contracts`-exposed settings service), not owned by one ERP module | `Application/DTOs/OrgDb/PreferenceDto.cs`, `Application/Commands/Org/Setting/Preference/**`, referenced via `IRepository<Preference>` from Sales, Inventory, Treasury, Accounting, Receivables, Payables | Not started — Phase 10. High fan-out (touches ~8 modules), low individual risk per touch point |
| `Domain/Entities/OrgDb/LogSys.cs` | System audit/activity log | Administration (or a future `Observability` building block, per the brief's own §20) | `Modules/Administration/Administration.Domain` or `BuildingBlocks/Observability` | `Application/DTOs/OrgDb/LogSysDto.cs` | Not started — Phase 10 |
| `Domain/Entities/OrgDb/Notification.cs` | User-facing notifications | Administration | `Modules/Administration/Administration.Domain` | `Application/DTOs/OrgDb/NotificationDto.cs` | Not started — Phase 10 |
| `Application/DTOs/OrgDb/CheckEmailAndPasswordDto.cs` | Login/auth DTO | Administration | `Modules/Administration/Administration.Application` | none | Not started — Phase 10 (small, low risk) |
| `Application/DTOs/OrgDb/CompanyProfileDto.cs` | Company profile | Organization (the `CompanyProfile` entity itself already lives in `Modules/Organization/Organization.Domain`) | `Modules/Organization/Organization.Application` | none found referencing it outside the legacy root — likely a dead duplicate left behind when `CompanyProfile` moved to Organization | Not started — needs a "is this actually used?" check before moving, Phase 10 |
| `Application/DTOs/OrgDb/OrderTypeDto.cs` | Sales `Order` type | Sales | `Modules/Sales/Sales.Application` | none found referencing it outside the legacy root — `Order`/`OrderType` entities already live in `Sales.Domain`; this DTO looks like a dead duplicate from before the module split | Not started — needs a "is this actually used?" check, Phase 10 |
| `Application/DTOs/OrgDb/PreferenceDto.cs` | DTO for `Preferences` | *(same cross-cutting home as Preferences.cs)* | Same as `Preferences.cs` | Used by `Application/Commands/Org/Setting/Preference/**` | Not started — Phase 10 |
| `Application/Commands/Org/Setting/Preference/**` (7 files: Create/Delete/DeleteList/Update handlers, Get/List/Search query handlers, MappingProfile) | Preference CRUD | *(same cross-cutting home)* | Same as `Preferences.cs` | `Preferences.cs`, `PreferenceDto.cs` | Not started — Phase 10 |
| `Application/Commands/Org/Financials/Integration/JournalInvoice/InvoiceJournalIntegration.cs` | The Invoice→Journal GL-posting bridge — **this is the exact class Phase 4 ("Sales must not create JournalEntry directly") is about** | Accounting | `Modules/Accounting/Accounting.Application` (or the posting logic gets re-expressed as an `Accounting.Contracts` command handled internally by Accounting, triggered by a `SalesInvoicePostedIntegrationEvent`) | Called directly from `Sales.Application` (`Invoices/Commands/*.cs`) and `Inventory.Application` (indirectly, via the parallel `TransactionJournalIntegration`) | Not started — Phase 4. **Highest-risk item in this table**: it is the live GL-posting path for every invoice in production; any change needs debit==credit integration tests before/after |
| `Application/Commands/Org/Financials/Integration/JournalTransaction/TransactionJournalIntegration.cs` | The Transaction→Journal GL-posting bridge (Inventory's analogue) | Accounting | `Modules/Accounting/Accounting.Application` | Called from `Inventory.Application` (`Transactions/Commands/*.cs`, `Integration/*.cs`) | Not started — Phase 8 (Inventory→Accounting), same risk profile as the Invoice bridge |
| `Application/Mappings/MappingProfile.cs` | Old monolith AutoMapper aggregator | *(none — this is the shrinking root aggregator itself)* | N/A — deleted once the last `PreferenceMappingProfile()` call it makes is migrated with `Preferences.cs` | Calls `PreferenceMappingProfile()` only (everything else already delegated to per-module `MappingProfile.cs` files, per its own inline comments) | Will disappear naturally once `Preferences.cs` moves — not separately tracked |

## Summary

- **11 files** are genuinely misplaced business code needing a real target module.
- **2 files** (`CompanyProfileDto.cs`, `OrderTypeDto.cs`) look like dead duplicates from before
  their entities moved into `Organization.Domain`/`Sales.Domain` respectively — worth a
  reference-check-then-delete pass rather than a "migrate" pass.
- **The Journal-posting bridges are the highest-priority, highest-risk item** — they are also
  *the* concrete mechanism the whole "Accounting must exclusively own JournalEntry" rule (Phase 4)
  is about. Everything else in this table is lower risk and can move independently.
- No entity in this table is duplicated elsewhere — moving each one is a pure relocation
  (namespace + project reference + DI registration + AutoMapper profile), not a redesign, per the
  brief's own "do not duplicate working ERP entities" rule.
