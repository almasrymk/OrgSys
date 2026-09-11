namespace Treasury.Domain
{
    [Table("FinancialType")]
    public class FinancialType : BaseModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public  override long Id { get ; set; }
        [StringLength(50, MinimumLength = 3)]
        public virtual string? Name { get; set; }

        public virtual int InOut { get; set; }

        public virtual string? Icon { get; set; }
    }
}