namespace Payables.Application.Balances.Queries;

using OrgSys.SharedKernel;
using Payables.Contracts.Balances;
using System.Net;

public sealed class GetSubledgerAgingQueryHandler(IRepository<Payable> repository) : IQueryHandler<GetSubledgerAgingQuery, AgingBucketDto>
{
    public async Task<Result<AgingBucketDto>> Handle(GetSubledgerAgingQuery request, CancellationToken cancellationToken)
    {
        var asOf = (request.AsOfDate ?? DateTime.Now).Date;
        var items = (await repository.GetListByFilterAsync(
            p => p.SupplierId == request.SupplierId && (p.LifecycleStatus == PayableStatus.Open || p.LifecycleStatus == PayableStatus.PartiallySettled)))?.ToList() ?? [];

        decimal current = 0, days31To60 = 0, days61To90 = 0, over90 = 0;
        foreach (var p in items)
        {
            var ageDays = (asOf - p.DueDate.Date).Days;
            if (ageDays <= 30) current += p.OutstandingAmount;
            else if (ageDays <= 60) days31To60 += p.OutstandingAmount;
            else if (ageDays <= 90) days61To90 += p.OutstandingAmount;
            else over90 += p.OutstandingAmount;
        }

        return new Result<AgingBucketDto>(HttpStatusCode.OK, new AgingBucketDto(current, days31To60, days61To90, over90), null);
    }
}
