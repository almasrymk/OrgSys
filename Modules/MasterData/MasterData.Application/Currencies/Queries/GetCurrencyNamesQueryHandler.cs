namespace MasterData.Application.Currencies.Queries
{
    using OrgSys.SharedKernel;
    using MasterData.Contracts.Currencies;
    using System.Net;

    /// <summary>Handles the Contracts-facing GetCurrencyNamesQuery — batch name lookup used by other modules instead of an EF Include across the module boundary. See docs/dependency-rules.md.</summary>
    public sealed class GetCurrencyNamesQueryHandler(IRepository<Currency> _Repository)
        : IQueryHandler<GetCurrencyNamesQuery, Dictionary<long, string?>>
    {
        public async Task<Result<Dictionary<long, string?>>> Handle(GetCurrencyNamesQuery request, CancellationToken cancellationToken)
        {
            if (request.CurrencyIds.Count == 0)
                return new Result<Dictionary<long, string?>>(HttpStatusCode.OK, [], null);

            var ids = request.CurrencyIds.Distinct().ToList();
            var currencies = await _Repository.GetListByFilterAsync(e => ids.Contains(e.Id));
            var names = (currencies ?? []).ToDictionary(e => e.Id, e => e.Name);

            return new Result<Dictionary<long, string?>>(HttpStatusCode.OK, names, null);
        }
    }
}
