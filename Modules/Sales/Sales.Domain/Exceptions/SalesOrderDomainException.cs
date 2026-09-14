namespace Sales.Domain.Exceptions;

/// <summary>Base type for a SalesOrder-invariant violation. Mirrors
/// Sales.Domain.Exceptions.QuotationDomainException / Advances.Domain.Exceptions.CustodyDomainException.</summary>
public abstract class SalesOrderDomainException(string message) : Exception(message);

/// <summary>A sales order line was added with a quantity or unit price that is not valid.</summary>
public sealed class InvalidSalesOrderLineException(string message) : SalesOrderDomainException(message);

/// <summary>A line was added/removed while the order is not Draft.</summary>
public sealed class SalesOrderNotEditableException(string message) : SalesOrderDomainException(message);

/// <summary>Confirm() was attempted on an empty order, or on an order that is not Draft.</summary>
public sealed class SalesOrderNotConfirmableException(string message) : SalesOrderDomainException(message);

/// <summary>RecordDelivery() would deliver more than a line's remaining (ordered - delivered - cancelled) quantity, or was attempted on an order not open for delivery.</summary>
public sealed class InvalidSalesOrderDeliveryException(string message) : SalesOrderDomainException(message);

/// <summary>RecordReturn() would return more than a line's delivered-minus-already-returned quantity (brief §28), or referenced a line that was never delivered (brief §27).</summary>
public sealed class InvalidSalesOrderReturnException(string message) : SalesOrderDomainException(message);

/// <summary>Cancel() was attempted on an order that is already fully Delivered or Cancelled.</summary>
public sealed class SalesOrderCannotBeCancelledException(string message) : SalesOrderDomainException(message);
