using Domain.Entities;

namespace Application.DTOs
{
    public class GeneralProductModelView : GeneralProduct
    {             
        public string GeneralClassificationName { get; set; }

        public List<GeneralProductUnitModelView> GeneralProductUnitList { get; set; }

        public List<GeneralProductRecipeModelView> GeneralProductRecipeList { get; set; }

        public List<GeneralProductPropertyElementModelView> GeneralProductPropertyElementList { get; set; }
    }
}