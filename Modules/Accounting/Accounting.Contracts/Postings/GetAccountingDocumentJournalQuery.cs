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

/// <summary>
/// Batch form of GetAccountingDocumentJournalQuery, keyed by SourceDocumentId — used by list/search
/// screens showing JournalId/JournalCode for many source documents at once (e.g. CommercialDocuments'
/// Invoice list, Inventory's Transaction list) instead of an IRepository&lt;Journal&gt; reference
/// across the module boundary. Unlike the single-document form this does not filter by
/// SourceDocumentTypeId — a source document's own type does not change, so its RefranceId alone is enough.
/// </summary>
public sealed record GetAccountingDocumentJournalsQuery(string ReferenceTable, IReadOnlyCollection<long> SourceDocumentIds)
    : IQuery<Dictionary<long, AccountingDocumentJournalDto>>;

public sealed record AccountingDocumentJournalDto(long JournalId, string? JournalCode);
