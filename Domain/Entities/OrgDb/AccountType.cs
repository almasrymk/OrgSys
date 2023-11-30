namespace Domain.Entities.OrgDb
{
    [Table("AccountType")]
    public class AccountType : BaseLockupEntity
    {
        [StringLength(50, MinimumLength = 3)]
        public override string? Name { get; set; }

        public  int DebitOrCredit { get; set; }
    }
}