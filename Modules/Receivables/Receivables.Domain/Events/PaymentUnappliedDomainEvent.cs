namespace Receivables.Domain.Events;

/// <summary>Raised by Receivable.Unapply when a previously-applied amount is restored to the outstanding balance (e.g. a receipt was reversed).</summary>
public sealed record PaymentUnappliedDomainEvent(long ReceivableId, decimal UnappliedAmount, decimal RemainingOutstandingAmount) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
