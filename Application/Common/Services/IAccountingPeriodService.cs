namespace Application.Common.Services
{
    using Domain.Entities;
    using Domain.Shared;

    /// <summary>
    /// Single source of truth for resolving and validating the FiscalYear/FiscalPeriod
    /// a journal date falls in. Used by Journal Create, Update (on date change), and Post
    /// so the same rules and messages apply everywhere a journal entry touches the ledger.
    /// </summary>
    public interface IAccountingPeriodService
    {
        Task<AccountingPeriodResult> ResolveAndValidateAsync(DateTime journalDate, CancellationToken cancellationToken = default);

        /// <summary>Plain lookup (no open/closed checks) — used when a journal's date hasn't changed and its
        /// already-resolved FiscalYear is needed for a check that isn't itself about period resolution.</summary>
        Task<FiscalYear?> GetFiscalYearAsync(long fiscalYearId, CancellationToken cancellationToken = default);

        /// <summary>Enforces the Opening Balance rules for a journal of the given type: entry date must equal
        /// the fiscal year's StartDate, and at most one non-deleted/non-cancelled Opening Balance journal may
        /// exist per fiscal year. No-ops (returns no errors) when <paramref name="journalTypeId"/> does not map
        /// to an Opening-Balance JournalType. Pass 0 for <paramref name="journalId"/> when creating a new journal.</summary>
        Task<List<Error>> ValidateOpeningBalanceAsync(long journalTypeId, DateTime journalDate, FiscalYear fiscalYear, long journalId, CancellationToken cancellationToken = default);
    }

    public sealed class AccountingPeriodResult
    {
        public bool Success { get; init; }

        public FiscalYear? FiscalYear { get; init; }

        public FiscalPeriod? FiscalPeriod { get; init; }

        public List<Error> Errors { get; init; } = [];

        public static AccountingPeriodResult Ok(FiscalYear fiscalYear, FiscalPeriod fiscalPeriod) => new()
        {
            Success = true,
            FiscalYear = fiscalYear,
            FiscalPeriod = fiscalPeriod
        };

        public static AccountingPeriodResult Fail(string message) => new()
        {
            Success = false,
            Errors = [new Error(message)]
        };
    }
}
