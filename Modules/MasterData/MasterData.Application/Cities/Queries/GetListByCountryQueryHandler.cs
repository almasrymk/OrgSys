namespace MasterData.Application.Cities.Queries
{
    using AutoMapper;
    using System.Linq.Expressions;
    using OrgSys.SharedKernel;

    public sealed record GetListByCountryCityQuery(string KeySearch  , long? CountryId , long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<CityDto> , IListQuery<ResultCollection<CityDto>>;

    public sealed class GetListByCountryQueryHandler(IRepository<MasterData.Domain.City> _Repository, IMapper mapper) : ListCommandHandler<GetListByCountryCityQuery, MasterData.Domain.City, CityDto>(_Repository, mapper)
    {
        public override Expression<Func<MasterData.Domain.City, bool>> CreateFilter(GetListByCountryCityQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch + "") || e.Name!.Contains(request.KeySearch)) &&   
            (request.CountryId == 0 || e.CountryId == request.CountryId) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }         
    }
}
