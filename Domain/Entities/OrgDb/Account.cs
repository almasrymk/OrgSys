namespace Domain.Entities.OrgDb
{
    [Table("Account")]
    public class Account : LockupTypeTreeEntity
    {
        [StringLength(50, MinimumLength = 3)]
        public override string? Name { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Debit { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Credit { get; set; }

        [ForeignKey("AccountType")]
        public override string? TypeId { get; set; }
       
        public virtual AccountType? AccountType { get; set; }
    }
}