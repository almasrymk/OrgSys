namespace Accounting.Contracts.Postings;

using OrgSys.SharedKernel;

/// <summary>
/// Creates a standalone, immediately-Posted Journal for a source document whose own workflow
/// requires real accounting-Posted semantics the moment it exists (Treasury's Financial/
/// FinancialTransfer transactions) — unlike PostAccountingDocumentCommand's resource-controlled
/// bridge (Invoice/Transaction), which upserts a Draft journal that only tracks its source
/// document's own Status over time via SyncStatusFromSourceDocument and never sets Posted.
///
/// Unlike PostAccountingDocumentCommand, this never upserts: a source document may post exactly
/// once through this command. Correcting a mistake goes through
/// ReverseAccountingDocumentJournalCommand, never a second call to this command. Resolves and
/// validates the FiscalYear/FiscalPeriod the same way the manual Journal Create/Post commands do,
/// and rejects an unbalanced entry or one referencing a non-postable account.
/// </summary>
public sealed record PostAccountingEntryCommand(
    string ReferenceTable,
    long SourceDocumentId,
    long SourceDocumentTypeId,
    string? SourceDocumentCode,
    long JournalTypeId,
    DateTime Date,
    DateTime CreateDate,
    long CreateUserId,
    long? BranchId,
    long? ShiftId,
    long CurrencyId,
    decimal Rate,
    string? Note,
    IReadOnlyCollection<AccountingPostingLine> Lines) : ICommand<PostAccountingEntryResult>;

public sealed record PostAccountingEntryResult(long JournalId, string? JournalCode);
