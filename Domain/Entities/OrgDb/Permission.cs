using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("Permission")]
    public class Permission : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get ; set ; }

        public virtual string Key { get; set; }

        public virtual string Name { get; set; }
    }
}