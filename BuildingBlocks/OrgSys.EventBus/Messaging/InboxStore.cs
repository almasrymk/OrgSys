namespace OrgSys.Messaging;

using OrgSys.SharedKernel;

public sealed class InboxStore(IRepository<InboxMessage> repository, IUnitOfWork unitOfWork) : IInboxStore
{
    public async Task<bool> TryClaimAsync(Guid eventId, string handlerName, CancellationToken cancellationToken = default)
    {
        var existing = await repository.GetByFilterAsync(
            e => e.EventId == eventId && e.HandlerName == handlerName,
            string.Empty);
        if (existing is not null && existing.Id > 0)
            return false;

        await repository.CreateAsync(new InboxMessage
        {
            EventId = eventId,
            HandlerName = handlerName,
            ProcessedOn = DateTime.UtcNow
        });
        await unitOfWork.SaveChangeAsync(cancellationToken);
        return true;
    }
}
