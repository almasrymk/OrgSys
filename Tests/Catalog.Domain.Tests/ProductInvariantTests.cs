using Catalog.Domain.Enums;

namespace Catalog.Domain.Tests;

public class ProductInvariantTests
{
    [Fact]
    public void New_product_defaults_to_active_stock_item_with_weighted_average()
    {
        var product = new Product { Name = "Widget", ClassificationId = 1 };

        Assert.True(product.IsActive);
        Assert.Equal(ProductType.StockItem, product.ProductType);
        Assert.Equal(TrackingType.None, product.TrackingType);
        Assert.Equal(CostingMethod.WeightedAverage, product.CostingMethod);
        Assert.False(product.ExpiryTracking);
    }

    [Fact]
    public void Batch_and_serial_is_a_combined_tracking_policy()
    {
        Assert.True(TrackingType.Batch.RequiresBatch());
        Assert.False(TrackingType.Batch.RequiresSerial());
        Assert.True(TrackingType.Serial.RequiresSerial());
        Assert.True(TrackingType.BatchAndSerial.RequiresBatch());
        Assert.True(TrackingType.BatchAndSerial.RequiresSerial());
        Assert.False(TrackingType.None.RequiresBatch());
        Assert.False(TrackingType.None.RequiresSerial());
    }

    [Fact]
    public void Product_type_is_independent_of_tracking_type()
    {
        var service = new Product
        {
            Name = "Install",
            ClassificationId = 1,
            ProductType = ProductType.Service,
            TrackingType = TrackingType.None
        };

        Assert.Equal(ProductType.Service, service.ProductType);
        Assert.Equal(TrackingType.None, service.TrackingType);
        Assert.NotEqual(typeof(ProductType), typeof(TrackingType));
    }
}
