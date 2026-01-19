namespace Application.Commands.Org.Setting.City.Queries
{
    using Utility;
    using AutoMapper;
    using Domain.Shared;
    using Entity.ModelView;
    using Domain.Abstraction;
    using System.Linq.Expressions;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using Application.Abstraction.Command;

    public sealed record SearchCityQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<CityModelView> ,ISearchQuery<ResultPagination<CityModelView>>;

    public sealed class SearchQueryHandler(IRepository<Entity.Model.City> _Repository, IMapper mapper) : SearchCommandHandler<SearchCityQuery, Entity.Model.City, CityModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.City, bool>> CreateFilter(SearchCityQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Status.Deleted && e.Hide != true;
        }

        public override string CreateInclude()
        {
            return "Country";
        }

        override public Func<IQueryable<Entity.Model.City>, IOrderedQueryable<Entity.Model.City>> CreateOrderBy(SearchCityQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}