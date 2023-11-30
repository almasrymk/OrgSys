using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("GeneralProductUnit", Schema = "admin")]
    public class GeneralProductUnit : BaseModel
    {
        [ForeignKey("GeneralProduct")]
        public virtual long GeneralProductId { get; set; }

        public virtual GeneralProduct GeneralProduct { get; set; }

        [ForeignKey("GeneralUnit")]
        public virtual long GeneralUnitId { get; set; }

        public virtual GeneralUnit GeneralUnit { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Rate { get; set; }

        public virtual bool DefaultUnit { get; set; }
    }
}