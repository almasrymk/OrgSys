namespace Inventory.Contracts.Stocks;

using OrgSys.SharedKernel;

/// <summary>
/// Public contract for batch-resolving Stock display names by id (e.g. Sales Invoice search
/// results showing StockName without an EF Include across the module boundary). Handled by
/// Inventory.Application. Missing ids are simply absent from the result.
/// </summary>
public record GetStockNamesQuery(IReadOnlyCollection<long> StockIds) : IQuery<Dictionary<long, string?>>;
