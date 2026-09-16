using OrgSys.SharedKernel;
using OrgSys.SharedKernel;
using OrgSys.SharedKernel;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Transactions;
using CommercialDocuments.Application.Invoices.Integration;
using Inventory.Contracts.Transactions;

namespace CommercialDocuments.Application.Invoices.Commands
{
    public record CancelInvoiceCommand(long Id) : ICommand, IUpdateCommand<Result>;

    public class CancelInvoiceCommandHandler(IUnitOfWork _UnitOfWork, IRepository<CommercialDocuments.Domain.Invoice> _Repository, IMapper mapper, IServiceProvider _provider, ISender sender) : UpdateCommandHandler<CancelInvoiceCommand, CommercialDocuments.Domain.Invoice>(_UnitOfWork, _Repository, mapper, _provider)
    {

        public override async Task<Result> Handle(CancelInvoiceCommand request, CancellationToken cancellationToken)
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

                invoice.Status = OrgSys.SharedKernel.Status.Cancel;
                await new InvoiceJournalPostingService(sender)
                    .SetStatusByInvoiceIdAsync(invoice.Id, OrgSys.SharedKernel.Status.Cancel);

                if (invoice.TransactionId > 0)
                {
                    var transactionResult = await sender.Send(
                        new SetTransactionStatusCommand(invoice.TransactionId.Value, OrgSys.SharedKernel.Status.Cancel),
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
