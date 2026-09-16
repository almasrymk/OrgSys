namespace Inventory.Application.Transactions.Commands
{
    using AutoMapper;
    using CommercialDocuments.Contracts.Invoices;
    using MediatR;
    using System.Linq.Expressions;
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
            var invoices = (await _provider.GetRequiredService<ISender>()
                .Send(new GetInvoicesByLinkedTransactionsQuery(request.Ids))).Response ?? [];
            if (invoices.Count > 0)
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
                await new TransactionJournalPostingService(_provider).DeleteByTransactionIdAsync(transaction.Id);
                await transactionProductRepository.ShiftDeleteAsync(e => e.TransactionId == transaction.Id);
            }
            
            return true;
        }
    }
}
