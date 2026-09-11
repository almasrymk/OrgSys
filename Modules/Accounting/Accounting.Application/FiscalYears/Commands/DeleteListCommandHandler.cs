namespace Accounting.Application.FiscalYears.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record DeleteListFiscalYearCommand(List<long> Ids) : ICommand, IDeleteListCommand<Result>;

    public sealed class DeleteListCommandHandler(IUnitOfWork _UnitOfWork, IRepository<Accounting.Domain.FiscalYear> _Repository, IServiceProvider _provider) : DeleteCommandHandler<DeleteListFiscalYearCommand, Accounting.Domain.FiscalYear>(_UnitOfWork, _Repository, _provider)
    {
        public override Expression<Func<Accounting.Domain.FiscalYear, bool>> CreateFilter(DeleteListFiscalYearCommand request)
        {
            return e => request.Ids.Contains(e.Id) && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}
