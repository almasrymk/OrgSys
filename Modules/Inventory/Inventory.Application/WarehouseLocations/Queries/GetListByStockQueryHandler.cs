namespace Inventory.Application.WarehouseLocations.Queries;

using System.Net;

public sealed record GetWarehouseLocationsByStockQuery(long StockId) : ICommandCollection<WarehouseLocationDto>;

public sealed class GetWarehouseLocationsByStockQueryHandler(IRepository<WarehouseLocation> repository, AutoMapper.IMapper mapper)
    : ICommandCollectionHandler<GetWarehouseLocationsByStockQuery, WarehouseLocationDto>
{
    public async Task<ResultCollection<WarehouseLocationDto>> Handle(GetWarehouseLocationsByStockQuery request, CancellationToken cancellationToken)
    {
        var locations = (await repository.GetListByFilterAsync(l => l.StockId == request.StockId))?.ToList() ?? [];
        var dtos = mapper.Map<List<WarehouseLocationDto>>(locations);
        return new ResultCollection<WarehouseLocationDto>(HttpStatusCode.OK, dtos, null);
    }
}
