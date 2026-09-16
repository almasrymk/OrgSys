namespace Purchasing.Application.PurchaseRequisitions.Commands;

using OrgSys.SharedKernel;
using Purchasing.Contracts.PurchaseRequisitions;
using Purchasing.Domain.Exceptions;
using System.Net;

public sealed class ApplyPurchaseRequisitionWorkflowDecisionCommandHandler(
    IUnitOfWork unitOfWork,
    IRepository<PurchaseRequisition> repository)
    : ICommandHandler<ApplyPurchaseRequisitionWorkflowDecisionCommand>
{
    public async Task<Result> Handle(ApplyPurchaseRequisitionWorkflowDecisionCommand request, CancellationToken cancellationToken)
    {
        if (request.Approved)
            return new Result(HttpStatusCode.OK, null);

        var requisition = await repository.GetByFilterAsync(
            e => e.Id == request.PurchaseRequisitionId,
            "PurchaseRequisitionProducts");
        if (requisition is null || requisition.Status == Status.Deleted)
            return new Result(HttpStatusCode.NotFound, [new Error("Purchase requisition not found.")]);

        try
        {
            requisition.Reject();
        }
        catch (PurchaseRequisitionDomainException ex)
        {
            return new Result(HttpStatusCode.BadRequest, [new Error(ex.Message)]);
        }

        await repository.UpdateAsync(requisition);
        if (await unitOfWork.SaveChangeAsync(cancellationToken) <= 0)
            return new Result(HttpStatusCode.InternalServerError, [new Error("Error saving changes")]);

        return new Result(HttpStatusCode.OK, null);
    }
}
