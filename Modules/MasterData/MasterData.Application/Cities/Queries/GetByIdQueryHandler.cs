namespace MasterData.Application.Cities.Queries
{
    using AutoMapper;
    using System.Linq.Expressions;
    using OrgSys.SharedKernel;

    public sealed record GetByIdCityQuery(long Id) : ICommand<CityDto> , IGetByIdQuery<Result<CityDto>>;

    public sealed class GetByIdQueryHandler(IRepository<MasterData.Domain.City> _Repository, IMapper mapper) : GetCommandHandler<GetByIdCityQuery, MasterData.Domain.City, CityDto>(_Repository, mapper)
    {
        public override Expression<Func<MasterData.Domain.City, bool>> CreateFilter(GetByIdCityQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}
