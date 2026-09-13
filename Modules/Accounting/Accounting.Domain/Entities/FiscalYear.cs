namespace Accounting.Domain
{
    using Accounting.Domain.Exceptions;

    [Table("FiscalYear")]
    public class FiscalYear : BaseModel
    {
        public string Name { get; set; } = default!;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsCurrent { get; set; }

        /// <summary>See FiscalPeriod.FiscalPeriodStatus for why this stays a normal public setter
        /// rather than private — same generic-CRUD backward-compatibility reasoning applies here.</summary>
        public FiscalYearStatus FiscalYearStatus { get; set; }

        public virtual ICollection<FiscalPeriod> Periods { get; set; }
            = new List<FiscalPeriod>();

        /// <summary>Final-authority check used by <see cref="Journal.Post"/> alongside FiscalPeriod.EnsureOpenForPosting — a Journal must never be posted into a Closed fiscal year.</summary>
        public void EnsureOpenForPosting()
        {
            if (FiscalYearStatus == FiscalYearStatus.Closed)
                throw new AccountingPeriodClosedException($"Fiscal year {Name} is closed. Journal entries cannot be created or posted.");
        }
    }
}
