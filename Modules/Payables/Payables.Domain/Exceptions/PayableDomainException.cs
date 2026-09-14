namespace Payables.Domain.Exceptions;

/// <summary>
/// Base type for an AP-invariant violation. The Application layer catches these at the command-
/// handler boundary and translates them into the existing OrgSys.SharedKernel.Result/Error shape —
/// Domain itself has no knowledge of HTTP status codes. Mirrors
/// Receivables.Domain.Exceptions.ReceivableDomainException.
/// </summary>
public abstract class PayableDomainException(string message) : Exception(message);

/// <summary>A payable was created with an original amount that is not greater than zero.</summary>
public sealed class InvalidPayableAmountException(string message) : PayableDomainException(message);

/// <summary>An allocation/unapplication/write-off amount was zero, negative, or exceeded the amount it was measured against.</summary>
public sealed class InvalidAllocationAmountException(string message) : PayableDomainException(message);

/// <summary>A payment application or write-off was attempted on a payable that is Cancelled, Settled, or WrittenOff.</summary>
public sealed class PayableNotOpenForApplicationException(string message) : PayableDomainException(message);

/// <summary>Cancellation was attempted on a payable that already has a payment applied or amount written off, or that is Settled/WrittenOff — unapply/reverse first.</summary>
public sealed class PayableCannotBeCancelledException(string message) : PayableDomainException(message);

/// <summary>A write-off was attempted without a reason.</summary>
public sealed class WriteOffReasonRequiredException(string message) : PayableDomainException(message);
