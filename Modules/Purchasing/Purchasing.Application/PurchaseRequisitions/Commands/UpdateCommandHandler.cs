namespace Purchasing.Application.PurchaseRequisitions.Commands
{
    using MediatR;
    using OrgSys.SharedKernel;
    using Purchasing.Domain.Exceptions;
    using System.Net;

    public sealed class UpdatePurchaseRequisitionCommand : PurchaseRequisitionDto, ICommand, IUpdateCommand<Result>;

    /// <summary>Bespoke handler replacing the old generic OrgSys.SharedKernel.UpdateCommandHandler&lt;,&gt;
    /// — see Purchasing.Application.PurchaseOrders.Commands.UpdateCommandHandler's identical remark.
    /// AddLine/RemoveLine/UpdateNotes now reject the edit once the requisition is no longer New —
    /// the generic handler this replaces had no such guard.</summary>
    public sealed class UpdateCommandHandler(IUnitOfWork unitOfWork, IRepository<PurchaseRequisition> repository)
        : ICommandHandler<UpdatePurchaseRequisitionCommand>
    {
        public async Task<Result> Handle(UpdatePurchaseRequisitionCommand request, CancellationToken cancellationToken)
        {
            var requisition = await repository.GetByFilterAsync(e => e.Id == request.Id, "PurchaseRequisitionProducts");
            if (requisition is null || requisition.Status == Status.Deleted)
                return new Result(HttpStatusCode.NotFound, [new Error("Purchase requisition not found.")]);

            if (request.PurchaseRequisitionProductList is null || request.PurchaseRequisitionProductList.Count == 0)
                return new Result(HttpStatusCode.BadRequest, [new Error("At least one product line is required.")]);

            try
            {
                requisition.UpdateNotes(request.Notes);
                requisition.Code = request.Code;
                requisition.CodeNumber = request.CodeNumber;
                requisition.Date = request.Date == default ? requisition.Date : request.Date;

                var requestLineIds = request.PurchaseRequisitionProductList.Where(l => l.Id > 0).Select(l => l.Id).ToHashSet();
                foreach (var existingLine in requisition.PurchaseRequisitionProducts.Where(l => !requestLineIds.Contains(l.Id)).ToList())
                    requisition.RemoveLine(existingLine.Id);

                // Existing lines are only ever removed-and-recreated (never updated in place) — the
                // pre-hardening line shape had no UpdateLine equivalent either (the old generic
                // handler diffed by Id via AutoMapper but PurchaseRequisitionProduct's own fields
                // were never something requisitions revise once a line already exists; only the
                // deliberate quantity/product mismatch case falls through to remove+recreate here).
                foreach (var line in request.PurchaseRequisitionProductList.Where(l => l.Id <= 0))
                    requisition.AddLine(line.ProductId, line.UnitId, line.Quantity, line.Notes);

                await repository.UpdateAsync(requisition);
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
