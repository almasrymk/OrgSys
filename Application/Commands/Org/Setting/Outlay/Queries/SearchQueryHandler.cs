namespace Application.Commands.Org.Setting.Outlay.Queries
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

    public sealed record SearchOutlayQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<OutlayModelView> ,ISearchQuery<ResultPagination<OutlayModelView>>;

    public sealed class SearchQueryHandler(IRepository<Entity.Model.Outlay> _Repository, IMapper mapper) : SearchCommandHandler<SearchOutlayQuery, Entity.Model.Outlay, OutlayModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.Outlay, bool>> CreateFilter(SearchOutlayQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Entity.Model.Outlay>, IOrderedQueryable<Entity.Model.Outlay>> CreateOrderBy(SearchOutlayQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}