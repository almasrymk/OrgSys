namespace Catalog.Domain.Enums;

/// <summary>
/// Replaces scattered booleans (HasBatch/HasSerial) with one explicit item-tracking policy
/// (brief §18). A flags enum rather than two separate booleans so "BatchAndSerial" reads as one
/// deliberate policy instead of an incidental combination.
/// </summary>
[Flags]
public enum TrackingType
{
    None = 0,
    Batch = 1,
    Serial = 2,
    BatchAndSerial = Batch | Serial
}

public static class TrackingTypeExtensions
{
    public static bool RequiresBatch(this TrackingType type) => type.HasFlag(TrackingType.Batch);

    public static bool RequiresSerial(this TrackingType type) => type.HasFlag(TrackingType.Serial);
}
