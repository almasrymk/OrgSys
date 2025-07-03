namespace Domain.Entities
{
    [Table("Branch")]
    public class Branch : BaseLockupEntity
    {
        [StringLength(50, MinimumLength = 3)]
        public override string? Name { get; set; }
    }
}