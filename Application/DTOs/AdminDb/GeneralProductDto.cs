using Domain.Entities;

namespace Application.DTOs
{
    public class GeneralProductDto : GeneralProduct
    {             
        public string GeneralClassificationName { get; set; }

        public List<GeneralProductUnitDto> GeneralProductUnitList { get; set; }

        public List<GeneralProductRecipeDto> GeneralProductRecipeList { get; set; }

        public List<GeneralProductPropertyElementDto> GeneralProductPropertyElementList { get; set; }
    }
}