namespace Accounting.Domain.Repositories;

/// <summary>
/// Aggregate-shaped persistence for Journal (the JournalItem lines travel with it — there is no
/// IJournalItemRepository; lines are never persisted independently of their Journal). Implemented
/// in Accounting.Infrastructure. The generic OrgSys.SharedKernel.IRepository&lt;T&gt; remains
/// available for read-only projections (Get/Search query handlers) — this interface exists for the
/// write side, where the aggregate's invariants must be the only path to mutation.
/// </summary>
public interface IJournalRepository
{
    /// <summary>Loads a Journal by id, always including its lines — every write-side use case needs them.</summary>
    Task<Journal?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    /// <summary>Resolves the single journal a source document (Invoice, inventory Transaction, ...) currently owns, if any — the Accounting.Contracts.Postings upsert key.</summary>
    Task<Journal?> GetBySourceDocumentAsync(string referenceTable, long sourceDocumentId, long sourceDocumentTypeId, CancellationToken cancellationToken = default);

    /// <summary>All journals posted against a source document, regardless of type — used by delete/status-sync, which (like the bridges they replace) do not filter by type.</summary>
    Task<IReadOnlyList<Journal>> GetAllBySourceDocumentAsync(string referenceTable, long sourceDocumentId, CancellationToken cancellationToken = default);

    /// <summary>Next sequential Code/CodeNumber for a given JournalTypeId — the same numbering scheme Create/Reverse always used.</summary>
    Task<long> GetNextCodeNumberAsync(long journalTypeId, CancellationToken cancellationToken = default);

    Task AddAsync(Journal journal, CancellationToken cancellationToken = default);

    /// <summary>Removes a journal and its lines outright — only ever legal for a Draft (never a Posted) journal; the caller enforces that via the aggregate before calling this.</summary>
    Task RemoveAsync(Journal journal, CancellationToken cancellationToken = default);
}
