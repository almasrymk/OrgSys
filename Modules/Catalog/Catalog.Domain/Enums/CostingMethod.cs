namespace Catalog.Domain.Enums;

/// <summary>Per-item costing method (brief §20-22). WeightedAverage is the default and the only
/// method that was effectively in use before this pass (a flat, manually-entered Product.Cost);
/// Fifo is opt-in per item and consumes InventoryCostLayer rows oldest-first.</summary>
public enum CostingMethod
{
    WeightedAverage = 0,
    Fifo = 1
}
