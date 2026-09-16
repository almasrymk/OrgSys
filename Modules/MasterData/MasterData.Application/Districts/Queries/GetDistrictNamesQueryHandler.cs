namespace MasterData.Application.Districts.Queries;

using MasterData.Contracts.Lookups;
using OrgSys.SharedKernel;
using System.Net;

public sealed class GetDistrictNamesQueryHandler(IRepository<District> repository)
    : IQueryHandler<GetDistrictNamesQuery, Dictionary<long, string?>>
{
    public async Task<Result<Dictionary<long, string?>>> Handle(GetDistrictNamesQuery request, CancellationToken cancellationToken)
    {
        if (request.DistrictIds.Count == 0)
            return new Result<Dictionary<long, string?>>(HttpStatusCode.OK, [], null);

        var ids = request.DistrictIds.Distinct().ToList();
        var rows = await repository.GetListByFilterAsync(e => ids.Contains(e.Id));
        var names = (rows ?? []).ToDictionary(e => e.Id, e => e.Name);
        return new Result<Dictionary<long, string?>>(HttpStatusCode.OK, names, null);
    }
}
