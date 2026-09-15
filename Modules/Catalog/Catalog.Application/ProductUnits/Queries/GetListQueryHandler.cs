using OrgSys.SharedKernel;
using OrgSys.SharedKernel;
using OrgSys.SharedKernel;
using AutoMapper;

namespace Catalog.Application.ProductUnits.Queries
{
    public sealed record GetListProductUnitQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<ProductUnitDto> , IListQuery<ResultCollection<ProductUnitDto>>;

    public sealed class GetListQueryHandler(IRepository<Catalog.Domain.ProductUnit> _Repository, IMapper mapper) : ListCommandHandler<GetListProductUnitQuery, Catalog.Domain.ProductUnit, ProductUnitDto>(_Repository, mapper)
    {       
        override public Func<IQueryable<Catalog.Domain.ProductUnit>, IOrderedQueryable<Catalog.Domain.ProductUnit>> CreateOrderBy(GetListProductUnitQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}