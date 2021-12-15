using Utility;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("Account")]
    public class Account : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; } 

        public decimal Debit { get; set; }

        public decimal Credit { get; set; }

        [ForeignKey("AccountType")]
        public long AccountTypeId { get; set; }
       
        public virtual AccountType AccountType { get; set; }
    }
}