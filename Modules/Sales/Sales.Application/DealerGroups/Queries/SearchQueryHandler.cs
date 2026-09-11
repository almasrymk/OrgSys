namespace Sales.Application.DealerGroups.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record SearchDealerGroupQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<DealerGroupDto> ,ISearchQuery<ResultPagination<DealerGroupDto>>;

    public sealed class SearchQueryHandler(IRepository<Sales.Domain.DealerGroup> _Repository, IMapper mapper) : SearchCommandHandler<SearchDealerGroupQuery, Sales.Domain.DealerGroup, DealerGroupDto>(_Repository, mapper)
    {
        public override Expression<Func<Sales.Domain.DealerGroup, bool>> CreateFilter(SearchDealerGroupQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.TypeId == request.TypeId &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Sales.Domain.DealerGroup>, IOrderedQueryable<Sales.Domain.DealerGroup>> CreateOrderBy(SearchDealerGroupQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}