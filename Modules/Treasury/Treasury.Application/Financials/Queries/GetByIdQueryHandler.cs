namespace Treasury.Application.Financials.Queries
{
    using CommercialDocuments.Contracts.Invoices;
    using MasterData.Contracts.Currencies;
    using MasterData.Contracts.Lookups;
    using Parties.Contracts.Dealers;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using MediatR;
    using System.Linq.Expressions;

    public sealed record GetByIdFinancialQuery(long Id) : ICommand<FinancialDto> , IGetByIdQuery<Result<FinancialDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Treasury.Domain.Financial> _Repository, IMapper mapper, ISender sender) :
        GetCommandHandler<GetByIdFinancialQuery, Treasury.Domain.Financial, FinancialDto>(_Repository, mapper)
    {
        public override string CreateInclude()
        {
            return "FinancialInvoices";
        }

        public override Expression<Func<Treasury.Domain.Financial, bool>> CreateFilter(GetByIdFinancialQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        public override async Task<Result<FinancialDto>> Handle(GetByIdFinancialQuery request, CancellationToken cancellationToken)
        {
            var result = await base.Handle(request, cancellationToken);
            if (result.Response is null || result.Response.Id == 0)
                return result;

            if (result.Response.DealerId is > 0)
            {
                var names = (await sender.Send(new GetDealerNamesQuery([result.Response.DealerId.Value]), cancellationToken)).Response ?? [];
                result.Response.DealerName = names.GetValueOrDefault(result.Response.DealerId.Value);
            }

            var paymentTypes = (await sender.Send(new GetPaymentTypeNamesQuery([result.Response.PaymentTypeId]), cancellationToken)).Response ?? [];
            result.Response.PaymentTypeName = paymentTypes.GetValueOrDefault(result.Response.PaymentTypeId);

            var currencies = (await sender.Send(new GetCurrencyNamesQuery([result.Response.CurrencyId]), cancellationToken)).Response ?? [];
            result.Response.CurrencyName = currencies.GetValueOrDefault(result.Response.CurrencyId);

            var invoiceIds = (result.Response.FinancialInvoiceList ?? [])
                .Where(e => e.InvoiceId is > 0)
                .Select(e => e.InvoiceId!.Value)
                .Distinct()
                .ToList();
            if (invoiceIds.Count > 0)
            {
                var nets = (await sender.Send(new GetInvoiceNetsQuery(invoiceIds), cancellationToken)).Response ?? [];
                foreach (var line in result.Response.FinancialInvoiceList!)
                    if (line.InvoiceId is > 0 && nets.TryGetValue(line.InvoiceId.Value, out var net))
                        line.Net = net;
            }

            return result;
        }
    }
}
