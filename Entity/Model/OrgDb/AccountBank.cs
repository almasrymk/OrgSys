using Utility;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("AccountBank")]
    public class AccountBank : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        public decimal Debit { get; set; }

        public decimal Credit { get; set; }

        [ForeignKey("Bank")]
        public long BankId { get; set; }
        
        public virtual Bank Bank { get; set; }

        [ForeignKey("BankBranch")]
        public long? BankBranchd { get; set; }
        
        public virtual BankBranch BankBranch { get; set; }

        [ForeignKey("Account")]
        public long? AccountId { get; set; }
       
        public virtual Account Account { get; set; }
    }
}