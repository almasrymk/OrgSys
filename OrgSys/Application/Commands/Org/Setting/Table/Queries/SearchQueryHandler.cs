namespace Application.Commands.Org.Setting.Table.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;
    using System.Linq.Expressions;
    using Utility;

    public sealed record SearchTableQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<TableModelView> ,ISearchQuery<ResultPagination<TableModelView>>;

    public sealed class SearchQueryHandler(IRepository<Entity.Model.Table> _Repository, IMapper mapper) : SearchCommandHandler<SearchTableQuery, Entity.Model.Table, TableModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.Table, bool>> CreateFilter(SearchTableQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Entity.Model.Table>, IOrderedQueryable<Entity.Model.Table>> CreateOrderBy(SearchTableQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}