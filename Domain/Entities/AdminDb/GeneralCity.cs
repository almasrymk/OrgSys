namespace Domain.Entities
{
    [Table("GeneralCity", Schema = "admin")]
    public class GeneralCity : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public virtual string Name { get; set; }

        [ForeignKey("GeneralCountry")]
        public virtual long GeneralCountryId { get; set; }
       
        public virtual GeneralCountry GeneralCountry { get; set; }
    }
}