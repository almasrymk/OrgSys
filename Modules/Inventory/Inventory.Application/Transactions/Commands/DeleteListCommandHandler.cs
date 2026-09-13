namespace Inventory.Application.Transactions.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;
    using global::Application.Commands.Org.Financials.Integration.JournalTransaction;
    using Microsoft.Extensions.DependencyInjection;
    using Inventory.Application.Transactions.Integration;

    public sealed record DeleteListTransactionCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Inventory.Domain.Transaction> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListTransactionCommand, Inventory.Domain.Transaction>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Inventory.Domain.Transaction, bool>> CreateFilter(DeleteListTransactionCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        public override async Task<bool> RemoveDetails(DeleteListTransactionCommand request)
        {
            var invoiceRepository = _provider.GetRequiredService<IRepository<CommercialDocuments.Domain.Invoice>>();
            var sourceInvoices = await invoiceRepository.GetListByFilterAsync(e => e.TransactionId.HasValue && request.Ids.Contains(e.TransactionId.Value));
            if (sourceInvoices?.Any() == true)
                throw new InvalidOperationException("Transactions created from invoices cannot be deleted");

            var transactions = await _Repository.GetListByFilterAsync(t => request.Ids.Contains(t.Id), "TransactionProducts");

            if (transactions == null || !transactions.Any())
                return false;



            if (transactions.Any(e => e.InventoryId is > 0))
                throw new InvalidOperationException("Transactions created from inventories cannot be deleted");

            var transactionProductRepository = _provider.GetRequiredService<IRepository<Inventory.Domain.TransactionProduct>>();
            foreach (var transaction in transactions)
            {
                await new TransferReceivedIntegration(_provider).DeleteReceivedAsync(transaction);
                await new TransactionJournalIntegration(_provider).DeleteByTransactionIdAsync(transaction.Id);
                await transactionProductRepository.ShiftDeleteAsync(e => e.TransactionId == transaction.Id);
            }
            
            return true;
        }
    }
}
