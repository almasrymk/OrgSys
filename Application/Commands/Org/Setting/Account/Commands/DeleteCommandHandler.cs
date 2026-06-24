namespace Application.Commands.Org.Setting.Account.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;

    public sealed record DeleteAccountCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.Account> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteAccountCommand, Domain.Entities.Account>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Domain.Entities.Account, bool>> CreateFilter(DeleteAccountCommand request)
        {
            return e => e.Id == request.Id && e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
    }
}