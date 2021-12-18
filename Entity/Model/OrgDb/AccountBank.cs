using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("AccountBank")]
    public class AccountBank : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public virtual string Name { get; set; }

        public virtual decimal Debit { get; set; }

        public virtual decimal Credit { get; set; }

        [ForeignKey("Bank")]
        public virtual long BankId { get; set; }
        
        public virtual Bank Bank { get; set; }

        [ForeignKey("BankBranch")]
        public virtual long? BankBranchd { get; set; }
        
        public virtual BankBranch BankBranch { get; set; }

        [ForeignKey("Account")]
        public virtual long? AccountId { get; set; }
       
        public virtual Account Account { get; set; }
    }
}