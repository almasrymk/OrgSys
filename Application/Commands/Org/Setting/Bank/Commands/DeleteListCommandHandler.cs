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

    public sealed record DeleteListBankCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Bank> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListBankCommand, Entity.Model.Bank>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Entity.Model.Bank, bool>> CreateFilter(DeleteListBankCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != Utility.Status.Deleted && e.Hide != true;
        }
    }
}