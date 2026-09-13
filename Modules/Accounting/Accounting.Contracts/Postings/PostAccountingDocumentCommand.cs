namespace Accounting.Contracts.Postings;

using OrgSys.SharedKernel;

/// <summary>
/// Source-neutral "post this document's accounting entry" contract (Invoice -> Journal,
/// Transaction -> Journal, ...). The source module determines posting intent (which accounts,
/// what amounts — its own business rules) and sends this; Accounting.Application owns Journal
/// creation/update itself and is never reached through Accounting.Domain directly. Replaces the
/// legacy root Application project's InvoiceJournalIntegration/TransactionJournalIntegration —
/// see the legacy-Application-elimination report.
///
/// Upserts the Journal keyed by (ReferenceTable, SourceDocumentId, SourceDocumentTypeId), matching
/// the one-journal-per-source-document behavior of the bridges this replaces.
/// </summary>
public sealed record PostAccountingDocumentCommand(
    string ReferenceTable,
    long SourceDocumentId,
    long SourceDocumentTypeId,
    string? SourceDocumentCode,
    long JournalTypeId,
    DateTime Date,
    DateTime CreateDate,
    long CreateUserId,
    DateTime? ModifyDate,
    long? ModifyUserId,
    long? BranchId,
    long? ShiftId,
    long CurrencyId,
    decimal Rate,
    string? Note,
    IReadOnlyCollection<AccountingPostingLine> Lines) : ICommand<PostAccountingDocumentResult>;

public sealed record PostAccountingDocumentResult(bool HasJournal);
