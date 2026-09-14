namespace Purchasing.Application.PurchaseRequisitions.Commands
{
    using MediatR;
    using OrgSys.SharedKernel;
    using Purchasing.Domain.Exceptions;
    using System.Net;

    /// <summary>Cancels a New or UnderReview requisition — new, additive capability (brief §11/§61);
    /// PurchaseRequisition.Cancel already enforces "never once Approved (sourced)".</summary>
    public sealed record CancelPurchaseRequisitionCommand(long Id) : IRequest<Result>;

    public sealed class CancelCommandHandler(IUnitOfWork unitOfWork, IRepository<PurchaseRequisition> repository)
        : IRequestHandler<CancelPurchaseRequisitionCommand, Result>
    {
        public async Task<Result> Handle(CancelPurchaseRequisitionCommand request, CancellationToken cancellationToken)
        {
            var requisition = await repository.GetByFilterAsync(e => e.Id == request.Id, string.Empty);
            if (requisition is null || requisition.Status == Status.Deleted)
                return new Result(HttpStatusCode.NotFound, [new Error("Purchase requisition not found.")]);

            try
            {
                requisition.Cancel();
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
