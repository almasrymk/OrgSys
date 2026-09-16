namespace Catalog.Contracts.Products;

using OrgSys.SharedKernel;

public record GetProductUnitsQuery(IReadOnlyCollection<long> ProductIds)
    : IQuery<IReadOnlyList<ProductUnitLookupDto>>;
