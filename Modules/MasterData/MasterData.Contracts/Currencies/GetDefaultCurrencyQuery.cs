namespace MasterData.Contracts.Currencies;

using OrgSys.SharedKernel;

/// <summary>
/// Public contract for resolving the system's default Currency (e.g. Receivables/Payables opening
/// balance postings). Handled by MasterData.Application. Returns null if no currency is marked
/// default.
/// </summary>
public record GetDefaultCurrencyQuery : IQuery<CurrencyLookupDto?>;
