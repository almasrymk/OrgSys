namespace Receivables.Domain.Events;

/// <summary>Raised by Receivable.WriteOff. Carries the reason for audit — see brief §19 (write-offs must record amount/reason, not just zero the balance).</summary>
public sealed record ReceivableWrittenOffDomainEvent(long ReceivableId, decimal WrittenOffAmount, string Reason, decimal RemainingOutstandingAmount) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
