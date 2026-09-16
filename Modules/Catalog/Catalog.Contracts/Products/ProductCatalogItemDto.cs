namespace Catalog.Contracts.Products;

public sealed record ProductCatalogItemDto(
    long Id,
    string? Name,
    string? Nickname,
    string? Barcode,
    decimal Price,
    decimal Cost,
    long ClassificationId,
    string? ClassificationName,
    IReadOnlyList<ProductUnitLookupDto> Units);
