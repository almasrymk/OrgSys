using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("Safe")]
    public class Safe : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public virtual string Name { get; set; }

        [ForeignKey("Account")]
        public virtual long? AccountId { get; set; }

        public virtual Account Account { get; set; }
    }
}