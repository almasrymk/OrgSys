namespace Reporting.Application.Projections;

using Reporting.Contracts.Projections;
using System.Net;

public sealed class GetCustomerAgingProjectionQueryHandler(IReportingProjectionStore store)
    : IQueryHandler<GetCustomerAgingProjectionQuery, IReadOnlyList<CustomerAgingRowDto>>
{
    public async Task<Result<IReadOnlyList<CustomerAgingRowDto>>> Handle(GetCustomerAgingProjectionQuery request, CancellationToken cancellationToken)
    {
        var rows = await store.GetAgingAsync(request.CustomerId, cancellationToken);
        var asOf = request.AsOfDate.Date;
        var dto = rows.Select(r => new CustomerAgingRowDto(
            r.CustomerId,
            r.InvoiceId,
            r.InvoiceDate,
            r.OriginalAmount,
            r.OutstandingAmount,
            Math.Max(0, (asOf - r.InvoiceDate.Date).Days))).ToList();
        return new Result<IReadOnlyList<CustomerAgingRowDto>>(HttpStatusCode.OK, dto, null);
    }
}

public sealed class GetSalesSummaryProjectionQueryHandler(IReportingProjectionStore store)
    : IQueryHandler<GetSalesSummaryProjectionQuery, IReadOnlyList<SalesSummaryRowDto>>
{
    public async Task<Result<IReadOnlyList<SalesSummaryRowDto>>> Handle(GetSalesSummaryProjectionQuery request, CancellationToken cancellationToken)
    {
        var rows = await store.GetSalesSummaryAsync(request.FromDate, request.ToDate, cancellationToken);
        var dto = rows.Select(r => new SalesSummaryRowDto(r.SummaryDate, r.BranchId, r.InvoiceCount, r.NetAmount)).ToList();
        return new Result<IReadOnlyList<SalesSummaryRowDto>>(HttpStatusCode.OK, dto, null);
    }
}
