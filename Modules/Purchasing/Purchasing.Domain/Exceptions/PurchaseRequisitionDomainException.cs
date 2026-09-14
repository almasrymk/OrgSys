namespace Purchasing.Domain.Exceptions;

/// <summary>Base type for a PurchaseRequisition-invariant violation. Application catches these at
/// the command-handler boundary and translates them into the existing OrgSys.SharedKernel.Result/
/// Error shape — Domain itself has no knowledge of HTTP status codes. Mirrors
/// Advances.Domain.Exceptions.CustodyDomainException / Sales.Domain.Exceptions.QuotationDomainException.</summary>
public abstract class PurchaseRequisitionDomainException(string message) : Exception(message);

/// <summary>A requisition line was added with a quantity that is not valid.</summary>
public sealed class InvalidPurchaseRequisitionLineException(string message) : PurchaseRequisitionDomainException(message);

/// <summary>A line was added/removed while the requisition is not New (draft).</summary>
public sealed class PurchaseRequisitionNotEditableException(string message) : PurchaseRequisitionDomainException(message);

/// <summary>Submit() was attempted on an empty requisition, or on a requisition that is not New.</summary>
public sealed class PurchaseRequisitionNotSubmittableException(string message) : PurchaseRequisitionDomainException(message);

/// <summary>Reject() was attempted on a requisition that is not UnderReview.</summary>
public sealed class PurchaseRequisitionNotOpenException(string message) : PurchaseRequisitionDomainException(message);

/// <summary>Cancel() was attempted on a requisition already Approved/Rejected.</summary>
public sealed class PurchaseRequisitionCannotBeCancelledException(string message) : PurchaseRequisitionDomainException(message);

/// <summary>RecordSourced() was attempted on a requisition that is not UnderReview, or has already been sourced (brief "cannot convert twice").</summary>
public sealed class PurchaseRequisitionCannotBeSourcedException(string message) : PurchaseRequisitionDomainException(message);
