namespace Application.Commands.Org.Setting.Shift.Queries
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

    public sealed record SearchShiftQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<ShiftModelView> ,ISearchQuery<ResultPagination<ShiftModelView>>;

    public sealed class SearchQueryHandler(IRepository<Entity.Model.Shift> _Repository, IMapper mapper) : SearchCommandHandler<SearchShiftQuery, Entity.Model.Shift, ShiftModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.Shift, bool>> CreateFilter(SearchShiftQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Entity.Model.Shift>, IOrderedQueryable<Entity.Model.Shift>> CreateOrderBy(SearchShiftQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}