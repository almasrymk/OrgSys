namespace Receivables.Application.Balances.Queries;

using OrgSys.SharedKernel;
using Receivables.Contracts.Balances;
using System.Net;

public sealed class GetCustomerSubledgerBalanceQueryHandler(IRepository<Receivable> repository) : IQueryHandler<GetCustomerSubledgerBalanceQuery, decimal>
{
    public async Task<Result<decimal>> Handle(GetCustomerSubledgerBalanceQuery request, CancellationToken cancellationToken)
    {
        var items = (await repository.GetListByFilterAsync(
            r => r.CustomerId == request.CustomerId && (r.LifecycleStatus == ReceivableStatus.Open || r.LifecycleStatus == ReceivableStatus.PartiallySettled)))?.ToList() ?? [];

        return new Result<decimal>(HttpStatusCode.OK, items.Sum(r => r.OutstandingAmount), null);
    }
}
