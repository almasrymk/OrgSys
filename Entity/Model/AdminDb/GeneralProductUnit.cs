using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("GeneralProductUnit", Schema = "admin")]
    public class GeneralProductUnit : BaseModel
    {
        [ForeignKey("GeneralProduct")]
        public long GeneralProductId { get; set; }

        public GeneralProduct GeneralProduct { get; set; }

        [ForeignKey("GeneralUnit")]
        public long GeneralUnitId { get; set; }

        public GeneralUnit GeneralUnit { get; set; }

        [Required]
        public decimal Rate { get; set; }

        public bool DefaultUnit { get; set; }
    }
}