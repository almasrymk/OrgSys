namespace Inventory.Application.InventoryBalances.Queries;

using System.Net;

public sealed record GetStockBalanceQuery(long ProductId, long StockId, long? LocationId, long? BatchId) : IQuery<StockBalanceDto>;

public sealed class GetStockBalanceQueryHandler(IRepository<InventoryBalance> balanceRepository) : IQueryHandler<GetStockBalanceQuery, StockBalanceDto>
{
    public async Task<Result<StockBalanceDto>> Handle(GetStockBalanceQuery request, CancellationToken cancellationToken)
    {
        var balance = await balanceRepository.GetByFilterAsync(
            b => b.ProductId == request.ProductId && b.StockId == request.StockId
                && b.LocationId == request.LocationId && b.BatchId == request.BatchId, "");

        var dto = balance is null
            ? new StockBalanceDto(request.ProductId, request.StockId, request.LocationId, request.BatchId, 0, 0, 0, 0, 0)
            : new StockBalanceDto(
                balance.ProductId, balance.StockId, balance.LocationId, balance.BatchId,
                balance.QuantityOnHand, balance.QuantityReserved, balance.QuantityAvailable, balance.AverageCost, balance.InventoryValue);

        return new Result<StockBalanceDto>(HttpStatusCode.OK, dto, null);
    }
}
