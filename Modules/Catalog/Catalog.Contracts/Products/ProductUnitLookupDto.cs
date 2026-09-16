namespace Catalog.Contracts.Products;

public sealed record ProductUnitLookupDto(
    long Id,
    long ProductId,
    long UnitId,
    decimal Rate,
    bool DefaultUnit,
    string? UnitName);
