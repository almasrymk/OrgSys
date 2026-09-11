namespace Accounting.Domain
{
    [Table("Account")]
    public class Account : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public virtual string? Name { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Debit { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Credit { get; set; }

        [ForeignKey("AccountType")]
        public long AccountTypeId { get; set; }

        public virtual AccountType? AccountType { get; set; }

        /// <summary>False marks a group/parent account used only for hierarchy — Journal postings
        /// (including a Dealer's receivable account link) must target a postable/detail account.</summary>
        public virtual bool IsPostable { get; set; } = true;
    }
}