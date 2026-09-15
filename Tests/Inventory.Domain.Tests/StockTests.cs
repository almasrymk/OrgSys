namespace Inventory.Domain.Tests;

public class StockTests
{
    [Fact]
    public void Create_warehouse_with_valid_data()
    {
        var stock = Stock.Create("WH-01", "Main Warehouse", branchId: 1);

        Assert.Equal("WH-01", stock.Code);
        Assert.Equal("Main Warehouse", stock.Name);
        Assert.Equal(1, stock.BranchId);
        Assert.True(stock.IsActive);
    }

    [Fact]
    public void Cannot_create_warehouse_without_code() =>
        Assert.Throws<WarehouseCodeRequiredException>(() => Stock.Create("", "Main", 1));

    [Fact]
    public void Cannot_create_warehouse_without_branch() =>
        Assert.Throws<WarehouseCodeRequiredException>(() => Stock.Create("WH-01", "Main", 0));

    [Fact]
    public void Inactive_warehouse_rejects_posting()
    {
        var stock = Stock.Create("WH-01", "Main", 1);
        stock.Deactivate();

        Assert.Throws<WarehouseInactiveException>(() => stock.EnsureActiveForPosting());
    }

    [Fact]
    public void Reactivated_warehouse_accepts_posting()
    {
        var stock = Stock.Create("WH-01", "Main", 1);
        stock.Deactivate();
        stock.Activate();

        stock.EnsureActiveForPosting(); // does not throw
    }
}
