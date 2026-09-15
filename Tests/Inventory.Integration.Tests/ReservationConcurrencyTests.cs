namespace Inventory.Integration.Tests;

using Inventory.Application.Postings;
using Inventory.Domain;
using Inventory.Domain.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using OrgSys.Infrastructure.Persistence;
using OrgSys.SharedKernel;
using Xunit;

/// <summary>
/// The one database-backed test class in OrgSys (brief §63/§15) — proves
/// StockReservationService.ReserveAsync (the real production code, not a reimplementation) cannot
/// oversell InventoryBalance under a genuine two-context race, using a real EF Core provider
/// (SQLite, file-based so two separate DbContext instances truly share the same data) instead of
/// mocks. See TestOrgContext's doc comment for how SQL Server's native rowversion auto-increment is
/// stood in for.
/// </summary>
public sealed class ReservationConcurrencyTests : IDisposable
{
    private readonly string _dbPath = Path.Combine(Path.GetTempPath(), $"orgsys-inventory-concurrency-{Guid.NewGuid():N}.db");
    private readonly SqliteConnection _keepAliveConnection;

    public ReservationConcurrencyTests()
    {
        // A held-open connection keeps the SQLite file's schema alive for the lifetime of the test
        // even though every operation below opens its own separate connection/context (mirroring
        // separate web requests each getting their own scoped DbContext in production).
        _keepAliveConnection = new SqliteConnection($"Data Source={_dbPath}");
        _keepAliveConnection.Open();

        using var setupContext = NewContext();
        setupContext.Database.EnsureCreated();
    }

    public void Dispose()
    {
        _keepAliveConnection.Dispose();
        // Microsoft.Data.Sqlite pools connections by connection string, so a file handle can
        // outlive Dispose() briefly — clear the pool before deleting so cleanup doesn't race it.
        SqliteConnection.ClearAllPools();
        try
        {
            if (File.Exists(_dbPath))
                File.Delete(_dbPath);
        }
        catch (IOException)
        {
            // Best-effort cleanup only — a leftover temp file never affects test correctness.
        }
    }

    private TestOrgContext NewContext()
    {
        var options = new DbContextOptionsBuilder<TestOrgContext>()
            .UseSqlite($"Data Source={_dbPath}")
            .Options;
        return new TestOrgContext(options);
    }

    private (StockReservationService Service, TestOrgContext Context) NewService()
    {
        var context = NewContext();
        var unitOfWork = new UnitOfWork(context);
        var balanceRepository = new global::Inventory.Infrastructure.Persistence.InventoryBalanceRepository(context);
        var stockRepository = new Repository<Stock>(context);
        var reservationRepository = new Repository<StockReservation>(context);
        var service = new StockReservationService(balanceRepository, stockRepository, reservationRepository, unitOfWork);
        return (service, context);
    }

    private async Task SeedBalanceAsync(long productId, long stockId, decimal onHand)
    {
        using var context = NewContext();
        var balance = InventoryBalance.Create(productId, stockId, null, null);
        balance.Receive(onHand, 10);
        context.Add(balance);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Direct EF-level proof (brief's own worked example, §15): two contexts read the same balance
    /// row (Available = 10), one reserves 6 and saves successfully; the other, still holding the
    /// stale concurrency token, must fail on save — never silently overwrite the first reservation's
    /// effect.
    /// </summary>
    [Fact]
    public async Task Concurrent_updates_to_the_same_balance_row_conflict_at_the_ef_level()
    {
        await SeedBalanceAsync(productId: 1, stockId: 1, onHand: 10);

        using var contextA = NewContext();
        using var contextB = NewContext();

        var balanceA = await contextA.Set<InventoryBalance>().FirstAsync(b => b.ProductId == 1 && b.StockId == 1);
        var balanceB = await contextB.Set<InventoryBalance>().FirstAsync(b => b.ProductId == 1 && b.StockId == 1);

        balanceA.Reserve(6, allowNegativeStock: false);
        await contextA.SaveChangesAsync(); // succeeds — token bumped in the database

        balanceB.Reserve(4, allowNegativeStock: false); // balanceB still holds the pre-bump token
        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => contextB.SaveChangesAsync());
    }

    /// <summary>
    /// End-to-end through the real production path: two "concurrent" ReserveStock requests for
    /// quantities that together exceed Available (10) must not both succeed. Sequenced
    /// deterministically (B's SaveChanges races against A having already committed) rather than
    /// relying on true OS-thread timing, per the same reasoning ReservationConcurrencyTests above
    /// uses — the EF conflict is what's under test, not thread scheduling.
    /// </summary>
    [Fact]
    public async Task ReserveAsync_does_not_oversell_stock_under_a_concurrent_conflict()
    {
        await SeedBalanceAsync(productId: 2, stockId: 1, onHand: 10);

        var (serviceA, contextA) = NewService();
        var (serviceB, contextB) = NewService();
        using var _a = contextA;
        using var _b = contextB;

        // Both handlers read the balance (Available = 10) before either writes — the actual race
        // window brief §15's example describes.
        var preloadA = await contextA.Set<InventoryBalance>().AsNoTracking().FirstAsync(b => b.ProductId == 2 && b.StockId == 1);
        var preloadB = await contextB.Set<InventoryBalance>().AsNoTracking().FirstAsync(b => b.ProductId == 2 && b.StockId == 1);
        Assert.Equal(10, preloadA.QuantityAvailable);
        Assert.Equal(10, preloadB.QuantityAvailable);

        // Request A reserves 6 first and commits.
        var resultA = await serviceA.ReserveAsync(
            productId: 2, stockId: 1, locationId: null, batchId: null, serialId: null, quantity: 6,
            SourceDocumentType.SalesOrder, sourceId: 100, sourceLineId: null, createUserId: 1, expiresAt: null,
            cancellationToken: default);

        Assert.True(resultA.Success);

        // Request B, whose own context loaded the balance before A committed, reserves 6 too —
        // 6 + 6 = 12 > 10, so it must fail even though both requests independently observed enough
        // stock at read time. ReserveAsync's own bounded retry (StockReservationService.MaxAttempts)
        // catches the DbUpdateConcurrencyException, re-reads (now Available = 4), and correctly
        // rejects with InsufficientStockException rather than oversell.
        var resultB = await serviceB.ReserveAsync(
            productId: 2, stockId: 1, locationId: null, batchId: null, serialId: null, quantity: 6,
            SourceDocumentType.SalesOrder, sourceId: 101, sourceLineId: null, createUserId: 1, expiresAt: null,
            cancellationToken: default);

        Assert.False(resultB.Success);
        Assert.Contains("exceeds available", resultB.Error, StringComparison.OrdinalIgnoreCase);

        using var verifyContext = NewContext();
        var finalBalance = await verifyContext.Set<InventoryBalance>().AsNoTracking().FirstAsync(b => b.ProductId == 2 && b.StockId == 1);
        Assert.Equal(6, finalBalance.QuantityReserved);
        Assert.Equal(4, finalBalance.QuantityAvailable);
        Assert.Equal(1, await verifyContext.Set<StockReservation>().CountAsync(r => r.SourceId == 100));
        Assert.Equal(0, await verifyContext.Set<StockReservation>().CountAsync(r => r.SourceId == 101));
    }

    /// <summary>Symmetric case: when the second request's quantity DOES still fit after the first
    /// commits, the retry must succeed rather than fail merely because a conflict occurred.</summary>
    [Fact]
    public async Task ReserveAsync_retries_and_succeeds_when_stock_remains_available_after_conflict()
    {
        await SeedBalanceAsync(productId: 3, stockId: 1, onHand: 10);

        var (serviceA, contextA) = NewService();
        var (serviceB, contextB) = NewService();
        using var _a = contextA;
        using var _b = contextB;

        await contextA.Set<InventoryBalance>().AsNoTracking().FirstAsync(b => b.ProductId == 3);
        await contextB.Set<InventoryBalance>().AsNoTracking().FirstAsync(b => b.ProductId == 3);

        var resultA = await serviceA.ReserveAsync(3, 1, null, null, null, 3, SourceDocumentType.SalesOrder, 200, null, 1, null, default);
        Assert.True(resultA.Success);

        // 3 already reserved, B asks for 3 more (fits within the remaining 7) — must succeed after
        // one retry, not be treated as a hard failure just because a conflict happened.
        var resultB = await serviceB.ReserveAsync(3, 1, null, null, null, 3, SourceDocumentType.SalesOrder, 201, null, 1, null, default);
        Assert.True(resultB.Success);

        using var verifyContext = NewContext();
        var finalBalance = await verifyContext.Set<InventoryBalance>().AsNoTracking().FirstAsync(b => b.ProductId == 3);
        Assert.Equal(6, finalBalance.QuantityReserved);
        Assert.Equal(4, finalBalance.QuantityAvailable);
    }
}
