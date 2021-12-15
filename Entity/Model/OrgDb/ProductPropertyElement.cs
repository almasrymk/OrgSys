using Utility;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("ProductPropertyElement")]
   public class ProductPropertyElement :BaseModel
    {
        [ForeignKey("Product")]
        public long? ProductId { get; set; }

        public Product Product { get; set; }

        [ForeignKey("Property")]
        public long? PropertyId { get; set; }

        public Property Property { get; set; }

        [ForeignKey("PropertyElement")]
        public long? PropertyElementId { get; set; }

        public PropertyElement PropertyElement { get; set; }
        public bool IsChecked { get; set; }
    }
}