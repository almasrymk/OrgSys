namespace Tax.Application.Snapshots.Commands;

using Tax.Application.ElectronicInvoicing;
using Tax.Contracts.Snapshots;
using Tax.Domain.Exceptions;
using System.Net;

public sealed class SnapshotInvoiceTaxCommandHandler(
    IUnitOfWork unitOfWork,
    IRepository<InvoiceTaxSnapshot> repository,
    IElectronicInvoiceSubmitter submitter)
    : ICommandHandler<SnapshotInvoiceTaxCommand, long>
{
    public async Task<Result<long>> Handle(SnapshotInvoiceTaxCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var existing = await repository.GetByFilterAsync(e => e.InvoiceId == request.InvoiceId, "Lines");
            if (existing is not null && existing.Id > 0)
                return new Result<long>(HttpStatusCode.OK, existing.Id, null);

            var snapshot = InvoiceTaxSnapshot.Capture(
                request.InvoiceId,
                request.InvoiceNumber,
                request.InvoiceTypeId,
                request.TaxType,
                request.TaxInput,
                request.DiscountType,
                request.Discount,
                request.Total,
                request.Net,
                request.CurrencyId,
                request.CapturedAt);

            foreach (var line in request.Lines ?? [])
                snapshot.AddLine(line.ProductId, line.Quantity, line.Price, line.Tax, line.Net, line.Total);

            await repository.CreateAsync(snapshot);
            if (await unitOfWork.SaveChangeAsync(cancellationToken) <= 0)
                return new Result<long>(HttpStatusCode.InternalServerError, 0, [new Error("Error")]);

            var submission = await submitter.SubmitAsync(snapshot, cancellationToken);
            snapshot.RecordSubmission(submission.Channel, submission.Status, submission.ExternalReference, submission.Error);
            await unitOfWork.SaveChangeAsync(cancellationToken);

            return new Result<long>(HttpStatusCode.OK, snapshot.Id, null);
        }
        catch (TaxDomainException ex)
        {
            return new Result<long>(HttpStatusCode.BadRequest, 0, [new Error(ex.Message)]);
        }
    }
}
