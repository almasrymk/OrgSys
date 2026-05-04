namespace Application.Commands.Org.Setting.AccountType.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;
    using System.Linq.Expressions;

    public sealed record DeleteAccountTypeCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.AccountType> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteAccountTypeCommand, Entity.Model.AccountType>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Entity.Model.AccountType, bool>> CreateFilter(DeleteAccountTypeCommand request)
        {
            return e => e.Id == request.Id && e.Status != Utility.Status.Deleted && e.Hide != true;
        }
    }
}