namespace Parties.Domain
{
    /// <summary>
    /// A contact person at a Dealer (brief §2.7) — a Dealer may have several; deleting a Dealer must
    /// not delete transaction history elsewhere (nothing outside Parties references PartyContact, so
    /// this is naturally satisfied).
    /// </summary>
    [Table("PartyContact")]
    public class PartyContact : BaseModel
    {
        [Required]
        [ForeignKey("Dealer")]
        public virtual long DealerId { get; set; }

        public virtual Dealer? Dealer { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 2)]
        public virtual string Name { get; set; } = null!;

        [StringLength(100)]
        public virtual string? JobTitle { get; set; }

        [StringLength(100)]
        public virtual string? Department { get; set; }

        [StringLength(30, MinimumLength = 3)]
        public virtual string? Email { get; set; }

        [StringLength(25, MinimumLength = 8)]
        public virtual string? Phone { get; set; }

        [StringLength(25, MinimumLength = 8)]
        public virtual string? Mobile { get; set; }

        public virtual bool IsPrimary { get; set; }

        public virtual bool IsActive { get; set; } = true;
    }
}
