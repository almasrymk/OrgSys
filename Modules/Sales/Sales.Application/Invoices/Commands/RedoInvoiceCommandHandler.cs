using OrgSys.SharedKernel;
using OrgSys.SharedKernel;
using OrgSys.SharedKernel;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Application.Commands.Org.Financials.Integration.JournalInvoice;
using Application.Commands.Org.Financials.Integration.JournalTransaction;


namespace Sales.Application.Invoices.Commands
{
    public record RedoInvoiceCommand(long Id) : ICommand, IUpdateCommand<Result>;

    public class RedoInvoiceCommandHandler(IUnitOfWork _UnitOfWork,
        IRepository<Sales.Domain.Invoice> _Repository, 
        IMapper mapper, IServiceProvider _provider
        ) 
        : UpdateCommandHandler<RedoInvoiceCommand, Sales.Domain.Invoice>(_UnitOfWork, _Repository, mapper, _provider)
    {

        public override async Task<Result> Handle(RedoInvoiceCommand request, CancellationToken cancellationToken)
        {
            await _UnitOfWork.BeginTransactionAsync();
            try
            {
                var invoice = await _Repository.GetByFilterAsync(x => x.Id == request.Id, await CreateInclude());


                if (invoice is null)
                {
                    await _UnitOfWork.RollbackAsync();
                    return new Result(HttpStatusCode.NotFound, new List<Error> { new Error("Invoice not found") });
                }

                invoice.Status = OrgSys.SharedKernel.Status.New;
                await new InvoiceJournalIntegration(_provider)
                    .SetStatusByInvoiceIdAsync(invoice.Id, OrgSys.SharedKernel.Status.New);

                if (invoice.TransactionId > 0)
                {
                    var transactionRepo = _provider.GetRequiredService<IRepository<Inventory.Domain.Transaction>>();

                    var transaction = await transactionRepo.GetByFilterAsync(x => x.Id == invoice.TransactionId, await CreateInclude());

                    if (transaction == null)
                    {
                        await _UnitOfWork.RollbackAsync();
                        return new Result(HttpStatusCode.InternalServerError, new List<Error> { new Error("Transaction not found") });
                    }

                    transaction.Status = OrgSys.SharedKernel.Status.New;
                    await new TransactionJournalIntegration(_provider)
                        .SetStatusByTransactionIdAsync(transaction.Id, OrgSys.SharedKernel.Status.New);
                }

                var saved = await _UnitOfWork.SaveChangeAsync(cancellationToken);
                if (saved > 0)
                {
                    await _UnitOfWork.CommitAsync();
                    return new Result(HttpStatusCode.OK, null);
                }

                await _UnitOfWork.RollbackAsync();
                return new Result(HttpStatusCode.InternalServerError, new List<Error> { new Error("Error saving changes") });
            }
            catch (Exception ex)
            {
                await _UnitOfWork.RollbackAsync();
                return new Result(HttpStatusCode.InternalServerError, new List<Error> { new Error(ex.Message) });

            }
        }
    }
}
