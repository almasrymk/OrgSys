namespace Application.Commands.Org.Transaction.Inventory.Queries
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

    public sealed record GetListInventoryQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize) : ICommandCollection<InventoryModelView>, IListQuery<ResultCollection<InventoryModelView>>;

    public sealed class GetListQueryHandler(IRepository<Entity.Model.Inventory> _Repository
        , IMapper mapper) : ListCommandHandler<GetListInventoryQuery, Entity.Model.Inventory, InventoryModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.Inventory, bool>> CreateFilter(GetListInventoryQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
           (string.IsNullOrEmpty(request.KeySearch) || e.Code.Contains(request.KeySearch)) &&
           (request.ParentId == 0 || e.ParentId == request.ParentId) &&
           (request.TypeId == 0 || e.TypeId == request.TypeId) &&
           e.Status != Status.Deleted && e.Hide != true;
        }

        override public Func<IQueryable<Entity.Model.Inventory>, IOrderedQueryable<Entity.Model.Inventory>> CreateOrderBy(GetListInventoryQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}