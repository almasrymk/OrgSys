namespace Treasury.Domain
{
    [Table("CashBox")]
    public class CashBox : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public virtual string? Name { get; set; }

        // No navigation to Accounting.Domain.Account — see Parties.Domain/Entities/Dealer.cs for why.
        public virtual long? AccountId { get; set; }

        [ForeignKey(nameof(FinancialAccount))]
        public virtual long? FinancialAccountId { get; set; }
        public virtual FinancialAccount? FinancialAccount { get; set; }

        [ForeignKey(nameof(Branch))]
        public virtual long? BranchId { get; set; }
        public virtual Branch? Branch { get; set; }

        [ForeignKey(nameof(KeeperUser))]
        public virtual long? KeeperUserId { get; set; }
        public virtual User? KeeperUser { get; set; }
    }
}
