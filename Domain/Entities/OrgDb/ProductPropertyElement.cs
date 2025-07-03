using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("ProductPropertyElement")]
   public class ProductPropertyElement : BaseEntity
    {
        [ForeignKey("Product")]
        public virtual long? ProductId { get; set; }

        public virtual Product Product { get; set; }

        [ForeignKey("Property")]
        public virtual long? PropertyId { get; set; }

        public virtual Property Property { get; set; }

        [ForeignKey("PropertyElement")]
        public virtual long? PropertyElementId { get; set; }

        public virtual PropertyElement PropertyElement { get; set; }

        public virtual bool IsChecked { get; set; }
    }
}