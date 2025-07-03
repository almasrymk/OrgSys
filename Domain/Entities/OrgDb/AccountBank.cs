namespace Domain.Entities
{
    [Table("AccountBank")]
    public class AccountBank : BaseLockupEntity
    {
        [StringLength(50, MinimumLength = 3)]
        public override string? Name { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Debit { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Credit { get; set; }

        [ForeignKey("Bank")]
        public virtual string? BankId { get; set; }
        
        public virtual Bank? Bank { get; set; }

        [ForeignKey("BankBranch")]
        public virtual string? BankBranchd { get; set; }
        
        public virtual BankBranch? BankBranch { get; set; }

        [ForeignKey("Account")]
        public virtual string? AccountId { get; set; }
       
        public virtual Account? Account { get; set; }
    }
}