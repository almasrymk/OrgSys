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

    public sealed record DeleteCurrencyCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Entity.Model.Currency> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteCurrencyCommand, Entity.Model.Currency>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Entity.Model.Currency, bool>> CreateFilter(DeleteCurrencyCommand request)
        {
            return e => e.Id == request.Id && e.Status != Utility.Status.Deleted && e.Hide != true;
        }
    }
}