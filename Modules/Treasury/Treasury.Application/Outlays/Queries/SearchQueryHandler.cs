namespace Treasury.Application.Outlays.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record SearchOutlayQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<OutlayDto> ,ISearchQuery<ResultPagination<OutlayDto>>;

    public sealed class SearchQueryHandler(IRepository<Treasury.Domain.Outlay> _Repository, IMapper mapper) : SearchCommandHandler<SearchOutlayQuery, Treasury.Domain.Outlay, OutlayDto>(_Repository, mapper)
    {
        public override Expression<Func<Treasury.Domain.Outlay, bool>> CreateFilter(SearchOutlayQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Treasury.Domain.Outlay>, IOrderedQueryable<Treasury.Domain.Outlay>> CreateOrderBy(SearchOutlayQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}