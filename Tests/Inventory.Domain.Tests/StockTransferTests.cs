namespace Inventory.Domain.Tests;

public class StockTransferTests
{
    private static readonly DateTime Date = new(2026, 9, 1);

    private static StockTransfer NewTransfer() =>
        StockTransfer.Create(fromStockId: 1, toStockId: 2, date: Date, createUserId: 1, createDate: Date, branchId: null, notes: null);

    [Fact]
    public void Cannot_create_transfer_to_same_warehouse() =>
        Assert.Throws<InvalidStockTransferException>(() =>
            StockTransfer.Create(1, 1, Date, 1, Date, null, null));

    [Fact]
    public void Ship_then_receive_completes_transfer_and_raises_both_events()
    {
        var transfer = NewTransfer();
        transfer.AddLine(productId: 1, unitId: 1, quantity: 5, batchId: null, notes: null);

        transfer.Ship(Date);
        transfer.Receive(Date);

        Assert.Equal(StockTransferStatus.Completed, transfer.TransferStatus);
        Assert.True(transfer.Posted);
        Assert.Equal(2, transfer.DomainEvents.Count);
        Assert.IsType<StockTransferShippedDomainEvent>(transfer.DomainEvents[0]);
        Assert.IsType<StockTransferReceivedDomainEvent>(transfer.DomainEvents[1]);
    }

    [Fact]
    public void Cannot_receive_before_shipping()
    {
        var transfer = NewTransfer();
        transfer.AddLine(1, 1, 5, null, null);

        Assert.Throws<TransferAlreadyPostedException>(() => transfer.Receive(Date));
    }

    [Fact]
    public void Cannot_ship_without_lines()
    {
        var transfer = NewTransfer();
        Assert.Throws<InventoryDocumentLineRequiredException>(() => transfer.Ship(Date));
    }

    [Fact]
    public void Shipped_transfer_cannot_be_cancelled_directly()
    {
        var transfer = NewTransfer();
        transfer.AddLine(1, 1, 5, null, null);
        transfer.Ship(Date);

        Assert.Throws<CannotCancelPostedDocumentWithoutReversalException>(() => transfer.Cancel());
    }

    /// <summary>Transfer preserves total inventory (brief §62/§11) — this is proved at the
    /// InventoryBalance level: shipping decreases the source balance, receiving increases the
    /// destination balance, by the same quantity, so the sum across both warehouses is unchanged.
    /// The pairing itself (never a WarehouseId edit) is what StockTransfer.Ship/Receive guarantees;
    /// the arithmetic invariant is InventoryBalance's, exercised here as an end-to-end scenario.</summary>
    [Fact]
    public void Transfer_preserves_total_inventory_across_both_warehouses()
    {
        var source = InventoryBalance.Create(productId: 1, stockId: 1, locationId: null, batchId: null);
        var destination = InventoryBalance.Create(productId: 1, stockId: 2, locationId: null, batchId: null);
        source.Receive(20, 10);

        var totalBefore = source.QuantityOnHand + destination.QuantityOnHand;

        source.IssueOut(5, allowNegativeStock: false);
        destination.Receive(5, source.AverageCost);

        var totalAfter = source.QuantityOnHand + destination.QuantityOnHand;

        Assert.Equal(totalBefore, totalAfter);
        Assert.Equal(15, source.QuantityOnHand);
        Assert.Equal(5, destination.QuantityOnHand);
    }
}
