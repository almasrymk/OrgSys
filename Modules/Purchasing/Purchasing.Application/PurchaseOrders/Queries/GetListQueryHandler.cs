namespace Purchasing.Application.PurchaseOrders.Queries
{
    using AutoMapper;
    using MediatR;
    using OrgSys.SharedKernel;
    using Parties.Contracts.Dealers;
    using System.Net;

    public sealed record GetListPurchaseOrderQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize)
        : ICommandCollection<PurchaseOrderDto>, IListQuery<ResultCollection<PurchaseOrderDto>>;

    /// <summary>Bespoke handler — see GetByIdQueryHandler's remark on why the generic
    /// OrgSys.SharedKernel.ListCommandHandler&lt;,,&gt; no longer applies. Filter/order logic unchanged.
    /// DealerName is patched via Parties.Contracts after PurchaseOrder.Dealer was dropped.</summary>
    public sealed class GetListQueryHandler(IRepository<PurchaseOrder> repository, IMapper mapper, ISender sender)
        : ICommandCollectionHandler<GetListPurchaseOrderQuery, PurchaseOrderDto>
    {
        public async Task<ResultCollection<PurchaseOrderDto>> Handle(GetListPurchaseOrderQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var rows = await repository.GetListByFilterAsync(
                    e => (string.IsNullOrEmpty(request.KeySearch) || e.Code!.Contains(request.KeySearch)) &&
                         e.Status != Status.Deleted && e.Hide != true,
                    q => q.OrderByDescending(e => e.Id),
                    string.Empty,
                    request.Page,
                    request.PageSize);

                var dtos = (rows ?? []).Select(mapper.Map<PurchaseOrderDto>).ToList();
                var dealerIds = dtos.Select(e => e.DealerId).Distinct().ToList();
                if (dealerIds.Count > 0)
                {
                    var names = (await sender.Send(new GetDealerNamesQuery(dealerIds), cancellationToken)).Response ?? [];
                    foreach (var dto in dtos)
                        dto.DealerName = names.GetValueOrDefault(dto.DealerId);
                }

                return new ResultCollection<PurchaseOrderDto>(HttpStatusCode.OK, dtos, null);
            }
            catch (Exception ex)
            {
                return new ResultCollection<PurchaseOrderDto>(HttpStatusCode.InternalServerError, [], [new Error(ex.Message)]);
            }
        }
    }
}
