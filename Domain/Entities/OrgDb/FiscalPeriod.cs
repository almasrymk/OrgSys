namespace Domain.Entities
{
    [Table("FiscalPeriod")]
    public class FiscalPeriod : BaseModel
    {
        [ForeignKey(nameof(FiscalYear))]
        public long FiscalYearId { get; set; }

        public virtual FiscalYear FiscalYear { get; set; } = default!;

        public int PeriodNumber { get; set; }

        public string Name { get; set; } = default!;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public FiscalPeriodStatus FiscalPeriodStatus { get; set; }
    }
}
