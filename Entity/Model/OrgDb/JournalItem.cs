using Utility;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("JournalItem")]
    public class JournalItem : BaseModel
    {        
        [ForeignKey("Journal")]
        public long JournalId { get; set; }

        public Journal Journal { get; set; }

        [ForeignKey("Account")]
        public long AccountId { get; set; }

        public virtual Account Account { get; set; }

        public decimal Debit { get; set; }

        public decimal Credit { get; set; }

        public string Note { get; set; }
    }
}