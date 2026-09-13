# CommercialDocuments Module

Companion to [`docs/modular-monolith-target-architecture.md`](./modular-monolith-target-architecture.md)
§13/§14. This document is the standalone reference for the module: what it owns, what it doesn't,
who depends on it, and how posting/returns flow through it.

## Purpose

`CommercialDocuments` is the single authoritative owner of the `Invoice` document — the shared
commercial-document concept previously owned by `Sales.Domain` even though `Purchasing` also
needed it. It exists because `Purchasing.Application` had grown a real dependency on
`Sales.Domain.Invoice` (via `LinkInvoiceCommandHandler`), which is exactly the
"`Purchasing.Application → Sales.Domain` merely because both use Invoice" coupling this codebase's
own architecture tests exist to prevent.

It is **not** a generic shared/utility project. It is a real bounded business capability: it owns
the Invoice document's shape, its lifecycle rules, its totals/discount/tax calculations, and its
type discriminator — nothing more.

## Ownership

**Owns** (moved from `Sales.Domain`/`Sales.Application`, unchanged in shape or behavior):

- `Invoice`, `InvoiceProduct` (entities, `[Table("Invoice")]`/`[Table("InvoiceProduct")]`)
- `InvoiceType` (DB-backed lookup/discriminator table, not a C# enum)
- Invoice CRUD/search CQRS (`CommercialDocuments.Application/Invoices/*`,
  `CommercialDocuments.Application/InvoiceTypes/*`)
- Invoice totals/discount/tax calculation logic (inline in the command handlers, moved as-is)
- The `InvoiceJournalIntegration` Invoice→Journal posting bridge remains where it always was — the
  legacy root `Application` project — CommercialDocuments does not insert accounting entries
  itself; it depends on the same bridge Sales did (see "Known remaining debt" below).

**Does not own** (explicitly, per brief — these stay exactly where they were):

- `Order`/`OrderProduct`/`OrderType` — stays in `Sales.Domain`. Sales owns its own quotation/order
  workflow; `Order.Invoice` is a plain EF navigation to the CommercialDocuments-owned Invoice, not a
  reason to move Order too.
- `PurchaseRequisition`/`PurchaseOrder` — stays in `Purchasing.Domain`.
- `Dealer`/`DealerGroup` (Parties), `Product` (Inventory), `Journal`/`Account` (Accounting),
  `Financial`/`FinancialInvoice` (Treasury), `Unit`/`Currency`/`PaymentType` (MasterData) — all
  referenced by ID/FK from Invoice, never duplicated.

## Invoice type discriminator (`InvoiceTypeId`)

The existing, already-in-production `TypeId` values are preserved exactly — do not renumber:

| Value | Meaning |
|---|---|
| 1 | Sales |
| 2 | Purchase |
| 3 | SalesReturn |
| 4 | PurchaseReturn |

`CommercialDocuments.Contracts.Invoices.InvoiceTypeId` is the semantic enum other modules should
use instead of bare integer literals (`TypeId == 2`) — used by `Purchasing.Application`'s
`LinkInvoiceCommandHandler` and by `CommercialDocuments.Application`'s own
`GetInvoiceReferenceQueryHandler`. Pre-existing magic-number checks elsewhere in the codebase
(Treasury, Reporting, the legacy `InvoiceJournalIntegration` bridge) were left as-is — renumbering
those call sites is cosmetic risk with no architectural benefit and wasn't attempted.

## Database

No schema change. `Invoice`/`InvoiceProduct`/`InvoiceType` map to the same tables they always did;
only the owning C# namespace changed (`Sales.Domain` → `CommercialDocuments.Domain`). Verified with
`dotnet ef migrations has-pending-model-changes` → *"No changes have been made to the model since
the last migration."* No new migration was needed at all — EF's model differ compares relational
shape (tables/columns/keys/FKs), not the CLR namespace of the mapped type, so the namespace move is
invisible to it.

The shared `OrgContext` (root `Infrastructure` project) continues to own the actual
`DbSet<Invoice>`/EF configuration, exactly as every other module's persistence does today —
`CommercialDocuments.Infrastructure` only holds the module's `AddCommercialDocumentsModule()`
composition-root registration (MediatR/AutoMapper/FluentValidation), matching the convention every
other module's `*.Infrastructure` project already follows.

## Allowed dependencies

```text
CommercialDocuments.Domain
    → SharedKernel
    → MasterData.Domain        (Invoice.Currency, Invoice.PaymentType)
    → Parties.Domain           (Invoice.Dealer)

CommercialDocuments.Application
    → CommercialDocuments.Domain
    → CommercialDocuments.Contracts
    → MasterData.Application   (Unit/UnitDto mapping)
    → Accounting.Domain        (Invoice Get/Search populate JournalId/JournalCode — same debt
                                 Sales.Application carried before the move, not introduced by it)
    → Treasury.Contracts, Inventory.Contracts (linked Transaction/Financial side effects)
    → root Domain/Application  (Preference + the InvoiceJournalIntegration bridge — not yet
                                 extracted from the monolith, same as before the move)

CommercialDocuments.Infrastructure
    → CommercialDocuments.Application
```

## Forbidden dependencies

```text
CommercialDocuments.Domain → Sales.Domain            (never — enforced by
                                                        CommercialDocumentsOwnershipTests)
CommercialDocuments.Domain → Purchasing.Domain        (never — enforced)
CommercialDocuments.Domain → Accounting.Domain        (never — enforced)
CommercialDocuments.Domain → Inventory.Domain         (never — enforced)
Purchasing.Application     → Sales.Domain             (never — enforced; was the trigger for this module)
Purchasing.Application     → CommercialDocuments.Domain (never — Purchasing only depends on
                                                          CommercialDocuments.Contracts)
```

## Who depends on CommercialDocuments, and how

| Consumer | Path | Reason |
|---|---|---|
| `Sales.Application` | *(none — Invoice CQRS moved here)* | Sales no longer owns Invoice; it owns Order only |
| `Sales.Domain` | `CommercialDocuments.Domain` (direct navigation) | `Order.Invoice` EF navigation |
| `Purchasing.Application` | `CommercialDocuments.Contracts` (`GetInvoiceReferenceQuery` via MediatR) | `LinkInvoiceCommandHandler` verifies a Purchase/PurchaseReturn invoice before linking it to a `PurchaseOrder` — the one consumer that was rewritten onto Contracts rather than repointed |
| `Treasury.Domain`/`.Application` | `CommercialDocuments.Domain` (direct repository/navigation) | `FinancialInvoice.Invoice` navigation; 6 Financial handlers read (and one mutates) linked Invoice — repointed from `Sales.Domain`, not rearchitected |
| `Inventory.Application` | `CommercialDocuments.Domain` (direct repository) | 5 Transaction handlers read the linked Invoice — repointed from `Sales.Domain` |
| `Reporting.Application` | `CommercialDocuments.Domain` (direct repository) | Read-only cross-module aggregator by design (no `Reporting.Domain`) — repointed from `Sales.Domain` |
| legacy root `Application` | `CommercialDocuments.Domain` | `InvoiceJournalIntegration` (Invoice→Journal posting bridge) — repointed from `Sales.Domain` |
| `API`/`OrgSys` (controllers, Razor views) | `CommercialDocuments.Application` | Invoice/InvoiceType endpoints and view models |

Only `Purchasing.Application` was rewritten onto the Contracts surface in this pass — every other
consumer already had a direct-repository-injection relationship with `Sales.Domain.Invoice` before
this move (documented as accepted, interim exceptions in `Tests/Architecture.Tests`), and repointing
the namespace preserves that same interim state against the new, correct owner rather than silently
expanding scope into a rewrite of six modules' internals.

## Creation & posting flow

Sales and Purchasing each remain responsible for their own workflow logic; both now request Invoice
creation the same way they always effectively did (this module didn't change *how* an Invoice gets
created — only *where the code that does it lives*):

```text
Sales.*                          Purchasing.*
   (initiates a Sales/           (initiates a Purchase/
    SalesReturn invoice)          PurchaseReturn invoice, or
        │                         links one via LinkInvoiceCommand)
        └──────────────┬──────────────┘
                        ▼
          CommercialDocuments.Application
          (Invoice CRUD, InvoiceType 1–4,
           totals/discount/tax calculation)
                        │
                        ▼
              InvoiceJournalIntegration
         (unchanged bridge — Accounting posting)
```

No `SalesInvoicePosted`/`PurchaseInvoicePosted` integration events were introduced in this pass —
the existing `InvoiceJournalIntegration` bridge (root `Application` project) already performs the
Invoice→Journal posting synchronously today, and replacing it with an event-driven design is a
separate, larger change than the ownership-correction this module addresses. It's recorded below as
remaining debt, not silently done.

## Returns

Handled exactly as before: `InvoiceType.SalesReturn`/`PurchaseReturn` (TypeId 3/4) are Invoice rows
like any other, distinguished by the same discriminator. No new `OriginalInvoiceId`/
`ReferenceInvoiceId` mechanism was introduced — none existed before this move, and adding one is
out of scope for an ownership-correction refactor.

## Known remaining debt (not fixed in this pass, tracked deliberately)

- **Treasury's 6 Invoice-touching handlers** (`Cancel`/`Create-Paid`/`DeleteList`/`PostTransaction`/
  `Redo`/`Update` under `Treasury.Application/Financials/Command`) still inject
  `IRepository<CommercialDocuments.Domain.Invoice>` directly — one of them
  (`CreateFinancialPaidInvoiceCommandHandler`) both reads and *mutates* `Invoice.Paid`/`Credit`.
  Rewriting these onto a `CommercialDocuments.Contracts` command was judged out of scope: it's a
  behavioral rewrite of Treasury-owned payment-collection logic, not a mechanical namespace move,
  and carries real regression risk with no test coverage protecting the current behavior beyond
  this pass's own new tests (which cover the Purchasing path, not Treasury's). Documented as an
  accepted `ModuleLayerDependencyTests` exception, same as the pre-existing Accounting-integration
  debt this codebase already tracks the same way.
- **Inventory's 5 Transaction handlers** and **Reporting's 2 report handlers**: same
  direct-repository-access pattern, same reasoning — pre-existing debt, repointed to the correct
  owner, not rewritten.
- **`InvoiceJournalIntegration`** stays in the legacy root `Application` project, not yet
  extracted into `CommercialDocuments.Application` itself or replaced with an integration event —
  same "hasn't been extracted from the monolith yet" state it was in before this move.
- **Magic-number `TypeId` checks** outside `CommercialDocuments`/`Purchasing.Application` (Treasury,
  Reporting, `InvoiceJournalIntegration`) were left as bare integers rather than converted to
  `InvoiceTypeId` — cosmetic-risk change not attempted.
