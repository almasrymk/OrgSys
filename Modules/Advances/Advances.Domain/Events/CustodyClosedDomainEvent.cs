namespace Advances.Domain.Events;

/// <summary>Raised by Custody.Close when a fully Settled custody is archived.</summary>
public sealed record CustodyClosedDomainEvent(long CustodyId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
