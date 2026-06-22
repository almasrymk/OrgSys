namespace Application.Commands.Org.Setting.Currency.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;

    public sealed record DeleteCurrencyCommand(long Id) : ICommand , IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.Currency> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteCurrencyCommand, Domain.Entities.Currency>(_UnitOfWork, _Repository , _provider)
    {
        public override Expression<Func<Domain.Entities.Currency, bool>> CreateFilter(DeleteCurrencyCommand request)
        {
            return e => e.Id == request.Id && e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
    }
}