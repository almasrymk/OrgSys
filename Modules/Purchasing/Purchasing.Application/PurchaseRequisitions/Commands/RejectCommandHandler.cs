namespace Purchasing.Application.PurchaseRequisitions.Commands
{
    using MediatR;
    using OrgSys.SharedKernel;
    using Purchasing.Domain.Exceptions;
    using System.Net;

    /// <summary>Moves a submitted (Status.UnderReview) requisition into Status.Rejected — new,
    /// additive capability (brief §11/§61); PurchaseRequisition.Reject already enforces the
    /// UnderReview-only invariant.</summary>
    public sealed record RejectPurchaseRequisitionCommand(long Id) : IRequest<Result>;

    public sealed class RejectCommandHandler(IUnitOfWork unitOfWork, IRepository<PurchaseRequisition> repository)
        : IRequestHandler<RejectPurchaseRequisitionCommand, Result>
    {
        public async Task<Result> Handle(RejectPurchaseRequisitionCommand request, CancellationToken cancellationToken)
        {
            var requisition = await repository.GetByFilterAsync(e => e.Id == request.Id, string.Empty);
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
}
