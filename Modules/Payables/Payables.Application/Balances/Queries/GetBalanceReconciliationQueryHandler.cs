namespace Payables.Application.Balances.Queries;

using MediatR;
using OrgSys.SharedKernel;
using Payables.Contracts.Balances;
using System.Net;

public sealed class GetBalanceReconciliationQueryHandler(ISender sender, IRepository<Payable> repository) : IQueryHandler<GetBalanceReconciliationQuery, BalanceReconciliationDto>
{
    public async Task<Result<BalanceReconciliationDto>> Handle(GetBalanceReconciliationQuery request, CancellationToken cancellationToken)
    {
        var glResult = await sender.Send(new GetSupplierBalanceQuery(request.SupplierId, request.AsOfDate), cancellationToken);
        if (glResult.StatusCode != HttpStatusCode.OK)
            return new Result<BalanceReconciliationDto>(glResult.StatusCode, null, glResult.Errors);

        var items = (await repository.GetListByFilterAsync(
            p => p.SupplierId == request.SupplierId && (p.LifecycleStatus == PayableStatus.Open || p.LifecycleStatus == PayableStatus.PartiallySettled)))?.ToList() ?? [];
        var subledgerBalance = items.Sum(p => p.OutstandingAmount);
        var glBalance = glResult.Response;

        return new Result<BalanceReconciliationDto>(HttpStatusCode.OK, new BalanceReconciliationDto(glBalance, subledgerBalance, glBalance - subledgerBalance), null);
    }
}
