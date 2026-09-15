namespace Inventory.Application.InventoryReceipts.Queries;

using System.Net;

public sealed record GetInventoryReceiptListQuery(long? StockId, DocumentStatus? Status) : ICommandCollection<InventoryReceiptDto>;

public sealed class GetInventoryReceiptListQueryHandler(IRepository<InventoryReceipt> repository)
    : ICommandCollectionHandler<GetInventoryReceiptListQuery, InventoryReceiptDto>
{
    public async Task<ResultCollection<InventoryReceiptDto>> Handle(GetInventoryReceiptListQuery request, CancellationToken cancellationToken)
    {
        var receipts = (await repository.GetListByFilterAsync(r =>
            (request.StockId == null || r.StockId == request.StockId)
            && (request.Status == null || r.LifecycleStatus == request.Status),
            "Lines"))?.OrderByDescending(r => r.Id).ToList() ?? [];

        var dtos = receipts.Select(GetInventoryReceiptByIdQueryHandler.ToDto).ToList();
        return new ResultCollection<InventoryReceiptDto>(HttpStatusCode.OK, dtos, null);
    }
}
