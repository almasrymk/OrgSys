namespace Domain.Entities
{
    [Table("Outlay")]
    public class Outlay : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public virtual string? Name { get; set; }
    }
}