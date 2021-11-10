using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("Product", Schema = "org")]
    public class Product : BaseModel
    {
        public Product()
        {
            ProductUnits = new HashSet<ProductUnit>();

            ProductRecipes = new HashSet<ProductRecipe>();

            ProductPropertyElements = new HashSet<ProductPropertyElement>();
        }
     
        [Required]
        public string Name { get; set; }

        public string Nickname { get; set; }

        [Required]
        public string Barcode { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal Cost { get; set; }

        [Required]
        public long ClassificationId { get; set; }

        public virtual Classification Classification { get; set; }

        public long? DealerId { get; set; }

        public virtual Dealer Dealer { get; set; }

        public string Recipe { get; set; }

        public ICollection<ProductUnit> ProductUnits { get; set; }

        public ICollection<ProductRecipe> ProductRecipes { get; set; }

        public ICollection<ProductPropertyElement> ProductPropertyElements { get; set; }
    }
}