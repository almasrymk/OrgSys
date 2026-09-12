namespace MasterData.Contracts.Currencies;

/// <summary>
/// Public read shape for a Currency, used by other modules instead of an EF navigation or direct
/// MasterData.Domain.Currency reference.
/// </summary>
public record CurrencyLookupDto(long Id, string? Name, decimal Rate, bool IsDefault);
