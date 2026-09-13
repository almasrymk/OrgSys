namespace Accounting.Domain.Events;

/// <summary>Raised when a Journal transitions from Draft to Posted (<see cref="Journal.Post"/>).</summary>
public sealed record JournalPostedDomainEvent(long JournalId, long FiscalYearId, long FiscalPeriodId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
