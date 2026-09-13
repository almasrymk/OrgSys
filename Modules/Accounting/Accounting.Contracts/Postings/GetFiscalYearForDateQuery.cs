namespace Accounting.Contracts.Postings;

using OrgSys.SharedKernel;

/// <summary>
/// Resolves the fiscal year a given date falls in — used by callers that need the fiscal-year
/// boundaries themselves for a same-fiscal-year duplicate check (e.g. Treasury's Opening Balance
/// posting, which rejects a second Opening Balance for the same Financial Account within one
/// fiscal year) without depending on Accounting.Domain.FiscalYear or Accounting.Application.
/// IAccountingPeriodService directly. Returns null when no fiscal year covers the date (the same
/// case IAccountingPeriodService.ResolveAndValidateAsync reports as a validation failure).
/// </summary>
public sealed record GetFiscalYearForDateQuery(DateTime Date) : IQuery<FiscalYearRangeDto?>;

public sealed record FiscalYearRangeDto(long FiscalYearId, DateTime StartDate, DateTime EndDate);
