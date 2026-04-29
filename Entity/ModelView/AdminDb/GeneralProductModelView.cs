using Entity.Model;
using System.Collections.Generic;

namespace Entity.ModelView
{
    public class GeneralProductModelView : GeneralProduct
    {             
        public string GeneralClassificationName { get; set; }

        public List<GeneralProductUnitModelView> GeneralProductUnits { get; set; }

        public List<GeneralProductRecipeModelView> GeneralProductRecipes { get; set; }

        public List<GeneralProductPropertyElementModelView> GeneralProductPropertyElements { get; set; }
    }
}