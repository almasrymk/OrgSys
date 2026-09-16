# ADR: Reporting projections vs live SQL

**Status:** accepted (Stage 14)  
**Date:** 2026-09-17

## Context

Stage 1 moved EX-AD9–AD14 out of `Reporting.Application` into `Reporting.Infrastructure.ReportingReadStore`, which still reads Invoice/Dealer/Financial/Stock via `IRepository<T>` of owner Domain types. Application no longer references those Domain assemblies.

## Decision

1. Keep live SQL reports (`IReportingReadStore`) as read-only facades. They never call `UpdateAsync`/`CreateAsync` on transactional entities.
2. Add event-fed projections (`CustomerAgingReadModel`, `SalesSummaryReadModel`) updated from `SalesInvoicePostedIntegrationEvent` through the outbox/inbox. Reporting.Application depends on CommercialDocuments.Contracts only.
3. Do not stamp TenantId on projection tables; they follow invoice/customer isolation via source events.

## Consequences

Live dealer/warehouse/safe reports still join owner tables in Infrastructure. Further reports can move to projections without reopening Application→Domain exceptions.
