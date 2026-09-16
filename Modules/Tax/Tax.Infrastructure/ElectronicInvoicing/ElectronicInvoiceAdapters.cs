namespace Tax.Infrastructure.ElectronicInvoicing;

using Tax.Application.ElectronicInvoicing;
using Tax.Domain;

/// <summary>ETA (Egypt) electronic invoice adapter. Disabled until credentials are configured.</summary>
public sealed class DisabledEtaElectronicInvoiceAdapter : IElectronicInvoiceAdapter
{
    public ElectronicInvoiceChannel Channel => ElectronicInvoiceChannel.Eta;

    public bool IsEnabled => false;

    public Task<ElectronicInvoiceSubmission> SubmitAsync(InvoiceTaxSnapshot snapshot, CancellationToken cancellationToken) =>
        Task.FromResult(new ElectronicInvoiceSubmission(
            Channel,
            ElectronicInvoiceSubmissionStatus.Skipped,
            null,
            "ETA adapter is not configured."));
}

/// <summary>ZATCA (Saudi Arabia) electronic invoice adapter. Disabled until credentials are configured.</summary>
public sealed class DisabledZatcaElectronicInvoiceAdapter : IElectronicInvoiceAdapter
{
    public ElectronicInvoiceChannel Channel => ElectronicInvoiceChannel.Zatca;

    public bool IsEnabled => false;

    public Task<ElectronicInvoiceSubmission> SubmitAsync(InvoiceTaxSnapshot snapshot, CancellationToken cancellationToken) =>
        Task.FromResult(new ElectronicInvoiceSubmission(
            Channel,
            ElectronicInvoiceSubmissionStatus.Skipped,
            null,
            "ZATCA adapter is not configured."));
}

public sealed class ElectronicInvoiceSubmitter(IEnumerable<IElectronicInvoiceAdapter> adapters) : IElectronicInvoiceSubmitter
{
    public async Task<ElectronicInvoiceSubmission> SubmitAsync(InvoiceTaxSnapshot snapshot, CancellationToken cancellationToken)
    {
        var adapter = adapters.FirstOrDefault(a => a.IsEnabled);
        if (adapter is null)
            return new ElectronicInvoiceSubmission(
                ElectronicInvoiceChannel.None,
                ElectronicInvoiceSubmissionStatus.Skipped,
                null,
                "No electronic invoice adapter is enabled.");

        return await adapter.SubmitAsync(snapshot, cancellationToken);
    }
}
