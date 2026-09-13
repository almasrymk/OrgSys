namespace Accounting.Contracts.IntegrationEvents;

using OrgSys.SharedKernel;

/// <summary>
/// Public cross-context notification that a Journal was posted — distinct from the internal
/// Accounting.Domain.Events.JournalPostedDomainEvent (never referenced outside this module).
/// No subscriber exists yet; this establishes the contract other bounded contexts may listen on
/// in a future phase, per the migration brief's Domain-Event-vs-Integration-Event guidance.
/// </summary>
public sealed record JournalPostedIntegrationEvent(long JournalId, long FiscalYearId, long FiscalPeriodId) : IntegrationEvent;
