namespace Application.Commands.Org.Setting.Country.Queries
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

    public sealed record GetListCountryQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<CountryDto> , IListQuery<ResultCollection<CountryDto>>;

    public sealed class GetListQueryHandler(IRepository<Domain.Entities.Country> _Repository, IMapper mapper) : ListCommandHandler<GetListCountryQuery, Domain.Entities.Country, CountryDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.Country, bool>> CreateFilter(GetListCountryQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Domain.Entities.Country>, IOrderedQueryable<Domain.Entities.Country>> CreateOrderBy(GetListCountryQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}