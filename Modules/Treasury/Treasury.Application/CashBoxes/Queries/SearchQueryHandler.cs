namespace Treasury.Application.CashBoxes.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record SearchCashBoxQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize) : ICommandPagination<CashBoxDto>, ISearchQuery<ResultPagination<CashBoxDto>>;

    public sealed class SearchQueryHandler(IRepository<Treasury.Domain.CashBox> _Repository, IMapper mapper) : SearchCommandHandler<SearchCashBoxQuery, Treasury.Domain.CashBox, CashBoxDto>(_Repository, mapper)
    {
        public override Expression<Func<Treasury.Domain.CashBox, bool>> CreateFilter(SearchCashBoxQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        override public Func<IQueryable<Treasury.Domain.CashBox>, IOrderedQueryable<Treasury.Domain.CashBox>> CreateOrderBy(SearchCashBoxQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}
