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

    public sealed record DeleteFiscalYearCommand(long Id) : ICommand, IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Domain.Entities.FiscalYear> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteFiscalYearCommand, Domain.Entities.FiscalYear>(_UnitOfWork, _Repository, _provider)
    {
        public override Expression<Func<Domain.Entities.FiscalYear, bool>> CreateFilter(DeleteFiscalYearCommand request)
        {
            return e => e.Id == request.Id && e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
    }
}
