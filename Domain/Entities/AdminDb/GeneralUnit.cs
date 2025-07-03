using Utility;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("GeneralUnit", Schema = "admin")]
    public class GeneralUnit : BaseEntity
    {
        [StringLength(50, MinimumLength = 2)]
        public virtual string Name { get; set; }
    }
}