namespace Inventory.Domain.Tests;

public class InventoryReceiptTests
{
    private static readonly DateTime Date = new(2026, 9, 1);

    private static InventoryReceipt NewReceipt() =>
        InventoryReceipt.Create(stockId: 1, locationId: null, dealerId: null, date: Date, createUserId: 1, createDate: Date, branchId: null, notes: null);

    [Fact]
    public void Cannot_create_receipt_without_warehouse() =>
        Assert.Throws<InventoryDocumentLineRequiredException>(() =>
            InventoryReceipt.Create(0, null, null, Date, 1, Date, null, null));

    [Fact]
    public void Cannot_post_receipt_without_lines()
    {
        var receipt = NewReceipt();
        Assert.Throws<InventoryDocumentLineRequiredException>(() => receipt.Post(Date));
    }

    [Fact]
    public void Cannot_add_line_with_zero_quantity()
    {
        var receipt = NewReceipt();
        Assert.Throws<InventoryDocumentLineRequiredException>(() => receipt.AddLine(1, 1, 0, 10, null, null));
    }

    [Fact]
    public void Posting_raises_one_event_per_line()
    {
        var receipt = NewReceipt();
        receipt.AddLine(productId: 1, unitId: 1, quantity: 10, unitCost: 5, batchId: null, notes: null);
        receipt.AddLine(productId: 2, unitId: 1, quantity: 4, unitCost: 8, batchId: null, notes: null);

        receipt.Post(Date);

        Assert.Equal(DocumentStatus.Posted, receipt.LifecycleStatus);
        Assert.True(receipt.Posted);
        Assert.Equal(2, receipt.DomainEvents.Count);
        Assert.All(receipt.DomainEvents, e => Assert.IsType<StockReceivedDomainEvent>(e));
    }

    // ----- Posted document cannot be edited -----

    [Fact]
    public void Cannot_add_line_after_posting()
    {
        var receipt = NewReceipt();
        receipt.AddLine(1, 1, 10, 5, null, null);
        receipt.Post(Date);

        Assert.Throws<CannotModifyPostedDocumentException>(() => receipt.AddLine(2, 1, 1, 1, null, null));
    }

    [Fact]
    public void Cannot_post_already_posted_receipt()
    {
        var receipt = NewReceipt();
        receipt.AddLine(1, 1, 10, 5, null, null);
        receipt.Post(Date);

        Assert.Throws<CannotModifyPostedDocumentException>(() => receipt.Post(Date));
    }

    [Fact]
    public void Cannot_cancel_posted_receipt_directly()
    {
        var receipt = NewReceipt();
        receipt.AddLine(1, 1, 10, 5, null, null);
        receipt.Post(Date);

        Assert.Throws<CannotCancelPostedDocumentWithoutReversalException>(() => receipt.Cancel());
    }

    [Fact]
    public void Draft_receipt_can_be_cancelled()
    {
        var receipt = NewReceipt();
        receipt.AddLine(1, 1, 10, 5, null, null);

        receipt.Cancel();

        Assert.Equal(DocumentStatus.Cancelled, receipt.LifecycleStatus);
    }
}
