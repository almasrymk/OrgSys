namespace Application.Commands.Org.Setting.Country.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;

    public sealed record SearchCountryQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<CountryDto> ,ISearchQuery<ResultPagination<CountryDto>>;

    public sealed class SearchQueryHandler(IRepository<Domain.Entities.Country> _Repository, IMapper mapper) : SearchCommandHandler<SearchCountryQuery, Domain.Entities.Country, CountryDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.Country, bool>> CreateFilter(SearchCountryQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Domain.Entities.Country>, IOrderedQueryable<Domain.Entities.Country>> CreateOrderBy(SearchCountryQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}