namespace Domain.Entities
{
    [Table("Journal")]
    public class Journal : MovementModel
    {       
        [ForeignKey("Currency")]
        public virtual long CurrencyId { get; set; }

        public virtual Currency? Currency { get; set; }

        public virtual decimal Rate { get; set; }

        public virtual long RefranceId { get; set; }

        public virtual long RefranceTypeId { get; set; }

        public virtual string? RefranceTable { get; set; }

        public virtual string? Note { get; set; }
        public virtual ICollection<JournalItem>? JournalItems { get; set; }
    }
}