namespace Accounting.Contracts.Postings;

using OrgSys.SharedKernel;

/// <summary>
/// Resolves whether a source document already has a posted Journal — used by the source module to
/// decide posting intent (e.g. "only auto-post if a journal already exists or auto-create is on"),
/// the same decision InvoiceJournalIntegration/TransactionJournalIntegration used to make by
/// querying Accounting.Domain.Journal directly. Returns null when no journal is posted.
/// </summary>
public sealed record GetAccountingDocumentJournalQuery(string ReferenceTable, long SourceDocumentId, long SourceDocumentTypeId)
    : IQuery<AccountingDocumentJournalDto?>;

public sealed record AccountingDocumentJournalDto(long JournalId);
