namespace MasterData.Application.Cities.Queries;

using MasterData.Contracts.Lookups;
using OrgSys.SharedKernel;
using System.Net;

public sealed class GetCityNamesQueryHandler(IRepository<City> repository)
    : IQueryHandler<GetCityNamesQuery, Dictionary<long, string?>>
{
    public async Task<Result<Dictionary<long, string?>>> Handle(GetCityNamesQuery request, CancellationToken cancellationToken)
    {
        if (request.CityIds.Count == 0)
            return new Result<Dictionary<long, string?>>(HttpStatusCode.OK, [], null);

        var ids = request.CityIds.Distinct().ToList();
        var rows = await repository.GetListByFilterAsync(e => ids.Contains(e.Id));
        var names = (rows ?? []).ToDictionary(e => e.Id, e => e.Name);
        return new Result<Dictionary<long, string?>>(HttpStatusCode.OK, names, null);
    }
}
