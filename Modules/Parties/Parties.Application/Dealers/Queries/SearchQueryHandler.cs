namespace Parties.Application.Dealers.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record SearchDealerQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<DealerDto> ,ISearchQuery<ResultPagination<DealerDto>>;

    public sealed class SearchQueryHandler(IRepository<Parties.Domain.Dealer> _Repository, IMapper mapper) : SearchCommandHandler<SearchDealerQuery, Parties.Domain.Dealer, DealerDto>(_Repository, mapper)
    {
        public override Expression<Func<Parties.Domain.Dealer, bool>> CreateFilter(SearchDealerQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.TypeId == request.TypeId &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
        
        override public Func<IQueryable<Parties.Domain.Dealer>, IOrderedQueryable<Parties.Domain.Dealer>> CreateOrderBy(SearchDealerQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }

        public override string CreateInclude()
        {
            // Country/City/District added — DealerDto exposes CountryName/CityName/DistrictName but
            // this handler wasn't loading those navigations, so they always came back null even
            // though CountryId/CityId/DistrictId themselves save and read back correctly.
            return "DealerGroup,Account,Country,City,District";
        }
    }
}