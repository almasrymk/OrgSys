namespace Advances.Domain.Events;

/// <summary>Raised by Custody.Settle when an expense settlement is applied against the outstanding amount, whether or not it fully settles the custody.</summary>
public sealed record CustodySettlementPostedDomainEvent(long CustodyId, decimal SettledAmount, decimal RemainingOutstandingAmount) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
