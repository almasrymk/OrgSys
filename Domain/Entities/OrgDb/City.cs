namespace Domain.Entities
{
    [Table("City")]
    public class City : BaseLockupEntity
    {
        [StringLength(50, MinimumLength = 3)]
        public override string? Name { get; set; }

        [ForeignKey("Country")]
        public string? CountryId { get; set; }
       
        public virtual Country? Country { get; set; }
    }
}