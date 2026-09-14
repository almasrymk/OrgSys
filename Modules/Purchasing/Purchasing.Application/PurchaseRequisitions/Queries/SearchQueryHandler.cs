namespace Purchasing.Application.PurchaseRequisitions.Queries
{
    using AutoMapper;
    using MediatR;
    using OrgSys.SharedKernel;
    using System.Net;

    public sealed record SearchPurchaseRequisitionQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize)
        : ICommandPagination<PurchaseRequisitionDto>, ISearchQuery<ResultPagination<PurchaseRequisitionDto>>;

    /// <summary>Bespoke handler — see Purchasing.Application.PurchaseOrders.Queries.SearchQueryHandler's
    /// remark. Filter/order logic unchanged.</summary>
    public sealed class SearchQueryHandler(IRepository<PurchaseRequisition> repository, IMapper mapper)
        : ICommandPaginationHandler<SearchPurchaseRequisitionQuery, PurchaseRequisitionDto>
    {
        public async Task<ResultPagination<PurchaseRequisitionDto>> Handle(SearchPurchaseRequisitionQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var page = await repository.GetPaginationByFilterAsync(
                    e => (string.IsNullOrEmpty(request.KeySearch) || e.Code!.Contains(request.KeySearch) || (e.Notes != null && e.Notes.Contains(request.KeySearch))) &&
                         e.Status != Status.Deleted && e.Hide != true,
                    q => q.OrderByDescending(e => e.Id),
                    string.Empty,
                    request.Page,
                    request.PageSize);

                if (page is null || page.Items is null)
                    return new ResultPagination<PurchaseRequisitionDto>(HttpStatusCode.InternalServerError, [], 0, 0, 0, [new Error("Error")]);

                return new ResultPagination<PurchaseRequisitionDto>(
                    HttpStatusCode.OK,
                    page.Items.Select(mapper.Map<PurchaseRequisitionDto>).ToList(),
                    page.Page, page.PageSize, page.TotalPages,
                    null);
            }
            catch (Exception ex)
            {
                return new ResultPagination<PurchaseRequisitionDto>(HttpStatusCode.InternalServerError, [], 0, 0, 0, [new Error(ex.Message)]);
            }
        }
    }
}
