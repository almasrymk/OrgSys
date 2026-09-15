namespace Catalog.Domain
{
    [Table("Unit")]
    public class Unit : BaseModel
    {
        [StringLength(50, MinimumLength = 2)]
        public virtual string? Name { get; set; }
    }
}