namespace Domain.Entities
{
    [Table("Bank")]
    public class Bank : BaseLockupEntity
    {
        [StringLength(50, MinimumLength = 3)]
        public override string? Name { get; set; }      
    }
}