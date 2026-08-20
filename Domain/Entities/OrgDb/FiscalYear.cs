namespace Domain.Entities
{
    [Table("FiscalYear")]
    public class FiscalYear : BaseModel
    {
        public string Name { get; set; } = default!;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsCurrent { get; set; }

        public FiscalYearStatus FiscalYearStatus { get; set; }

        public virtual ICollection<FiscalPeriod> Periods { get; set; }
            = new List<FiscalPeriod>();
    }
}
