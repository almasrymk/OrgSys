namespace Domain.Entities
{
    [Table("BankBranch")]
    public class BankBranch : BaseLockupEntity
    {
        [StringLength(50, MinimumLength = 3)]
        public override string? Name { get; set; }

        [ForeignKey("Bank")]
        public virtual string? BankId { get; set; }

        public virtual Bank? Bank { get; set; }

        [ForeignKey("Country")]
        public virtual string? CountryId { get; set; }

        public virtual Country? Country { get; set; }

        [ForeignKey("City")]
        public virtual string? CityId { get; set; }

        public virtual City? City { get; set; }

        [ForeignKey("District")]
        public virtual string? DistrictId { get; set; }

        public virtual District? District { get; set; }
    }
}