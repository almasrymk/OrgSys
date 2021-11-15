using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("Store")]
    public class Store : BaseModel
    {
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        [ForeignKey("Branch")]
        public long BranchId { get; set; }

        public Branch Branch { get; set; }
    }
}