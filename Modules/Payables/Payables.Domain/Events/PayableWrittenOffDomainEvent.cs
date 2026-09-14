namespace Payables.Domain.Events;

/// <summary>Raised by Payable.WriteOff. Carries the reason for audit.</summary>
public sealed record PayableWrittenOffDomainEvent(long PayableId, decimal WrittenOffAmount, string Reason, decimal RemainingOutstandingAmount) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
