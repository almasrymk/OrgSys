namespace Catalog.Domain.Enums;

/// <summary>
/// Classifies what a Product represents for the purposes of stock tracking and commercial
/// transactions (docs/catalog/catalog-target-architecture.md §2). Backfilled on relocation from
/// the existing TrackingType (TrackingType != None -> StockItem, otherwise -> NonStockItem); no
/// existing row is auto-classified Service since nothing in prior data distinguishes a service
/// from an untracked non-stock item.
/// </summary>
public enum ProductType
{
    StockItem = 0,
    NonStockItem = 1,
    Service = 2
}
