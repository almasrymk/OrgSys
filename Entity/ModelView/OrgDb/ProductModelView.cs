using Entity.Model;
using System.Collections.Generic;

namespace Entity.ModelView
{
    public class ProductModelView : Product
    { 

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