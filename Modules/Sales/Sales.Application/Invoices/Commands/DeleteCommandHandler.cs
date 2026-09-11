namespace Sales.Application.Invoices.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using Microsoft.Extensions.DependencyInjection;
    using System.Linq.Expressions;
    using global::Application.Commands.Org.Financials.Integration.JournalInvoice;
    using global::Application.Commands.Org.Financials.Integration.JournalTransaction;

    public sealed record DeleteInvoiceCommand(long Id) : ICommand, IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork,
        IRepository<Invoice> _Repository,
        IRepository<Financial> _FinancialRepo,
        IServiceProvider _provider) : DeleteCommandHandler<DeleteInvoiceCommand,Invoice>(_UnitOfWork, _Repository, _provider)
    {
        public override Expression<Func<Sales.Domain.Invoice, bool>> CreateFilter(DeleteInvoiceCommand request)
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

            //var financialRepo = _provider.GetRequiredService<IRepository<Financial>>();

            var financial = await _FinancialRepo.GetByFilterAsync(e => e.FinancialInvoices.Select(f => f.InvoiceId).Contains(request.Id), "FinancialInvoices");
            if (financial != null)
            {
                //financial.FinancialInvoices.Clear();
                await _FinancialRepo.ShiftDeleteAsync(f => f.Id == financial.Id);

            }

            var transactionRepo = _provider.GetRequiredService<IRepository<Transaction>>();

            var transaction = await transactionRepo.GetByFilterAsync(e => e.Id == invoice.TransactionId, "TransactionProducts");

            if (transaction != null)
            {
                await new TransactionJournalIntegration(_provider).DeleteByTransactionIdAsync(transaction.Id);
                transaction.TransactionProducts?.Clear();
                await transactionRepo.ShiftDeleteAsync(t => t.Id == transaction.Id);
            }

            // The base delete handler deletes the invoice and saves all staged changes
            // together. Saving here would try to delete the referenced transaction
            // while the invoice still exists and can violate the foreign key.
            return true;
        }
    }
}
