namespace Payables.Application.OpenItems.Queries;

using OrgSys.SharedKernel;
using Payables.Contracts.Payables;
using System.Net;

public sealed class GetOverduePayablesQueryHandler(IRepository<Payable> repository) : IQueryHandler<GetOverduePayablesQuery, List<PayableDto>>
{
    public async Task<Result<List<PayableDto>>> Handle(GetOverduePayablesQuery request, CancellationToken cancellationToken)
    {
        var asOf = (request.AsOfDate ?? DateTime.Now).Date;
        var items = (await repository.GetListByFilterAsync(
            p => (request.SupplierId == null || p.SupplierId == request.SupplierId)
                && (p.LifecycleStatus == PayableStatus.Open || p.LifecycleStatus == PayableStatus.PartiallySettled)
                && p.DueDate < asOf,
            q => q.OrderBy(p => p.DueDate).ThenBy(p => p.Id)))?.ToList() ?? [];

        return new Result<List<PayableDto>>(HttpStatusCode.OK, items.Select(p => GetOutstandingPayablesQueryHandler.ToDto(p, asOf)).ToList(), null);
    }
}
