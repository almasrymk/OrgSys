namespace Domain.Entities
{
    [Table("Country")]
    public class Country : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public virtual string Name { get; set; }      
    }
}