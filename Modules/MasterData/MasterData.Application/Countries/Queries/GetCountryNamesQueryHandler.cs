namespace MasterData.Application.Countries.Queries;

using MasterData.Contracts.Lookups;
using OrgSys.SharedKernel;
using System.Net;

public sealed class GetCountryNamesQueryHandler(IRepository<Country> repository)
    : IQueryHandler<GetCountryNamesQuery, Dictionary<long, string?>>
{
    public async Task<Result<Dictionary<long, string?>>> Handle(GetCountryNamesQuery request, CancellationToken cancellationToken)
    {
        if (request.CountryIds.Count == 0)
            return new Result<Dictionary<long, string?>>(HttpStatusCode.OK, [], null);

        var ids = request.CountryIds.Distinct().ToList();
        var rows = await repository.GetListByFilterAsync(e => ids.Contains(e.Id));
        var names = (rows ?? []).ToDictionary(e => e.Id, e => e.Name);
        return new Result<Dictionary<long, string?>>(HttpStatusCode.OK, names, null);
    }
}
