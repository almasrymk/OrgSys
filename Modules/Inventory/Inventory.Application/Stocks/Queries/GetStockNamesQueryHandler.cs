namespace Inventory.Application.Stocks.Queries;

using OrgSys.SharedKernel;
using System.Net;
using Inventory.Contracts.Stocks;

/// <summary>
/// Handles the Contracts-facing GetStockNamesQuery — batch name lookup used by other modules
/// (e.g. Sales Invoice search results) instead of an EF Include/flatten across the module
/// boundary. See docs/dependency-rules.md.
/// </summary>
public sealed class GetStockNamesQueryHandler(IRepository<Stock> stockRepository)
    : IQueryHandler<GetStockNamesQuery, Dictionary<long, string?>>
{
    public async Task<Result<Dictionary<long, string?>>> Handle(GetStockNamesQuery request, CancellationToken cancellationToken)
    {
        if (request.StockIds.Count == 0)
            return new Result<Dictionary<long, string?>>(HttpStatusCode.OK, [], null);

        var ids = request.StockIds.Distinct().ToList();
        var stocks = await stockRepository.GetListByFilterAsync(e => ids.Contains(e.Id));
        var names = (stocks ?? []).ToDictionary(e => e.Id, e => e.Name);

        return new Result<Dictionary<long, string?>>(HttpStatusCode.OK, names, null);
    }
}
