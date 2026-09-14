namespace Receivables.Domain.Events;

/// <summary>Raised by Receivable.Apply when a payment/allocation amount is applied against the open item, whether or not it fully settles it.</summary>
public sealed record PaymentAppliedDomainEvent(long ReceivableId, decimal AppliedAmount, decimal RemainingOutstandingAmount) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
