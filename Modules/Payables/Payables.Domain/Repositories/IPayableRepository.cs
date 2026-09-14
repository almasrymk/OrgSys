namespace Payables.Domain.Repositories;

/// <summary>
/// Aggregate-shaped Payable persistence — methods reflect actual use cases (idempotent creation
/// from a source document, FIFO payment application, lookup by id) rather than a generic CRUD
/// surface. Mirrors Receivables.Domain.Repositories.IReceivableRepository.
/// </summary>
public interface IPayableRepository
{
    Task<Payable?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<Payable?> GetBySourceDocumentAsync(SourceDocumentType sourceDocumentType, long sourceDocumentId, CancellationToken cancellationToken = default);

    /// <summary>Scoped by SupplierId too (not just SourceDocumentType+SourceDocumentId) because an
    /// OpeningBalance's SourceDocumentId is a FiscalYearId — shared across every supplier's opening
    /// balance in that year — unlike a PurchaseInvoice's SourceDocumentId, which is already globally
    /// unique on its own.</summary>
    Task<bool> ExistsForSourceDocumentAsync(SourceDocumentType sourceDocumentType, long sourceDocumentId, long supplierId, CancellationToken cancellationToken = default);

    Task AddAsync(Payable payable, CancellationToken cancellationToken = default);

    /// <summary>Open items (Open/PartiallySettled) for one supplier, oldest DocumentDate first — the
    /// FIFO application order Payables.Application.Payments uses.</summary>
    Task<IReadOnlyList<Payable>> GetOpenBySupplierAsync(long supplierId, CancellationToken cancellationToken = default);
}
