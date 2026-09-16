namespace Catalog.Application.Products.Queries;

using Catalog.Contracts.Products;
using OrgSys.SharedKernel;
using System.Net;

public sealed class GetProductCatalogQueryHandler(IRepository<Product> repository)
    : IQueryHandler<GetProductCatalogQuery, IReadOnlyList<ProductCatalogItemDto>>
{
    public async Task<Result<IReadOnlyList<ProductCatalogItemDto>>> Handle(GetProductCatalogQuery request, CancellationToken cancellationToken)
    {
        var products = await repository.GetListByFilterAsync(
            e => e.Status != Status.Deleted && e.Hide != true,
            "Classification,ProductUnits,ProductUnits.Unit");

        var items = (products ?? []).Select(p => new ProductCatalogItemDto(
            p.Id,
            p.Name,
            p.Nickname,
            p.Barcode,
            p.Price,
            p.Cost,
            p.ClassificationId,
            p.Classification?.Name,
            (p.ProductUnits ?? [])
                .Select(u => new ProductUnitLookupDto(u.Id, u.ProductId, u.UnitId, u.Rate, u.DefaultUnit, u.Unit?.Name))
                .ToList())).ToList();

        return new Result<IReadOnlyList<ProductCatalogItemDto>>(HttpStatusCode.OK, items, null);
    }
}
