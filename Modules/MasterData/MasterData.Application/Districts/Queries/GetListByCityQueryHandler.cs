namespace MasterData.Application.Districts.Queries
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetListByCityDistrictQuery(string KeySearch, long? CityId, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<DistrictDto> , IListQuery<ResultCollection<DistrictDto>>;

    public sealed class GetListByCityQueryHandler(IRepository<MasterData.Domain.District> _Repository, IMapper mapper) : ListCommandHandler<GetListByCityDistrictQuery, MasterData.Domain.District, DistrictDto>(_Repository, mapper)
    {
        public override Expression<Func<MasterData.Domain.District, bool>> CreateFilter(GetListByCityDistrictQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            (request.CityId == 0 || e.CityId == request.CityId) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }         
    }
}
