namespace Parties.Domain
{
    /// <summary>
    /// A typed, repeatable address for a Dealer (brief §2.8) — additive alongside the pre-existing
    /// flat <see cref="Dealer.Address"/>/Country/City/District fields, which stay exactly as they
    /// are (still the Dealer's single "default" address for every screen/report that predates this).
    /// Country/City/District navigations mirror Dealer's own existing pattern — Parties.Domain
    /// already has the accepted exception to reference MasterData.Domain (Dealer.Country/City/
    /// District), so this adds no new Architecture.Tests entry.
    /// </summary>
    [Table("PartyAddress")]
    public class PartyAddress : BaseModel
    {
        [Required]
        [ForeignKey("Dealer")]
        public virtual long DealerId { get; set; }

        public virtual Dealer? Dealer { get; set; }

        [Required]
        public virtual PartyAddressType AddressType { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 3)]
        public virtual string Line1 { get; set; } = null!;

        [StringLength(500)]
        public virtual string? Line2 { get; set; }

        [ForeignKey("Country")]
        public virtual long? CountryId { get; set; }

        public virtual Country? Country { get; set; }

        [ForeignKey("City")]
        public virtual long? CityId { get; set; }

        public virtual City? City { get; set; }

        [ForeignKey("District")]
        public virtual long? DistrictId { get; set; }

        public virtual District? District { get; set; }

        [StringLength(20)]
        public virtual string? PostalCode { get; set; }

        public virtual bool IsPrimary { get; set; }
    }
}
