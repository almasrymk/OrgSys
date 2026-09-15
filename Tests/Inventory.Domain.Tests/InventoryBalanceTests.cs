namespace Inventory.Domain.Tests;

public class InventoryBalanceTests
{
    private static InventoryBalance NewBalance() => InventoryBalance.Create(productId: 1, stockId: 1, locationId: null, batchId: null);

    // ----- Receipt increases stock -----

    [Fact]
    public void Receive_increases_quantity_on_hand()
    {
        var balance = NewBalance();

        balance.Receive(10, 5);

        Assert.Equal(10, balance.QuantityOnHand);
    }

    [Fact]
    public void Cannot_receive_zero_or_negative_quantity()
    {
        var balance = NewBalance();
        Assert.Throws<InsufficientStockException>(() => balance.Receive(0, 5));
        Assert.Throws<InsufficientStockException>(() => balance.Receive(-1, 5));
    }

    // ----- Weighted average cost -----

    [Fact]
    public void Weighted_average_cost_recomputed_on_receive()
    {
        var balance = NewBalance();
        balance.Receive(100, 10); // 100 @ 10

        balance.Receive(50, 16); // 50 @ 16 -> ((100*10)+(50*16))/150 = 12

        Assert.Equal(150, balance.QuantityOnHand);
        Assert.Equal(12m, balance.AverageCost);
    }

    [Fact]
    public void Issue_does_not_change_average_cost()
    {
        var balance = NewBalance();
        balance.Receive(100, 10);

        balance.IssueOut(40, allowNegativeStock: false);

        Assert.Equal(10, balance.AverageCost);
        Assert.Equal(60, balance.QuantityOnHand);
    }

    // ----- Issue decreases stock / cannot issue unavailable stock -----

    [Fact]
    public void Issue_decreases_quantity_on_hand()
    {
        var balance = NewBalance();
        balance.Receive(10, 5);

        balance.IssueOut(4, allowNegativeStock: false);

        Assert.Equal(6, balance.QuantityOnHand);
    }

    [Fact]
    public void Cannot_issue_more_than_available()
    {
        var balance = NewBalance();
        balance.Receive(10, 5);

        Assert.Throws<InsufficientStockException>(() => balance.IssueOut(11, allowNegativeStock: false));
    }

    [Fact]
    public void Negative_stock_allowed_when_warehouse_configured_for_it()
    {
        var balance = NewBalance();
        balance.Receive(10, 5);

        balance.IssueOut(15, allowNegativeStock: true);

        Assert.Equal(-5, balance.QuantityOnHand);
    }

    [Fact]
    public void Issue_respects_reserved_quantity_even_when_on_hand_is_sufficient()
    {
        var balance = NewBalance();
        balance.Receive(10, 5);
        balance.Reserve(6, allowNegativeStock: false); // Available = 4

        Assert.Throws<InsufficientStockException>(() => balance.IssueOut(5, allowNegativeStock: false));
    }

    // ----- Reservation reduces Available, not OnHand -----

    [Fact]
    public void Reserve_reduces_available_but_not_on_hand()
    {
        var balance = NewBalance();
        balance.Receive(100, 10);

        balance.Reserve(30, allowNegativeStock: false);

        Assert.Equal(100, balance.QuantityOnHand);
        Assert.Equal(30, balance.QuantityReserved);
        Assert.Equal(70, balance.QuantityAvailable);
    }

    [Fact]
    public void Cannot_reserve_more_than_available()
    {
        var balance = NewBalance();
        balance.Receive(10, 5);

        Assert.Throws<InsufficientStockException>(() => balance.Reserve(11, allowNegativeStock: false));
    }

    // ----- Release restores Available -----

    [Fact]
    public void Release_reservation_restores_available()
    {
        var balance = NewBalance();
        balance.Receive(100, 10);
        balance.Reserve(30, allowNegativeStock: false);

        balance.ReleaseReservation(30);

        Assert.Equal(100, balance.QuantityOnHand);
        Assert.Equal(0, balance.QuantityReserved);
        Assert.Equal(100, balance.QuantityAvailable);
    }

    [Fact]
    public void Cannot_release_more_than_reserved() =>
        Assert.Throws<InsufficientStockException>(() => NewBalance().ReleaseReservation(1));

    // ----- Fulfilled reservation consumes stock correctly -----

    [Fact]
    public void Fulfill_reservation_reduces_both_on_hand_and_reserved()
    {
        var balance = NewBalance();
        balance.Receive(100, 10);
        balance.Reserve(30, allowNegativeStock: false);

        balance.FulfillReservation(30);

        Assert.Equal(70, balance.QuantityOnHand);
        Assert.Equal(0, balance.QuantityReserved);
        Assert.Equal(70, balance.QuantityAvailable);
    }

    [Fact]
    public void Cannot_fulfill_more_than_reserved()
    {
        var balance = NewBalance();
        balance.Receive(100, 10);
        balance.Reserve(10, allowNegativeStock: false);

        Assert.Throws<InsufficientStockException>(() => balance.FulfillReservation(11));
    }
}
