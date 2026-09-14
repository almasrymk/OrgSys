namespace Advances.Domain.Exceptions;

/// <summary>
/// Base type for a Custody-invariant violation. The Application layer catches these at the command-
/// handler boundary and translates them into the existing OrgSys.SharedKernel.Result/Error shape —
/// Domain itself has no knowledge of HTTP status codes. Mirrors Payables.Domain.Exceptions.PayableDomainException.
/// </summary>
public abstract class CustodyDomainException(string message) : Exception(message);

/// <summary>A custody was created (or issued) with an amount that is not greater than zero, or an invalid currency rate.</summary>
public sealed class InvalidCustodyAmountException(string message) : CustodyDomainException(message);

/// <summary>A custody was created without a purpose.</summary>
public sealed class CustodyPurposeRequiredException(string message) : CustodyDomainException(message);

/// <summary>Approve() was attempted on a custody that is not Draft.</summary>
public sealed class CustodyNotApprovableException(string message) : CustodyDomainException(message);

/// <summary>MarkIssued() was attempted on a custody that is not Approved (covers "cannot issue twice" — once Issued, the status is no longer Approved).</summary>
public sealed class CustodyNotIssuableException(string message) : CustodyDomainException(message);

/// <summary>A settlement or return amount was zero, negative, or exceeded the outstanding amount it was measured against.</summary>
public sealed class InvalidCustodySettlementAmountException(string message) : CustodyDomainException(message);

/// <summary>Settle()/Return() was attempted on a custody that is not Issued or PartiallySettled.</summary>
public sealed class CustodyNotOpenForSettlementException(string message) : CustodyDomainException(message);

/// <summary>Cancel() was attempted on a custody that is not Draft — an Approved/Issued custody has (or is about to have) a real money movement and must be settled/returned instead.</summary>
public sealed class CustodyCannotBeCancelledException(string message) : CustodyDomainException(message);

/// <summary>Close() was attempted on a custody that is not fully Settled (zero outstanding).</summary>
public sealed class CustodyCannotBeClosedException(string message) : CustodyDomainException(message);

/// <summary>TransferHolder() was attempted with an invalid target holder, on a custody not open for transfer, or (brief §40) as an unsupported partial transfer.</summary>
public sealed class InvalidCustodyTransferException(string message) : CustodyDomainException(message);
