namespace Inventory.Application.InventoryReceipts.Integration;

using Inventory.Contracts.Receipts;
using MediatR;
using OrgSys.SharedKernel;

public sealed class GoodsReceiptPostedIntegrationEventHandler(IInboxStore inbox)
    : INotificationHandler<GoodsReceiptPostedIntegrationEvent>
{
    public Task Handle(GoodsReceiptPostedIntegrationEvent notification, CancellationToken cancellationToken) =>
        inbox.TryClaimAsync(notification.EventId, nameof(GoodsReceiptPostedIntegrationEventHandler), cancellationToken);
}
