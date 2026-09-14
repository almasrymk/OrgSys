namespace Receivables.Application.Balances.Queries;

using OrgSys.SharedKernel;
using Receivables.Contracts.Balances;
using System.Net;

public sealed class GetSubledgerAgingQueryHandler(IRepository<Receivable> repository) : IQueryHandler<GetSubledgerAgingQuery, AgingBucketDto>
{
    public async Task<Result<AgingBucketDto>> Handle(GetSubledgerAgingQuery request, CancellationToken cancellationToken)
    {
        var asOf = (request.AsOfDate ?? DateTime.Now).Date;
        var items = (await repository.GetListByFilterAsync(
            r => r.CustomerId == request.CustomerId && (r.LifecycleStatus == ReceivableStatus.Open || r.LifecycleStatus == ReceivableStatus.PartiallySettled)))?.ToList() ?? [];

        decimal current = 0, days31To60 = 0, days61To90 = 0, over90 = 0;
        foreach (var r in items)
        {
            var ageDays = (asOf - r.DueDate.Date).Days;
            if (ageDays <= 30) current += r.OutstandingAmount;
            else if (ageDays <= 60) days31To60 += r.OutstandingAmount;
            else if (ageDays <= 90) days61To90 += r.OutstandingAmount;
            else over90 += r.OutstandingAmount;
        }

        return new Result<AgingBucketDto>(HttpStatusCode.OK, new AgingBucketDto(current, days31To60, days61To90, over90), null);
    }
}
