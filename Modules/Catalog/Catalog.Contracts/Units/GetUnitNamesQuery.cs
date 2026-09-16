namespace Catalog.Contracts.Units;

using OrgSys.SharedKernel;

public record GetUnitNamesQuery(IReadOnlyCollection<long> UnitIds) : IQuery<Dictionary<long, string?>>;
