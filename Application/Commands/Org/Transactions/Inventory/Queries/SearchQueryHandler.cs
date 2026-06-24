namespace Application.Commands.Org.Transactions.Inventory.Queries
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

    public sealed record SearchInventoryQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<InventoryDto> ,ISearchQuery<ResultPagination<InventoryDto>>;

    public sealed class SearchQueryHandler(IRepository<Domain.Entities.Inventory> _Repository, IMapper mapper) : SearchCommandHandler<SearchInventoryQuery, Domain.Entities.Inventory, InventoryDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.Inventory, bool>> CreateFilter(SearchInventoryQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Code.Contains(request.KeySearch)) &&
            (request.ParentId ==0 || e.ParentId == request.ParentId) &&
            (request.TypeId == 0 || e.TypeId == request.TypeId) &&
            e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }

        public override string CreateInclude()
        {
            return "Stock";
        }

        override public Func<IQueryable<Domain.Entities.Inventory>, IOrderedQueryable<Domain.Entities.Inventory   >> CreateOrderBy(SearchInventoryQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}