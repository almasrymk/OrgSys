using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("ProductPropertyElement")]
   public class ProductPropertyElement :BaseModel
    {
        public long ProductId { get; set; }

        public virtual Product Product { get; set; }

        public long PropertyId { get; set; }

        public virtual Property Property { get; set; }

        public long PropertyElementId { get; set; }

        public virtual PropertyElement PropertyElement { get; set; }

        public Boolean IsChecked { get; set; }
    }
}