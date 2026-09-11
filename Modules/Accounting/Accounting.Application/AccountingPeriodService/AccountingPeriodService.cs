namespace Accounting.Application
{

    public sealed class AccountingPeriodService(
        IRepository<FiscalYear> _FiscalYearRepository,
        IRepository<FiscalPeriod> _FiscalPeriodRepository,
        IRepository<JournalType> _JournalTypeRepository,
        IRepository<Journal> _JournalRepository) : IAccountingPeriodService
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

        public async Task<FiscalYear?> GetFiscalYearAsync(long fiscalYearId, CancellationToken cancellationToken = default)
        {
            var fiscalYears = await _FiscalYearRepository.GetListByFilterAsync(e => e.Id == fiscalYearId);
            return fiscalYears?.FirstOrDefault();
        }

        public async Task<List<Error>> ValidateOpeningBalanceAsync(long journalTypeId, DateTime journalDate, FiscalYear fiscalYear, long journalId, CancellationToken cancellationToken = default)
        {
            var errors = new List<Error>();

            var journalTypes = await _JournalTypeRepository.GetListByFilterAsync(e => e.Id == journalTypeId);
            var journalType = journalTypes?.FirstOrDefault();
            if (journalType is null || !journalType.IsOpeningBlance)
                return errors;

            if (journalDate.Date != fiscalYear.StartDate.Date)
            {
                errors.Add(new Error($"Opening Balance entry date must equal the fiscal year start date ({fiscalYear.StartDate:yyyy-MM-dd})."));
                return errors;
            }

            var openingBalanceTypeIds = (await _JournalTypeRepository.GetListByFilterAsync(e => e.IsOpeningBlance))
                ?.Select(e => e.Id).ToList() ?? [];

            var duplicates = await _JournalRepository.GetListByFilterAsync(e =>
                e.Id != journalId &&
                e.FiscalYearId == fiscalYear.Id &&
                openingBalanceTypeIds.Contains(e.JournalTypeId) &&
                e.Status != Status.Deleted &&
                e.Status != Status.Cancel &&
                e.Hide != true);

            if (duplicates is not null && duplicates.Any())
                errors.Add(new Error($"An Opening Balance journal already exists for fiscal year {fiscalYear.Name}."));

            return errors;
        }
    }
}
