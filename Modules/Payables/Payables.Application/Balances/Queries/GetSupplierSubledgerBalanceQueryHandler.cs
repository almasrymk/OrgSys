namespace Payables.Application.Balances.Queries;

using OrgSys.SharedKernel;
using Payables.Contracts.Balances;
using System.Net;

public sealed class GetSupplierSubledgerBalanceQueryHandler(IRepository<Payable> repository) : IQueryHandler<GetSupplierSubledgerBalanceQuery, decimal>
{
    public async Task<Result<decimal>> Handle(GetSupplierSubledgerBalanceQuery request, CancellationToken cancellationToken)
    {
        var items = (await repository.GetListByFilterAsync(
            p => p.SupplierId == request.SupplierId && (p.LifecycleStatus == PayableStatus.Open || p.LifecycleStatus == PayableStatus.PartiallySettled)))?.ToList() ?? [];

        return new Result<decimal>(HttpStatusCode.OK, items.Sum(p => p.OutstandingAmount), null);
    }
}
