namespace Application.Commands.Org.Setting.FiscalYear.Commands
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;

    public sealed record DeleteListFiscalYearCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.FiscalYear> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListFiscalYearCommand, Domain.Entities.FiscalYear>(_UnitOfWork, _Repository, _provider)
    {
        public override Expression<Func<Domain.Entities.FiscalYear, bool>> CreateFilter(DeleteListFiscalYearCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
    }
}
