namespace Inventory.Contracts.Products;

using OrgSys.SharedKernel;

/// <summary>
/// Public contract for batch-resolving Product display names by id (e.g. Sales Invoice line
/// items showing ProductName without an EF Include across the module boundary). Handled by
/// Inventory.Application. Missing ids are simply absent from the result.
/// </summary>
public record GetProductNamesQuery(IReadOnlyCollection<long> ProductIds) : IQuery<Dictionary<long, string?>>;
