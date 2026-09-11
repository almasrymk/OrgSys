namespace Organization.Application.Tables.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record SearchTableQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<TableDto> ,ISearchQuery<ResultPagination<TableDto>>;

    public sealed class SearchQueryHandler(IRepository<Organization.Domain.Table> _Repository, IMapper mapper) : SearchCommandHandler<SearchTableQuery, Organization.Domain.Table, TableDto>(_Repository, mapper)
    {
        public override Expression<Func<Organization.Domain.Table, bool>> CreateFilter(SearchTableQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Organization.Domain.Table>, IOrderedQueryable<Organization.Domain.Table>> CreateOrderBy(SearchTableQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}