namespace MasterData.Application.Units.Queries
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetByIdUnitQuery(long Id) : ICommand<UnitDto> , IGetByIdQuery<Result<UnitDto>>;

    public sealed class GetByIdQueryHandler(IRepository<MasterData.Domain.Unit> _Repository, IMapper mapper) : GetCommandHandler<GetByIdUnitQuery, MasterData.Domain.Unit, UnitDto>(_Repository, mapper)
    {
        public override Expression<Func<MasterData.Domain.Unit, bool>> CreateFilter(GetByIdUnitQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}
