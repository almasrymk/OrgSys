namespace Sales.Domain.Exceptions;

/// <summary>Base type for a Quotation-invariant violation. Application catches these at the
/// command-handler boundary and translates them into the existing OrgSys.SharedKernel.Result/Error
/// shape — Domain itself has no knowledge of HTTP status codes. Mirrors
/// Advances.Domain.Exceptions.CustodyDomainException.</summary>
public abstract class QuotationDomainException(string message) : Exception(message);

/// <summary>A quotation line was added with a quantity or unit price that is not valid.</summary>
public sealed class InvalidQuotationLineException(string message) : QuotationDomainException(message);

/// <summary>A line was added/removed, or the quotation sent, while it is not Draft.</summary>
public sealed class QuotationNotEditableException(string message) : QuotationDomainException(message);

/// <summary>Send() was attempted on an empty quotation, or on a quotation that is not Draft.</summary>
public sealed class QuotationNotSendableException(string message) : QuotationDomainException(message);

/// <summary>Accept()/Reject()/Expire() was attempted on a quotation that is not Sent.</summary>
public sealed class QuotationNotOpenException(string message) : QuotationDomainException(message);

/// <summary>Expire() was attempted before the quotation's ValidUntil date has actually passed.</summary>
public sealed class QuotationNotYetExpiredException(string message) : QuotationDomainException(message);

/// <summary>Cancel() was attempted on a quotation already Accepted/Converted/Rejected/Expired.</summary>
public sealed class QuotationCannotBeCancelledException(string message) : QuotationDomainException(message);

/// <summary>MarkConverted() was attempted on a quotation that is not Accepted, or that has already been converted (brief §66 "cannot convert twice").</summary>
public sealed class QuotationCannotBeConvertedException(string message) : QuotationDomainException(message);
