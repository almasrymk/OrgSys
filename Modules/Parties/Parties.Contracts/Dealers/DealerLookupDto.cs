namespace Parties.Contracts.Dealers;

/// <summary>
/// Public read shape for a Dealer (Party), used by other modules instead of an EF navigation or
/// direct Parties.Domain.Dealer reference. Mirrors the fields Accounting/Receivables/Payables
/// validators and reports currently read directly off the entity.
/// </summary>
public record DealerLookupDto(long Id, string? Name, long TypeId, long? AccountId, bool Hide, bool IsDeleted);
