namespace Payables.Application.OpenItems.Queries;

using OrgSys.SharedKernel;
using Payables.Contracts.Payables;
using System.Net;

public sealed class GetOutstandingPayablesQueryHandler(IRepository<Payable> repository) : IQueryHandler<GetOutstandingPayablesQuery, List<PayableDto>>
{
    public async Task<Result<List<PayableDto>>> Handle(GetOutstandingPayablesQuery request, CancellationToken cancellationToken)
    {
        var asOf = DateTime.Now;
        var items = (await repository.GetListByFilterAsync(
            p => (request.SupplierId == null || p.SupplierId == request.SupplierId)
                && (p.LifecycleStatus == PayableStatus.Open || p.LifecycleStatus == PayableStatus.PartiallySettled),
            q => q.OrderBy(p => p.DocumentDate).ThenBy(p => p.Id)))?.ToList() ?? [];

        return new Result<List<PayableDto>>(HttpStatusCode.OK, items.Select(p => ToDto(p, asOf)).ToList(), null);
    }

    internal static PayableDto ToDto(Payable p, DateTime asOfDate) => new(
        p.Id, p.SupplierId, p.SourceDocumentType.ToString(), p.SourceDocumentId, p.SourceDocumentNumber,
        p.DocumentDate, p.DueDate, p.CurrencyId, p.OriginalAmount, p.OutstandingAmount, p.LifecycleStatus.ToString(), p.IsOverdue(asOfDate));
}
