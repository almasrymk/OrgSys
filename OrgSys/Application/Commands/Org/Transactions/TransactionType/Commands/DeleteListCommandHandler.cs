namespace Application.Commands.Org.Transactions.TransactionType.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;
    using System.Linq.Expressions;

    public sealed record DeleteListTransactionTypeCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.TransactionType> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListTransactionTypeCommand, Entity.Model.TransactionType>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Entity.Model.TransactionType, bool>> CreateFilter(DeleteListTransactionTypeCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != Utility.Status.Deleted && e.Hide != true;
        }
    }
}