namespace Accounting.Domain
{
    [Table("AccountType")]
    public class AccountType : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public virtual string? Name { get; set; }
        public virtual int DebitOrCredit { get; set; }
    }
}