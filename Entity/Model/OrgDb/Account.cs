using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("Account")]
    public class Account : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public virtual string Name { get; set; } 

        public virtual decimal Debit { get; set; }

        public virtual decimal Credit { get; set; }

        [ForeignKey("AccountType")]
        public long AccountTypeId { get; set; }
       
        public virtual AccountType AccountType { get; set; }
    }
}