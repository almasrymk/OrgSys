namespace MasterData.Application.Cities.Queries
{
    using AutoMapper;
    using System.Linq.Expressions;
    using OrgSys.SharedKernel;

    public sealed record SearchCityQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<CityDto> ,ISearchQuery<ResultPagination<CityDto>>;

    public sealed class SearchQueryHandler(IRepository<MasterData.Domain.City> _Repository, IMapper mapper) : SearchCommandHandler<SearchCityQuery, MasterData.Domain.City, CityDto>(_Repository, mapper)
    {
        public override Expression<Func<MasterData.Domain.City, bool>> CreateFilter(SearchCityQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Name!.Contains(request.KeySearch)) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        public override string CreateInclude()
        {
            return "Country";
        }

        override public Func<IQueryable<MasterData.Domain.City>, IOrderedQueryable<MasterData.Domain.City>> CreateOrderBy(SearchCityQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}
