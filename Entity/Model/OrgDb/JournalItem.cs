using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("JournalItem")]
    public class JournalItem : BaseModel
    {        
        [ForeignKey("Journal")]
        public virtual long JournalId { get; set; }

        public virtual Journal Journal { get; set; }

        [ForeignKey("Account")]
        public virtual long AccountId { get; set; }

        public virtual Account Account { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Debit { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Credit { get; set; }

        public virtual string Note { get; set; }
    }
}