namespace Parties.Application.Dealers.Queries
{
    using OrgSys.SharedKernel;
    using Parties.Contracts.Dealers;
    using System.Net;

    /// <summary>
    /// Handles the Contracts-facing GetDealerNamesQuery — batch name lookup used by other modules
    /// instead of an EF Include/flatten across the module boundary. See docs/dependency-rules.md.
    /// </summary>
    public sealed class GetDealerNamesQueryHandler(IRepository<Parties.Domain.Dealer> _Repository)
        : IQueryHandler<GetDealerNamesQuery, Dictionary<long, string?>>
    {
        public async Task<Result<Dictionary<long, string?>>> Handle(GetDealerNamesQuery request, CancellationToken cancellationToken)
        {
            if (request.DealerIds.Count == 0)
                return new Result<Dictionary<long, string?>>(HttpStatusCode.OK, [], null);

            var ids = request.DealerIds.Distinct().ToList();
            var dealers = await _Repository.GetListByFilterAsync(e => ids.Contains(e.Id));
            var names = (dealers ?? []).ToDictionary(e => e.Id, e => e.Name);

            return new Result<Dictionary<long, string?>>(HttpStatusCode.OK, names, null);
        }
    }
}
