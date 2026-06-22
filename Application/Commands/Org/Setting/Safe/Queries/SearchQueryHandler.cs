namespace Application.Commands.Org.Setting.Safe.Queries
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

    public sealed record SearchSafeQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<SafeModelView> ,ISearchQuery<ResultPagination<SafeModelView>>;

    public sealed class SearchQueryHandler(IRepository<Domain.Entities.Safe> _Repository, IMapper mapper) : SearchCommandHandler<SearchSafeQuery, Domain.Entities.Safe, SafeModelView>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.Safe, bool>> CreateFilter(SearchSafeQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Domain.Entities.Safe>, IOrderedQueryable<Domain.Entities.Safe>> CreateOrderBy(SearchSafeQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}