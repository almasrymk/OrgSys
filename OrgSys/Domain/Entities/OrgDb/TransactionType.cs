using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("TransactionType")]
    public class TransactionType : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public virtual string? Name { get; set; }

        public virtual int InOut { get; set; }

        public virtual string? Icon { get; set; }
    }
}