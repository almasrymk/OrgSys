namespace Application.Common.Services
{
    using Domain.Abstraction;
    using Domain.Entities;
    using Domain.Enums;

    public sealed class AccountingPeriodService(
        IRepository<FiscalYear> _FiscalYearRepository,
        IRepository<FiscalPeriod> _FiscalPeriodRepository) : IAccountingPeriodService
    {
        public async Task<AccountingPeriodResult> ResolveAndValidateAsync(DateTime journalDate, CancellationToken cancellationToken = default)
        {
            var date = journalDate.Date;

            var fiscalYears = await _FiscalYearRepository.GetListByFilterAsync(
                e => e.StartDate.Date <= date && e.EndDate.Date >= date &&
                     e.Status != Status.Deleted && e.Hide != true);
            var fiscalYear = fiscalYears?.FirstOrDefault();

            if (fiscalYear is null)
                return AccountingPeriodResult.Fail($"No fiscal year is configured for journal date {date:yyyy-MM-dd}.");

            if (fiscalYear.FiscalYearStatus == FiscalYearStatus.Closed)
                return AccountingPeriodResult.Fail($"Fiscal year {fiscalYear.Name} is closed. Journal entries cannot be created or posted.");

            var fiscalPeriods = await _FiscalPeriodRepository.GetListByFilterAsync(
                e => e.FiscalYearId == fiscalYear.Id && e.StartDate.Date <= date && e.EndDate.Date >= date &&
                     e.Status != Status.Deleted && e.Hide != true);
            var fiscalPeriod = fiscalPeriods?.FirstOrDefault();

            if (fiscalPeriod is null)
                return AccountingPeriodResult.Fail($"No fiscal period is configured for journal date {date:yyyy-MM-dd}.");

            if (fiscalPeriod.FiscalPeriodStatus == FiscalPeriodStatus.Closed)
                return AccountingPeriodResult.Fail($"Fiscal period {fiscalPeriod.Name} is closed. Journal entries cannot be created or posted.");

            if (fiscalPeriod.FiscalPeriodStatus == FiscalPeriodStatus.Locked)
                return AccountingPeriodResult.Fail($"Fiscal period {fiscalPeriod.Name} is locked. Journal entries cannot be created or posted.");

            return AccountingPeriodResult.Ok(fiscalYear, fiscalPeriod);
        }
    }
}
