namespace Application.Commands.Org.Setting.DealerGroup.Queries
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

    public sealed record SearchDealerGroupQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<DealerGroupModelView> ,ISearchQuery<ResultPagination<DealerGroupModelView>>;

    public sealed class SearchQueryHandler(IRepository<Entity.Model.DealerGroup> _Repository, IMapper mapper) : SearchCommandHandler<SearchDealerGroupQuery, Entity.Model.DealerGroup, DealerGroupModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.DealerGroup, bool>> CreateFilter(SearchDealerGroupQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Entity.Model.DealerGroup>, IOrderedQueryable<Entity.Model.DealerGroup>> CreateOrderBy(SearchDealerGroupQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}