namespace Catalog.Contracts.Pricing;

public record ResolvedPriceDto(long ProductId, decimal Price, long? CurrencyId, long? PriceListId, string AppliedRule);
