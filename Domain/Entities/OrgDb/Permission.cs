namespace Domain.Entities
{
    [Table("Permission")]
    public class Permission : BaseModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override long Id { get ; set ; }

        public virtual string Key { get; set; }

        public virtual string Name { get; set; }
    }
}