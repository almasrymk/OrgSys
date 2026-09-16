namespace MasterData.Application.PaymentTypes.Queries;

using MasterData.Contracts.Lookups;
using OrgSys.SharedKernel;
using System.Net;

public sealed class GetPaymentTypeNamesQueryHandler(IRepository<PaymentType> repository)
    : IQueryHandler<GetPaymentTypeNamesQuery, Dictionary<long, string?>>
{
    public async Task<Result<Dictionary<long, string?>>> Handle(GetPaymentTypeNamesQuery request, CancellationToken cancellationToken)
    {
        if (request.PaymentTypeIds.Count == 0)
            return new Result<Dictionary<long, string?>>(HttpStatusCode.OK, [], null);

        var ids = request.PaymentTypeIds.Distinct().ToList();
        var rows = await repository.GetListByFilterAsync(e => ids.Contains(e.Id));
        var names = (rows ?? []).ToDictionary(e => e.Id, e => e.Name);
        return new Result<Dictionary<long, string?>>(HttpStatusCode.OK, names, null);
    }
}
