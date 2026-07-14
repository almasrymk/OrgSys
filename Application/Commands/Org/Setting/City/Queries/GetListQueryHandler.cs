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

    public sealed record GetListCityQuery(string KeySearch , long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<CityDto> , IListQuery<ResultCollection<CityDto>>;

    public sealed class GetListQueryHandler(IRepository<Domain.Entities.City> _Repository, IMapper mapper) : ListCommandHandler<GetListCityQuery, Domain.Entities.City, CityDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.City, bool>> CreateFilter(GetListCityQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&            
            e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Domain.Entities.City>, IOrderedQueryable<Domain.Entities.City>> CreateOrderBy(GetListCityQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}