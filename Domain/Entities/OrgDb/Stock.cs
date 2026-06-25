namespace Domain.Entities
{
    [Table("Stock")]
    public class Stock : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public virtual string? Name { get; set; }

        [ForeignKey("Branch")]
        public virtual long BranchId { get; set; }

        public virtual Branch? Branch { get; set; }
    }
}