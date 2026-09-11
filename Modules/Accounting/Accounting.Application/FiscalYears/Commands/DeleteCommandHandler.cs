namespace Accounting.Application.FiscalYears.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record DeleteFiscalYearCommand(long Id) : ICommand, IDeleteCommand<Result>;

    public sealed class DeleteCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Accounting.Domain.FiscalYear> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteFiscalYearCommand, Accounting.Domain.FiscalYear>(_UnitOfWork, _Repository, _provider)
    {
        public override Expression<Func<Accounting.Domain.FiscalYear, bool>> CreateFilter(DeleteFiscalYearCommand request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}
