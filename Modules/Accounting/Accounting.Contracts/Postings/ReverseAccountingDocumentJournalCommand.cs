namespace Accounting.Contracts.Postings;

using OrgSys.SharedKernel;

/// <summary>
/// Reverses the Posted journal owned by a source document (Treasury's Financial/FinancialTransfer
/// Reverse handlers — see the Accounting DDD cleanup report). Books a proper accounting reversal
/// (Journal.CreateReversal): the original is never mutated, a new Posted journal is created with
/// every line's Debit/Credit swapped, and the two are linked. Mirrors
/// Accounting.Application.Journals.Commands.ReverseJournalCommand for the direct-Journal case;
/// this is the resource-controlled equivalent — the caller does not need to know the original
/// journal's Id, only which source document it belongs to.
/// </summary>
public sealed record ReverseAccountingDocumentJournalCommand(string ReferenceTable, long SourceDocumentId) : ICommand<ReverseAccountingDocumentJournalResult>;

public sealed record ReverseAccountingDocumentJournalResult(long OriginalJournalId, long ReversalJournalId);
