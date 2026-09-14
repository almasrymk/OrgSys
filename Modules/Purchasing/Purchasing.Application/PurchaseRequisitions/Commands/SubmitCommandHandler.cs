namespace Purchasing.Application.PurchaseRequisitions.Commands
{
    using MediatR;
    using OrgSys.SharedKernel;
    using Purchasing.Domain.Exceptions;
    using System.Net;

    /// <summary>Moves a draft requisition (Status.New) into Status.UnderReview — "submitted",
    /// ready for someone with permission to convert it into a PurchaseOrder. No approval gate is
    /// modeled: submitting does not require anyone else's sign-off, it just marks the requisition
    /// as no longer a private draft.</summary>
    public sealed record SubmitPurchaseRequisitionCommand(long Id) : IRequest<Result>;

    public sealed class SubmitCommandHandler(IUnitOfWork unitOfWork, IRepository<PurchaseRequisition> repository)
        : IRequestHandler<SubmitPurchaseRequisitionCommand, Result>
    {
        public async Task<Result> Handle(SubmitPurchaseRequisitionCommand request, CancellationToken cancellationToken)
        {
            var requisition = await repository.GetByFilterAsync(e => e.Id == request.Id, "PurchaseRequisitionProducts");
            if (requisition is null || requisition.Status == Status.Deleted)
                return new Result(HttpStatusCode.NotFound, [new Error("Purchase requisition not found.")]);

            try
            {
                requisition.Submit();
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
}
