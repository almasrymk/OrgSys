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

    public sealed record GetListDistrictQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<DistrictDto> , IListQuery<ResultCollection<DistrictDto>>;

    public sealed class GetListQueryHandler(IRepository<Domain.Entities.District> _Repository, IMapper mapper) : ListCommandHandler<GetListDistrictQuery, Domain.Entities.District, DistrictDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.District, bool>> CreateFilter(GetListDistrictQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&           
            e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Domain.Entities.District>, IOrderedQueryable<Domain.Entities.District>> CreateOrderBy(GetListDistrictQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }

        public override string CreateInclude()
        {
            return "City,Country";
        }
    }
}