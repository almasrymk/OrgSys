namespace Inventory.Application.TransactionTypes.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetListTransactionTypeQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize) : ICommandCollection<TransactionTypeDto>, IListQuery<ResultCollection<TransactionTypeDto>>;

    public sealed class GetListQueryHandler(IRepository<Inventory.Domain.TransactionType> _Repository, IMapper mapper) : ListCommandHandler<GetListTransactionTypeQuery, Inventory.Domain.TransactionType, TransactionTypeDto>(_Repository, mapper)
    {
        public override Expression<Func<Inventory.Domain.TransactionType, bool>> CreateFilter(GetListTransactionTypeQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
           (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
           (request.ParentId == 0 || e.ParentId == request.ParentId) &&
           (request.TypeId == 0 || e.TypeId == request.TypeId) &&
           e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        override public Func<IQueryable<Inventory.Domain.TransactionType>, IOrderedQueryable<Inventory.Domain.TransactionType>> CreateOrderBy(GetListTransactionTypeQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}