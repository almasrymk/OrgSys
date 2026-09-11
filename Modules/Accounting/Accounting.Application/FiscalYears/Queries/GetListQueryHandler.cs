namespace Accounting.Application.FiscalYears.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetListFiscalYearQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize) : ICommandCollection<FiscalYearDto>, IListQuery<ResultCollection<FiscalYearDto>>;

    public sealed class GetListQueryHandler(IRepository<Accounting.Domain.FiscalYear> _Repository, IMapper mapper) : ListCommandHandler<GetListFiscalYearQuery, Accounting.Domain.FiscalYear, FiscalYearDto>(_Repository, mapper)
    {
        public override Expression<Func<Accounting.Domain.FiscalYear, bool>> CreateFilter(GetListFiscalYearQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        override public Func<IQueryable<Accounting.Domain.FiscalYear>, IOrderedQueryable<Accounting.Domain.FiscalYear>> CreateOrderBy(GetListFiscalYearQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}
