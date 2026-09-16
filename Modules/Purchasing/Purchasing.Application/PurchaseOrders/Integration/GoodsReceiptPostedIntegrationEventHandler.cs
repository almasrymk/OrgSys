namespace Purchasing.Application.PurchaseOrders.Integration;

using Inventory.Contracts.Receipts;
using MediatR;
using Purchasing.Domain.Exceptions;

public sealed class GoodsReceiptPostedIntegrationEventHandler(
    IRepository<PurchaseOrder> repository,
    IUnitOfWork unitOfWork,
    IInboxStore inbox) : INotificationHandler<GoodsReceiptPostedIntegrationEvent>
{
    public async Task Handle(GoodsReceiptPostedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        if (!await inbox.TryClaimAsync(notification.EventId, nameof(GoodsReceiptPostedIntegrationEventHandler), cancellationToken))
            return;

        if (notification.PurchaseOrderId is not > 0 || notification.Lines is null || notification.Lines.Count == 0)
            return;

        var order = await repository.GetByFilterAsync(
            e => e.Id == notification.PurchaseOrderId && e.Status != Status.Deleted,
            "PurchaseOrderProducts");
        if (order is null)
            return;

        try
        {
            foreach (var posted in notification.Lines)
            {
                var line = order.PurchaseOrderProducts.FirstOrDefault(l =>
                    l.ProductId == posted.ProductId && l.UnitId == posted.UnitId && l.RemainingQuantity > 0);
                if (line is null)
                    continue;

                var quantity = Math.Min(posted.Quantity, line.RemainingQuantity);
                if (quantity > 0)
                    order.RecordReceipt(line.Id, quantity);
            }

            await repository.UpdateAsync(order);
            await unitOfWork.SaveChangeAsync(cancellationToken);
        }
        catch (PurchaseOrderDomainException)
        {
            // Over-receipt and cancelled orders are ignored after the inbox claim so retries stay idempotent.
        }
    }
}
