using OrgSys.SharedKernel;
using OrgSys.SharedKernel;
using OrgSys.SharedKernel;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Application.Commands.Org.Financials.Integration.JournalInvoice;
using Inventory.Contracts.Transactions;


namespace Sales.Application.Invoices.Commands
{
    public record RedoInvoiceCommand(long Id) : ICommand, IUpdateCommand<Result>;

    public class RedoInvoiceCommandHandler(IUnitOfWork _UnitOfWork,
        IRepository<Sales.Domain.Invoice> _Repository,
        IMapper mapper, IServiceProvider _provider, ISender sender
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
                    var transactionResult = await sender.Send(
                        new SetTransactionStatusCommand(invoice.TransactionId.Value, OrgSys.SharedKernel.Status.New),
                        cancellationToken);

                    if (transactionResult.StatusCode != HttpStatusCode.OK)
                    {
                        await _UnitOfWork.RollbackAsync();
                        return transactionResult;
                    }
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
