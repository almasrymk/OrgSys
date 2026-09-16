namespace Sales.Application.Quotations.Queries;

using System.Net;

public sealed record GetQuotationByIdQuery(long Id) : IQuery<QuotationDto>;

public sealed class GetQuotationByIdQueryHandler(IRepository<Quotation> repository)
    : IQueryHandler<GetQuotationByIdQuery, QuotationDto>
{
    public async Task<Result<QuotationDto>> Handle(GetQuotationByIdQuery request, CancellationToken cancellationToken)
    {
        var quotation = await repository.GetByFilterAsync(e => e.Id == request.Id && e.Status != Status.Deleted, "Lines");
        if (quotation is null)
            return new Result<QuotationDto>(HttpStatusCode.NotFound, null, [new Error("Quotation not found.")]);

        return new Result<QuotationDto>(HttpStatusCode.OK, ToDto(quotation), null);
    }

    internal static QuotationDto ToDto(Quotation quotation) => new(
        quotation.Id, quotation.Code, quotation.CustomerId, quotation.ValidUntil, quotation.CurrencyId, quotation.Rate,
        quotation.Subtotal, quotation.DiscountAmount, quotation.TaxAmount, quotation.TotalAmount,
        quotation.LifecycleStatus, quotation.ConvertedToSalesOrderId, quotation.Notes, quotation.Date, quotation.BranchId,
        quotation.Lines.Select(l => new QuotationLineDto(
            l.Id, l.ProductId, l.ProductName, l.UnitId, l.Quantity, l.UnitPrice, l.DiscountAmount, l.TaxAmount,
            l.LineTotal, l.RequestedDeliveryDate, l.Notes)).ToList());
}
