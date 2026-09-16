namespace CommercialDocuments.Application.Invoices.Queries;

using CommercialDocuments.Contracts.Invoices;
using MasterData.Contracts.Currencies;
using MediatR;
using OrgSys.SharedKernel;
using System.Net;

public sealed class GetInvoicePaymentInfoQueryHandler(
    IRepository<CommercialDocuments.Domain.Invoice> repository,
    ISender sender)
    : IQueryHandler<GetInvoicePaymentInfoQuery, InvoicePaymentInfoDto?>
{
    public async Task<Result<InvoicePaymentInfoDto?>> Handle(GetInvoicePaymentInfoQuery request, CancellationToken cancellationToken)
    {
        var invoice = await repository.GetByFilterAsync(e => e.Id == request.Id, string.Empty);
        if (invoice is null)
            return new Result<InvoicePaymentInfoDto?>(HttpStatusCode.OK, null, null);

        decimal currencyRate = 0;
        var currencies = (await sender.Send(new GetCurrencyLookupsQuery([invoice.CurrencyId]), cancellationToken)).Response;
        if (currencies is not null && currencies.TryGetValue(invoice.CurrencyId, out var currency))
            currencyRate = currency.Rate;

        var dto = new InvoicePaymentInfoDto(
            invoice.Id,
            invoice.Code,
            invoice.TypeId,
            invoice.DealerId,
            invoice.Net,
            invoice.Paid,
            invoice.Credit,
            invoice.Remaining,
            invoice.Rate,
            currencyRate,
            invoice.CurrencyId,
            invoice.PaymentTypeId,
            invoice.TransactionId,
            invoice.StockId,
            invoice.Date,
            invoice.CreateDate,
            invoice.CreateUserId,
            invoice.ShiftId,
            invoice.BranchId,
            invoice.Notes);

        return new Result<InvoicePaymentInfoDto?>(HttpStatusCode.OK, dto, null);
    }
}
