namespace Treasury.Domain
{
    [Table("Bank")]
    public class Bank : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public virtual string? Name { get; set; }

        /// <summary>Scalar-only MasterData Country reference. FK preserved in OrgContext.</summary>
        public virtual long? CountryId { get; set; }
    }
}