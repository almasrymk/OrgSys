namespace Payables.Infrastructure.Persistence;

/// <summary>Composed on top of the generic OrgSys.SharedKernel.IRepository&lt;Payable&gt; — mirrors Receivables.Infrastructure.Persistence.ReceivableRepository.</summary>
public sealed class PayableRepository(IRepository<Payable> repository) : IPayableRepository
{
    public async Task<Payable?> GetByIdAsync(long id, CancellationToken cancellationToken = default) =>
        await repository.GetByFilterAsync(p => p.Id == id, "");

    public async Task<Payable?> GetBySourceDocumentAsync(SourceDocumentType sourceDocumentType, long sourceDocumentId, CancellationToken cancellationToken = default) =>
        await repository.GetByFilterAsync(p => p.SourceDocumentType == sourceDocumentType && p.SourceDocumentId == sourceDocumentId, "");

    public async Task<bool> ExistsForSourceDocumentAsync(SourceDocumentType sourceDocumentType, long sourceDocumentId, long supplierId, CancellationToken cancellationToken = default) =>
        await repository.AnyAsync(p => p.SourceDocumentType == sourceDocumentType && p.SourceDocumentId == sourceDocumentId && p.SupplierId == supplierId, cancellationToken);

    public async Task AddAsync(Payable payable, CancellationToken cancellationToken = default) =>
        await repository.CreateAsync(payable);

    public async Task<IReadOnlyList<Payable>> GetOpenBySupplierAsync(long supplierId, CancellationToken cancellationToken = default)
    {
        var payables = await repository.GetListByFilterAsync(
            p => p.SupplierId == supplierId && (p.LifecycleStatus == PayableStatus.Open || p.LifecycleStatus == PayableStatus.PartiallySettled),
            q => q.OrderBy(p => p.DocumentDate).ThenBy(p => p.Id));
        return (payables ?? []).ToList();
    }
}
