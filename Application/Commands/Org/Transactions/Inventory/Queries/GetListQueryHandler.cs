namespace Application.Commands.Org.Transaction.Inventory.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;

    public sealed record GetListInventoryQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize) : ICommandCollection<InventoryDto>, IListQuery<ResultCollection<InventoryDto>>;

    public sealed class GetListQueryHandler(IRepository<Domain.Entities.Inventory> _Repository
        , IMapper mapper) : ListCommandHandler<GetListInventoryQuery, Domain.Entities.Inventory, InventoryDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.Inventory, bool>> CreateFilter(GetListInventoryQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
           (string.IsNullOrEmpty(request.KeySearch) || e.Code.Contains(request.KeySearch)) &&
           (request.ParentId == 0 || e.ParentId == request.ParentId) &&
           (request.TypeId == 0 || e.TypeId == request.TypeId) &&
           e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }

        override public Func<IQueryable<Domain.Entities.Inventory>, IOrderedQueryable<Domain.Entities.Inventory>> CreateOrderBy(GetListInventoryQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}