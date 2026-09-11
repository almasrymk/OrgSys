namespace Inventory.Application.TransactionTypes.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record SearchTransactionTypeQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<TransactionTypeDto> ,ISearchQuery<ResultPagination<TransactionTypeDto>>;

    public sealed class SearchQueryHandler(IRepository<Inventory.Domain.TransactionType> _Repository, IMapper mapper) : SearchCommandHandler<SearchTransactionTypeQuery, Inventory.Domain.TransactionType, TransactionTypeDto>(_Repository, mapper)
    {
        public override Expression<Func<Inventory.Domain.TransactionType, bool>> CreateFilter(SearchTransactionTypeQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Code.Contains(request.KeySearch)) &&
            (request.ParentId ==0 || e.ParentId == request.ParentId) &&
            (request.TypeId == 0 || e.TypeId == request.TypeId) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        public override string CreateInclude()
        {
            return "";
        }
        override public Func<IQueryable<Inventory.Domain.TransactionType>, IOrderedQueryable<Inventory.Domain.TransactionType>> CreateOrderBy(SearchTransactionTypeQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}