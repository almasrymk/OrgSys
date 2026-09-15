namespace Inventory.Application.StockAdjustmentReasons.Queries;

using System.Net;

public sealed record GetStockAdjustmentReasonListQuery() : ICommandCollection<StockAdjustmentReasonDto>;

public sealed class GetListQueryHandler(IRepository<StockAdjustmentReason> repository, AutoMapper.IMapper mapper)
    : ICommandCollectionHandler<GetStockAdjustmentReasonListQuery, StockAdjustmentReasonDto>
{
    public async Task<ResultCollection<StockAdjustmentReasonDto>> Handle(GetStockAdjustmentReasonListQuery request, CancellationToken cancellationToken)
    {
        var reasons = (await repository.GetListByFilterAsync(e => e.IsActive))?.ToList() ?? [];
        var dtos = mapper.Map<List<StockAdjustmentReasonDto>>(reasons);
        return new ResultCollection<StockAdjustmentReasonDto>(HttpStatusCode.OK, dtos, null);
    }
}
