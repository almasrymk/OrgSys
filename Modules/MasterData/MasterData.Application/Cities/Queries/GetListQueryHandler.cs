namespace MasterData.Application.Cities.Queries
{
    using AutoMapper;
    using System.Linq.Expressions;
    using OrgSys.SharedKernel;

    public sealed record GetListCityQuery(string KeySearch , long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<CityDto> , IListQuery<ResultCollection<CityDto>>;

    public sealed class GetListQueryHandler(IRepository<MasterData.Domain.City> _Repository, IMapper mapper) : ListCommandHandler<GetListCityQuery, MasterData.Domain.City, CityDto>(_Repository, mapper)
    {
        public override Expression<Func<MasterData.Domain.City, bool>> CreateFilter(GetListCityQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name!.Contains(request.KeySearch)) &&            
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<MasterData.Domain.City>, IOrderedQueryable<MasterData.Domain.City>> CreateOrderBy(GetListCityQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}
