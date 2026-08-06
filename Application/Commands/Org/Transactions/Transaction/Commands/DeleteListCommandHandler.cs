namespace Application.Commands.Org.Transactions.Transaction.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;
    using Application.Commands.Org.Financials.Integration.JournalTransaction;
    using Microsoft.Extensions.DependencyInjection;

    public sealed record DeleteListTransactionCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.Transaction> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListTransactionCommand, Domain.Entities.Transaction>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Domain.Entities.Transaction, bool>> CreateFilter(DeleteListTransactionCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }

        public override async Task<bool> RemoveDetails(DeleteListTransactionCommand request)
        {
            var invoiceRepository = _provider.GetRequiredService<IRepository<Domain.Entities.Invoice>>();
            var sourceInvoices = await invoiceRepository.GetListByFilterAsync(e => e.TransactionId.HasValue && request.Ids.Contains(e.TransactionId.Value));
            if (sourceInvoices?.Any() == true)
                throw new InvalidOperationException("Transactions created from invoices cannot be deleted");

            var transactions = await _Repository.GetListByFilterAsync(t => request.Ids.Contains(t.Id), "TransactionProducts");

            if (transactions == null || !transactions.Any())
                return false;



            if (transactions.Any(e => e.InventoryId is > 0))
                throw new InvalidOperationException("Transactions created from inventories cannot be deleted");

            var transactionProductRepository = _provider.GetRequiredService<IRepository<Domain.Entities.TransactionProduct>>();
            foreach (var transaction in transactions)
            {
                await new TransactionJournalIntegration(_provider).DeleteByTransactionIdAsync(transaction.Id);
                await transactionProductRepository.ShiftDeleteAsync(e => e.TransactionId == transaction.Id);
            }
            
            return true;
        }
    }
}
