namespace Inventory.Domain.Repositories;

/// <summary>
/// The only repository in Inventory that must hand back a tracked (not detached) entity — every
/// other repository in this codebase uses the generic IRepository&lt;T&gt;'s "load, mutate a
/// detached copy, UpdateAsync" idiom, but that idiom re-reads the row inside UpdateAsync
/// (Repository&lt;T&gt;.UpdateAsync calls dbEntity.Find(Ob.Id) then overwrites CurrentValues),
/// which resets EF's OriginalValues right before the write and silently defeats optimistic
/// concurrency. InventoryBalance is the one place in OrgSys that actually needs the concurrency
/// token to work (brief §15/§63), so its repository instead returns an entity that stays attached
/// to the same IOrgContext/DbContext for the life of the request — the caller mutates it in place
/// (Receive/IssueOut/Reserve/...) and a plain IUnitOfWork.SaveChangeAsync() lets EF's native
/// concurrency check run against the RowVersion actually read at the start of the request, not one
/// re-fetched immediately before the write. See docs/ddd/inventory-target-architecture.md §8.
/// </summary>
public interface IInventoryBalanceRepository
{
    Task<InventoryBalance?> GetTrackedAsync(long productId, long stockId, long? locationId, long? batchId, CancellationToken cancellationToken = default);

    /// <summary>Returns the existing tracked balance row, or creates (and stages for insert) a new
    /// zeroed one if this Product+Stock(+Location)(+Batch) combination has never been touched.</summary>
    Task<InventoryBalance> GetOrCreateTrackedAsync(long productId, long stockId, long? locationId, long? batchId, CancellationToken cancellationToken = default);

    /// <summary>No-tracking read for queries (availability checks that don't intend to write,
    /// reporting) — never mutate the result of this call.</summary>
    Task<InventoryBalance?> GetReadOnlyAsync(long productId, long stockId, long? locationId, long? batchId, CancellationToken cancellationToken = default);
}
