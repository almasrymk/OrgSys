namespace Application.Commands.Org.Setting.Unit.Queries
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

    public sealed record SearchUnitQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<UnitModelView> ,ISearchQuery<ResultPagination<UnitModelView>>;

    public sealed class SearchQueryHandler(IRepository<Domain.Entities.Unit> _Repository, IMapper mapper) : SearchCommandHandler<SearchUnitQuery, Domain.Entities.Unit, UnitModelView>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.Unit, bool>> CreateFilter(SearchUnitQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Domain.Entities.Unit>, IOrderedQueryable<Domain.Entities.Unit>> CreateOrderBy(SearchUnitQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}