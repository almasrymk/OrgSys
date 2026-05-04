using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("TransactionType")]
    public class TransactionType : BaseModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override long Id { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public virtual string Name { get; set; }

        public virtual int InOut { get; set; }

        public virtual string Icon { get; set; }
    }
}