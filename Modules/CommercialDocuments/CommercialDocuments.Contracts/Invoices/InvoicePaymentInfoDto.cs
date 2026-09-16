namespace CommercialDocuments.Contracts.Invoices;

public sealed record InvoicePaymentInfoDto(
    long Id,
    string? Code,
    long TypeId,
    long DealerId,
    decimal Net,
    decimal Paid,
    decimal Credit,
    decimal Remaining,
    decimal Rate,
    decimal CurrencyRate,
    long CurrencyId,
    long PaymentTypeId,
    long? TransactionId,
    long? StockId,
    DateTime Date,
    DateTime CreateDate,
    long CreateUserId,
    long? ShiftId,
    long? BranchId,
    string? Notes);
