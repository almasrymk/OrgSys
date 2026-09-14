namespace Purchasing.Domain.Exceptions;

/// <summary>Base type for a PurchaseOrder-invariant violation. Mirrors
/// Purchasing.Domain.Exceptions.PurchaseRequisitionDomainException.</summary>
public abstract class PurchaseOrderDomainException(string message) : Exception(message);

/// <summary>A purchase order line was added with a quantity or price that is not valid.</summary>
public sealed class InvalidPurchaseOrderLineException(string message) : PurchaseOrderDomainException(message);

/// <summary>A line was added/removed, or the header edited, while the order is not New (draft) — matches
/// brief §27 "Issued/approved PO cannot be arbitrarily edited" (not previously enforced by the
/// generic Update handler; enforcing it now is a deliberate, requested correctness fix, not a wire-format change).</summary>
public sealed class PurchaseOrderNotEditableException(string message) : PurchaseOrderDomainException(message);

/// <summary>Supplier required, or no lines, when creating/confirming an order.</summary>
public sealed class InvalidPurchaseOrderException(string message) : PurchaseOrderDomainException(message);

/// <summary>LinkInvoice() was attempted on an order already linked to an invoice.</summary>
public sealed class PurchaseOrderAlreadyLinkedException(string message) : PurchaseOrderDomainException(message);

/// <summary>Cancel() was attempted on an order that is not New.</summary>
public sealed class PurchaseOrderCannotBeCancelledException(string message) : PurchaseOrderDomainException(message);

/// <summary>RecordReceipt() would receive more than a line's remaining (ordered - received - cancelled) quantity, or was attempted on a Cancelled/Rejected order.</summary>
public sealed class InvalidPurchaseOrderReceiptException(string message) : PurchaseOrderDomainException(message);

/// <summary>RecordReturn() would return more than a line's received-minus-already-returned quantity, or referenced a line that was never received.</summary>
public sealed class InvalidPurchaseOrderReturnException(string message) : PurchaseOrderDomainException(message);

/// <summary>An operation was attempted against an order that is Cancelled/Rejected/Deleted.</summary>
public sealed class PurchaseOrderNotOpenException(string message) : PurchaseOrderDomainException(message);
