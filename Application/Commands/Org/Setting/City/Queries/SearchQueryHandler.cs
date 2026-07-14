namespace Application.Commands.Org.Setting.City.Queries
{
    using AutoMapper;
    using Domain.Shared;
    using Application.DTOs;
    using Domain.Abstraction;
    using System.Linq.Expressions;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using Application.Abstraction.Command;

    public sealed record SearchCityQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<CityDto> ,ISearchQuery<ResultPagination<CityDto>>;

    public sealed class SearchQueryHandler(IRepository<Domain.Entities.City> _Repository, IMapper mapper) : SearchCommandHandler<SearchCityQuery, Domain.Entities.City, CityDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.City, bool>> CreateFilter(SearchCityQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }

        public override string CreateInclude()
        {
            return "Country";
        }

        override public Func<IQueryable<Domain.Entities.City>, IOrderedQueryable<Domain.Entities.City>> CreateOrderBy(SearchCityQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}