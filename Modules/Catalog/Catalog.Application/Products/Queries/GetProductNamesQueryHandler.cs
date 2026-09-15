namespace Catalog.Application.Products.Queries;

using OrgSys.SharedKernel;
using System.Net;
using Catalog.Contracts.Products;

/// <summary>
/// Handles the Contracts-facing GetProductNamesQuery — batch name lookup used by other modules
/// (e.g. Sales Invoice line items) instead of an EF Include/flatten across the module boundary.
/// See docs/dependency-rules.md.
/// </summary>
public sealed class GetProductNamesQueryHandler(IRepository<Product> productRepository)
    : IQueryHandler<GetProductNamesQuery, Dictionary<long, string?>>
{
    public async Task<Result<Dictionary<long, string?>>> Handle(GetProductNamesQuery request, CancellationToken cancellationToken)
    {
        if (request.ProductIds.Count == 0)
            return new Result<Dictionary<long, string?>>(HttpStatusCode.OK, [], null);

        var ids = request.ProductIds.Distinct().ToList();
        var products = await productRepository.GetListByFilterAsync(e => ids.Contains(e.Id));
        var names = (products ?? []).ToDictionary(e => e.Id, e => e.Name);

        return new Result<Dictionary<long, string?>>(HttpStatusCode.OK, names, null);
    }
}
