namespace Domain.Entities
{
    [Table("BankBranch")]
    public class BankBranch : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public virtual string Name { get; set; }

        [ForeignKey("Bank")]
        public virtual long BankId { get; set; }

        public virtual Bank Bank { get; set; }

        [ForeignKey("Country")]
        public virtual long CountryId { get; set; }

        public virtual Country Country { get; set; }

        [ForeignKey("City")]
        public virtual long CityId { get; set; }

        public virtual City City { get; set; }

        [ForeignKey("District")]
        public virtual long DistrictId { get; set; }

        public virtual District District { get; set; }
    }
}