namespace Catalog.Application.Units.Queries;

using Catalog.Contracts.Units;
using OrgSys.SharedKernel;
using System.Net;

public sealed class GetUnitNamesQueryHandler(IRepository<Catalog.Domain.Unit> repository)
    : IQueryHandler<GetUnitNamesQuery, Dictionary<long, string?>>
{
    public async Task<Result<Dictionary<long, string?>>> Handle(GetUnitNamesQuery request, CancellationToken cancellationToken)
    {
        if (request.UnitIds.Count == 0)
            return new Result<Dictionary<long, string?>>(HttpStatusCode.OK, [], null);

        var ids = request.UnitIds.Distinct().ToList();
        var rows = await repository.GetListByFilterAsync(e => ids.Contains(e.Id));
        var names = (rows ?? []).ToDictionary(e => e.Id, e => e.Name);
        return new Result<Dictionary<long, string?>>(HttpStatusCode.OK, names, null);
    }
}
