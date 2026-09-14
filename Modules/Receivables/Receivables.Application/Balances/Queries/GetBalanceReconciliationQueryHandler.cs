namespace Receivables.Application.Balances.Queries;

using MediatR;
using OrgSys.SharedKernel;
using Receivables.Contracts.Balances;
using System.Net;

public sealed class GetBalanceReconciliationQueryHandler(ISender sender, IRepository<Receivable> repository) : IQueryHandler<GetBalanceReconciliationQuery, BalanceReconciliationDto>
{
    public async Task<Result<BalanceReconciliationDto>> Handle(GetBalanceReconciliationQuery request, CancellationToken cancellationToken)
    {
        var glResult = await sender.Send(new GetCustomerBalanceQuery(request.CustomerId, request.AsOfDate), cancellationToken);
        if (glResult.StatusCode != HttpStatusCode.OK)
            return new Result<BalanceReconciliationDto>(glResult.StatusCode, null, glResult.Errors);

        var items = (await repository.GetListByFilterAsync(
            r => r.CustomerId == request.CustomerId && (r.LifecycleStatus == ReceivableStatus.Open || r.LifecycleStatus == ReceivableStatus.PartiallySettled)))?.ToList() ?? [];
        var subledgerBalance = items.Sum(r => r.OutstandingAmount);
        var glBalance = glResult.Response;

        return new Result<BalanceReconciliationDto>(HttpStatusCode.OK, new BalanceReconciliationDto(glBalance, subledgerBalance, glBalance - subledgerBalance), null);
    }
}
