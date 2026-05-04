using Utility;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("GeneralUnit", Schema = "admin")]
    public class GeneralUnit : BaseModel
    {
        [StringLength(50, MinimumLength = 2)]
        public virtual string Name { get; set; }
    }
}