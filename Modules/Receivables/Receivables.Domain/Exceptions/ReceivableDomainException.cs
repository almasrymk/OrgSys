namespace Receivables.Domain.Exceptions;

/// <summary>
/// Base type for an AR-invariant violation. The Application layer catches these at the command-
/// handler boundary and translates them into the existing OrgSys.SharedKernel.Result/Error shape —
/// Domain itself has no knowledge of HTTP status codes. Mirrors Accounting.Domain.Exceptions.
/// AccountingDomainException.
/// </summary>
public abstract class ReceivableDomainException(string message) : Exception(message);

/// <summary>A receivable was created with an original amount that is not greater than zero.</summary>
public sealed class InvalidReceivableAmountException(string message) : ReceivableDomainException(message);

/// <summary>An allocation/unapplication/write-off amount was zero, negative, or exceeded the amount it was measured against.</summary>
public sealed class InvalidAllocationAmountException(string message) : ReceivableDomainException(message);

/// <summary>A payment application or write-off was attempted on a receivable that is Cancelled, Settled, or WrittenOff.</summary>
public sealed class ReceivableNotOpenForApplicationException(string message) : ReceivableDomainException(message);

/// <summary>Cancellation was attempted on a receivable that already has a payment applied or amount written off, or that is Settled/WrittenOff — unapply/reverse first.</summary>
public sealed class ReceivableCannotBeCancelledException(string message) : ReceivableDomainException(message);

/// <summary>A write-off was attempted without a reason.</summary>
public sealed class WriteOffReasonRequiredException(string message) : ReceivableDomainException(message);
