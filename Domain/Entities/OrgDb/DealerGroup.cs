namespace Domain.Entities
{
    [Table("DealerGroup")]
    public class DealerGroup : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public virtual string Name { get; set; }      
    }
}