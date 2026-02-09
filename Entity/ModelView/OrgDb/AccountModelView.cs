using Entity.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.ModelView
{
    public class AccountModelView : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public virtual string Name { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Debit { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Credit { get; set; }

        public long AccountTypeId { get; set; }

        public string AccountTypeName { get; set; }

        public string ParentName { get; set; }
    }
}