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

        /// <summary>Scalar-only reference into Organization.Domain.Branch — no EF navigation.
        /// FK preserved via Fluent HasOne(typeof(Branch)) in OrgContext.</summary>
        public virtual long? BranchId { get; set; }

        /// <summary>Scalar-only reference into Administration.Domain.User — no EF navigation.
        /// FK preserved via Fluent HasOne(typeof(User)) in OrgContext, same pattern as
        /// MovementModel.CreateUserId.</summary>
        public virtual long? KeeperUserId { get; set; }
    }
}
