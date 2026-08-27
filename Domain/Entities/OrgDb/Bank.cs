namespace Domain.Entities
{
    [Table("Bank")]
    public class Bank : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public virtual string? Name { get; set; }

        [ForeignKey("Country")]
        public virtual long? CountryId { get; set; }

        public virtual Country? Country { get; set; }
    }
}