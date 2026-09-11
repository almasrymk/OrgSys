namespace Accounting.Application.FiscalYears.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetByIdFiscalYearQuery(long Id) : ICommand<FiscalYearDto>, IGetByIdQuery<Result<FiscalYearDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Accounting.Domain.FiscalYear> _Repository, IMapper mapper) : GetCommandHandler<GetByIdFiscalYearQuery, Accounting.Domain.FiscalYear, FiscalYearDto>(_Repository, mapper)
    {
        public override Expression<Func<Accounting.Domain.FiscalYear, bool>> CreateFilter(GetByIdFiscalYearQuery request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        public override string CreateInclude()
        {
            return "Periods";
        }
    }
}
