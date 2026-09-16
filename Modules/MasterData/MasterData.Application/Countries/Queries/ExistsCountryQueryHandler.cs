namespace MasterData.Application.Countries.Queries;

using MasterData.Contracts.Lookups;
using OrgSys.SharedKernel;
using System.Net;

public sealed class ExistsCountryQueryHandler(IRepository<Country> repository)
    : IQueryHandler<ExistsCountryQuery, bool>
{
    public async Task<Result<bool>> Handle(ExistsCountryQuery request, CancellationToken cancellationToken)
    {
        var exists = await repository.AnyAsync(e => e.Id == request.Id, cancellationToken);
        return new Result<bool>(HttpStatusCode.OK, exists, null);
    }
}
