namespace Inventory.Application.Transactions.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetListTransactionQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize) : ICommandCollection<TransactionDto>, IListQuery<ResultCollection<TransactionDto>>;

    public sealed class GetListQueryHandler(IRepository<Inventory.Domain.Transaction> _Repository, IMapper mapper) : ListCommandHandler<GetListTransactionQuery, Inventory.Domain.Transaction, TransactionDto>(_Repository, mapper)
    {
        public override Expression<Func<Inventory.Domain.Transaction, bool>> CreateFilter(GetListTransactionQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
           (string.IsNullOrEmpty(request.KeySearch) || e.Code.Contains(request.KeySearch)) &&
           (request.ParentId == 0 || e.ParentId == request.ParentId) &&
           (request.TypeId == 0
               || e.TypeId == request.TypeId
               || (request.TypeId == 1 && e.TypeId == 5)
               || (request.TypeId == 2 && e.TypeId == 6)) &&
           e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        public override string CreateInclude()
        {
            return "Dealer,Stock,ToStock";
        }

        override public Func<IQueryable<Inventory.Domain.Transaction>, IOrderedQueryable<Inventory.Domain.Transaction>> CreateOrderBy(GetListTransactionQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}
