namespace Receivables.Infrastructure.Persistence;

/// <summary>Composed on top of the generic OrgSys.SharedKernel.IRepository&lt;Receivable&gt; — mirrors Accounting.Infrastructure.Persistence.JournalRepository.</summary>
public sealed class ReceivableRepository(IRepository<Receivable> repository) : IReceivableRepository
{
    public async Task<Receivable?> GetByIdAsync(long id, CancellationToken cancellationToken = default) =>
        await repository.GetByFilterAsync(r => r.Id == id, "");

    public async Task<Receivable?> GetBySourceDocumentAsync(SourceDocumentType sourceDocumentType, long sourceDocumentId, CancellationToken cancellationToken = default) =>
        await repository.GetByFilterAsync(r => r.SourceDocumentType == sourceDocumentType && r.SourceDocumentId == sourceDocumentId, "");

    public async Task<bool> ExistsForSourceDocumentAsync(SourceDocumentType sourceDocumentType, long sourceDocumentId, long customerId, CancellationToken cancellationToken = default) =>
        await repository.AnyAsync(r => r.SourceDocumentType == sourceDocumentType && r.SourceDocumentId == sourceDocumentId && r.CustomerId == customerId, cancellationToken);

    public async Task AddAsync(Receivable receivable, CancellationToken cancellationToken = default) =>
        await repository.CreateAsync(receivable);

    public async Task<IReadOnlyList<Receivable>> GetOpenByCustomerAsync(long customerId, CancellationToken cancellationToken = default)
    {
        var receivables = await repository.GetListByFilterAsync(
            r => r.CustomerId == customerId && (r.LifecycleStatus == ReceivableStatus.Open || r.LifecycleStatus == ReceivableStatus.PartiallySettled),
            q => q.OrderBy(r => r.DocumentDate).ThenBy(r => r.Id));
        return (receivables ?? []).ToList();
    }
}
