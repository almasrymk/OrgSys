namespace Domain.Entities
{
    [Table("Safe")]
    public class Safe : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public virtual string? Name { get; set; }

        [ForeignKey("Account")]
        public virtual long? AccountId { get; set; }

        public virtual Account? Account { get; set; }

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
