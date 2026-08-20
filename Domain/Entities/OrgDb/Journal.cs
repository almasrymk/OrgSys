namespace Domain.Entities
{
    [Table("Journal")]
    public class Journal : MovementModel
    {
        [ForeignKey("JournalType")]
        public virtual long JournalTypeId { get; set; }

        public virtual JournalType? JournalType { get; set; }

        [ForeignKey("Currency")]
        public virtual long CurrencyId { get; set; }

        public virtual Currency? Currency { get; set; }

        public virtual decimal Rate { get; set; }

        [ForeignKey("FiscalYear")]
        public virtual long FiscalYearId { get; set; }

        public virtual FiscalYear? FiscalYear { get; set; }

        [ForeignKey("FiscalPeriod")]
        public virtual long FiscalPeriodId { get; set; }

        public virtual FiscalPeriod? FiscalPeriod { get; set; }

        public virtual long RefranceId { get; set; }

        public virtual string? RefranceCode { get; set; }
        
        public virtual long RefranceTypeId { get; set; }

        public virtual string? RefranceTable { get; set; }

        public virtual string? Note { get; set; }
        public virtual ICollection<JournalItem>? JournalItems { get; set; }
    }
}
