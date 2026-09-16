namespace CommercialDocuments.Application.Invoices.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using Microsoft.Extensions.DependencyInjection;
    using System.Linq.Expressions;
    using CommercialDocuments.Application.Invoices.Integration;
    using Inventory.Contracts.Transactions;
    using Treasury.Contracts.Financials;
    using MediatR;

    public sealed record DeleteListInvoiceCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<CommercialDocuments.Domain.Invoice> _Repository, IServiceProvider _provider, ISender sender) : DeleteCommandHandler<DeleteListInvoiceCommand, CommercialDocuments.Domain.Invoice>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<CommercialDocuments.Domain.Invoice, bool>> CreateFilter(DeleteListInvoiceCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }


        public override async Task<bool> RemoveDetails(DeleteListInvoiceCommand request)
        {
            foreach (var invoiceId in request.Ids)
            {
                var invoice = await _Repository.GetByFilterAsync(i => i.Id == invoiceId,"InvoiceProducts");

                if (invoice == null)
                    continue;

                await new InvoiceJournalPostingService(sender).DeleteByInvoiceIdAsync(invoiceId);
                invoice.InvoiceProducts!.Clear();

                await sender.Send(new DeleteFinancialsByInvoiceCommand(invoiceId));

                if (invoice.TransactionId is > 0)
                    await sender.Send(new DeleteTransactionByInvoiceCommand(invoice.TransactionId.Value));
            }

            // The base delete handler deletes the invoices and persists the complete
            // graph in one SaveChanges call, allowing EF to order dependent deletes.
            return true;
        }
    }
}
