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

    public sealed record GetListOutlayQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<OutlayModelView> , IListQuery<ResultCollection<OutlayModelView>>;

    public sealed class GetListQueryHandler(IRepository<Entity.Model.Outlay> _Repository, IMapper mapper) : ListCommandHandler<GetListOutlayQuery, Entity.Model.Outlay, OutlayModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.Outlay, bool>> CreateFilter(GetListOutlayQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Entity.Model.Outlay>, IOrderedQueryable<Entity.Model.Outlay>> CreateOrderBy(GetListOutlayQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}