namespace Inventory.Application.InventoryReceipts.Queries;

using System.Net;

public sealed record GetInventoryReceiptByIdQuery(long Id) : IQuery<InventoryReceiptDto>;

public sealed class GetInventoryReceiptByIdQueryHandler(IRepository<InventoryReceipt> repository)
    : IQueryHandler<GetInventoryReceiptByIdQuery, InventoryReceiptDto>
{
    public async Task<Result<InventoryReceiptDto>> Handle(GetInventoryReceiptByIdQuery request, CancellationToken cancellationToken)
    {
        var receipt = await repository.GetByFilterAsync(r => r.Id == request.Id, "Lines");
        if (receipt is null)
            return new Result<InventoryReceiptDto>(HttpStatusCode.NotFound, null, [new Error("Inventory receipt not found.")]);

        return new Result<InventoryReceiptDto>(HttpStatusCode.OK, ToDto(receipt), null);
    }

    internal static InventoryReceiptDto ToDto(InventoryReceipt receipt) => new(
        receipt.Id, receipt.Code, receipt.StockId, receipt.LocationId, receipt.DealerId, receipt.Date,
        receipt.LifecycleStatus, receipt.Notes,
        receipt.Lines.Select(l => new InventoryReceiptLineDto(l.Id, l.RowNumber, l.ProductId, l.UnitId, l.Quantity, l.UnitCost, l.BatchId, l.Notes)).ToList());
}
