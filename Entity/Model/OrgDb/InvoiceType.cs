using Utility;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("InvoiceType")]
    public class InvoiceType : BaseModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override long Id { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        public int InOut { get; set; }

        public string Icon { get; set; }

        public string Group { get; set; }
    }
}