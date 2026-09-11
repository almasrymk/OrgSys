namespace Inventory.Application.Properties.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record SearchPropertyQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<PropertyDto> ,ISearchQuery<ResultPagination<PropertyDto>>;

    public sealed class SearchQueryHandler(IRepository<Inventory.Domain.Property> _Repository, IMapper mapper) : SearchCommandHandler<SearchPropertyQuery, Inventory.Domain.Property, PropertyDto>(_Repository, mapper)
    {
        public override Expression<Func<Inventory.Domain.Property, bool>> CreateFilter(SearchPropertyQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Inventory.Domain.Property>, IOrderedQueryable<Inventory.Domain.Property>> CreateOrderBy(SearchPropertyQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}