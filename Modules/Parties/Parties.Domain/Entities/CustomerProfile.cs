namespace Parties.Domain
{
    /// <summary>
    /// The Customer role on top of a shared Dealer identity (brief §2.5) — one per Dealer (unique
    /// FK). Deliberately thin: procurement-specific fields (LeadTime, MOQ, SupplierProductCode) stay
    /// out of Party entirely per brief §2.6, and receivable-accounting configuration beyond "which
    /// GL account" stays in AR's own module, not here (brief §2.5's own caution). CreditLimit/
    /// IsCreditAllowed/IsOnHold are the minimal commercial flags the brief explicitly lists that have
    /// nowhere else to live.
    /// </summary>
    [Table("CustomerProfile")]
    public class CustomerProfile : BaseModel
    {
        [Required]
        [ForeignKey("Dealer")]
        public virtual long DealerId { get; set; }

        public virtual Dealer? Dealer { get; set; }

        /// <summary>Scalar-only reference into Accounting.Domain.Account — same GeneralLedger
        /// bounded-context isolation as Dealer.AccountId (Parties.Domain must not reference
        /// Accounting.Domain). Provisioned through the same
        /// DealerReceivableAccountProvisioning helper Dealer's own Create/Update already use.</summary>
        public virtual long? AccountId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal? CreditLimit { get; set; }

        public virtual bool IsCreditAllowed { get; set; } = true;

        public virtual bool IsOnHold { get; set; }
    }
}
