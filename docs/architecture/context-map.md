# OrgSys Context Map

Companion to [`domain-ownership.md`](domain-ownership.md) (the "who owns what" table) and
[`remaining-contexts-current-state.md`](remaining-contexts-current-state.md) (Phase 0 discovery).
This document records the supplies/consumes relationships between every bounded context after the
6 new contexts land, per brief §59.

```text
SaaS
  → supplies: Tenant identity, Plan/Feature/Subscription, ITenantFeatureService (feature/limit
    enforcement)
  → consumed by: every other context, transitively, via TenantId scoping + ICurrentTenant
    (SharedKernel abstraction — SaaS is not referenced directly by Domain assemblies elsewhere)

Organization
  → supplies: Company (TenantId-scoped), Branch, Department, OrganizationSettings
  → consumes: SaaS (Company.TenantId, scalar only), MasterData (Country/DefaultCurrency, scalar
    only, Application-layer only — documented exception)
  → consumed by: Administration (User.BranchId), Treasury (CashBox/BankAccount.BranchId),
    Budgeting (CostCenter/Department/Project → Organization.Department by ID), Workflow
    (ApprovalPolicy.CompanyId scalar), every MovementModel-derived transactional entity
    (BranchId — already universal)

IAM (Administration, extended)
  → supplies: User, Role, Permission, RolePermission, TenantMembership, UserBranchAccess,
    ICurrentUser/ICurrentTenant enforcement
  → consumes: Organization (Branch/Company existence checks), SaaS (Tenant existence checks) —
    Application-layer only, scalar IDs
  → consumed by: every context needing "who is doing this / are they allowed to" — Workflow
    (ApprovalLevel.RoleId/UserId), Budgeting (Budget.CreatedBy/ApprovedBy), Reporting (scoping every
    query by current Tenant/Branch access), API controllers (policy-based [Authorize])

GeneralLedger (Accounting)
  → supplies: Account, Journal, JournalItem, FiscalYear, FiscalPeriod — actual financial postings
  → consumes: unchanged by this pass
  → consumed by: Budgeting (Budget vs Actual — read-only, via Accounting.Contracts/Reporting
    projection, never a direct Domain reference), Reporting (Financial Statements)

Budgeting
  → supplies: CostCenter, Department-reference (DepartmentId → Organization), Project (analytic),
    DimensionDefinition/DimensionValue, Budget, BudgetLine, BudgetPeriodAllocation, BudgetRevision,
    BudgetControlPolicy
  → consumes: Organization (Company/Department/Branch — scalar IDs), Accounting.Contracts (Actual
    postings, read-only), Workflow (via integration event, not a Domain reference — see below)
  → consumed by: Reporting (Budget vs Actual projection), GeneralLedger/Sales/Purchasing/Inventory
    document lines (optional CostCenterId/DepartmentId/ProjectId dimension — scalar only, added
    incrementally, not required by this pass to touch every existing document line)

Workflow
  → supplies: ApprovalPolicy, ApprovalLevel, WorkflowInstance, ApprovalTask, ApprovalDelegation —
    a single generic approval engine for every document type
  → consumes: IAM (Role/User existence for ApprovalLevel assignment, scalar IDs)
  → consumed by: Budgeting (BudgetSubmittedForApprovalIntegrationEvent out, DocumentApproved/
    RejectedIntegrationEvent in), and any future document type (Purchasing, Sales, GL) — via the
    same ContextName/DocumentType/DocumentId contract, never a Domain reference in either direction

Reporting
  → supplies: read-only projections and report/query services only — Trial Balance, Balance Sheet,
    P&L, AR/AP Aging (existing), Budget vs Actual (new)
  → consumes: every other context's Domain directly (existing, documented, by-design exception —
    docs/dependency-rules.md), scoped by IAM's current Tenant/Branch access
  → consumed by: nothing (terminal node — no other context depends on Reporting)

SaaS Billing (inside SaaS, not a separate context)
  → supplies: SubscriptionCharge/SaaSInvoice/SaaSPayment (OrgSys's own revenue from tenants)
  → explicitly does NOT reuse Sales.Invoice / CommercialDocuments.Invoice (ERP tenant-facing
    documents) — kept fully separate per brief §57
```

## Dependency direction (extends `modular-monolith-target-architecture.md` §11)

```text
                                    OrgSys.Api (Host)
                                          │
     ┌──────────┬──────────┬─────────────┼─────────────┬──────────┬──────────┐
     │          │          │             │              │          │          │
  Workflow  Budgeting  Reporting   (11 existing      IAM/Admin  Organization  SaaS
     │          │          │      txn modules)           │          │          │
     └────┬─────┴────┬─────┴──────────┬──────────────────┘          │          │
          │          │                │                             │          │
          └──────────┴──Contracts/Events──────────────────────────────────────┘
                                          │
                                    Organization
                                          │
                                        SaaS
                                          │
                                    SharedKernel (incl. ICurrentTenant/ICurrentUser)
```

`SaaS` sits below `Organization` (Company.TenantId) but is otherwise a leaf like MasterData — no
outbound Domain dependencies of its own. `IAM`/`Administration` sits beside `Organization` at the
reference-data tier, consumed by everyone, consuming only `Organization`/`SaaS` at the Application
layer (scalar existence checks, same pattern already established for `Organization` →
`MasterData`).
