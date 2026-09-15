namespace Parties.Domain
{
    /// <summary>
    /// The Supplier role on top of a shared Dealer identity (brief §2.6) — one per Dealer (unique
    /// FK). Deliberately thin — see CustomerProfile's own doc comment for the same reasoning;
    /// procurement-specific supplier/product relationship data (LeadTime, MOQ, SupplierProductCode/
    /// Price) stays in Purchasing, not here (brief §2.6).
    /// </summary>
    [Table("SupplierProfile")]
    public class SupplierProfile : BaseModel
    {
        [Required]
        [ForeignKey("Dealer")]
        public virtual long DealerId { get; set; }

        public virtual Dealer? Dealer { get; set; }

        /// <summary>Scalar-only reference into Accounting.Domain.Account — see
        /// CustomerProfile.AccountId. Provisioned through the same DealerPayableAccountProvisioning
        /// helper Dealer's own Create/Update already use.</summary>
        public virtual long? AccountId { get; set; }

        public virtual bool IsApproved { get; set; } = true;

        public virtual bool IsOnHold { get; set; }
    }
}
