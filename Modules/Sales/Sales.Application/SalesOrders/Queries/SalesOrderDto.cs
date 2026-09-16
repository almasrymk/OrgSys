namespace Sales.Application.SalesOrders.Queries;

public sealed record SalesOrderLineDto(
    long Id,
    long ProductId,
    string ProductName,
    long UnitId,
    decimal OrderedQuantity,
    decimal UnitPrice,
    decimal DiscountAmount,
    decimal TaxAmount,
    decimal LineTotal,
    decimal DeliveredQuantity,
    decimal ReturnedQuantity,
    decimal CancelledQuantity,
    DateTime? RequestedDeliveryDate,
    string? Notes);

public sealed record SalesOrderDto(
    long Id,
    string? Code,
    long CustomerId,
    DateTime? RequestedDeliveryDate,
    long CurrencyId,
    decimal Rate,
    long? SourceQuotationId,
    decimal Subtotal,
    decimal DiscountAmount,
    decimal TaxAmount,
    decimal TotalAmount,
    SalesOrderStatus LifecycleStatus,
    string? Notes,
    DateTime Date,
    long? BranchId,
    List<SalesOrderLineDto> Lines);
