namespace Accounting.Domain.Exceptions;

/// <summary>
/// Base type for a General Ledger accounting-invariant violation. The Application layer catches
/// these at the command-handler boundary and translates them into the existing
/// OrgSys.SharedKernel.Result/Error shape — Domain itself has no knowledge of HTTP status codes.
/// </summary>
public abstract class AccountingDomainException(string message) : Exception(message);

/// <summary>A Posted journal was targeted by an operation reserved for Draft entries (edit, delete, cancel, direct status change).</summary>
public sealed class JournalAlreadyPostedException(string message) : AccountingDomainException(message);

/// <summary>Total Debit did not equal total Credit, or the journal has no valid (non-zero) lines — double-entry is violated.</summary>
public sealed class JournalNotBalancedException(string message) : AccountingDomainException(message);

/// <summary>A journal created and owned by another module (RefranceTable is set) was targeted directly instead of through that module.</summary>
public sealed class JournalControlledByResourceException(string message) : AccountingDomainException(message);

/// <summary>Reversal was attempted on a journal that is not Posted, has no lines, or has already been reversed.</summary>
public sealed class JournalCannotBeReversedException(string message) : AccountingDomainException(message);

/// <summary>Cancellation was attempted on a journal that is already Posted — use reversal instead.</summary>
public sealed class JournalCannotBeCancelledException(string message) : AccountingDomainException(message);

/// <summary>Redo (un-cancel) was attempted on a journal that is Posted or Reversed rather than Cancelled.</summary>
public sealed class JournalCannotBeRedoneException(string message) : AccountingDomainException(message);

/// <summary>The fiscal year or fiscal period a journal date resolves to is closed/locked — posting is not allowed.</summary>
public sealed class AccountingPeriodClosedException(string message) : AccountingDomainException(message);

/// <summary>A journal line referenced a group/parent or inactive account, which cannot receive postings.</summary>
public sealed class AccountNotPostableException(string message) : AccountingDomainException(message);
