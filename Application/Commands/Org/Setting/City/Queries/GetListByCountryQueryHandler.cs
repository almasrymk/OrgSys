namespace Application.Commands.Org.Setting.City.Queries
{
    using Utility;
    using AutoMapper;
    using Domain.Shared;
    using Application.DTOs;
    using Domain.Abstraction;
    using System.Linq.Expressions;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using Application.Abstraction.Command;

    public sealed record GetListByCountryCityQuery(string KeySearch  , long? CountryId , long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<CityModelView> , IListQuery<ResultCollection<CityModelView>>;

    public sealed class GetListByCountryQueryHandler(IRepository<Domain.Entities.City> _Repository, IMapper mapper) : ListCommandHandler<GetListByCountryCityQuery, Domain.Entities.City, CityModelView>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.City, bool>> CreateFilter(GetListByCountryCityQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch + "") || e.Name.Contains(request.KeySearch)) &&   
            (request.CountryId == 0 || e.CountryId == request.CountryId) &&
            e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }         
    }
}