namespace OrgSys.SharedKernel;

/// <summary>
/// Marker for an in-module domain event. Distinct from <see cref="OrgSys.SharedKernel.IIntegrationEvent"/>
/// (see OrgSys.EventBus) which crosses module boundaries.
/// </summary>
public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}
