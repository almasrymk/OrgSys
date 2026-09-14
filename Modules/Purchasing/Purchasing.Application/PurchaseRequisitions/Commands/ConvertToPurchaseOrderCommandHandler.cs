namespace Purchasing.Application.PurchaseRequisitions.Commands
{
    using MediatR;
    using OrgSys.SharedKernel;
    using Purchasing.Application.PurchaseOrders;
    using Purchasing.Domain.Exceptions;
    using System.Net;

    /// <summary>
    /// Converts a submitted (Status.UnderReview) requisition into a new PurchaseOrder for the
    /// given supplier: copies each requisition line's Product/Unit/Quantity across with Price left
    /// at 0 (no price was chosen at the requisition stage) — edit the resulting order via the
    /// normal PurchaseOrder update endpoint to fill in prices before it's sent to the supplier.
    /// Marks the requisition Status.Approved (its lifecycle ends here; it does not track the
    /// order's own subsequent status) via PurchaseRequisition.RecordSourced, which also records
    /// each line's OrderedQuantity — today always the line's full remaining quantity (whole-
    /// requisition conversion, matching the pre-hardening behavior exactly). One requisition can
    /// only be converted once — enforced by RecordSourced's own Status guard, not just this
    /// handler's.
    /// </summary>
    public sealed record ConvertToPurchaseOrderCommand(long PurchaseRequisitionId, long DealerId) : IRequest<Result<long>>;

    public sealed class ConvertToPurchaseOrderCommandHandler(
        IUnitOfWork unitOfWork,
        IRepository<PurchaseRequisition> requisitionRepository,
        IRepository<Purchasing.Domain.PurchaseOrder> orderRepository) : IRequestHandler<ConvertToPurchaseOrderCommand, Result<long>>
    {
        public async Task<Result<long>> Handle(ConvertToPurchaseOrderCommand request, CancellationToken cancellationToken)
        {
            var requisition = await requisitionRepository.GetByFilterAsync(
                e => e.Id == request.PurchaseRequisitionId, "PurchaseRequisitionProducts");
            if (requisition is null || requisition.Status == Status.Deleted)
                return new Result<long>(HttpStatusCode.NotFound, 0, [new Error("Purchase requisition not found.")]);

            if (requisition.Status != Status.UnderReview)
                return new Result<long>(HttpStatusCode.BadRequest, 0, [new Error("Only a submitted (UnderReview) requisition can be converted to a Purchase Order.")]);

            var now = DateTime.Now;

            await unitOfWork.BeginTransactionAsync();
            try
            {
                var order = Purchasing.Domain.PurchaseOrder.Create(
                    dealerId: request.DealerId,
                    createUserId: requisition.CreateUserId,
                    createDate: now,
                    purchaseRequisitionId: requisition.Id,
                    notes: requisition.Notes);

                var orderedQuantities = new Dictionary<long, decimal>();
                foreach (var line in requisition.PurchaseRequisitionProducts)
                {
                    order.AddLine(line.ProductId, line.UnitId, line.RemainingQuantity, price: 0, notes: line.Notes);
                    orderedQuantities[line.Id] = line.RemainingQuantity;
                }

                await orderRepository.CreateAsync(order);
                await unitOfWork.SaveChangeAsync(cancellationToken);

                requisition.RecordSourced(orderedQuantities);
                await requisitionRepository.UpdateAsync(requisition);
                if (await unitOfWork.SaveChangeAsync(cancellationToken) <= 0)
                {
                    await unitOfWork.RollbackAsync();
                    return new Result<long>(HttpStatusCode.InternalServerError, 0, [new Error("Error saving changes")]);
                }

                await unitOfWork.CommitAsync();
                return new Result<long>(HttpStatusCode.OK, order.Id, null);
            }
            catch (PurchaseRequisitionDomainException ex)
            {
                await unitOfWork.RollbackAsync();
                return new Result<long>(HttpStatusCode.BadRequest, 0, [new Error(ex.Message)]);
            }
            catch (PurchaseOrderDomainException ex)
            {
                await unitOfWork.RollbackAsync();
                return new Result<long>(HttpStatusCode.BadRequest, 0, [new Error(ex.Message)]);
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync();
                return new Result<long>(HttpStatusCode.InternalServerError, 0, [new Error(ex.Message)]);
            }
        }
    }
}
