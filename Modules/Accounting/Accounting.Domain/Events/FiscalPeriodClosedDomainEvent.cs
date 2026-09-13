namespace Accounting.Domain.Events;

/// <summary>Raised when a Fiscal Period is closed (<see cref="FiscalPeriod.Close"/>), blocking further postings into it.</summary>
public sealed record FiscalPeriodClosedDomainEvent(long FiscalPeriodId, long FiscalYearId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
