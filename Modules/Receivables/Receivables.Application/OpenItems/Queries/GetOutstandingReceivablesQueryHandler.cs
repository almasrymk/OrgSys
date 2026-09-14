namespace Receivables.Application.OpenItems.Queries;

using OrgSys.SharedKernel;
using Receivables.Contracts.Receivables;
using System.Net;

public sealed class GetOutstandingReceivablesQueryHandler(IRepository<Receivable> repository) : IQueryHandler<GetOutstandingReceivablesQuery, List<ReceivableDto>>
{
    public async Task<Result<List<ReceivableDto>>> Handle(GetOutstandingReceivablesQuery request, CancellationToken cancellationToken)
    {
        var asOf = DateTime.Now;
        var items = (await repository.GetListByFilterAsync(
            r => (request.CustomerId == null || r.CustomerId == request.CustomerId)
                && (r.LifecycleStatus == ReceivableStatus.Open || r.LifecycleStatus == ReceivableStatus.PartiallySettled),
            q => q.OrderBy(r => r.DocumentDate).ThenBy(r => r.Id)))?.ToList() ?? [];

        return new Result<List<ReceivableDto>>(HttpStatusCode.OK, items.Select(r => ToDto(r, asOf)).ToList(), null);
    }

    internal static ReceivableDto ToDto(Receivable r, DateTime asOfDate) => new(
        r.Id, r.CustomerId, r.SourceDocumentType.ToString(), r.SourceDocumentId, r.SourceDocumentNumber,
        r.DocumentDate, r.DueDate, r.CurrencyId, r.OriginalAmount, r.OutstandingAmount, r.LifecycleStatus.ToString(), r.IsOverdue(asOfDate));
}
