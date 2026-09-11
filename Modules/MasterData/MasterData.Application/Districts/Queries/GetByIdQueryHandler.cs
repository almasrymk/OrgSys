namespace MasterData.Application.Districts.Queries
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetByIdDistrictQuery(long Id) : ICommand<DistrictDto> , IGetByIdQuery<Result<DistrictDto>>;

    public sealed class GetByIdQueryHandler(IRepository<MasterData.Domain.District> _Repository, IMapper mapper) : GetCommandHandler<GetByIdDistrictQuery, MasterData.Domain.District, DistrictDto>(_Repository, mapper)
    {
        public override Expression<Func<MasterData.Domain.District, bool>> CreateFilter(GetByIdDistrictQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}
