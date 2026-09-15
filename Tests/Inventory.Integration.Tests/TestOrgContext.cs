namespace Inventory.Integration.Tests;

using Inventory.Domain;
using Microsoft.EntityFrameworkCore;
using OrgSys.SharedKernel;

/// <summary>
/// A minimal, SQLite-backed IOrgContext — maps only InventoryBalance/StockReservation/Stock (with
/// their cross-module navigations ignored) instead of the full ~100-entity production OrgContext,
/// so this test project needs no SQL-Server-only artifacts (rowversion binary type isn't native to
/// SQLite — mapped instead as a plain INTEGER counter, see InventoryBalance's RowVersion config
/// below) and no cross-module project references. ResetDbContextState mirrors
/// OrgContext.ResetDbContextState exactly (revert Modified entries to OriginalValues -> Unchanged,
/// so a subsequent tracked re-query refreshes them from the database) — the actual behavior
/// StockReservationService's retry loop depends on.
/// </summary>
public sealed class TestOrgContext(DbContextOptions<TestOrgContext> options) : DbContext(options), IOrgContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<InventoryBalance>(b =>
        {
            b.Ignore(e => e.Product);
            b.Ignore(e => e.Stock);
            b.Ignore(e => e.Location);
            b.Ignore(e => e.Batch);
            b.Ignore(e => e.DomainEvents);
            // SQLite has no native rowversion type — EF's SQLite provider maps a [Timestamp] byte[]
            // to a BLOB it does not auto-increment. Using a plain concurrency-token int/long instead
            // proves the exact same mechanism (EF compares OriginalValues vs. the DB on UPDATE and
            // throws DbUpdateConcurrencyException on mismatch) without relying on SQL Server-only
            // auto-generation. Production (SQL Server) keeps its real rowversion; this is a test-only
            // substitute for the same *concurrency token* concept.
            b.Property<int>("ConcurrencyToken").IsConcurrencyToken();
            b.Ignore(e => e.RowVersion);
        });

        modelBuilder.Entity<StockReservation>(b =>
        {
            b.Ignore(e => e.Product);
            b.Ignore(e => e.Stock);
            b.Ignore(e => e.Location);
            b.Ignore(e => e.Batch);
            b.Ignore(e => e.Serial);
            b.Ignore(e => e.DomainEvents);
        });

        modelBuilder.Entity<Stock>(b =>
        {
            b.Ignore(e => e.Branch);
        });
    }

    public void ResetDbContextState()
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.State = EntityState.Detached;
                    break;
                case EntityState.Modified:
                    entry.CurrentValues.SetValues(entry.OriginalValues);
                    entry.State = EntityState.Unchanged;
                    break;
                case EntityState.Deleted:
                    entry.State = EntityState.Unchanged;
                    break;
            }
        }
    }

    public Task BeginTransactionAsync() => Database.BeginTransactionAsync();

    public Task CommitAsync() => Database.CommitTransactionAsync();

    public Task RollbackAsync() => Database.RollbackTransactionAsync();

    /// <summary>
    /// SQLite has no store-generated auto-incrementing rowversion, so this test stands in for what
    /// SQL Server's `rowversion` column does automatically on every UPDATE: bump the shadow
    /// "ConcurrencyToken" int for every Modified InventoryBalance right before the actual SQL runs.
    /// EF still does the real concurrency check (comparing this context's OriginalValue for that
    /// property against whatever value is actually in the database) — only the "how does the token
    /// change on every save" part is emulated; the detection mechanism itself is EF's own.
    /// </summary>
    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        BumpConcurrencyTokens();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        BumpConcurrencyTokens();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void BumpConcurrencyTokens()
    {
        foreach (var entry in ChangeTracker.Entries<InventoryBalance>())
        {
            if (entry.State == EntityState.Modified || entry.State == EntityState.Added)
            {
                var current = (int)entry.Property("ConcurrencyToken").CurrentValue!;
                entry.Property("ConcurrencyToken").CurrentValue = current + 1;
            }
        }
    }
}
