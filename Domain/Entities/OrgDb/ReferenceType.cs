namespace Domain.Entities
{
    [Table("ReferenceType")]
    public class ReferenceType : BaseModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override long Id { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public virtual string? Name { get; set; }
    }
}
