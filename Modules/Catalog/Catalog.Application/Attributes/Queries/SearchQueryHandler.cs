namespace Catalog.Application.Attributes.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record SearchPropertyQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<PropertyDto> ,ISearchQuery<ResultPagination<PropertyDto>>;

    public sealed class SearchQueryHandler(IRepository<Catalog.Domain.Property> _Repository, IMapper mapper) : SearchCommandHandler<SearchPropertyQuery, Catalog.Domain.Property, PropertyDto>(_Repository, mapper)
    {
        public override Expression<Func<Catalog.Domain.Property, bool>> CreateFilter(SearchPropertyQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Catalog.Domain.Property>, IOrderedQueryable<Catalog.Domain.Property>> CreateOrderBy(SearchPropertyQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}