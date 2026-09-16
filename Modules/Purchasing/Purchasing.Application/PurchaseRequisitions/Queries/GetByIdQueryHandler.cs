namespace Purchasing.Application.PurchaseRequisitions.Queries
{
    using AutoMapper;
    using Catalog.Contracts.Products;
    using Catalog.Contracts.Units;
    using MediatR;
    using OrgSys.SharedKernel;
    using System.Net;

    public sealed record GetByIdPurchaseRequisitionQuery(long Id) : ICommand<PurchaseRequisitionDto>, IGetByIdQuery<Result<PurchaseRequisitionDto>>;

    /// <summary>Bespoke handler — see Purchasing.Application.PurchaseOrders.Queries.GetByIdQueryHandler's
    /// remark on why the generic OrgSys.SharedKernel.GetCommandHandler&lt;,,&gt; no longer applies.
    /// Filter/include logic unchanged.</summary>
    public sealed class GetByIdQueryHandler(IRepository<PurchaseRequisition> repository, IMapper mapper, ISender sender)
        : ICommandHandler<GetByIdPurchaseRequisitionQuery, PurchaseRequisitionDto>
    {
        public async Task<Result<PurchaseRequisitionDto>> Handle(GetByIdPurchaseRequisitionQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var requisition = await repository.GetByFilterAsync(
                    e => e.Id == request.Id && e.Status != Status.Deleted && e.Hide != true,
                    "PurchaseRequisitionProducts");

                var dto = requisition is null ? new PurchaseRequisitionDto() : mapper.Map<PurchaseRequisitionDto>(requisition);
                var lines = dto.PurchaseRequisitionProductList;
                if (lines is { Count: > 0 })
                {
                    var productIds = lines.Select(e => e.ProductId).Distinct().ToList();
                    var unitIds = lines.Select(e => e.UnitId).Distinct().ToList();
                    var productNames = (await sender.Send(new GetProductNamesQuery(productIds), cancellationToken)).Response ?? [];
                    var unitNames = (await sender.Send(new GetUnitNamesQuery(unitIds), cancellationToken)).Response ?? [];
                    foreach (var line in lines)
                    {
                        line.ProductName = productNames.GetValueOrDefault(line.ProductId);
                        line.UnitName = unitNames.GetValueOrDefault(line.UnitId);
                    }
                }

                return new Result<PurchaseRequisitionDto>(HttpStatusCode.OK, dto, null);
            }
            catch (Exception ex)
            {
                return new Result<PurchaseRequisitionDto>(HttpStatusCode.InternalServerError, null, [new Error(ex.Message)]);
            }
        }
    }
}
