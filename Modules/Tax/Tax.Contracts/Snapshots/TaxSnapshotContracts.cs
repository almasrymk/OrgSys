namespace Tax.Contracts.Snapshots;

using OrgSys.SharedKernel;

public sealed record InvoiceTaxLineInput(
    long ProductId,
    decimal Quantity,
    decimal Price,
    decimal Tax,
    decimal Net,
    decimal Total);

public sealed record SnapshotInvoiceTaxCommand(
    long InvoiceId,
    string? InvoiceNumber,
    long InvoiceTypeId,
    int TaxType,
    decimal TaxInput,
    int DiscountType,
    decimal Discount,
    decimal Total,
    decimal Net,
    long CurrencyId,
    IReadOnlyList<InvoiceTaxLineInput> Lines,
    DateTime CapturedAt) : ICommand<long>;

public sealed record InvoiceTaxSnapshotLineDto(
    long ProductId,
    decimal Quantity,
    decimal Price,
    decimal Tax,
    decimal Net,
    decimal Total);

public sealed record InvoiceTaxSnapshotDto(
    long Id,
    long InvoiceId,
    string? InvoiceNumber,
    long InvoiceTypeId,
    int TaxType,
    decimal TaxInput,
    decimal TaxAmount,
    decimal Total,
    decimal Net,
    long CurrencyId,
    DateTime CapturedAt,
    string SubmissionChannel,
    string SubmissionStatus,
    string? ExternalReference,
    IReadOnlyList<InvoiceTaxSnapshotLineDto> Lines);

public sealed record GetInvoiceTaxSnapshotQuery(long InvoiceId) : IQuery<InvoiceTaxSnapshotDto>;
