namespace Inventory.Application.InventoryBalances.Queries;

using System.Net;

/// <summary>Backs the "Stock Balance" inquiry screen (brief §68) — filters are optional, both null
/// means "every balance row" (small enough to page client-side for a first cut; a dedicated
/// pagination query can follow if this grows large).</summary>
public sealed record GetStockBalancesQuery(long? StockId, long? ProductId) : ICommandCollection<StockBalanceDto>;

public sealed class GetStockBalancesQueryHandler(IRepository<InventoryBalance> balanceRepository)
    : ICommandCollectionHandler<GetStockBalancesQuery, StockBalanceDto>
{
    public async Task<ResultCollection<StockBalanceDto>> Handle(GetStockBalancesQuery request, CancellationToken cancellationToken)
    {
        var balances = (await balanceRepository.GetListByFilterAsync(b =>
            (request.StockId == null || b.StockId == request.StockId)
            && (request.ProductId == null || b.ProductId == request.ProductId)))?.ToList() ?? [];

        var dtos = balances.Select(b => new StockBalanceDto(
            b.ProductId, b.StockId, b.LocationId, b.BatchId,
            b.QuantityOnHand, b.QuantityReserved, b.QuantityAvailable, b.AverageCost, b.InventoryValue)).ToList();

        return new ResultCollection<StockBalanceDto>(HttpStatusCode.OK, dtos, null);
    }
}
