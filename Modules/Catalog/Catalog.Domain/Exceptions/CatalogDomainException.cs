namespace Catalog.Domain.Exceptions;

/// <summary>Base type for Catalog domain-invariant violations, matching the
/// PayableDomainException/InventoryDomainException convention used by every other module.</summary>
public abstract class CatalogDomainException(string message) : Exception(message);
