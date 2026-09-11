namespace Organization.Domain
{
    [Table("Branch")]
    public class Branch : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public string? Name { get; set; }
    }
}