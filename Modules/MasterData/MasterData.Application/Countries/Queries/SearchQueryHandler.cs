namespace MasterData.Application.Countries.Queries
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record SearchCountryQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<CountryDto> ,ISearchQuery<ResultPagination<CountryDto>>;

    public sealed class SearchQueryHandler(IRepository<MasterData.Domain.Country> _Repository, IMapper mapper) : SearchCommandHandler<SearchCountryQuery, MasterData.Domain.Country, CountryDto>(_Repository, mapper)
    {
        public override Expression<Func<MasterData.Domain.Country, bool>> CreateFilter(SearchCountryQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<MasterData.Domain.Country>, IOrderedQueryable<MasterData.Domain.Country>> CreateOrderBy(SearchCountryQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}
