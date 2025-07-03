using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("InvoiceType")]
    public class InvoiceType : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public virtual string Name { get; set; }

        public virtual int InOut { get; set; }

        public virtual string Icon { get; set; }

        public virtual string Group { get; set; }
    }
}