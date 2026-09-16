# OrgSys Domain Ownership Matrix

Source-of-truth table per brief §60. Extends [`../module-ownership.md`](../module-ownership.md)
(the existing, pre-this-pass ownership map for the original 11 modules) — that document is not
duplicated here, only concepts newly introduced or newly resolved by this pass are listed.

| Concept | Owning Context | Notes |
|---|---|---|
| `Tenant` | **SaaS** | New. Top of the Tenant→Company→Branch hierarchy (ADR: `adr/tenant-vs-company.md`) |
| `Plan`, `Feature`, `PlanFeature` | **SaaS** | New |
| `Subscription` | **SaaS** | New. Fully separate from ERP `Invoice`/`Payable`/`Receivable` — see brief §57 |
| `TenantMembership` | **IAM (Administration)** | New. Physically lives in `Administration.Domain` (it's "which Users belong to which Tenant," an IAM concern), scalar `TenantId` FK to `SaaS.Domain.Tenant`, no navigation |
| `Company` | **Organization** | Already built (Organization Phase 1). Gains `TenantId` (scalar) this pass |
| `Branch` | **Organization** | Already built and already has `CompanyId`. Unchanged this pass except gaining `TenantId` in the retrofit (transitively scoped via `CompanyId` today, but the retrofit adds it directly per ADR for query-filter simplicity) |
| `Department` | **Organization** | New. Per brief §13: Department is an org-structure concept, not a Budget concept — Organization owns it, Budgeting references `DepartmentId` only. **Decision, not a discovery finding** — recorded here to prevent re-litigation. |
| `FiscalYear`, `FiscalPeriod` | **GeneralLedger (Accounting)** | Unchanged — already-documented, already-correct placement (`module-ownership.md`). Not revisited. |
| `Currency`, `Country`, `City`, `District` | **MasterData** | Unchanged — already-documented, already-correct placement. Not revisited. |
| `User`, `Role`, `Permission`, `RolePermission` | **IAM (Administration)** | Already built. Extended (not replaced) with `TenantId` (retrofit) and `UserBranchAccess` |
| `UserBranchAccess` | **IAM (Administration)** | New. Scope beyond the existing single `User.BranchId` — see IAM target-architecture doc |
| `CostCenter` | **Budgeting** | New |
| `Project` (analytic/cost project) | **Budgeting** | New. Not to be confused with any software-engineering "project" concept — none exists in OrgSys |
| `DimensionDefinition`, `DimensionValue` | **Budgeting** | New — for *custom* dimensions only (e.g. Region, SalesChannel). `CostCenter`/`Department`/`Project` remain first-class entities, not reduced to dimension rows. |
| `Budget`, `BudgetLine`, `BudgetPeriodAllocation`, `BudgetRevision`, `BudgetControlPolicy` | **Budgeting** | New |
| `ApprovalPolicy`, `ApprovalLevel`, `WorkflowInstance`, `ApprovalTask`, `ApprovalDelegation` | **Workflow** | New. One generic engine — no per-module approval engines (`SalesApproval`/`BudgetApproval`, etc. must not be created) |
| Financial Statement / Budget vs Actual read models | **Reporting** | Read-only projections. Reporting never owns a transactional entity. |
| `Account`, `Journal`, `JournalItem` | **GeneralLedger (Accounting)** | Unchanged |

## Explicit non-ownership (brief §98 "no duplicate ownership" check)

- `FiscalYear` has exactly one owner (Accounting) — Organization/Budgeting/Reporting all reference
  `FiscalYearId` by scalar FK or `Accounting.Contracts`, never a second table.
- `Department` has exactly one owner (Organization) — Budgeting references `DepartmentId` only.
- `Tenant` has exactly one owner (SaaS) — Organization's `Company.TenantId` is a scalar FK, not a
  second Tenant concept.
- `Budget` actuals come from GeneralLedger's posted `Journal`/`JournalItem` data exclusively —
  Budgeting never creates a parallel "actual transaction" table.
- Workflow is one engine shared by every document type — `WorkflowInstance.DocumentType` is a
  string/enum discriminator (`"Budget"`, `"PurchaseOrder"`, etc.), not a per-module class hierarchy.
- SaaS `Subscription`/`Plan` billing is fully separate from ERP `Invoice`/`Payable`/`Receivable` —
  zero shared tables, zero shared CQRS handlers.

## Status tracking for the multi-tenant retrofit (ADR-driven, brief §75/§98)

| Module | TenantId added (nullable) | Backfilled | TenantId required + query filter |
|---|---|---|---|
| SaaS (Tenant itself — N/A, is the root) | — | — | — |
| Organization (Company) | **done** (2026-09-16, `docs/architecture/saas-tenant-phase1-result.md`) | N/A — live `Company` table doesn't exist yet, nothing to backfill | pending |
| Organization (Branch, Department) | pending | pending | pending |
| Administration (User) | pending | pending | pending |
| Accounting | pending | pending | pending |
| Treasury | pending | pending | pending |
| Sales / Parties / CommercialDocuments | pending | pending | pending |
| Purchasing | pending | pending | pending |
| Inventory / Catalog | pending | pending | pending |
| Receivables / Payables | pending | pending | pending |
| Advances | pending | pending | pending |
| Reporting | pending | pending | pending |

This table is updated as each phase below lands — it is the single place to check "is module X
tenant-scoped yet" instead of re-deriving it from migration history each time.
