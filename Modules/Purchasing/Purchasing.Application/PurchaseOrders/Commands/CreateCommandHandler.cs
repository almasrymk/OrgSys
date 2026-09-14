namespace Purchasing.Application.PurchaseOrders.Commands
{
    using MediatR;
    using OrgSys.SharedKernel;
    using Purchasing.Domain.Exceptions;
    using System.Net;

    public sealed class CreatePurchaseOrderCommand : PurchaseOrderDto, ICommand, ICreateCommand<Result>;

    /// <summary>
    /// Bespoke handler replacing the old generic OrgSys.SharedKernel.CreateCommandHandler&lt;,&gt;
    /// (which mapped the DTO straight onto the entity via AutoMapper) — PurchaseOrder now has
    /// private setters and is only constructible/mutable through its own domain methods, so this
    /// handler explicitly calls PurchaseOrder.Create/AddLine instead. The command's own shape
    /// (CreatePurchaseOrderCommand : PurchaseOrderDto) is unchanged, so the HTTP request contract
    /// this replaces is identical.
    /// </summary>
    public sealed class CreateCommandHandler(IUnitOfWork unitOfWork, IRepository<PurchaseOrder> repository)
        : ICommandHandler<CreatePurchaseOrderCommand>
    {
        public async Task<Result> Handle(CreatePurchaseOrderCommand request, CancellationToken cancellationToken)
        {
            if (request.PurchaseOrderProductList is null || request.PurchaseOrderProductList.Count == 0)
                return new Result(HttpStatusCode.BadRequest, [new Error("At least one product line is required.")]);

            try
            {
                var order = PurchaseOrder.Create(
                    dealerId: request.DealerId,
                    createUserId: request.CreateUserId,
                    createDate: request.CreateDate == default ? DateTime.Now : request.CreateDate,
                    purchaseRequisitionId: request.PurchaseRequisitionId,
                    branchId: request.BranchId,
                    discount: request.Discount,
                    discountType: request.DiscountType,
                    tax: request.Tax,
                    taxType: request.TaxType,
                    notes: request.Notes);

                order.Code = request.Code;
                order.CodeNumber = request.CodeNumber;
                order.TypeId = request.TypeId;
                order.ParentId = request.ParentId;
                order.Date = request.Date == default ? order.Date : request.Date;
                order.ShiftId = request.ShiftId;

                foreach (var line in request.PurchaseOrderProductList)
                    order.AddLine(line.ProductId, line.UnitId, line.Quantity, line.Price, line.Notes);

                await repository.CreateAsync(order);
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
