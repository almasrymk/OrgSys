namespace Catalog.Application.Brands.Queries
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record SearchBrandQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize) : ICommandPagination<BrandDto>, ISearchQuery<ResultPagination<BrandDto>>;

    public sealed class SearchQueryHandler(IRepository<Catalog.Domain.Brand> _Repository, IMapper mapper) : SearchCommandHandler<SearchBrandQuery, Catalog.Domain.Brand, BrandDto>(_Repository, mapper)
    {
        public override Expression<Func<Catalog.Domain.Brand, bool>> CreateFilter(SearchBrandQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        override public Func<IQueryable<Catalog.Domain.Brand>, IOrderedQueryable<Catalog.Domain.Brand>> CreateOrderBy(SearchBrandQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}
