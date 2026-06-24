namespace Domain.Entities
{
    [Table("District")]
    public class District : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public virtual string Name { get; set; }

        [ForeignKey("Country")]
        public virtual long? CountryId { get; set; }

        public virtual Country Country { get; set; }

        [ForeignKey("City")]
        public virtual long? CityId { get; set; }

        public virtual City City { get; set; }
    }
}