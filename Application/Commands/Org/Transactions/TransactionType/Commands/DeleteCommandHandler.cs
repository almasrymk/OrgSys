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

    public sealed record DeleteTransactionTypeCommand(long Id) : ICommand, IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.TransactionType> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteTransactionTypeCommand, Entity.Model.TransactionType>(_UnitOfWork, _Repository, _provider)
    {
        public override Expression<Func<Entity.Model.TransactionType, bool>> CreateFilter(DeleteTransactionTypeCommand request)
        {
            return e => e.Id == request.Id && e.Status != Utility.Status.Deleted && e.Hide != true;
        }
    }
}