namespace Accounting.Contracts.IntegrationEvents;

using OrgSys.SharedKernel;

/// <summary>Public cross-context notification that a Draft journal was cancelled.</summary>
public sealed record JournalCancelledIntegrationEvent(long JournalId) : IntegrationEvent;
