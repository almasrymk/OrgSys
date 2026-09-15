using OrgSys.SharedKernel;
using OrgSys.SharedKernel;
using OrgSys.SharedKernel;
using AutoMapper;

namespace Catalog.Application.ProductUnits.Queries
{
    public sealed record SearchProductUnitQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<ProductUnitDto> ,ISearchQuery<ResultPagination<ProductUnitDto>>;

    public sealed class SearchQueryHandler(IRepository<Catalog.Domain.ProductUnit> _Repository, IMapper mapper) : SearchCommandHandler<SearchProductUnitQuery, Catalog.Domain.ProductUnit, ProductUnitDto>(_Repository, mapper)
    {        
        override public Func<IQueryable<Catalog.Domain.ProductUnit>, IOrderedQueryable<Catalog.Domain.ProductUnit>> CreateOrderBy(SearchProductUnitQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}