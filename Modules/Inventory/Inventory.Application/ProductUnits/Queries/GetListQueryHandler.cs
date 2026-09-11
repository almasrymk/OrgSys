using OrgSys.SharedKernel;
using OrgSys.SharedKernel;
using OrgSys.SharedKernel;
using AutoMapper;

namespace Inventory.Application.ProductUnits.Queries
{
    public sealed record GetListProductUnitQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<ProductUnitDto> , IListQuery<ResultCollection<ProductUnitDto>>;

    public sealed class GetListQueryHandler(IRepository<Inventory.Domain.ProductUnit> _Repository, IMapper mapper) : ListCommandHandler<GetListProductUnitQuery, Inventory.Domain.ProductUnit, ProductUnitDto>(_Repository, mapper)
    {       
        override public Func<IQueryable<Inventory.Domain.ProductUnit>, IOrderedQueryable<Inventory.Domain.ProductUnit>> CreateOrderBy(GetListProductUnitQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}