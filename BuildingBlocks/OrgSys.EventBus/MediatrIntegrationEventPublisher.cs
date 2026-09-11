using MediatR;

namespace OrgSys.SharedKernel;

public sealed class MediatrIntegrationEventPublisher(IPublisher publisher) : IIntegrationEventPublisher
{
    public Task PublishAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
        => publisher.Publish(integrationEvent, cancellationToken);
}
