namespace Catalog.Contracts.Products;

using OrgSys.SharedKernel;

public record GetProductCatalogQuery : IQuery<IReadOnlyList<ProductCatalogItemDto>>;
