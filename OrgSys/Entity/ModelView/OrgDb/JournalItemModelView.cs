using Entity.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.ModelView
{
    public class JournalItemModelView : BaseModel
    {
         public virtual long JournalId { get; set; }

        public string AccountName { get; set; }
         
        public virtual long AccountId { get; set; }

        public string JournalCode { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Debit { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Credit { get; set; }

        public virtual string Note { get; set; }
    }
}