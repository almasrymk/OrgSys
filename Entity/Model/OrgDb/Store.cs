using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("Store")]
    public class Store : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public virtual string Name { get; set; }

        [ForeignKey("Branch")]
        public virtual long BranchId { get; set; }

        public virtual Branch Branch { get; set; }
    }
}