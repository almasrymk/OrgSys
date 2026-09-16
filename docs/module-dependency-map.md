# OrgSys Module Dependency Map

## 1. Target dependency graph

```
Module.Domain
    ↓
SharedKernel only

Module.Application
    ↓
own Domain
    ↓
own Contracts
    ↓
SharedKernel / EventBus

Module.Infrastructure
    ↓
own Application
    ↓
own Domain
    ↓
own Contracts

Other modules
    ↓
Module.Contracts only
```

No cross-module `Domain -> Domain`, `Application -> Domain`, `Application -> Application`,
`Domain -> Application`, or `Domain -> Infrastructure` edge is allowed except the documented,
tested exceptions in §3-5. `Contracts` projects reference `SharedKernel` only, so any module can
depend on any other module's `Contracts` without creating a cycle.

## 2. Violation report (Project / Illegal dependency / Why / Replacement / Files / Risk)

Violations **fixed** in the Sales.Application pass (commit `723ad0f9`):

| Project | Illegal dependency | Why it is illegal | Correct replacement | Files affected | Risk |
|---|---|---|---|---|---|
| `Sales.Application` | `Inventory.Domain` | Application reaching another module's entities/repositories directly — bypasses Inventory's own business rules and couples Sales to Inventory's schema | `Inventory.Contracts` (`SetTransactionStatusCommand`, `DeleteTransactionByInvoiceCommand`, `GetProductNamesQuery`, `GetStockNamesQuery`) | `Invoices/Commands/{CancelInvoiceCommandHandler,RedoInvoiceCommandHandler,DeleteCommandHandler,DeleteListCommandHandler}.cs`, `Invoices/Queries/{GetByIdQueryHandler,SearchQueryHandler}.cs` | Low — same DB transaction, same SQL, verified via `dotnet ef migrations has-pending-model-changes` (clean) and a live API smoke test |
| `Sales.Application` | `Inventory.Application` | Reaching past `Contracts` into the handler layer directly | `Inventory.Contracts.Transactions.CreateTransactionByInvoiceCommand` | `Invoices/Commands/CreateCommandHandler.cs` | Low — record moved, handler unchanged |
| `Sales.Application` | `Treasury.Domain` | Same pattern, `IRepository<Financial>` accessed directly | `Treasury.Contracts.Financials.DeleteFinancialsByInvoiceCommand` | `Invoices/Commands/{DeleteCommandHandler,DeleteListCommandHandler}.cs` | Low |

Violations **not yet fixed** (documented, scoped to later phases per this brief's own ordering —
not touched in this pass):

| Project | Illegal dependency | Why it is illegal | Correct replacement | Files affected | Risk | Phase |
|---|---|---|---|---|---|---|
| `Sales.Application` | `Accounting.Domain` / `Accounting.Application` / root `InvoiceJournalIntegration` | **Closed.** Invoices now live in CommercialDocuments; dealers in Parties. Both reach Accounting only through `Accounting.Contracts` (`GetAccountingDocumentJournalQuery`, `PostAccountingDocumentCommand`, `IReceivableAccountValidator`/`IPayableAccountValidator`). 2026-09-16: `InvoiceJournalPostingService` also stopped reading `Preference`/`Dealer` via Domain repositories — it uses `GetPreferenceValuesQuery` / `GetDealerByIdQuery`. Integration coverage: `Tests/Accounting.Integration.Tests`. `SalesInvoicePostedIntegrationEvent` remains an AR side-effect (Receivables), not a second GL post — see `docs/architecture/receivables-ddd-migration.md`. | `Accounting.Contracts` | successor files under `CommercialDocuments.Application/Invoices/**` and `Parties.Application/Dealers/**` | Medium | 4 — done |
| `Sales.Application` | root `Domain` (`Preference`) | `Preference` entity never extracted from the monolith | Move `Preference` to a shared/Administration home, or keep as a genuinely cross-cutting technical concept | Every handler using `IRepository<Preference>` (widespread — see `docs/legacy-migration-map.md`) | Low functionally, high in surface area (touches every module) | 10 |
| `Inventory.Application`, `Treasury.Application`, `Receivables.Application`, `Payables.Application`, `Administration.Application`, `Reporting.Application` | various (see `docs/dependency-rules.md` for the full per-module table) | Same Application→Domain/Application leakage pattern, in modules other than Sales | Corresponding `*.Contracts` per target module | See `docs/dependency-rules.md` | Medium-High, module-by-module | 7 (Purchasing), 8 (Inventory/Treasury/Accounting cross-deps) |

## 3. `AcceptedDomainExceptions` (Domain→Domain) — `Tests/Architecture.Tests/ModuleDependencyTests.cs`

| From → To | Why it exists | Entity/navigation | Target solution | Phase |
|---|---|---|---|---|
| `Administration` → `Organization` | `User.BranchId` keeps its EF navigation to `Branch` | `User.Branch` | Redesign as a Contracts lookup | 9 |
| `Accounting` → `MasterData` | `Journal.CurrencyId` keeps its EF navigation to `Currency` | `Journal.Currency` | Contracts lookup | 9 |
| `Treasury` → `MasterData` | Bank/BankBranch/FinancialAccount/Financial/FinancialTransfer navigations to Country/City/District/Currency/PaymentType | multiple | Contracts lookups | 9 |
| `Treasury` → `Organization` | CashBox/BankAccount navigation to `Branch` | multiple | Contracts lookup | 9 |
| `Treasury` → `Administration` | `CashBox.KeeperUserId` navigation to `User` | `CashBox.KeeperUser` | Contracts lookup | 9 |
| `Treasury` → `Accounting` | CashBox/BankAccount/FinancialAccount → `Account`; `Financial` → `Journal` | multiple | Contracts lookup | 9 |
| `Treasury` → `Sales` | `Financial.Dealer`, `FinancialInvoice.Invoice` navigations | `Financial.Dealer`, `FinancialInvoice.Invoice` | Replace with `DealerId`-only + Sales.Contracts lookup | 8 |
| `Inventory` → `Sales` | `Product.Dealer`, `Transaction.Dealer`, `Transaction.Order` navigations — **the resolved Sales/Inventory circular coupling** (Sales' own navigations to Inventory were already dropped) | `Product.Dealer`, `Transaction.Dealer`, `Transaction.Order` | Replace with `DealerId`/`OrderId`-only + Sales.Contracts lookup | 8 |
| `Sales`, `Inventory`, `Treasury`, `Accounting` | → `MasterData`/`Organization`/`Administration`/`Accounting` various | Original-schema navigations (Branch, Shift, Currency, Account, etc.) | Contracts lookups | 9 |

**9 distinct `(Module, DependsOnModule)` pairs, each with a one-line reason in the test file.**
None were added by this pass — all pre-date it. Target: reduce toward zero via Phase 8-9, one
navigation redesign at a time, each requiring an EF migration to confirm the schema doesn't
silently change (see the established `dotnet ef migrations has-pending-model-changes` verification
pattern used throughout this codebase's history).

## 4. `AcceptedApplicationDomainExceptions` (Application→Domain) — `ModuleLayerDependencyTests.cs`

20 entries. Full table with per-entry offending types in `docs/dependency-rules.md` §"Application→Domain".
Grouped by root cause:

| Root cause | Modules affected | Target solution | Phase |
|---|---|---|---|
| Journal/Account lookups for display (`JournalId`/`JournalCode`) or GL provisioning | Sales, Inventory, Treasury, Receivables, Payables → Accounting | `Accounting.Contracts` | 4, 8 |
| Dealer/DealerType reads (Financial.Dealer, opening-balance handlers, Inventory's Product/Transaction.Dealer) | Inventory, Treasury, Receivables, Payables → Sales | `Sales.Contracts` | 8 |
| Currency/Classification/Branch/Shift reference-data reads | Inventory, Treasury, Receivables, Payables, Administration → MasterData/Organization | Left as accepted (see §5 reasoning) or `Contracts` lookups where cheap | 9 |
| Reporting reads every module's entities directly by design (no `Reporting.Domain`) | Reporting → Sales, Treasury, Inventory, MasterData | Accepted by design; revisit only if/when Reporting needs a real read model | — (not a migration target, an architectural choice) |

## 5. `AcceptedApplicationApplicationExceptions` (Application→Application) — `ModuleLayerDependencyTests.cs`

6 entries — all "safe shared dependency" cases (MasterData reference data, or a
Accounting/Receivables validator interface reused by multiple consuming modules). Full table in
`docs/dependency-rules.md` §"Application→Application". These are lower priority to close than the
Domain-level ones: MasterData in particular is treated the same way almost every layered
architecture treats a shared reference-data module — an accepted foundation dependency, not
routed through a heavier Contracts indirection, unless a real behavioral reason (not just
"purity") emerges.

## 6. Why exceptions are not reduced in this pass

This brief's own Phase 9 ("Reduce AcceptedDomainExceptions... do not add new exceptions unless
absolutely unavoidable") is explicitly listed *after* Phases 2-8. Phase 2 (this pass) added
**zero** new exceptions and removed the three Sales→Inventory/Treasury edges without needing any
new exception entries (the fix was structural — Contracts — not an allow-list addition). Reducing
the pre-existing 9 + 20 + 6 = 35 entries is real work spanning Phases 4, 8, and 9 and is
deliberately not attempted here.
