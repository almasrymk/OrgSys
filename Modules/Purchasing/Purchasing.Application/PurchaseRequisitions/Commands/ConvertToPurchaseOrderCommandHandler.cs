namespace Purchasing.Application.PurchaseRequisitions.Commands
{
    using MediatR;
    using OrgSys.SharedKernel;
    using Purchasing.Application.PurchaseOrders;
    using System.Net;

    /// <summary>
    /// Converts a submitted (Status.UnderReview) requisition into a new PurchaseOrder for the
    /// given supplier: copies each requisition line's Product/Unit/Quantity across with Price left
    /// at 0 (no price was chosen at the requisition stage) — edit the resulting order via the
    /// normal PurchaseOrder update endpoint to fill in prices before it's sent to the supplier.
    /// Marks the requisition Status.Approved (its lifecycle ends here; it does not track the
    /// order's own subsequent status). One requisition can only be converted once.
    /// </summary>
    public sealed record ConvertToPurchaseOrderCommand(long PurchaseRequisitionId, long DealerId) : IRequest<Result<long>>;

    public sealed class ConvertToPurchaseOrderCommandHandler(
        IUnitOfWork _UnitOfWork,
        IRepository<PurchaseRequisition> _RequisitionRepository,
        IRepository<Purchasing.Domain.PurchaseOrder> _OrderRepository) : IRequestHandler<ConvertToPurchaseOrderCommand, Result<long>>
    {
        public async Task<Result<long>> Handle(ConvertToPurchaseOrderCommand request, CancellationToken cancellationToken)
        {
            var requisition = await _RequisitionRepository.GetByFilterAsync(
                e => e.Id == request.PurchaseRequisitionId, "PurchaseRequisitionProducts");
            if (requisition is null || requisition.Status == Status.Deleted)
                return new Result<long>(HttpStatusCode.NotFound, 0, [new Error("Purchase requisition not found.")]);

            if (requisition.Status != Status.UnderReview)
                return new Result<long>(HttpStatusCode.BadRequest, 0, [new Error("Only a submitted (UnderReview) requisition can be converted to a Purchase Order.")]);

            var lines = (requisition.PurchaseRequisitionProducts ?? []).Select((line, index) => new Purchasing.Domain.PurchaseOrderProduct
            {
                RowNumber = index + 1,
                ProductId = line.ProductId,
                UnitId = line.UnitId,
                Quantity = line.Quantity,
                Price = 0,
                Total = 0,
                Notes = line.Notes
            }).ToList();

            var order = new Purchasing.Domain.PurchaseOrder
            {
                DealerId = request.DealerId,
                PurchaseRequisitionId = requisition.Id,
                Date = DateTime.Now,
                CreateDate = DateTime.Now,
                CreateUserId = requisition.CreateUserId,
                Notes = requisition.Notes,
                PurchaseOrderProducts = lines
            };

            await _UnitOfWork.BeginTransactionAsync();
            try
            {
                await _OrderRepository.CreateAsync(order);
                await _UnitOfWork.SaveChangeAsync(cancellationToken);

                requisition.Status = Status.Approved;
                await _RequisitionRepository.UpdateAsync(requisition);
                if (await _UnitOfWork.SaveChangeAsync(cancellationToken) <= 0)
                {
                    await _UnitOfWork.RollbackAsync();
                    return new Result<long>(HttpStatusCode.InternalServerError, 0, [new Error("Error saving changes")]);
                }

                await _UnitOfWork.CommitAsync();
                return new Result<long>(HttpStatusCode.OK, order.Id, null);
            }
            catch (Exception ex)
            {
                await _UnitOfWork.RollbackAsync();
                return new Result<long>(HttpStatusCode.InternalServerError, 0, [new Error(ex.Message)]);
            }
        }
    }
}
