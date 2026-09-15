namespace Inventory.Infrastructure.Persistence;

using Inventory.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

/// <summary>See IInventoryBalanceRepository's doc comment for why this goes straight to IOrgContext
/// instead of composing over the generic IRepository&lt;InventoryBalance&gt; the way every other new
/// repository in this codebase does (e.g. Payables.Infrastructure.Persistence.PayableRepository) —
/// the generic Repository&lt;T&gt;.UpdateAsync's "Find-then-SetValues" idiom defeats optimistic
/// concurrency, and InventoryBalance is the one aggregate that actually needs it (brief §15).</summary>
public sealed class InventoryBalanceRepository(IOrgContext context) : IInventoryBalanceRepository
{
    public Task<InventoryBalance?> GetTrackedAsync(long productId, long stockId, long? locationId, long? batchId, CancellationToken cancellationToken = default) =>
        Query(productId, stockId, locationId, batchId).FirstOrDefaultAsync(cancellationToken);

    public async Task<InventoryBalance> GetOrCreateTrackedAsync(long productId, long stockId, long? locationId, long? batchId, CancellationToken cancellationToken = default)
    {
        var existing = await GetTrackedAsync(productId, stockId, locationId, batchId, cancellationToken);
        if (existing is not null)
            return existing;

        var balance = InventoryBalance.Create(productId, stockId, locationId, batchId);
        await context.Set<InventoryBalance>().AddAsync(balance, cancellationToken);
        return balance;
    }

    public Task<InventoryBalance?> GetReadOnlyAsync(long productId, long stockId, long? locationId, long? batchId, CancellationToken cancellationToken = default) =>
        Query(productId, stockId, locationId, batchId).AsNoTracking().FirstOrDefaultAsync(cancellationToken);

    private IQueryable<InventoryBalance> Query(long productId, long stockId, long? locationId, long? batchId) =>
        context.Set<InventoryBalance>().Where(b =>
            b.ProductId == productId && b.StockId == stockId && b.LocationId == locationId && b.BatchId == batchId);
}
