namespace Application.Commands.Org.Setting.Bank.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;
    using System.Linq.Expressions;

    public sealed record DeleteBankCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Bank> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteBankCommand, Entity.Model.Bank>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Entity.Model.Bank, bool>> CreateFilter(DeleteBankCommand request)
        {
            return e => e.Id == request.Id && e.Status != Utility.Status.Deleted && e.Hide != true;
        }
    }
}