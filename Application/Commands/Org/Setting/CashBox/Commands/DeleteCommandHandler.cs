namespace Application.Commands.Org.Setting.CashBox.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;

    public sealed record DeleteCashBoxCommand(long Id) : ICommand, IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.CashBox> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteCashBoxCommand, Domain.Entities.CashBox>(_UnitOfWork, _Repository, _provider)
    {
        public override Expression<Func<Domain.Entities.CashBox, bool>> CreateFilter(DeleteCashBoxCommand request)
        {
            return e => e.Id == request.Id && e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
    }
}
