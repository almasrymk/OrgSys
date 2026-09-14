namespace Payables.Domain.Events;

/// <summary>Raised by Payable.Apply the moment OutstandingAmount reaches zero (Status -> Settled).</summary>
public sealed record PayableSettledDomainEvent(long PayableId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
