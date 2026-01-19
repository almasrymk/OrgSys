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

    public sealed record GetListCityQuery(string KeySearch , long? CountryId , long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<CityModelView> , IListQuery<ResultCollection<CityModelView>>;

    public sealed class GetListQueryHandler(IRepository<Entity.Model.City> _Repository, IMapper mapper) : ListCommandHandler<GetListCityQuery, Entity.Model.City, CityModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.City, bool>> CreateFilter(GetListCityQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            (!request.CountryId.HasValue || e.CountryId == request.CountryId.Value) &&
            e.Status != Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Entity.Model.City>, IOrderedQueryable<Entity.Model.City>> CreateOrderBy(GetListCityQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}