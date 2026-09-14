namespace Receivables.Domain.Repositories;

/// <summary>
/// Aggregate-shaped Receivable persistence — methods reflect actual use cases (idempotent creation
/// from a source document, FIFO payment application, lookup by id) rather than a generic CRUD
/// surface. Mirrors Accounting.Domain.Repositories.IJournalRepository's own doc comment: composed
/// in Infrastructure on top of the existing generic OrgSys.SharedKernel.IRepository&lt;Receivable&gt;
/// rather than a second EF abstraction.
/// </summary>
public interface IReceivableRepository
{
    Task<Receivable?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<Receivable?> GetBySourceDocumentAsync(SourceDocumentType sourceDocumentType, long sourceDocumentId, CancellationToken cancellationToken = default);

    /// <summary>Scoped by CustomerId too (not just SourceDocumentType+SourceDocumentId) because an
    /// OpeningBalance's SourceDocumentId is a FiscalYearId — shared across every customer's opening
    /// balance in that year — unlike a SalesInvoice's SourceDocumentId, which is already globally
    /// unique on its own.</summary>
    Task<bool> ExistsForSourceDocumentAsync(SourceDocumentType sourceDocumentType, long sourceDocumentId, long customerId, CancellationToken cancellationToken = default);

    Task AddAsync(Receivable receivable, CancellationToken cancellationToken = default);

    /// <summary>Open items (Open/PartiallySettled) for one customer, oldest DocumentDate first — the
    /// FIFO application order Receivables.Application.Payments uses (same policy
    /// GetCustomerAgingQueryHandler's GL-derived FIFO matching already applies).</summary>
    Task<IReadOnlyList<Receivable>> GetOpenByCustomerAsync(long customerId, CancellationToken cancellationToken = default);
}
