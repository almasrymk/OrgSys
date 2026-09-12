namespace Purchasing.Application.PurchaseRequisitions.Queries
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record SearchPurchaseRequisitionQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize)
        : ICommandPagination<PurchaseRequisitionDto>, ISearchQuery<ResultPagination<PurchaseRequisitionDto>>;

    public sealed class SearchQueryHandler(IRepository<PurchaseRequisition> _Repository, IMapper mapper)
        : SearchCommandHandler<SearchPurchaseRequisitionQuery, PurchaseRequisition, PurchaseRequisitionDto>(_Repository, mapper)
    {
        public override Expression<Func<PurchaseRequisition, bool>> CreateFilter(SearchPurchaseRequisitionQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Code!.Contains(request.KeySearch) || (e.Notes != null && e.Notes.Contains(request.KeySearch))) &&
            e.Status != Status.Deleted && e.Hide != true;
        }

        public override Func<IQueryable<PurchaseRequisition>, IOrderedQueryable<PurchaseRequisition>> CreateOrderBy(SearchPurchaseRequisitionQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}
