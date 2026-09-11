namespace Accounting.Domain
{
    [Table("JournalType")]
    public class JournalType : BaseModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override long Id { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public virtual string? Name { get; set; }

        public virtual bool IsOpeningBlance { get; set; }

        public virtual string? Icon { get; set; }

        public virtual string? Group { get; set; }
    }
}