namespace Domain.Entities.OrgDb
{
    [Table("Bank")]
    public class Bank : BaseLockupEntity
    {
        [StringLength(50, MinimumLength = 3)]
        public override string? Name { get; set; }      
    }
}