namespace Catalog.Application.Units.Queries
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetByIdUnitQuery(long Id) : ICommand<UnitDto> , IGetByIdQuery<Result<UnitDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Catalog.Domain.Unit> _Repository, IMapper mapper) : GetCommandHandler<GetByIdUnitQuery, Catalog.Domain.Unit, UnitDto>(_Repository, mapper)
    {
        public override Expression<Func<Catalog.Domain.Unit, bool>> CreateFilter(GetByIdUnitQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}
