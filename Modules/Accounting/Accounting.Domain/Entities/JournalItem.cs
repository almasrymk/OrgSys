namespace Accounting.Domain
{
    [Table("JournalItem")]
    public class JournalItem : BaseModel
    {
        [ForeignKey("Journal")]
        public virtual long JournalId { get; set; }

        public virtual Journal? Journal { get; set; }

        [ForeignKey("Account")]
        public virtual long AccountId { get; set; }

        public virtual Account? Account { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Debit { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Credit { get; set; }

        public virtual string? Note { get; set; }

        /// <summary>
        /// A line is meaningful only if it carries exactly one of Debit/Credit as a positive amount —
        /// a 0/0 line (or one with both sides set) must never count toward a "balanced" journal.
        /// See Journal.Post and the GeneralLedger migration report for the gap this closes.
        /// </summary>
        public bool IsValid => Debit > 0 ^ Credit > 0;
    }
}
