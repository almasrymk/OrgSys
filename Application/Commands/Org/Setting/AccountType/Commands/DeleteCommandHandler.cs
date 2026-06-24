namespace Application.Commands.Org.Setting.AccountType.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;

    public sealed record DeleteAccountTypeCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.AccountType> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteAccountTypeCommand, Domain.Entities.AccountType>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Domain.Entities.AccountType, bool>> CreateFilter(DeleteAccountTypeCommand request)
        {
            return e => e.Id == request.Id && e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
    }
}