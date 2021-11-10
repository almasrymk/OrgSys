using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("Store", Schema = "org")]
    public class Store : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        public long BranchId { get; set; }

        public virtual Branch Branch { get; set; }
    }
}