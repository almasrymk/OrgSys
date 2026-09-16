# DDD rules (OrgSys)

1. One bounded context per `Modules/<Name>` set: Domain, Application, Contracts, Infrastructure. Reporting has no Domain.
2. Aggregates own invariants (private setters, factories, domain exceptions). Application handlers map those to `Result`.
3. Cross-module calls use Contracts (`ICommand` / `IQuery` / integration events). Never another module's Domain or Application.
4. Identity stays in Administration (`User`, `Role`, `Permission`, `IPasswordHasher`, `MustResetPassword`, `User.BranchId`). Do not extract Identity.
5. Invoices stay in CommercialDocuments. Custody stays in Advances. Do not recreate them in Sales or Treasury.
6. SQL FKs may exist without Domain navigations (`HasOne(typeof(X)).WithMany().HasForeignKey("XId")`).
7. Architecture.Tests allow-lists stay empty. Isolation regressions fail CI.
8. Tenant isolation is company-rooted (`ICurrentTenant` from JWT). Do not stamp `TenantId` on every table.
9. Workflow may start/decide approvals; it must not mutate `PurchaseOrder`.
10. Financial movements use the unified Treasury model (`FinancialAccount` + `Financial`). Do not add parallel CashTransaction/BankTransaction tables.
11. Tax snapshots on journaled invoices; ETA/ZATCA adapters stay in Tax.Infrastructure.
12. Fixed-asset categories store GL account **IDs**; post depreciation via `Accounting.Contracts`.
13. Outbox/Inbox is the reliability path. Handlers must be idempotent on `EventId`.
