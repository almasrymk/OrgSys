namespace Inventory.Domain.Tests;

public class StockAdjustmentTests
{
    private static readonly DateTime Date = new(2026, 9, 1);

    private static StockAdjustment NewAdjustment() =>
        StockAdjustment.Create(stockId: 1, reasonId: 1, date: Date, createUserId: 1, createDate: Date, branchId: null, notes: null);

    [Fact]
    public void Cannot_create_adjustment_without_reason() =>
        Assert.Throws<InventoryDocumentLineRequiredException>(() =>
            StockAdjustment.Create(1, 0, Date, 1, Date, null, null));

    [Fact]
    public void Increase_line_posts_with_in_direction()
    {
        var adjustment = NewAdjustment();
        adjustment.AddLine(1, 1, MovementDirection.In, 10, null, "found stock");

        adjustment.Post(Date);

        var evt = Assert.Single(adjustment.DomainEvents);
        var adjusted = Assert.IsType<StockAdjustedDomainEvent>(evt);
        Assert.Equal(MovementDirection.In, adjusted.Direction);
        Assert.Equal(10, adjusted.Quantity);
    }

    [Fact]
    public void Decrease_line_posts_with_out_direction()
    {
        var adjustment = NewAdjustment();
        adjustment.AddLine(1, 1, MovementDirection.Out, 3, null, "damaged");

        adjustment.Post(Date);

        var adjusted = Assert.IsType<StockAdjustedDomainEvent>(Assert.Single(adjustment.DomainEvents));
        Assert.Equal(MovementDirection.Out, adjusted.Direction);
    }

    [Fact]
    public void Mixed_increase_and_decrease_lines_both_post()
    {
        var adjustment = NewAdjustment();
        adjustment.AddLine(1, 1, MovementDirection.In, 10, null, null);
        adjustment.AddLine(2, 1, MovementDirection.Out, 4, null, null);

        adjustment.Post(Date);

        Assert.Equal(2, adjustment.DomainEvents.Count);
    }
}
