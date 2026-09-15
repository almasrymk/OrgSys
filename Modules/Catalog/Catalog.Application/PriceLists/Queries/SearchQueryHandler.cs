namespace Catalog.Application.PriceLists.Queries
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record SearchPriceListQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize) : ICommandPagination<PriceListDto>, ISearchQuery<ResultPagination<PriceListDto>>;

    public sealed class SearchQueryHandler(IRepository<Catalog.Domain.PriceList> _Repository, IMapper mapper) : SearchCommandHandler<SearchPriceListQuery, Catalog.Domain.PriceList, PriceListDto>(_Repository, mapper)
    {
        public override Expression<Func<Catalog.Domain.PriceList, bool>> CreateFilter(SearchPriceListQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        override public Func<IQueryable<Catalog.Domain.PriceList>, IOrderedQueryable<Catalog.Domain.PriceList>> CreateOrderBy(SearchPriceListQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}
