namespace Application.Commands.Org.Transactions.TransactionType.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;

    public sealed record DeleteTransactionTypeCommand(long Id) : ICommand, IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.TransactionType> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteTransactionTypeCommand, Domain.Entities.TransactionType>(_UnitOfWork, _Repository, _provider)
    {
        public override Expression<Func<Domain.Entities.TransactionType, bool>> CreateFilter(DeleteTransactionTypeCommand request)
        {
            return e => e.Id == request.Id && e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
    }
}