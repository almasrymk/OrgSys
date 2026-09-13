namespace Accounting.Contracts.IntegrationEvents;

using OrgSys.SharedKernel;

/// <summary>Public cross-context notification that a Posted journal was reversed by a new, linked journal.</summary>
public sealed record JournalReversedIntegrationEvent(long OriginalJournalId, long ReversalJournalId) : IntegrationEvent;
