namespace Purchasing.Application.PurchaseRequisitions.Queries
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetListPurchaseRequisitionQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize)
        : ICommandCollection<PurchaseRequisitionDto>, IListQuery<ResultCollection<PurchaseRequisitionDto>>;

    public sealed class GetListQueryHandler(IRepository<PurchaseRequisition> _Repository, IMapper mapper)
        : ListCommandHandler<GetListPurchaseRequisitionQuery, PurchaseRequisition, PurchaseRequisitionDto>(_Repository, mapper)
    {
        public override Expression<Func<PurchaseRequisition, bool>> CreateFilter(GetListPurchaseRequisitionQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Code!.Contains(request.KeySearch) || (e.Notes != null && e.Notes.Contains(request.KeySearch))) &&
            e.Status != Status.Deleted && e.Hide != true;
        }

        public override Func<IQueryable<PurchaseRequisition>, IOrderedQueryable<PurchaseRequisition>> CreateOrderBy(GetListPurchaseRequisitionQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}
