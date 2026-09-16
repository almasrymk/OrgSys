namespace Sales.Application.Quotations.Queries;

public sealed record QuotationLineDto(
    long Id,
    long ProductId,
    string ProductName,
    long UnitId,
    decimal Quantity,
    decimal UnitPrice,
    decimal DiscountAmount,
    decimal TaxAmount,
    decimal LineTotal,
    DateTime? RequestedDeliveryDate,
    string? Notes);

public sealed record QuotationDto(
    long Id,
    string? Code,
    long CustomerId,
    DateTime ValidUntil,
    long CurrencyId,
    decimal Rate,
    decimal Subtotal,
    decimal DiscountAmount,
    decimal TaxAmount,
    decimal TotalAmount,
    QuotationStatus LifecycleStatus,
    long? ConvertedToSalesOrderId,
    string? Notes,
    DateTime Date,
    long? BranchId,
    List<QuotationLineDto> Lines);
