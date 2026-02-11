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

    public sealed record GetListByCountryCityQuery(string KeySearch  , long? CountryId , long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<CityModelView> , IListQuery<ResultCollection<CityModelView>>;

    public sealed class GetListByCountryQueryHandler(IRepository<Entity.Model.City> _Repository, IMapper mapper) : ListCommandHandler<GetListByCountryCityQuery, Entity.Model.City, CityModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.City, bool>> CreateFilter(GetListByCountryCityQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch + "") || e.Name.Contains(request.KeySearch)) &&   
            (request.CountryId == 0 || e.CountryId == request.CountryId) &&
            e.Status != Status.Deleted && e.Hide != true;
        }         
    }
}