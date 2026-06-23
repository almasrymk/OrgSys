namespace Application.Commands.Org.Setting.District.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;
    using Utility;

    public sealed record GetListByCityDistrictQuery(string KeySearch, long? CityId, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<DistrictDto> , IListQuery<ResultCollection<DistrictDto>>;

    public sealed class GetListByCityQueryHandler(IRepository<Domain.Entities.District> _Repository, IMapper mapper) : ListCommandHandler<GetListByCityDistrictQuery, Domain.Entities.District, DistrictDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.District, bool>> CreateFilter(GetListByCityDistrictQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            (request.CityId == 0 || e.CityId == request.CityId) &&
            e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }         
    }
}