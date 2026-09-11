namespace OrgSys.SharedKernel;

/// <summary>
/// Optional base for new module aggregates that want explicit domain-event collection.
/// Existing entities (BaseModel/MovementModel-derived) are not required to inherit this —
/// see docs/modular-monolith-target-architecture.md §2 for why the existing entity shape
/// stays as-is rather than being retrofitted onto this base.
/// </summary>
public abstract class AggregateRoot
{
    private readonly List<IDomainEvent> _domainEvents = new();

    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void Raise(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    public void ClearDomainEvents() => _domainEvents.Clear();
}
