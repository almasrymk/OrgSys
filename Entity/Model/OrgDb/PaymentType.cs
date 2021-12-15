using Utility;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("PaymentType")]
    public class PaymentType : BaseModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override long Id { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }
    }
}