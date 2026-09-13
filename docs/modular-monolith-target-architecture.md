# OrgSys — Modular Monolith Target Architecture

Companion to [`modular-monolith-analysis.md`](./modular-monolith-analysis.md). That document
describes what exists today; this document defines where it is going and the rules that govern
how it gets there. Nothing here is implemented yet — Phase 1 (foundation only) starts after this
document is reviewed.

---

## 1. Target Solution Structure

Per the brief, folders-first, `.csproj`-per-project only where the solution can't already express
the boundary cleanly. Given the current solution is small (4 active projects), the recommended
starting point is the **full `BuildingBlocks/` + `Modules/*` + `Host/` project layout** from the
brief, since Application alone already has 518 files across 6 feature areas — a single project
with folder-only boundaries would not give C#'s compiler any way to enforce the dependency rules
in §6, and enforcement is the entire point of this refactor (brief §29).

```text
OrgSys.sln
│
├── BuildingBlocks/
│   ├── OrgSys.SharedKernel/        (BaseEntity/AggregateRoot/ValueObject/DomainEvent/Result, generic CQRS marker interfaces)
│   ├── OrgSys.Contracts/           (cross-module contract marker types only, if any are truly global — expect this to stay near-empty; module contracts live in each module's own Contracts project)
│   ├── OrgSys.Infrastructure/      (generic Repository<T>/UnitOfWork abstractions, logging, caching abstractions)
│   └── OrgSys.EventBus/            (in-process MediatR-notification-based bus now; swappable transport later)
│
├── Modules/
│   ├── Administration/  {Administration.Domain, .Application, .Infrastructure, .Contracts}
│   ├── Organization/    {Organization.Domain, .Application, .Infrastructure, .Contracts}
│   ├── MasterData/      {MasterData.Domain, .Application, .Infrastructure, .Contracts}
│   ├── Sales/           {Sales.Domain, .Application, .Infrastructure, .Contracts}
│   ├── CommercialDocuments/ {CommercialDocuments.Domain, .Application, .Infrastructure, .Contracts}  (added 2026-09-13 — see §14; owns Invoice/InvoiceProduct/InvoiceType, shared by Sales and Purchasing)
│   ├── Purchasing/      {Purchasing.Domain, .Application, .Infrastructure, .Contracts}
│   ├── Inventory/       {Inventory.Domain, .Application, .Infrastructure, .Contracts}
│   ├── Receivables/     {Receivables.Domain, .Application, .Infrastructure, .Contracts}
│   ├── Payables/        {Payables.Domain, .Application, .Infrastructure, .Contracts}
│   ├── Treasury/        {Treasury.Domain, .Application, .Infrastructure, .Contracts}
│   ├── Accounting/      {Accounting.Domain, .Application, .Infrastructure, .Contracts}
│   └── Reporting/       {Reporting.Application, .Infrastructure, .Contracts}   (no Domain — read-only per brief §11)
│
└── Host/
    └── OrgSys.Api/      (replaces today's API project — composition root only)
```

`Sales.Domain` and `Purchasing.Domain` will **each reference the same physical `Dealer`/`Invoice`
tables** through a resolution described in §4 — they are listed as separate projects because they
own separate *behavior* (Customer lifecycle vs Supplier lifecycle), not because the data is
physically duplicated.

`Domain/`, `Application/`, `Infrastructure/`, `API/` (today's four projects) are retired once
every module's slice has a home; **this happens gradually across Phases 2–10**, not in Phase 1.
During the transition both the old projects and the new `Modules/*` projects coexist, with code
moving module-by-module per the phase order in the analysis doc §19.

---

## 2. SharedKernel — kept deliberately small

Per brief §18, only universal, business-meaning-free concepts belong here. From the current
codebase, this means:

| Goes in SharedKernel | Stays in the owning module |
|---|---|
| `Result`, `Error`, `Pagination`, `ResultCollection<T>`, `ResultPagination<T>` (from `Domain/Shared/Result.cs`) | — |
| Generic CQRS marker interfaces (`ICommand`, `IQuery<T>`, `ICommandHandler`, etc. from `Application/Interfaces/{CQRS,Command,Query}` — moved with corrected namespaces per analysis §5) | — |
| Generic base handlers (`CreateCommandHandler<TDto,TModel>`, `GetQuery`, `ListQuery`, `SearchCommandHandler` from `Application/Common/{Commands,Queries}`) | — |
| The **shape** of `BaseModel`/`MovementModel` (Id, Code, Status, Hide, Date, Posted, Review, HasJournal as a reusable "auditable transactional document" base) | The FK properties on `MovementModel` (`CreateUserId→User`, `ShiftId→Shift`, `BranchId→Branch`) do **not** belong in SharedKernel — they're Administration/Organization concepts. Model this as an interface (`IAuditableDocument` with `Guid`-or-`long` CreateUserId, no navigation property) that modules implement, not a base class with cross-module navigation properties baked in. |
| `IRepository<T>`/`IUnitOfWork` **abstractions** (interfaces only) | Their `OrgContext`-tied implementation stays in each module's Infrastructure (or, during the transition, in a shared `BuildingBlocks/OrgSys.Infrastructure` until module-specific persistence is introduced — see §7) |
| `FluentValidationFilter<TRequest,TResponse>` pipeline behavior | — |

Explicitly **not** in SharedKernel, per brief §18 and confirmed by the analysis: `Customer`/
`Dealer`, `Supplier`, `Invoice`, `Product`, `Account`, `Journal`, `Financial`/`FinancialTransaction`
— these are module-owned business entities even though they're widely referenced.

The dead `Domain/Shared/{LockupType*, TransactionType*}Entity.cs` hierarchy identified in the
analysis (§1) is **not** revived as part of SharedKernel — it was unused before and introducing it
now would just create a second, competing base-class story. Delete it during Phase 1 cleanup.

---

## 3. Module Ownership — Final Mapping

This resolves every entity from the analysis doc's mapping table (§13) to exactly one owning
module, with explicit notes where the analysis flagged shared ownership.

### Administration
`User`, `Role`, `Permission`, `RolePermission`, the `Commands/Org/Auth` slice (Login/CheckEmail/
CheckPassword/HavePassword), permission-tree seed data. **Dormant `AdminDb`** (Client, Plan,
LoginUser, Nationality, TypeActivity, General* lookups) stays out of the active module surface
until the team resolves the open question in the analysis doc §22 — if revived, it becomes a
`tenant-provisioning` sub-area inside Administration, not a separate module, since brief §3
defines Administration as owning "Authorization-related application logic" broadly.

### Organization
`Branch`, `CompanyProfile`, `Shift`, `Table`.

### MasterData
`Country`, `City`, `District`, `Unit`, `Classification`, `Currency`, `ReferenceType`,
`PaymentType`. Dormant `General*` AdminDb equivalents stay dormant (duplicate of the above,
resolve together with the Administration/AdminDb question).

### Sales
`Invoice`+`InvoiceProduct` where `InvoiceType ∈ {Sales Invoice, Sales Return}`, `Order`/
`OrderProduct`/`OrderType` (future — currently unwired), the "Customer" view of `Dealer`/
`DealerGroup` (see §4 for the resolution mechanism).

### Purchasing
`Invoice`+`InvoiceProduct` where `InvoiceType ∈ {Purchase Invoice, Purchase Return}`, the
"Supplier" view of `Dealer`/`DealerGroup` (see §4). **Update (2026-09-12)**: Purchasing is no
longer an empty shell — it now owns `PurchaseRequisition`+`PurchaseRequisitionProduct`
(Status.New → UnderReview → Approved, no formal approval gate) and
`PurchaseOrder`+`PurchaseOrderProduct` (`Dealer` = supplier, optional `PurchaseRequisitionId`
provenance link, optional `InvoiceId` set only by a manual `LinkInvoiceCommand` once a Purchase
Invoice has been created the normal way through Sales — no automatic Invoice/Journal/Transaction
generation happens anywhere in this module). See `Modules/Purchasing/Purchasing.Domain/Entities`
and the `PurchaseRequisitionController`/`PurchaseOrderController` API surface. Migration
`20260912074112_AddPurchasingModule` creates the four new tables; **not yet applied to any
database** — needs `dotnet ef database update` when the team is ready.

### Inventory
`Product`, `ProductUnit`, `ProductRecipe`, `Property`/`PropertyElement`/`ProductPropertyElement`
(currently unmapped — activate only if the team confirms intent), `Stock`, `Transaction`+
`TransactionProduct`+`TransactionType`, `Inventory`(entity)+`InventoryProduct`.
`Product.DealerId` (preferred-supplier link) becomes a nullable reference resolved through
`Purchasing.Contracts.ISupplierLookup`, not a direct FK navigation, once Inventory and Purchasing
are separate assemblies.

### Treasury
`CashBox`, `Bank`, `BankBranch`, `BankAccount`, `FinancialAccount`, `Financial`, `FinancialType`
(+ `Domain.Enums.FinancialTransactionType`, kept as one Id space per analysis §3), `FinancialTransfer`,
`FinancialInvoice`, `Outlay`. Extension points only (per brief §7) for: Cheque lifecycle status
history, Advances/Custody lifecycle, Notes Receivable/Payable — modeled as new `FinancialType`
rows plus (when a Cheque needs its own status history that a single `Financial` row can't express)
a new `Cheque` child-entity referencing `Financial`, not a parallel transaction table.

### Accounting
`Account`, `AccountType`, `Journal`+`JournalItem`+`JournalType`, `FiscalYear`, `FiscalPeriod`.

### Receivables
No dedicated tables — by design (2026-09-12 decision, see below). Owns: customer balance/aging
**application logic and read models**, built live over the existing Journal/JournalItem ledger
(no separate OpenItem/Allocation persistence) plus `Parties.Contracts`/`IReceivableAccountValidator`
for dealer identity, *not* new customer master data. `SetCustomerOpeningBalanceCommand` already
lives here. **Update**: `GetCustomerBalanceQuery`/`GetCustomerAgingQuery` (`Receivables.Contracts.
Balances`) now give Receivables its own real "Outstanding Balance"/"Aging" surface — aging is
computed by FIFO-matching each Journal debit (charge) against later credits (receipts) on the
customer's receivable account, since there is no per-invoice open-item ledger to age directly.
Exposed via `ReceivableController` (`GET /Receivable/{dealerId}/Balance`, `.../Aging`).

### Payables
Mirror of Receivables for suppliers, consuming `Accounting.Application`'s validators +
`Parties.Contracts`. `SetSupplierOpeningBalanceCommand` already lives here. **Update**: mirrors
Receivables' Balance/Aging addition — `GetSupplierBalanceQuery`/`GetSupplierAgingQuery`
(`Payables.Contracts.Balances`), Debit/Credit roles swapped (a payable account's "charge" posts as
Credit, its "payment" as Debit). Exposed via `PayableController` (`GET /Payable/{dealerId}/Balance`,
`.../Aging`), alongside the existing `OpeningBalance` endpoint.

### Reporting
`Commands/Org/Reports/*` and `DTOs/Report/*` move here wholesale, re-pointed at each module's
Contracts/read models per module as those become available (this is why Reporting is Phase 9 —
it can only be finished once its dependencies exist as Contracts).

### Dealer/Invoice cross-cutting resolution — read this before moving `Dealer` or `Invoice`

`Dealer` (customer or supplier via `DealerType`) and `Invoice` (four document kinds via
`InvoiceType`) are **not split into separate tables** — brief §4/§5/§23 explicitly forbid that.
The resolution:

- **Physical entity + EF mapping ownership**: assign to **Sales** (it already owns the "primary"
  document flow — Sales Invoice is `InvoiceType=1`, the seed default). Sales.Infrastructure owns
  the `IEntityTypeConfiguration<Dealer>`/`IEntityTypeConfiguration<Invoice>` and the migration
  history for these tables going forward.
- **Purchasing's access**: Purchasing.Application depends on `Sales.Contracts` for read/write
  access to Dealer-as-Supplier and Invoice-as-PurchaseInvoice — e.g.
  `Sales.Contracts.IDealerModuleApi.GetSupplier(id)`, `ICreatePurchaseInvoiceCommand` handled
  inside Sales.Application but exposed as a Purchasing-facing contract. This keeps the brief's
  rule "no cross-module DbSet/Repository/domain-entity access" intact — Purchasing never sees
  `Sales.Domain.Dealer`, only `Sales.Contracts.SupplierDto`.
- Alternative considered and rejected: physically duplicating `Dealer`/`Invoice` per module.
  Rejected because it directly violates brief §6 ("Do NOT duplicate entities or screens") and
  §23 ("preserve existing type-driven unified models").
- This is the one module boundary in this system that is *not* a clean microservice seam — flag
  it explicitly if/when Sales or Purchasing is ever extracted to a real microservice: at that
  point `Dealer`/`Invoice` ownership has to fully move to one service and the other becomes a
  true client of it (no more shared DB access), which is a bigger step than any other module
  extraction in this list.

---

## 4. Contracts — What Each Module Exposes

Per brief §13, every module gets a `Contracts` project with DTOs, public interfaces, lookup
models, and integration events. Concretely, seeded from what the analysis found other modules
already need today:

```text
Accounting.Contracts/
├── AccountLookupDto, AccountTreeNodeDto
├── IAccountingModuleApi          (GetAccount, ValidatePostable, GetOrCreateDealerAccount)
├── IAccountingPeriodApi          (ResolveFiscalPeriod, ValidatePostingDate — replaces direct AccountingPeriodService injection)
└── Events/ JournalEntryPosted, JournalEntryReversed

Treasury.Contracts/
├── FinancialAccountLookupDto, CashBoxLookupDto, BankAccountLookupDto
├── ITreasuryModuleApi            (PostFinancialTransaction, GetFinancialAccountBalance)
└── Events/ PaymentPosted, ReceiptPosted, FinancialTransferPosted

Inventory.Contracts/
├── ProductLookupDto, StockLookupDto, TransactionLookupDto
├── IInventoryModuleApi           (PostStockTransaction, GetProductBalance)
└── Events/ InventoryTransactionPosted

Sales.Contracts/
├── CustomerLookupDto, SupplierLookupDto (both backed by Dealer — see §3 resolution), SalesInvoiceLookupDto
├── IDealerModuleApi              (GetCustomer, GetSupplier, GetOrCreateDealerAccount)
├── ISalesInvoiceModuleApi        (CreateSalesInvoice, GetInvoicesNotReturned)
├── IPurchaseInvoiceModuleApi     (consumed by Purchasing — see §3)
└── Events/ SalesInvoicePosted, SalesInvoiceCancelled

Purchasing.Contracts/
├── PurchaseOrderLookupDto (future)
└── Events/ PurchaseInvoicePosted, PurchaseInvoiceCancelled

MasterData.Contracts/
├── CountryLookupDto, CityLookupDto, DistrictLookupDto, CurrencyLookupDto, UnitLookupDto, ClassificationLookupDto, PaymentTypeLookupDto, ReferenceTypeLookupDto
└── IMasterDataModuleApi

Organization.Contracts/
├── BranchLookupDto, ShiftLookupDto
└── IOrganizationModuleApi

Administration.Contracts/
├── UserLookupDto, RoleLookupDto, PermissionLookupDto
└── IAdministrationModuleApi      (GetCurrentUserPermissions — the future home of server-side permission enforcement, see analysis §10/§17)

Receivables.Contracts / Payables.Contracts/
└── {Customer,Supplier}BalanceDto, AgingBucketDto, IReceivablesModuleApi / IPayablesModuleApi

Reporting.Contracts/
└── (consumer only — no other module depends on Reporting)
```

This directly replaces the direct-repository-injection pattern the analysis found in Invoice/
Transaction/Financial/Journal handlers (analysis §5, §14): e.g.
`CreateJournalByInvoiceCommandHandler` today injects `IRepository<Journal>` directly; after the
split it calls `IAccountingModuleApi` (or reacts to a `SalesInvoicePosted` event that Accounting's
own handler subscribes to — see §5 for which pattern to use where).

---

## 5. Integration Events vs Direct Contract Calls — when to use which

The brief allows both synchronous Contracts calls and MediatR-notification integration events.
Guidance, derived from the specific coupling patterns the analysis found:

- **Use a synchronous Contracts call** when the caller needs the result *in the same request* to
  decide what to do next — e.g. `Invoice` creation needs `Inventory`'s stock-availability check
  before it can succeed; `Dealer` creation needs `Accounting`'s `GetOrCreateDealerAccount` before
  the dealer record is complete. This matches today's direct-injection call sites almost 1:1 —
  they become interface calls instead of repository calls, same control flow.
- **Use an integration event (in-process MediatR notification for now)** when the reaction is a
  side effect that doesn't gate the original operation's success — e.g. today's `*Integration`
  classes (`InvoiceJournalIntegration`, `TransactionJournalIntegration`,
  `TransferReceivedIntegration`, `InventoryAdjustmentIntegration`) all fire *after* the primary
  entity is already saved. These become: Sales publishes `SalesInvoicePosted`; Accounting,
  Inventory, and Receivables each have their own `INotificationHandler<SalesInvoicePosted>` that
  does its own posting inside its own module, using its own `IUnitOfWork`.
- **Consequence for the circular-dependency risks in analysis §15**: the Sales↔Treasury↔Accounting
  triangle and the Inventory↔Sales triangle both resolve this way — `SalesInvoicePosted` flows
  one direction (Sales → {Inventory, Accounting, Receivables}), and nothing calls back into Sales
  synchronously from those handlers. If a handler *does* need to call back (e.g. Inventory needs
  to tell Sales the stock transaction failed), that's a synchronous Contracts call in the
  *original* request path (checked before Sales commits), not a call from inside an event handler.
- **Transactionality note**: today everything is one `SaveChangesAsync` call inside one
  `OrgContext`. In-process MediatR notifications published *within* the same
  `SaveChangesAsync`/transaction scope preserve this — event handlers run synchronously, in the
  same DB transaction, before the HTTP response returns. This must be true until module-specific
  `DbContext`s exist (explicitly deferred, brief §17); do not introduce eventual consistency /
  outbox patterns yet, since nothing about the current single-database architecture requires it.

---

## 6. Module Dependency Rules

### Allowed

```text
Module.Domain          → SharedKernel
Module.Application      → Module.Domain
Module.Application      → OtherModule.Contracts
Module.Infrastructure    → Module.Application
Module.Infrastructure    → Module.Domain
Module.Contracts         → SharedKernel   (DTOs/interfaces only, no Domain types leak out)
Host/OrgSys.Api          → every Module's registration extension + every Module.Contracts (for composition only, never Module.Domain/Infrastructure directly)
```

### Forbidden

```text
Sales.Domain          → Inventory.Domain           (per brief §29 example)
Sales.Domain          → Treasury.Domain
Inventory.Domain       → Accounting.Domain
Treasury.Domain        → Accounting.Domain
AnyModule.Application   → OtherModule.Domain         (must go through OtherModule.Contracts)
AnyModule.Application   → OtherModule.Infrastructure  (must go through OtherModule.Contracts)
AnyModule.*            → OtherModule.*Repository/DbSet (no cross-module EF access, ever)
```

### Enforcement (brief §29)

Add an architecture-test project (e.g. `Tests/Architecture.Tests`, using `NetArchTest.Rules` or
equivalent) asserting the forbidden list above by assembly reference, run in CI. Per analysis
§20, write this test in **Phase 1**, before any module code exists — it will pass trivially at
first (there's nothing to violate yet) and starts catching real violations the moment Phase 2
(Accounting) begins.

---

## 7. Database & EF Core Strategy

- **One physical database, one connection string, `OrgContext` remains the single `DbContext`**
  through Phases 1–9, exactly as brief §16/§17 requires. No `module.*` schema prefixes are applied
  to actual tables yet — §3's "Database Ownership Map" in the analysis doc is a *code-ownership*
  map, not a migration plan.
- **What does change**: `OnModelCreating`'s current single block of fluent configuration gets
  split into per-module `IEntityTypeConfiguration<T>` classes, physically located in each module's
  `Infrastructure/Persistence/Configurations/` folder, and `OrgContext.OnModelCreating` calls
  `modelBuilder.ApplyConfigurationsFromAssembly(...)` once per module assembly instead of inline
  fluent calls. This is purely a code-organization change — it must produce a **zero-diff EF Core
  migration** (verify with `dotnet ef migrations add VerifyNoOp` producing an empty
  `Up()`/`Down()`, then delete it) before being considered done for a given module, per brief §28.
- **Repository/UnitOfWork**: stays the shared generic `IRepository<T>`/`IUnitOfWork` from
  `BuildingBlocks/OrgSys.Infrastructure`, still backed by the one `OrgContext`, for the same
  reason — brief §17 explicitly says not to force multiple `DbContext`s during the first pass.
  Each module's own `Infrastructure` project only adds module-specific query/repository code
  where the generic repository genuinely doesn't fit (e.g. the `JournalItem`-summing balance
  query) — it does not reimplement the generic CRUD plumbing per module.
- **Migrations**: continue living in one place (today's `Infrastructure/Migrations`) until/unless
  the team later decides to split `DbContext`s per module — not part of this refactor's scope.
- **`AdminContext`**: left exactly as-is (excluded from compilation, unregistered) until the open
  question in analysis §22 is resolved. Do not "fix" its exclusion as a side effect of this work.

---

## 8. CQRS / MediatR Organization

Per brief §15, each module's Application project gets its own feature-first structure, replacing
today's single `Application/Commands/Org/<Area>/<Feature>/{Commands,Queries}` tree:

```text
Sales.Application/
├── Invoices/
│   ├── Commands/     (CreateSalesInvoice, UpdateSalesInvoice, CancelSalesInvoice, RedoSalesInvoice, CollectPaidInvoice, ...)
│   ├── Queries/      (GetSalesInvoiceById, SearchSalesInvoices, GetInvoicesNotReturned, ...)
│   └── Handlers/     (or colocate handler with command/query, matching today's convention — either is acceptable, pick one and apply it consistently across all modules)
├── Dealers/           (Customer-facing slice of Dealer)
│   ├── Commands/
│   └── Queries/
└── DependencyInjection/
    └── ServiceCollectionExtensions.cs   (AddSalesModule)
```

- MediatR/AutoMapper/FluentValidation registration scanning moves from "one assembly, scanned
  once in `Program.cs`" (today) to "each module's `AddXModule()` calls
  `services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<...>())` for its own
  assembly," and `Host/OrgSys.Api`'s composition root calls every module's `AddXModule()`. This
  directly fixes the duplicate-registration issue found in analysis §9 finding #5 by construction
  — there's no longer a single place to accidentally register the same assembly twice.
- AutoMapper: each module gets its own real `Profile` class (not a `partial class` spread across
  files pretending to be one profile, per analysis §5) — e.g. `Sales.Application.MappingProfile :
  Profile`. `AddAutoMapper` in the Host registers every module's profile type explicitly.
- FluentValidation: each module's validators move with their commands; rename the
  copy-pasted-from-User filenames identified in analysis §5 while moving them (e.g.
  `CreateUserCommandValidator.cs` containing `CreateBankCommandValidator` becomes
  `CreateBankCommandValidator.cs` in `Treasury.Application/Banks/Validators/`).
- The `FluentValidationFilter<TRequest,TResponse>` pipeline behavior is registered once, globally,
  from `BuildingBlocks/OrgSys.SharedKernel`/`OrgSys.Infrastructure` — it's generic, not
  module-specific.

---

## 9. Host / Composition Root

`Host/OrgSys.Api/Program.cs` becomes purely composition:

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddSharedKernel()
    .AddAdministrationModule(builder.Configuration)
    .AddOrganizationModule(builder.Configuration)
    .AddMasterDataModule(builder.Configuration)
    .AddSalesModule(builder.Configuration)
    .AddPurchasingModule(builder.Configuration)
    .AddInventoryModule(builder.Configuration)
    .AddTreasuryModule(builder.Configuration)
    .AddAccountingModule(builder.Configuration)
    .AddReceivablesModule(builder.Configuration)
    .AddPayablesModule(builder.Configuration)
    .AddReportingModule(builder.Configuration);

builder.Services.AddDbContext<OrgContext>(...);   // still one shared context, per §7
builder.Services.AddAuthentication(...).AddJwtBearer(...);
builder.Services.AddSwaggerGen(...);              // once, not twice — fixes analysis §9 finding #5
builder.Services.AddControllers();                 // once, not twice

var app = builder.Build();
app.UseMiddleware<GlobalExceptionMiddleware>();     // moved before UseAuthentication/UseAuthorization — fixes analysis §9 finding #4
app.UseHttpsRedirection();
app.UseCors("AngularClient");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
```

Each `AddXModule()` extension method lives in that module's `Infrastructure` project (per brief
§20) and internally registers that module's `DbContext`-dependent services (still pointing at the
one shared `OrgContext` per §7), MediatR assembly, AutoMapper profile, FluentValidation
validators, and any module-specific singleton services (e.g. `Treasury`'s equivalent of today's
`IReceivableAccountValidator`/`IPayableAccountValidator`, redesigned per §3's Receivables/Payables
resolution).

Controllers move into each module (or stay in `Host/OrgSys.Api/Controllers` grouped by module
folder, if the team prefers controllers-stay-thin-and-central — either satisfies brief §20's "API
acts as composition root" as long as controllers contain no business logic, only MediatR
dispatch). Given the current `BaseController<...>` generic-CRUD pattern is itself shared
infrastructure, the pragmatic choice is: **keep `BaseController<...>` in
`BuildingBlocks/OrgSys.Infrastructure` (or a thin `Host`-local shared base), move concrete
controllers into `Host/OrgSys.Api/Controllers/<Module>/` folders** matching each module, preserving
today's `[Route("[controller]")]` behavior exactly (analysis §17 finding #1) — no `AddApplicationPart`
complexity needed since controllers stay physically in the one `Host` assembly.

---

## 10. Angular Alignment

Per brief §21 and analysis §11/§17, no route changes are required for Phase 1–9 — routes are
controller-attribute-driven, not folder-driven. However, the analysis found the *Angular menu*
already groups things closer to the target modules than the current backend folders do
(`Setting/` is the outlier). Recommended parallel-track cleanup (not blocking, not required by
this refactor, but worth flagging to whoever owns `OrgSys.Angular`):

| Backend module (this refactor) | Angular alignment opportunity |
|---|---|
| Accounting | Angular's "Accounting" menu group already separates from "Settings" — once `AccountController` moves out of `Setting/`, Angular's `/accounting/accounts` route can point at it directly without an API gateway shim |
| Treasury | `CashBoxController` should join `Financials/*` controllers physically, matching Angular's "Financial" menu group already including Cash Boxes |
| Sales/Purchasing | `DealerController`/`DealerGroupController` should get their own controller folder (or two, mirroring `Sales`/`Purchasing`) matching Angular's dedicated "Customers & Suppliers" menu group |

This is purely a backend code-organization move (§17 in the analysis already flags it as low
risk) — it does not require Angular changes since routes don't change.

---

## 11. Final Dependency Direction

```text
                              OrgSys.Api (Host)
                                    │
     ┌───────────┬───────────┬─────┼─────┬────────────┬────────────┐
     │           │           │     │     │            │            │
   Sales    Purchasing  Inventory Treasury Receivables Payables  Reporting
     │           │           │     │     │            │            │
     └─────┬─────┴─────┬─────┴──┬──┴─────┴─────┬──────┘            │
           │           │        │              │                   │
           └───────────┴───Contracts/Events────┴───────────────────┘
                                    │
                              Accounting
                                    │
                        MasterData · Organization · Administration
                                    │
                              SharedKernel
```

Accounting sits below the transactional modules (per analysis §14's "most-depended-upon" finding)
but still communicates only through `Accounting.Contracts` — nothing reaches into
`Accounting.Domain`/`Accounting.Infrastructure` directly, including Treasury and Inventory (brief
§29's explicit forbidden-dependency examples). MasterData/Organization/Administration sit
below everything as pure reference data with no outbound module dependencies of their own.

---

## 12. Summary of Rules (brief's Final Principle, restated against this codebase)

- **Modules own business behavior** — `Financial`+`FinancialType`, `Invoice`+`InvoiceType`,
  `Transaction`+`TransactionType` stay unified inside their owning module exactly as they are
  today; no per-movement-type entity splitting.
- **Contracts define communication** — every direct repository injection the analysis found
  crossing a future module boundary (Invoice→Financial/Journal/Transaction, Transaction→Invoice/
  Journal, Setting/Dealer→Account/JournalItem) becomes a `Module.Contracts` interface call or an
  integration event, per §5's guidance on which to use where.
- **SharedKernel contains only universal concepts** — `Result`/generic CQRS plumbing only; no
  `Dealer`, `Invoice`, `Account`, `Financial`, `Journal`.
- **Infrastructure is module-owned** — except the generic `Repository<T>`/`UnitOfWork`/
  `OrgContext`, which stay shared per brief §17 until module-specific persistence is a deliberate
  later decision.
- **The API host only composes modules** — `Program.cs` becomes a list of `AddXModule()` calls;
  the duplicate registrations and middleware-ordering issue found in the analysis get fixed as a
  natural side effect of this restructuring, not as a separate unrelated change.
- **No module accesses another module's internals** — enforced by project references plus an
  architecture-test project, written in Phase 1 per §6 above.

---

## 13. Addendum — Parties / Catalog / CommercialDocuments / ReferenceData evaluation

A later, differently-scoped brief asked specifically whether shared business concepts should be
split out of Sales/Inventory/MasterData into four new modules: `Parties`, `Catalog`,
`CommercialDocuments`, `ReferenceData`. Full evidence is in
[`docs/shared-business-capabilities-review.md`](./shared-business-capabilities-review.md) and
[`docs/masterdata-decomposition.md`](./masterdata-decomposition.md); this section records the
resulting decision so it doesn't get re-litigated without new evidence.

| Proposed module | Decision | Why |
|---|---|---|
| **Parties** | **Done.** `Dealer`/`DealerGroup`/`DealerType` relocated from `Sales.Domain`/`Sales.Application` to a new `Modules/Parties/{Parties.Domain,.Application,.Contracts,.Infrastructure}` (same `[Table("Dealer")]`/`[Table("DealerGroup")]` mapping — confirmed zero schema change via `dotnet ef migrations has-pending-model-changes`). Consumers (Accounting, Treasury, Inventory, Receivables, Payables, Reporting, plus Sales itself, root `Application`/`Infrastructure`, and the legacy `OrgSys` MVC app) now reference `Parties.Domain`/`Parties.Application` directly, in the same style as their pre-existing direct references to `Sales.Domain` — this relocation fixes *ownership*, not the pre-existing lack-of-Contracts pattern; `Parties.Contracts` is scaffolded but still empty, matching `Sales.Contracts`/`MasterData.Contracts`'s current state. Architecture.Tests' accepted-exception tables were updated to point at `Parties` wherever they previously pointed at `Sales` for Dealer-related reasons; all 740 tests pass. This supersedes §3's "Dealer/Invoice cross-cutting resolution", which assigned `Dealer` to Sales alongside `Invoice` — that pairing was reasonable when the only question was layer-boundary hardening, but doesn't hold once the question is "does this concept have independent business ownership" (brief §20's test): `Invoice` passes as a Sales workflow artifact, `Dealer` did not (6-module fan-out, none of it Sales-specific). **Update**: `Parties.Contracts` now has a real surface (`DealerType`, `DealerLookupDto`, `GetDealerByIdQuery`, `GetDealerNamesQuery`). `Accounting.Application`'s AR/AP validators, and `Receivables.Application`/`Payables.Application`'s opening-balance handlers, were migrated off direct `Parties.Domain` access onto this Contracts surface — those 3 modules now have **zero** Application-layer dependency on `Parties.Domain`. **Still not done, and deliberately deferred**: `Treasury.Application`/`Domain`, `Inventory.Application`/`Domain`, `Reporting.Application`, and `Sales.Domain` itself still reach `Parties.Domain.Dealer` directly via EF navigation (`Financial.Dealer`, `Product.Dealer`/`Transaction.Dealer`, `Invoice.Dealer`/`Order.Dealer`) — removing those means dropping Domain-level navigation properties, the same higher-risk, one-navigation-at-a-time work this doc already scopes as Phase 9 for every other module's identical Country/City/Currency/Account/Branch/Shift navigations. Not attempted here to keep this change consistent with how the rest of the codebase is currently phased. |
| **Catalog** | **Not justified — do not add.** `Product` already has exactly one owner (`Inventory.Domain`), `Classification`/`Unit` are correctly owned by MasterData and referenced by FK, and Sales/Purchasing already reference `Product` by ID only (no navigation, no duplication). No `ProductCategory`/`ProductGroup`/`Barcode`-entity/`PriceList` exists to give a new module real content. |
| **CommercialDocuments** | **Superseded — done, 2026-09-13.** The 2026-09-12 "not justified" conclusion held only while `Purchasing` had zero domain code; once `Purchasing.Application.LinkInvoiceCommandHandler` started reading `Sales.Domain.Invoice` directly (real `Purchasing.Application → Sales.Domain` coupling), the "Purchasing has zero domain code to protect from duplication" premise no longer applied. See §14 for the full implementation record. |
| **ReferenceData** | **Content is already correct; rename is optional.** All 8 `MasterData.Domain` entities (`Country, City, District, Currency, Classification, PaymentType, ReferenceType, Unit`) are genuine lightweight reference data — nothing business-aggregate-shaped is hiding there. **Update**: `MasterData.Contracts` now has its first real entry (`CurrencyLookupDto`/`GetDefaultCurrencyQuery`), and `Receivables.Application`/`Payables.Application` were migrated off `IRepository<Currency>` onto it. The three DTOs that leaked the domain entity by inheritance (`CountryDto : Country`, `PaymentTypeDto : PaymentType`, `ReferenceTypeDto : ReferenceType`) were also fixed to extend `BaseModel` like every other MasterData DTO. Still open: Country/City/District/Classification/PaymentType/Unit/ReferenceType Contracts entries, and the wider Domain-level navigation coupling (Bank.Country, Journal.Currency, Invoice.Currency, etc.) — same deferred-to-Phase-9 reasoning as Parties. Renaming `MasterData` → `ReferenceData` is still cosmetic/optional. |

Net effect on §1's target solution structure: add one new module, `Modules/Parties/
{Parties.Domain, .Application, .Infrastructure, .Contracts}`, positioned at the same
reference-data tier as MasterData/Organization/Administration in §11's dependency diagram (it has
no outbound dependencies of its own, and Sales/Purchasing/Treasury/Inventory/Accounting/
Receivables/Payables/Reporting all depend on `Parties.Contracts`). No other module count changes.

---

## 14. Addendum — CommercialDocuments implementation (2026-09-13)

Full detail in [`docs/commercial-documents-module.md`](./commercial-documents-module.md). Summary
of what changed and why, so §13's table doesn't need re-litigating:

**Trigger**: `Purchasing.Application.LinkInvoiceCommandHandler` had grown a real
`IRepository<Sales.Domain.Invoice>` injection (`Purchasing.Application → Sales.Domain`), the exact
"Application depends on another module's Domain merely because both use Invoice" problem the
brief's architecture rules exist to prevent. This made the 2026-09-12 "Invoice already has exactly
one clean owner, no new module needed" conclusion stale — Purchasing now had real domain code
depending on Sales' Invoice, not zero.

**What moved**: `Invoice`, `InvoiceProduct`, `InvoiceType` relocated from `Sales.Domain` to a new
`Modules/CommercialDocuments/{CommercialDocuments.Domain, .Application, .Contracts,
.Infrastructure}` (same `[Table("Invoice")]`/`[Table("InvoiceProduct")]`/`[Table("InvoiceType")]`
mapping — confirmed zero schema change via `dotnet ef migrations has-pending-model-changes`, no
new migration needed at all). The Invoice/InvoiceType CQRS handlers, DTOs and AutoMapper profile
(`Sales.Application/Invoices/*`, `Sales.Application/InvoiceTypes/*` — 26 files) moved with it into
`CommercialDocuments.Application`, unchanged in behavior. `Sales.Application` is left owning only
`OrderDto`/`OrderProductDto` (Order has no CQRS surface yet) — Sales Order/Quotation workflow
remains entirely Sales' own, per §3.

**Purchasing decoupled for real**: `LinkInvoiceCommandHandler` no longer injects
`IRepository<Invoice>` at all. It resolves the target invoice through a new
`CommercialDocuments.Contracts.Invoices.GetInvoiceReferenceQuery` (same `IQuery<T>`/MediatR pattern
already used by `Parties.Contracts.GetDealerByIdQuery`), handled by
`CommercialDocuments.Application.GetInvoiceReferenceQueryHandler`. `Purchasing.Application.csproj`
no longer references `Sales.Domain` or `CommercialDocuments.Domain` — only
`CommercialDocuments.Contracts`. The old `TypeId is 2 or 4` magic-number check became
`CommercialDocuments.Contracts.Invoices.InvoiceTypeId.Purchase`/`.PurchaseReturn` (existing DB IDs
1–4 preserved exactly, per brief §9).

**Other consumers repointed, not rewritten**: `Treasury.Domain`/`.Application` (`FinancialInvoice.
Invoice` navigation + 6 handlers), `Inventory.Application` (5 Transaction handlers),
`Reporting.Application` (2 report handlers, `InvoiceType`-only), `Sales.Domain` itself (`Order.
Invoice` navigation), and the legacy root `Application` project's `InvoiceJournalIntegration`
bridge all had their `Sales.Domain.Invoice`/`InvoiceType` references mechanically repointed to
`CommercialDocuments.Domain` — same direct-repository-injection shape as before, just against the
new owner. **Deliberately not done in this pass**: rewriting those 6 Treasury handlers (which both
read *and mutate* Invoice.Paid/Credit) onto a Contracts-based command is out of scope for an
ownership-correction change — same "Phase 4-class debt, tracked not fixed" reasoning this doc
already applies to Sales→Accounting's JournalId/JournalCode reads and Inventory's equivalent. All
now appear as documented `AcceptedApplicationDomainExceptions`/`AcceptedDomainExceptions` entries in
`Tests/Architecture.Tests`, retargeted from `Sales`/`CommercialDocuments` instead of introducing new
technical debt.

**Verification**: `dotnet build` (0 errors), `dotnet test` (887/887 architecture tests + 66/66
application tests, including 7 new tests for `GetInvoiceReferenceQueryHandler`/
`LinkInvoiceCommandHandler`), `dotnet ef migrations has-pending-model-changes` → "No changes have
been made to the model since the last migration."

Net effect on §1: add `Modules/CommercialDocuments/{CommercialDocuments.Domain, .Application,
.Contracts, .Infrastructure}`, sitting between Sales and Purchasing in §11's dependency diagram —
both depend on `CommercialDocuments.Contracts`, neither depends on the other's Domain because of
Invoice anymore.
