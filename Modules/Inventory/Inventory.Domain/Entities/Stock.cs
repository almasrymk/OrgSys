namespace Inventory.Domain
{
    [Table("Stock")]
    public class Stock : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public virtual string? Name { get; set; }

        [ForeignKey("Branch")]
        public virtual long BranchId { get; set; }

        public virtual Branch? Branch { get; set; }

        // No navigation to Accounting.Domain.Account — see Dealer.cs (Parties.Domain) for why;
        // same GeneralLedger bounded-context isolation rule applies here.
        public virtual long? AccountId { get; set; }
    }
}
