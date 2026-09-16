# Remaining risks

1. **Unapplied migrations.** Advances, Sales, Workflow, Budgeting, Tax, Fixed Assets, Outbox/Inbox, Reporting projections, and `InventoryReceipt.PurchaseOrderId` exist as EF migrations only. Deploy after explicit approval.
2. **Shared `OrgContext`.** Modular monolith still uses one DbContext. Module boundaries are compile-time, not database-time.
3. **Company-rooted tenancy.** Tables without `TenantId` rely on Company/Branch/Stock graphs. Bugs in that graph leak data.
4. **AR/AP eventual consistency.** Invoice/payment posting is now outbox-driven. UI that assumed same-transaction subledger rows can show a brief lag.
5. **Reporting dual path.** Live SQL reports still join owner tables in Infrastructure; projections cover aging and sales summary only.
6. **Angular workspaces.** Purchasing/Sales/SaaS/Tax/FA/Budget/Workflow screens are search/lookup workspaces, not full document editors. Price-list entries, Department list, receipt `purchaseOrderId`, and three-way match lookup are present.
7. **Anonymous CheckEmail.** Login and email-existence remain anonymous; password probes require a JWT.
8. **Health check needs a database.** `/health` includes `AddDbContextCheck<OrgContext>` — fails if OrgConnection is down.
9. **Dual UI.** Legacy MVC `OrgSys` host is still in the solution.
10. **CI on ubuntu** runs Architecture, Application, and Domain tests (in-memory/mocked). Integration tests are not in CI; adding them later would need SQL.
