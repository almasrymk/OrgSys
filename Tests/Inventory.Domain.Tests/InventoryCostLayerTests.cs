namespace Inventory.Domain.Tests;

public class InventoryCostLayerTests
{
    private static readonly DateTime Date = new(2026, 9, 1);

    [Fact]
    public void Consume_takes_no_more_than_remaining_quantity()
    {
        var layer = InventoryCostLayer.Create(productId: 1, stockId: 1, batchId: null, receiptMovementId: 10, receiptDate: Date, quantity: 100, unitCost: 10);

        var consumed = layer.Consume(150);

        Assert.Equal(100, consumed);
        Assert.Equal(0, layer.RemainingQuantity);
        Assert.True(layer.IsDepleted);
    }

    [Fact]
    public void Fifo_consumes_oldest_layer_first()
    {
        var layer1 = InventoryCostLayer.Create(1, 1, null, 10, Date, 100, 10);
        var layer2 = InventoryCostLayer.Create(1, 1, null, 11, Date.AddDays(1), 50, 12);
        var layers = new[] { layer1, layer2 };

        // Issue 120: consumes 100 @ 10 from layer1, then 20 @ 12 from layer2 — brief §22 example.
        decimal remaining = 120;
        decimal totalCost = 0;
        foreach (var layer in layers)
        {
            if (remaining <= 0) break;
            var taken = layer.Consume(remaining);
            totalCost += taken * layer.UnitCost;
            remaining -= taken;
        }

        Assert.Equal(0, remaining);
        Assert.Equal(0, layer1.RemainingQuantity);
        Assert.Equal(30, layer2.RemainingQuantity);
        Assert.Equal((100 * 10) + (20 * 12), totalCost);
    }

    [Fact]
    public void Cannot_create_layer_with_negative_cost() =>
        Assert.Throws<InsufficientStockException>(() => InventoryCostLayer.Create(1, 1, null, 10, Date, 10, -1));
}
