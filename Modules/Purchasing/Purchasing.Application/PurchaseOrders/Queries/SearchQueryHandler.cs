namespace Purchasing.Application.PurchaseOrders.Queries
{
    using AutoMapper;
    using MediatR;
    using OrgSys.SharedKernel;
    using Parties.Contracts.Dealers;
    using System.Net;

    public sealed record SearchPurchaseOrderQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize)
        : ICommandPagination<PurchaseOrderDto>, ISearchQuery<ResultPagination<PurchaseOrderDto>>;

    /// <summary>Bespoke handler — see GetByIdQueryHandler's remark on why the generic
    /// OrgSys.SharedKernel.SearchCommandHandler&lt;,,&gt; no longer applies. Filter/order logic unchanged.
    /// DealerName is patched via Parties.Contracts after PurchaseOrder.Dealer was dropped.</summary>
    public sealed class SearchQueryHandler(IRepository<PurchaseOrder> repository, IMapper mapper, ISender sender)
        : ICommandPaginationHandler<SearchPurchaseOrderQuery, PurchaseOrderDto>
    {
        public async Task<ResultPagination<PurchaseOrderDto>> Handle(SearchPurchaseOrderQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var page = await repository.GetPaginationByFilterAsync(
                    e => (string.IsNullOrEmpty(request.KeySearch) || e.Code!.Contains(request.KeySearch)) &&
                         e.Status != Status.Deleted && e.Hide != true,
                    q => q.OrderByDescending(e => e.Id),
                    string.Empty,
                    request.Page,
                    request.PageSize);

                if (page is null || page.Items is null)
                    return new ResultPagination<PurchaseOrderDto>(HttpStatusCode.InternalServerError, [], 0, 0, 0, [new Error("Error")]);

                var dtos = page.Items.Select(mapper.Map<PurchaseOrderDto>).ToList();
                var dealerIds = dtos.Select(e => e.DealerId).Distinct().ToList();
                if (dealerIds.Count > 0)
                {
                    var names = (await sender.Send(new GetDealerNamesQuery(dealerIds), cancellationToken)).Response ?? [];
                    foreach (var dto in dtos)
                        dto.DealerName = names.GetValueOrDefault(dto.DealerId);
                }

                return new ResultPagination<PurchaseOrderDto>(
                    HttpStatusCode.OK,
                    dtos,
                    page.Page, page.PageSize, page.TotalPages,
                    null);
            }
            catch (Exception ex)
            {
                return new ResultPagination<PurchaseOrderDto>(HttpStatusCode.InternalServerError, [], 0, 0, 0, [new Error(ex.Message)]);
            }
        }
    }
}
