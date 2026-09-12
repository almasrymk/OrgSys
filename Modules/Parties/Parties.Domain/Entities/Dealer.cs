namespace Parties.Domain
{
    [Table("Dealer")]
    public class Dealer : BaseModel
    {
        [Required]
        public virtual string Name { get; set; } = null!;

        [StringLength(25, MinimumLength = 8)] 
        public virtual string? Phone { get; set; }

        [StringLength(30, MinimumLength = 3)]
        public virtual string? Email { get; set; }

        [StringLength(500, MinimumLength = 3)]
        public virtual string? Address { get; set; }

        [ForeignKey("DealerGroup")]
        public virtual long? DealerGroupId { get; set; }

        public virtual DealerGroup? DealerGroup { get; set; }

        [ForeignKey("Country")]
        public virtual long? CountryId { get; set; }

        public virtual Country? Country { get; set; }

        [ForeignKey("City")]
        public virtual long? CityId { get; set; }

        public virtual City? City { get; set; }

        [ForeignKey("District")]
        public virtual long? DistrictId { get; set; }

        public virtual District? District { get; set; }

        [ForeignKey("Account")]
        public virtual long? AccountId { get; set; }

        public virtual Account? Account { get; set; }
    }
}