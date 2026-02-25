namespace Application.Commands.Org.Setting.Currency.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;
    using System.Linq.Expressions;

    public sealed record DeleteListCurrencyCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;   

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Currency> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListCurrencyCommand, Entity.Model.Currency>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Entity.Model.Currency, bool>> CreateFilter(DeleteListCurrencyCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != Utility.Status.Deleted && e.Hide != true;
        }
    }
}