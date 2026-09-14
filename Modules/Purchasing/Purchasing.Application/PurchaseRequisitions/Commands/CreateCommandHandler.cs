namespace Purchasing.Application.PurchaseRequisitions.Commands
{
    using MediatR;
    using OrgSys.SharedKernel;
    using Purchasing.Domain.Exceptions;
    using System.Net;

    public sealed class CreatePurchaseRequisitionCommand : PurchaseRequisitionDto, ICommand, ICreateCommand<Result>;

    /// <summary>Bespoke handler replacing the old generic OrgSys.SharedKernel.CreateCommandHandler&lt;,&gt;
    /// — see Purchasing.Application.PurchaseOrders.Commands.CreateCommandHandler's identical remark.</summary>
    public sealed class CreateCommandHandler(IUnitOfWork unitOfWork, IRepository<PurchaseRequisition> repository)
        : ICommandHandler<CreatePurchaseRequisitionCommand>
    {
        public async Task<Result> Handle(CreatePurchaseRequisitionCommand request, CancellationToken cancellationToken)
        {
            if (request.PurchaseRequisitionProductList is null || request.PurchaseRequisitionProductList.Count == 0)
                return new Result(HttpStatusCode.BadRequest, [new Error("At least one product line is required.")]);

            try
            {
                var requisition = PurchaseRequisition.Create(
                    createUserId: request.CreateUserId,
                    createDate: request.CreateDate == default ? DateTime.Now : request.CreateDate,
                    branchId: request.BranchId,
                    notes: request.Notes);

                requisition.Code = request.Code;
                requisition.CodeNumber = request.CodeNumber;
                requisition.TypeId = request.TypeId;
                requisition.ParentId = request.ParentId;
                requisition.Date = request.Date == default ? requisition.Date : request.Date;
                requisition.ShiftId = request.ShiftId;

                foreach (var line in request.PurchaseRequisitionProductList)
                    requisition.AddLine(line.ProductId, line.UnitId, line.Quantity, line.Notes);

                await repository.CreateAsync(requisition);
                if (await unitOfWork.SaveChangeAsync(cancellationToken) <= 0)
                    return new Result(HttpStatusCode.InternalServerError, [new Error("Error saving changes")]);

                return new Result(HttpStatusCode.OK, null);
            }
            catch (PurchaseRequisitionDomainException ex)
            {
                return new Result(HttpStatusCode.BadRequest, [new Error(ex.Message)]);
            }
        }
    }
}
