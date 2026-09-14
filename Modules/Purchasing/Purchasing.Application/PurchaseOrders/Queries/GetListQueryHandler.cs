namespace Purchasing.Application.PurchaseOrders.Queries
{
    using AutoMapper;
    using MediatR;
    using OrgSys.SharedKernel;
    using System.Net;

    public sealed record GetListPurchaseOrderQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize)
        : ICommandCollection<PurchaseOrderDto>, IListQuery<ResultCollection<PurchaseOrderDto>>;

    /// <summary>Bespoke handler — see GetByIdQueryHandler's remark on why the generic
    /// OrgSys.SharedKernel.ListCommandHandler&lt;,,&gt; no longer applies. Filter/order logic unchanged.</summary>
    public sealed class GetListQueryHandler(IRepository<PurchaseOrder> repository, IMapper mapper)
        : ICommandCollectionHandler<GetListPurchaseOrderQuery, PurchaseOrderDto>
    {
        public async Task<ResultCollection<PurchaseOrderDto>> Handle(GetListPurchaseOrderQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var rows = await repository.GetListByFilterAsync(
                    e => (string.IsNullOrEmpty(request.KeySearch) || e.Code!.Contains(request.KeySearch) || e.Dealer!.Name.Contains(request.KeySearch)) &&
                         e.Status != Status.Deleted && e.Hide != true,
                    q => q.OrderByDescending(e => e.Id),
                    "Dealer",
                    request.Page,
                    request.PageSize);

                return new ResultCollection<PurchaseOrderDto>(
                    HttpStatusCode.OK,
                    (rows ?? []).Select(mapper.Map<PurchaseOrderDto>).ToList(),
                    null);
            }
            catch (Exception ex)
            {
                return new ResultCollection<PurchaseOrderDto>(HttpStatusCode.InternalServerError, [], [new Error(ex.Message)]);
            }
        }
    }
}
