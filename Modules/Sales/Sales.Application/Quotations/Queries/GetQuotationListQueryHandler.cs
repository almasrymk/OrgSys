namespace Sales.Application.Quotations.Queries;

using System.Net;

public sealed record GetQuotationListQuery(long? CustomerId, QuotationStatus? Status) : ICommandCollection<QuotationDto>;

public sealed class GetQuotationListQueryHandler(IRepository<Quotation> repository)
    : ICommandCollectionHandler<GetQuotationListQuery, QuotationDto>
{
    public async Task<ResultCollection<QuotationDto>> Handle(GetQuotationListQuery request, CancellationToken cancellationToken)
    {
        var rows = (await repository.GetListByFilterAsync(e =>
            e.Status != Status.Deleted
            && (request.CustomerId == null || e.CustomerId == request.CustomerId)
            && (request.Status == null || e.LifecycleStatus == request.Status),
            "Lines"))?.OrderByDescending(e => e.Id).ToList() ?? [];

        return new ResultCollection<QuotationDto>(HttpStatusCode.OK, rows.Select(GetQuotationByIdQueryHandler.ToDto).ToList(), null);
    }
}
