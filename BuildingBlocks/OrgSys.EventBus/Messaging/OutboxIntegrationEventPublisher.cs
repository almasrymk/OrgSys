namespace OrgSys.Messaging;

using OrgSys.SharedKernel;
using System.Text.Json;

public sealed class OutboxIntegrationEventPublisher(
    IRepository<OutboxMessage> repository,
    IUnitOfWork unitOfWork) : IIntegrationEventPublisher
{
    public async Task PublishAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        var type = integrationEvent.GetType();
        await repository.CreateAsync(new OutboxMessage
        {
            EventId = integrationEvent.EventId,
            EventType = type.AssemblyQualifiedName ?? type.FullName ?? type.Name,
            Payload = JsonSerializer.Serialize(integrationEvent, type),
            OccurredOn = integrationEvent.OccurredOn
        });
        await unitOfWork.SaveChangeAsync(cancellationToken);
    }
}
