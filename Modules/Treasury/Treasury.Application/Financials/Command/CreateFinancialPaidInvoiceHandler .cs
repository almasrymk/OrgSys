using Administration.Contracts.Preferences;
using AutoMapper;
using CommercialDocuments.Contracts.Invoices;
using MediatR;
using System.Net;

namespace Treasury.Application.Financials.Command
{
    public sealed record CreateFinancialPaidInvoiceCommand(long InvoiceId) : ICommand, ICreateCommand<Result>;

    public sealed class CreateFinancialPaidInvoiceCommandHandler(IUnitOfWork _UnitOfWork,
        IRepository<Treasury.Domain.Financial> _Repository, IMapper mapper, ISender sender) :
        CreateCommandHandler<CreateFinancialPaidInvoiceCommand, Treasury.Domain.Financial>(_UnitOfWork, _Repository, mapper)
    {
        public override async Task<Result> Handle(CreateFinancialPaidInvoiceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var invoice = (await sender.Send(new GetInvoicePaymentInfoQuery(request.InvoiceId), cancellationToken)).Response;

                if (invoice is null)
                    return new Result(HttpStatusCode.InternalServerError, new List<Error> { new Error("Invoice not found") });

                if (invoice.Credit > 0)
                {
                    var financial = new Treasury.Domain.Financial
                    {
                        DealerId = invoice.DealerId,
                        PaymentTypeId = invoice.PaymentTypeId,
                        CurrencyId = invoice.CurrencyId,
                        Amount = invoice.Credit,
                        Rate = invoice.Rate > 0 ? invoice.Rate : invoice.CurrencyRate,
                        AmountByDefaultCurrency = invoice.Credit * (invoice.Rate > 0 ? invoice.Rate : invoice.CurrencyRate),
                        CreateDate = DateTime.Now,
                        Date = invoice.Date,
                        TypeId = invoice.TypeId == 1 || invoice.TypeId == 4 ? 1 : 2,
                        CreateUserId = invoice.CreateUserId,
                        ShiftId = invoice.ShiftId,
                        BranchId = invoice.BranchId,
                        Notes = invoice.Notes
                    };

                    var financialTypeId = invoice.TypeId == 1 || invoice.TypeId == 4 ? 1L : 2L;
                    var cashBoxPref = (await sender.Send(
                        new GetPreferenceValueQuery("Financial", financialTypeId, "DefaultCashBox"),
                        cancellationToken)).Response;

                    financial.FinancialAccountId = int.Parse(cashBoxPref ?? "0");

                    FinancialInvoice financialInvoice = new FinancialInvoice
                    {
                        TypeId = invoice.TypeId == 1 || invoice.TypeId == 4 ? 1 : 2,
                        Amount = invoice.Credit,
                        InvoiceId = invoice.Id
                    };
                    financial.FinancialInvoices = new List<FinancialInvoice> { financialInvoice };

                    await sender.Send(new SetInvoiceSettlementFromAllocationCommand(invoice.Id, financial.Amount), cancellationToken);
                    await _Repository.CreateAsync(financial);
                    await _UnitOfWork.SaveChangeAsync();
                }
            return new Result(HttpStatusCode.OK, new List<Error>());
            }
            catch (Exception ex)
            {
                return new Result(HttpStatusCode.InternalServerError,new List<Error>{new Error(ex.Message)});
            }
        }
    }
}
