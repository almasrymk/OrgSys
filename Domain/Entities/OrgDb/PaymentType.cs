namespace Domain.Entities
{
    [Table("PaymentType")]
    public class PaymentType : BaseModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override long Id { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public virtual string Name { get; set; }
    }
}