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
