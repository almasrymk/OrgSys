namespace CommercialDocuments.Application.Invoices.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using Microsoft.Extensions.DependencyInjection;
    using System.Linq.Expressions;
    using global::Application.Commands.Org.Financials.Integration.JournalInvoice;
    using Inventory.Contracts.Transactions;
    using Treasury.Contracts.Financials;
    using MediatR;

    public sealed record DeleteInvoiceCommand(long Id) : ICommand, IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork,
        IRepository<Invoice> _Repository,
        IServiceProvider _provider,
        ISender sender) : DeleteCommandHandler<DeleteInvoiceCommand,Invoice>(_UnitOfWork, _Repository, _provider)
    {
        public override Expression<Func<CommercialDocuments.Domain.Invoice, bool>> CreateFilter(DeleteInvoiceCommand request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        public override async Task<bool> RemoveDetails(DeleteInvoiceCommand request)
        {
            var invoice = await _Repository.GetByFilterAsync(i => i.Id == request.Id, "InvoiceProducts");
            if (invoice == null)
                return false;

            await new InvoiceJournalIntegration(_provider).DeleteByInvoiceIdAsync(request.Id);
            invoice.InvoiceProducts!.Clear();

            await sender.Send(new DeleteFinancialsByInvoiceCommand(request.Id));

            if (invoice.TransactionId is > 0)
                await sender.Send(new DeleteTransactionByInvoiceCommand(invoice.TransactionId.Value));

            // The base delete handler deletes the invoice and saves all staged changes
            // together. Saving here would try to delete the referenced transaction
            // while the invoice still exists and can violate the foreign key.
            return true;
        }
    }
}
