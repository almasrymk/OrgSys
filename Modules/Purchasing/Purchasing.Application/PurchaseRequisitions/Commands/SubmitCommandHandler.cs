namespace Purchasing.Application.PurchaseRequisitions.Commands
{
    using MediatR;
    using OrgSys.SharedKernel;
    using System.Net;

    /// <summary>Moves a draft requisition (Status.New) into Status.UnderReview — "submitted",
    /// ready for someone with permission to convert it into a PurchaseOrder. No approval gate is
    /// modeled: submitting does not require anyone else's sign-off, it just marks the requisition
    /// as no longer a private draft.</summary>
    public sealed record SubmitPurchaseRequisitionCommand(long Id) : IRequest<Result>;

    public sealed class SubmitCommandHandler(IUnitOfWork _UnitOfWork, IRepository<PurchaseRequisition> _Repository)
        : IRequestHandler<SubmitPurchaseRequisitionCommand, Result>
    {
        public async Task<Result> Handle(SubmitPurchaseRequisitionCommand request, CancellationToken cancellationToken)
        {
            var requisition = await _Repository.GetByFilterAsync(e => e.Id == request.Id, string.Empty);
            if (requisition is null || requisition.Status == Status.Deleted)
                return new Result(HttpStatusCode.NotFound, [new Error("Purchase requisition not found.")]);

            if (requisition.Status != Status.New)
                return new Result(HttpStatusCode.BadRequest, [new Error("Only a draft (New) requisition can be submitted.")]);

            requisition.Status = Status.UnderReview;
            await _Repository.UpdateAsync(requisition);
            if (await _UnitOfWork.SaveChangeAsync(cancellationToken) <= 0)
                return new Result(HttpStatusCode.InternalServerError, [new Error("Error saving changes")]);

            return new Result(HttpStatusCode.OK, null);
        }
    }
}
