namespace Application.Commands.Org.Setting.Dealer.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;

    public sealed record SearchDealerQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<DealerDto> ,ISearchQuery<ResultPagination<DealerDto>>;

    public sealed class SearchQueryHandler(IRepository<Domain.Entities.Dealer> _Repository, IMapper mapper) : SearchCommandHandler<SearchDealerQuery, Domain.Entities.Dealer, DealerDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.Dealer, bool>> CreateFilter(SearchDealerQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.TypeId == request.TypeId &&
            e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
        
        override public Func<IQueryable<Domain.Entities.Dealer>, IOrderedQueryable<Domain.Entities.Dealer>> CreateOrderBy(SearchDealerQuery request)
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