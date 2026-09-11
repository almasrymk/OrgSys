using OrgSys.SharedKernel;
using OrgSys.SharedKernel;
using OrgSys.SharedKernel;
using AutoMapper;

namespace Inventory.Application.ProductUnits.Queries
{
    public sealed record SearchProductUnitQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<ProductUnitDto> ,ISearchQuery<ResultPagination<ProductUnitDto>>;

    public sealed class SearchQueryHandler(IRepository<Inventory.Domain.ProductUnit> _Repository, IMapper mapper) : SearchCommandHandler<SearchProductUnitQuery, Inventory.Domain.ProductUnit, ProductUnitDto>(_Repository, mapper)
    {        
        override public Func<IQueryable<Inventory.Domain.ProductUnit>, IOrderedQueryable<Inventory.Domain.ProductUnit>> CreateOrderBy(SearchProductUnitQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}