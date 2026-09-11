namespace Administration.Domain
{
    [Table("Role")]
    public class Role : BaseModel
    {        
        [StringLength(50, MinimumLength = 3)]
        public virtual string? Name { get; set; }
    }
}