namespace MasterData.Application.Currencies.Queries;

using MasterData.Contracts.Lookups;
using OrgSys.SharedKernel;
using System.Net;

public sealed class ExistsCurrencyQueryHandler(IRepository<Currency> repository)
    : IQueryHandler<ExistsCurrencyQuery, bool>
{
    public async Task<Result<bool>> Handle(ExistsCurrencyQuery request, CancellationToken cancellationToken)
    {
        var exists = await repository.AnyAsync(e => e.Id == request.Id, cancellationToken);
        return new Result<bool>(HttpStatusCode.OK, exists, null);
    }
}
