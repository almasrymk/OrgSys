namespace Treasury.Domain
{
    [Table("BankAccount")]
    public class BankAccount : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public virtual string? Name { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Debit { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Credit { get; set; }

        [ForeignKey("Bank")]
        public virtual long BankId { get; set; }

        public virtual Bank? Bank { get; set; }

        [ForeignKey("BankBranch")]
        public virtual long? BankBranchd { get; set; }

        public virtual BankBranch? BankBranch { get; set; }

        [ForeignKey("Account")]
        public virtual long? AccountId { get; set; }

        public virtual Account? Account { get; set; }

        [ForeignKey(nameof(FinancialAccount))]
        public virtual long? FinancialAccountId { get; set; }
        public virtual FinancialAccount? FinancialAccount { get; set; }

        [StringLength(100)]
        public string? AccountNumber { get; set; }

        [StringLength(34)]
        public string? IBAN { get; set; }

        [StringLength(11)]
        public string? SwiftCode { get; set; }

        [ForeignKey(nameof(Branch))]
        public virtual long? BranchId { get; set; }

        public virtual Branch? Branch { get; set; }
    }
}