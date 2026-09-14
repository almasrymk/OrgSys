namespace Payables.Domain.Events;

/// <summary>Raised by Payable.Unapply when a previously-applied amount is restored to the outstanding balance (e.g. a payment was reversed).</summary>
public sealed record PaymentUnappliedDomainEvent(long PayableId, decimal UnappliedAmount, decimal RemainingOutstandingAmount) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
