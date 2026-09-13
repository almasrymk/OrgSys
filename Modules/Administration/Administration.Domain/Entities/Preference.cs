namespace Administration.Domain
{
    [Table("Preference")]
    public class Preference : BaseModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override long Id { get; set; }

        public virtual string? Key { get; set; }

        public virtual string? Value { get; set; }

        public virtual string? Reference { get; set; }

        public virtual long? UserId { get; set; }
    }
}