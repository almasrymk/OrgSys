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

    public sealed record DeleteTransactionCommand(long Id) : ICommand, IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Transaction> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteTransactionCommand, Entity.Model.Transaction>(_UnitOfWork, _Repository, _provider)
    {
        public override Expression<Func<Entity.Model.Transaction, bool>> CreateFilter(DeleteTransactionCommand request)
        {
            return e => e.Id == request.Id && e.Status != Utility.Status.Deleted && e.Hide != true;
        }
    }
}