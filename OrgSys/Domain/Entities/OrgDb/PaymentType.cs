using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("PaymentType")]
    public class PaymentType : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public virtual string Name { get; set; }
    }
}