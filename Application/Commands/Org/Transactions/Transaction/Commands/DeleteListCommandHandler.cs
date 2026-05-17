namespace Application.Commands.Org.Transactions.Transaction.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;
    using System.Linq.Expressions;

    public sealed record DeleteListTransactionCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Transaction> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListTransactionCommand, Entity.Model.Transaction>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Entity.Model.Transaction, bool>> CreateFilter(DeleteListTransactionCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != Utility.Status.Deleted && e.Hide != true;
        }

        public override async Task<bool> RemoveDetails(DeleteListTransactionCommand request)
        {

            var transactions = await _Repository.GetListByFilterAsync(t => request.Ids.Contains(t.Id), "TransactionProducts");

            if (transactions == null || !transactions.Any())
                return false;



            foreach (var transactionProduct in transactions)
                transactionProduct.TransactionProducts.Clear();
            
            return true;
        }
    }
}