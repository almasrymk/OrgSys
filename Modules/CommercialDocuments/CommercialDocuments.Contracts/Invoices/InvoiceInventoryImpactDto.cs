namespace CommercialDocuments.Contracts.Invoices;

public sealed record InvoiceInventoryLineDto(
    long ProductId,
    long UnitId,
    long? StockId,
    decimal Quantity,
    decimal Price,
    string? Notes);

public sealed record InvoiceInventoryImpactDto(
    long Id,
    string? Code,
    long TypeId,
    long DealerId,
    long? StockId,
    long? TransactionId,
    DateTime Date,
    DateTime CreateDate,
    long CreateUserId,
    long? ShiftId,
    long? BranchId,
    string? Notes,
    bool Posted,
    IReadOnlyList<InvoiceInventoryLineDto> Lines);
