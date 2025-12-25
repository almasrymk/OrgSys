namespace Application.Commands.Org.Setting.Country.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;
    using System.Linq.Expressions;
    using Utility;

    public sealed record SearchCountryQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<CountryModelView> ,ISearchQuery<ResultPagination<CountryModelView>>;

    public sealed class SearchQueryHandler(IRepository<Entity.Model.Country> _Repository, IMapper mapper) : SearchCommandHandler<SearchCountryQuery, Entity.Model.Country, CountryModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.Country, bool>> CreateFilter(SearchCountryQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Entity.Model.Country>, IOrderedQueryable<Entity.Model.Country>> CreateOrderBy(SearchCountryQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}