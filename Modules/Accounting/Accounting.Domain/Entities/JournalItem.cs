namespace Accounting.Domain
{
    [Table("JournalItem")]
    public class JournalItem : BaseModel
    {
        /// <summary>EF materialization constructor only — business code must go through
        /// <see cref="Journal.AddLine"/>/<see cref="Journal.UpdateLine"/>, never construct a
        /// JournalItem directly (it has no meaning or repository outside its Journal aggregate).</summary>
        protected JournalItem() { }

        internal JournalItem(long accountId, decimal debit, decimal credit, string? note)
        {
            AccountId = accountId;
            Debit = debit;
            Credit = credit;
            Note = note;
        }

        [ForeignKey("Journal")]
        public virtual long JournalId { get; internal set; }

        public virtual Journal? Journal { get; set; }

        [ForeignKey("Account")]
        public virtual long AccountId { get; private set; }

        public virtual Account? Account { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Debit { get; private set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Credit { get; private set; }

        public virtual string? Note { get; private set; }

        /// <summary>The only way to change an existing line's amounts/account/note — called
        /// exclusively by <see cref="Journal.UpdateLine"/>, which owns the "Draft, not
        /// resource-controlled, account postable" invariants; this method only carries out the
        /// already-validated change.</summary>
        internal void Update(long accountId, decimal debit, decimal credit, string? note)
        {
            AccountId = accountId;
            Debit = debit;
            Credit = credit;
            Note = note;
        }

        /// <summary>
        /// A line is meaningful only if it carries exactly one of Debit/Credit as a positive amount —
        /// a 0/0 line (or one with both sides set) must never count toward a "balanced" journal.
        /// See Journal.Post and the GeneralLedger migration report for the gap this closes.
        /// </summary>
        public bool IsValid => Debit > 0 ^ Credit > 0;
    }
}
