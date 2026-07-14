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

    public sealed record SearchDistrictQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<DistrictDto> ,ISearchQuery<ResultPagination<DistrictDto>>;

    public sealed class SearchQueryHandler(IRepository<Domain.Entities.District> _Repository, IMapper mapper) : SearchCommandHandler<SearchDistrictQuery, Domain.Entities.District, DistrictDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.District, bool>> CreateFilter(SearchDistrictQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Domain.Entities.District>, IOrderedQueryable<Domain.Entities.District>> CreateOrderBy(SearchDistrictQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }

        public override string CreateInclude()
        {
            return "City,Country";
        }
    }
}