namespace Parties.Contracts.Dealers;

using OrgSys.SharedKernel;

/// <summary>
/// Public contract for resolving a single Dealer by id (e.g. Accounting's AR/AP account
/// validators). Handled by Parties.Application. Returns null when not found.
/// </summary>
public record GetDealerByIdQuery(long Id) : IQuery<DealerLookupDto?>;
