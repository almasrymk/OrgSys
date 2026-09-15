namespace Catalog.Domain.Enums;

/// <summary>
/// The value shape a Property (attribute definition) expects. Default is Selection, matching every
/// existing Property row's implicit current behavior (docs/catalog/catalog-target-architecture.md
/// §2) — this enum only makes that implicit shape explicit, it does not change existing behavior.
/// </summary>
public enum AttributeDataType
{
    Text = 0,
    Number = 1,
    Boolean = 2,
    Date = 3,
    Selection = 4,
    MultiSelection = 5
}
