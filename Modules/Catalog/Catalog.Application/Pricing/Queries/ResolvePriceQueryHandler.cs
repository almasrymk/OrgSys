namespace Catalog.Application.Pricing.Queries;

using OrgSys.SharedKernel;
using System.Net;
using Catalog.Contracts.Pricing;

/// <summary>
/// Price resolution (docs/catalog/catalog-target-architecture.md §2/§10). Looks for an active,
/// currently-valid PriceListEntry for the product (optionally scoped to one PriceList and
/// respecting MinQuantity breaks), falling back to Product.Price — the brief's explicitly allowed
/// "default/simple price" — when no entry applies. Never guesses a price that isn't backed by
/// either an entry or the product's own default.
/// </summary>
public sealed class ResolvePriceQueryHandler(
    IRepository<Catalog.Domain.PriceListEntry> entryRepository,
    IRepository<Catalog.Domain.Product> productRepository)
    : IQueryHandler<ResolvePriceQuery, ResolvedPriceDto?>
{
    public async Task<Result<ResolvedPriceDto?>> Handle(ResolvePriceQuery request, CancellationToken cancellationToken)
    {
        var entries = await entryRepository.GetListByFilterAsync(e =>
            e.ProductId == request.ProductId &&
            e.IsActive &&
            (request.PriceListId == null || e.PriceListId == request.PriceListId) &&
            (e.UnitId == null || request.UnitId == null || e.UnitId == request.UnitId) &&
            (e.ValidFrom == null || e.ValidFrom <= request.Date) &&
            (e.ValidTo == null || e.ValidTo >= request.Date) &&
            (e.MinQuantity == null || e.MinQuantity <= request.Quantity));

        var best = (entries ?? [])
            .OrderByDescending(e => e.MinQuantity ?? 0)
            .FirstOrDefault();

        if (best != null)
        {
            return new Result<ResolvedPriceDto?>(
                HttpStatusCode.OK,
                new ResolvedPriceDto(request.ProductId, best.Price, null, best.PriceListId, "PriceListEntry"),
                null);
        }

        var product = await productRepository.GetByFilterAsync(p => p.Id == request.ProductId, "");
        if (product == null)
            return new Result<ResolvedPriceDto?>(HttpStatusCode.NotFound, null, [new Error("Product not found")]);

        return new Result<ResolvedPriceDto?>(
            HttpStatusCode.OK,
            new ResolvedPriceDto(request.ProductId, product.Price, null, null, "ProductDefaultPrice"),
            null);
    }
}
