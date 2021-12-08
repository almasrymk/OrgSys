using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("GeneralProduct", Schema = "admin")]
    public class GeneralProduct : BaseModel
    {       
        [Required]
        public string Name { get; set; }

        public string Nickname { get; set; }

        [Required]
        public string Barcode { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal Cost { get; set; }

        [ForeignKey("GeneralClassification")]
        public long GeneralClassificationId { get; set; }

        public GeneralClassification GeneralClassification { get; set; }      

        public string Recipe { get; set; }

        public ICollection<GeneralProductUnit> GeneralProductUnits { get; set; }

        public ICollection<GeneralProductRecipe> GeneralProductRecipes { get; set; }

        public ICollection<GeneralProductPropertyElement> GeneralProductPropertyElements { get; set; }
    }
}