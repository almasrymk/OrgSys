namespace Catalog.Application.Products.Queries;

using Catalog.Contracts.Products;
using OrgSys.SharedKernel;
using System.Net;

public sealed class GetProductUnitsQueryHandler(IRepository<ProductUnit> repository)
    : IQueryHandler<GetProductUnitsQuery, IReadOnlyList<ProductUnitLookupDto>>
{
    public async Task<Result<IReadOnlyList<ProductUnitLookupDto>>> Handle(GetProductUnitsQuery request, CancellationToken cancellationToken)
    {
        if (request.ProductIds.Count == 0)
            return new Result<IReadOnlyList<ProductUnitLookupDto>>(HttpStatusCode.OK, [], null);

        var ids = request.ProductIds.Distinct().ToList();
        var rows = await repository.GetListByFilterAsync(e => ids.Contains(e.ProductId), "Unit");
        var lookups = (rows ?? [])
            .Select(e => new ProductUnitLookupDto(e.Id, e.ProductId, e.UnitId, e.Rate, e.DefaultUnit, e.Unit?.Name))
            .ToList();
        return new Result<IReadOnlyList<ProductUnitLookupDto>>(HttpStatusCode.OK, lookups, null);
    }
}
