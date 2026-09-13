namespace Accounting.Domain.Events;

/// <summary>Raised when a closed Fiscal Period is reopened (<see cref="FiscalPeriod.Reopen"/>), allowing postings again.</summary>
public sealed record FiscalPeriodReopenedDomainEvent(long FiscalPeriodId, long FiscalYearId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
