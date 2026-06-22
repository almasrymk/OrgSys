namespace Domain.Entities
{
    [Table("InvoiceType")]
    public class InvoiceType : BaseModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override long Id { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public virtual string Name { get; set; }

        public virtual int InOut { get; set; }

        public virtual string Icon { get; set; }

        public virtual string Group { get; set; }
    }
}