namespace Treasury.Domain
{
    [Table("BankBranch")]
    public class BankBranch : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public virtual string? Name { get; set; }

        [ForeignKey("Bank")]
        public virtual long BankId { get; set; }

        public virtual Bank? Bank { get; set; }

        /// <summary>Scalar-only MasterData geo references. FKs preserved in OrgContext.</summary>
        public virtual long CountryId { get; set; }

        public virtual long CityId { get; set; }

        public virtual long DistrictId { get; set; }
    }
}