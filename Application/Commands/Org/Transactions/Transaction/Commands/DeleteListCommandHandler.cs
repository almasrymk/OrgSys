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

    public sealed record DeleteListTransactionCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.Transaction> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListTransactionCommand, Domain.Entities.Transaction>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Domain.Entities.Transaction, bool>> CreateFilter(DeleteListTransactionCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }

        public override async Task<bool> RemoveDetails(DeleteListTransactionCommand request)
        {

            var transactions = await _Repository.GetListByFilterAsync(t => request.Ids.Contains(t.Id), "TransactionProducts");

            if (transactions == null || !transactions.Any())
                return false;



            foreach (var transactionProduct in transactions)
            {
                await new TransactionJournalIntegration(_provider).DeleteByTransactionIdAsync(transactionProduct.Id);
                transactionProduct.TransactionProducts.Clear();
            }
            
            return true;
        }
    }
}
