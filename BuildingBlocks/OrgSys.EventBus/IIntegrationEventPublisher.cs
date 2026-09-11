namespace OrgSys.SharedKernel;

/// <summary>
/// Thin facade over the notification bus so module handlers depend on this interface
/// instead of MediatR's IPublisher directly — keeps the transport swap in §5 of the target
/// architecture doc a one-file change (the DI registration), not a search-and-replace.
/// </summary>
public interface IIntegrationEventPublisher
{
    Task PublishAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken = default);
}
