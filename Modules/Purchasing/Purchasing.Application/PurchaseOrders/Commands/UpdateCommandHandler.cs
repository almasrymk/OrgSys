namespace Purchasing.Application.PurchaseOrders.Commands
{
    using MediatR;
    using OrgSys.SharedKernel;
    using Purchasing.Domain.Exceptions;
    using System.Net;

    public sealed class UpdatePurchaseOrderCommand : PurchaseOrderDto, ICommand, IUpdateCommand<Result>;

    /// <summary>
    /// Bespoke handler replacing the old generic OrgSys.SharedKernel.UpdateCommandHandler&lt;,&gt;
    /// — see CreateCommandHandler's identical remark. Syncs PurchaseOrderProductList against the
    /// aggregate's own lines (add new rows, update existing ones by Id, remove missing ones) through
    /// PurchaseOrder.AddLine/UpdateLine/RemoveLine, all of which now reject the edit outright once
    /// the order is no longer New — the generic handler this replaces had no such guard at all
    /// (brief §27 "Issued/approved PO cannot be arbitrarily edited"); enforcing it here is a
    /// deliberate, requested correctness fix, not a wire-format change.
    /// </summary>
    public sealed class UpdateCommandHandler(IUnitOfWork unitOfWork, IRepository<PurchaseOrder> repository)
        : ICommandHandler<UpdatePurchaseOrderCommand>
    {
        public async Task<Result> Handle(UpdatePurchaseOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await repository.GetByFilterAsync(e => e.Id == request.Id, "PurchaseOrderProducts");
            if (order is null || order.Status == Status.Deleted)
                return new Result(HttpStatusCode.NotFound, [new Error("Purchase order not found.")]);

            if (request.PurchaseOrderProductList is null || request.PurchaseOrderProductList.Count == 0)
                return new Result(HttpStatusCode.BadRequest, [new Error("At least one product line is required.")]);

            try
            {
                order.UpdateHeader(request.Discount, request.DiscountType, request.Tax, request.TaxType, request.Notes);
                order.Code = request.Code;
                order.CodeNumber = request.CodeNumber;
                order.Date = request.Date == default ? order.Date : request.Date;

                var requestLineIds = request.PurchaseOrderProductList.Where(l => l.Id > 0).Select(l => l.Id).ToHashSet();
                foreach (var existingLine in order.PurchaseOrderProducts.Where(l => !requestLineIds.Contains(l.Id)).ToList())
                    order.RemoveLine(existingLine.Id);

                foreach (var line in request.PurchaseOrderProductList)
                {
                    if (line.Id > 0)
                        order.UpdateLine(line.Id, line.ProductId, line.UnitId, line.Quantity, line.Price, line.Notes);
                    else
                        order.AddLine(line.ProductId, line.UnitId, line.Quantity, line.Price, line.Notes);
                }

                await repository.UpdateAsync(order);
                if (await unitOfWork.SaveChangeAsync(cancellationToken) <= 0)
                    return new Result(HttpStatusCode.InternalServerError, [new Error("Error saving changes")]);

                return new Result(HttpStatusCode.OK, null);
            }
            catch (PurchaseOrderDomainException ex)
            {
                return new Result(HttpStatusCode.BadRequest, [new Error(ex.Message)]);
            }
        }
    }
}
