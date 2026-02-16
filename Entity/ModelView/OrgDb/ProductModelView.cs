using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.ModelView
{
    public class ProductModelView : BaseModel
    {
        [Required]
        public virtual string Name { get; set; }

        public virtual string Nickname { get; set; }

        public virtual string Barcode { get; set; }

        public virtual string Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Price { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Cost { get; set; }

        public virtual long ClassificationId { get; set; }

        public virtual long? DealerId { get; set; }

        public virtual string Recipe { get; set; }      

        public decimal Quantity { get; set; }

        public string ClassificationName { get; set; }

        public string DealerName { get; set; }

        public decimal Balance { get; set; }

        public List<ProductUnitModelView> ProductUnitList { get; set; }

        public List<ProductRecipeModelView> ProductRecipeList { get; set; }

        public List<ProductPropertyElementModelView> ProductPropertyElementList { get; set; }

        public List<TreeView> ProductPropertyTree { get; set; }
    }
}