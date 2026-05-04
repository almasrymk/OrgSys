namespace Application.Commands.Org.Setting.District.Queries
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

    public sealed record SearchDistrictQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<DistrictModelView> ,ISearchQuery<ResultPagination<DistrictModelView>>;

    public sealed class SearchQueryHandler(IRepository<Entity.Model.District> _Repository, IMapper mapper) : SearchCommandHandler<SearchDistrictQuery, Entity.Model.District, DistrictModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.District, bool>> CreateFilter(SearchDistrictQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Entity.Model.District>, IOrderedQueryable<Entity.Model.District>> CreateOrderBy(SearchDistrictQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }

        public override string CreateInclude()
        {
            return "City,Country";
        }
    }
}