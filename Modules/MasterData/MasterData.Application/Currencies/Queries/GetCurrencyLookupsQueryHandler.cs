namespace MasterData.Application.Currencies.Queries;

using MasterData.Contracts.Currencies;
using OrgSys.SharedKernel;
using System.Net;

public sealed class GetCurrencyLookupsQueryHandler(IRepository<Currency> repository)
    : IQueryHandler<GetCurrencyLookupsQuery, Dictionary<long, CurrencyLookupDto>>
{
    public async Task<Result<Dictionary<long, CurrencyLookupDto>>> Handle(GetCurrencyLookupsQuery request, CancellationToken cancellationToken)
    {
        if (request.CurrencyIds.Count == 0)
            return new Result<Dictionary<long, CurrencyLookupDto>>(HttpStatusCode.OK, [], null);

        var ids = request.CurrencyIds.Distinct().ToList();
        var rows = await repository.GetListByFilterAsync(e => ids.Contains(e.Id));
        var lookups = (rows ?? []).ToDictionary(
            e => e.Id,
            e => new CurrencyLookupDto(e.Id, e.Name, e.Rate, e.IsDefault));
        return new Result<Dictionary<long, CurrencyLookupDto>>(HttpStatusCode.OK, lookups, null);
    }
}
