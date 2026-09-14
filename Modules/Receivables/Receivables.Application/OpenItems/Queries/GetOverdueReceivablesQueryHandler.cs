namespace Receivables.Application.OpenItems.Queries;

using OrgSys.SharedKernel;
using Receivables.Contracts.Receivables;
using System.Net;

public sealed class GetOverdueReceivablesQueryHandler(IRepository<Receivable> repository) : IQueryHandler<GetOverdueReceivablesQuery, List<ReceivableDto>>
{
    public async Task<Result<List<ReceivableDto>>> Handle(GetOverdueReceivablesQuery request, CancellationToken cancellationToken)
    {
        var asOf = (request.AsOfDate ?? DateTime.Now).Date;
        var items = (await repository.GetListByFilterAsync(
            r => (request.CustomerId == null || r.CustomerId == request.CustomerId)
                && (r.LifecycleStatus == ReceivableStatus.Open || r.LifecycleStatus == ReceivableStatus.PartiallySettled)
                && r.DueDate < asOf,
            q => q.OrderBy(r => r.DueDate).ThenBy(r => r.Id)))?.ToList() ?? [];

        return new Result<List<ReceivableDto>>(HttpStatusCode.OK, items.Select(r => GetOutstandingReceivablesQueryHandler.ToDto(r, asOf)).ToList(), null);
    }
}
