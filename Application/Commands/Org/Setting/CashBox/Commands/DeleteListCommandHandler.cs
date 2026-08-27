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

    public sealed record DeleteListCashBoxCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.CashBox> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListCashBoxCommand, Domain.Entities.CashBox>(_UnitOfWork, _Repository, _provider)
    {
        public override Expression<Func<Domain.Entities.CashBox, bool>> CreateFilter(DeleteListCashBoxCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
    }
}
