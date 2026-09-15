namespace Organization.Contracts.Companies;

/// <summary>
/// Public read shape for a Company, used by other modules instead of a direct
/// Organization.Domain.Company reference. Mirrors MasterData.Contracts.Currencies.CurrencyLookupDto.
/// </summary>
public record CompanyLookupDto(long Id, string LegalName, string? TradeName, bool IsActive);
