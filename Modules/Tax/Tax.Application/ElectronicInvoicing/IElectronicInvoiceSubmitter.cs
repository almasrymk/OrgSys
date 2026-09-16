namespace Tax.Application.ElectronicInvoicing;

using Tax.Domain;

public sealed record ElectronicInvoiceSubmission(
    ElectronicInvoiceChannel Channel,
    ElectronicInvoiceSubmissionStatus Status,
    string? ExternalReference,
    string? Error);

public interface IElectronicInvoiceAdapter
{
    ElectronicInvoiceChannel Channel { get; }

    bool IsEnabled { get; }

    Task<ElectronicInvoiceSubmission> SubmitAsync(InvoiceTaxSnapshot snapshot, CancellationToken cancellationToken);
}

public interface IElectronicInvoiceSubmitter
{
    Task<ElectronicInvoiceSubmission> SubmitAsync(InvoiceTaxSnapshot snapshot, CancellationToken cancellationToken);
}
