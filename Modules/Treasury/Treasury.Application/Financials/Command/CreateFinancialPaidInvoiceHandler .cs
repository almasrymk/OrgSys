using Administration.Contracts.Preferences;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace Treasury.Application.Financials.Command
{
    public sealed record CreateFinancialPaidInvoiceCommand(long InvoiceId) : ICommand, ICreateCommand<Result>;


    public sealed class CreateFinancialPaidInvoiceCommandHandler(IUnitOfWork _UnitOfWork, 
        IRepository<Treasury.Domain.Financial> _Repository, IServiceProvider _Provider, IMapper mapper, ISender sender) :
        CreateCommandHandler<CreateFinancialPaidInvoiceCommand, Treasury.Domain.Financial>(_UnitOfWork, _Repository, mapper)
    {


        public override async Task<Result> Handle(CreateFinancialPaidInvoiceCommand request, CancellationToken cancellationToken)
        {

            try
            {
                var invoiceRepo = _Provider.GetRequiredService<IRepository<CommercialDocuments.Domain.Invoice>>();

                var invoice = await invoiceRepo.GetByFilterAsync(x => x.Id == request.InvoiceId, "");

                if (invoice is null)
                    return new Result(HttpStatusCode.InternalServerError, new List<Error> { new Error("Invoice not found") });

                if (invoice.Credit > 0)
                {
                    var financial = mapper.Map<Treasury.Domain.Financial>(invoice);
                    financial.Id = 0;
                    financial.Amount = invoice.Credit;
                    financial.Rate = invoice.Rate > 0 ? invoice.Rate : (invoice.Currency?.Rate ?? 0);
                    financial.AmountByDefaultCurrency = invoice.Credit * financial.Rate;
                    financial.CreateDate = DateTime.Now;
                    financial.TypeId = invoice.TypeId == 1 || invoice.TypeId == 4 ? 1 : 2;

                    var financialTypeId = invoice.TypeId == 1 || invoice.TypeId == 4 ? 1L : 2L;
                    var cashBoxPref = (await sender.Send(
                        new GetPreferenceValueQuery("Financial", financialTypeId, "DefaultCashBox"),
                        cancellationToken)).Response;

                    financial.FinancialAccountId = int.Parse(cashBoxPref ?? "0");

                    //invoice.CodeNumber = await _Repository.GetMaxByFilterAsync(e => e.TypeId == invoice.TypeId, e => e.CodeNumber) + 1;

                    //invoice.Code = invoice.CodeNumber.ToString();

                    //FinancialInvoice financialInvoice = mapper.Map<Treasury.Domain.FinancialInvoice>(invoice);
                    FinancialInvoice financialInvoice = new FinancialInvoice();
                    financialInvoice.Id = 0;
                    financialInvoice.TypeId = invoice.TypeId == 1 || invoice.TypeId == 4 ? 1 : 2;
                    financialInvoice.Amount = invoice.Credit;
                    financialInvoice.InvoiceId = invoice.Id;
                    financial.FinancialInvoices = new List<FinancialInvoice>();
                    financial.FinancialInvoices.Add(financialInvoice);
                    invoice.Credit = invoice.Net - invoice.Paid - financial.Amount;
                    invoice.Paid = invoice.Net - invoice.Credit;

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
