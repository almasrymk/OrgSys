namespace Purchasing.Application.PurchaseOrders.Commands
{
    using MediatR;
    using OrgSys.SharedKernel;
    using Purchasing.Domain.Exceptions;
    using System.Net;

    /// <summary>Cancels a New (draft) purchase order — new, additive capability (brief §26/§27);
    /// PurchaseOrder.Cancel already enforces "only if untouched" (no receipts, no linked invoice).</summary>
    public sealed record CancelPurchaseOrderCommand(long Id) : IRequest<Result>;

    public sealed class CancelCommandHandler(IUnitOfWork unitOfWork, IRepository<PurchaseOrder> repository)
        : IRequestHandler<CancelPurchaseOrderCommand, Result>
    {
        public async Task<Result> Handle(CancelPurchaseOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await repository.GetByFilterAsync(e => e.Id == request.Id, string.Empty);
            if (order is null || order.Status == Status.Deleted)
                return new Result(HttpStatusCode.NotFound, [new Error("Purchase order not found.")]);

            try
            {
                order.Cancel();
            }
            catch (PurchaseOrderDomainException ex)
            {
                return new Result(HttpStatusCode.BadRequest, [new Error(ex.Message)]);
            }

            await repository.UpdateAsync(order);
            if (await unitOfWork.SaveChangeAsync(cancellationToken) <= 0)
                return new Result(HttpStatusCode.InternalServerError, [new Error("Error saving changes")]);

            return new Result(HttpStatusCode.OK, null);
        }
    }
}
