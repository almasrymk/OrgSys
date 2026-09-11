namespace Inventory.Application.ProductUnits.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetByIdProductUnitQuery(long Id) : ICommand<ProductUnitDto> , IGetByIdQuery<Result<ProductUnitDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Inventory.Domain.ProductUnit> _Repository, IMapper mapper) : GetCommandHandler<GetByIdProductUnitQuery, Inventory.Domain.ProductUnit, ProductUnitDto>(_Repository, mapper)
    {
        public override Expression<Func<Inventory.Domain.ProductUnit, bool>> CreateFilter(GetByIdProductUnitQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}