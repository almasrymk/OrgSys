namespace Domain.Entities
{
    [Table("Account")]
    public class Account : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public virtual string? Name { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Debit { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Credit { get; set; }

        [ForeignKey("AccountType")]
        public long AccountTypeId { get; set; }
       
        public virtual AccountType? AccountType { get; set; }
    }
}