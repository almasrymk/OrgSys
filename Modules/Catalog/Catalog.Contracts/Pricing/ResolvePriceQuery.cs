namespace Catalog.Contracts.Pricing;

using OrgSys.SharedKernel;

/// <summary>
/// Public contract for commercial price resolution (docs/catalog/catalog-target-architecture.md
/// §2/§10). Other modules (Sales, Purchasing) call this instead of reading Product.Price or
/// PriceListEntry directly, so Pricing can be extracted to its own bounded context later without
/// breaking callers. If PriceListId is omitted, resolution falls back to the given PriceList's
/// default/active entry, and ultimately to Product.Price if no entry applies.
/// </summary>
public record ResolvePriceQuery(long ProductId, long? PriceListId, decimal Quantity, long? UnitId, DateTime Date) : IQuery<ResolvedPriceDto?>;
