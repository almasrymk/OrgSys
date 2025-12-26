namespace Application.Commands.Org.Setting.City.Queries
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

    public sealed record GetListCityQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<CityModelView> , IListQuery<ResultCollection<CityModelView>>;

    public sealed class GetListQueryHandler(IRepository<Entity.Model.City> _Repository, IMapper mapper) : ListCommandHandler<GetListCityQuery, Entity.Model.City, CityModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.City, bool>> CreateFilter(GetListCityQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Entity.Model.City>, IOrderedQueryable<Entity.Model.City>> CreateOrderBy(GetListCityQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}