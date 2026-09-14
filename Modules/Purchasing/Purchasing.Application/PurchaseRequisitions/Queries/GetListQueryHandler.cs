namespace Purchasing.Application.PurchaseRequisitions.Queries
{
    using AutoMapper;
    using MediatR;
    using OrgSys.SharedKernel;
    using System.Net;

    public sealed record GetListPurchaseRequisitionQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize)
        : ICommandCollection<PurchaseRequisitionDto>, IListQuery<ResultCollection<PurchaseRequisitionDto>>;

    /// <summary>Bespoke handler — see Purchasing.Application.PurchaseOrders.Queries.GetListQueryHandler's
    /// remark. Filter/order logic unchanged.</summary>
    public sealed class GetListQueryHandler(IRepository<PurchaseRequisition> repository, IMapper mapper)
        : ICommandCollectionHandler<GetListPurchaseRequisitionQuery, PurchaseRequisitionDto>
    {
        public async Task<ResultCollection<PurchaseRequisitionDto>> Handle(GetListPurchaseRequisitionQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var rows = await repository.GetListByFilterAsync(
                    e => (string.IsNullOrEmpty(request.KeySearch) || e.Code!.Contains(request.KeySearch) || (e.Notes != null && e.Notes.Contains(request.KeySearch))) &&
                         e.Status != Status.Deleted && e.Hide != true,
                    q => q.OrderByDescending(e => e.Id),
                    string.Empty,
                    request.Page,
                    request.PageSize);

                return new ResultCollection<PurchaseRequisitionDto>(
                    HttpStatusCode.OK,
                    (rows ?? []).Select(mapper.Map<PurchaseRequisitionDto>).ToList(),
                    null);
            }
            catch (Exception ex)
            {
                return new ResultCollection<PurchaseRequisitionDto>(HttpStatusCode.InternalServerError, [], [new Error(ex.Message)]);
            }
        }
    }
}
