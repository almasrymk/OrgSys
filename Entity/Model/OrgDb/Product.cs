using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("Product")]
    public class Product : BaseModel
    {       
        [Required]
        public string Name { get; set; }

        public string Nickname { get; set; }

        [Required]
        public string Barcode { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal Cost { get; set; }

        [ForeignKey("Classification")]
        public long ClassificationId { get; set; }

        public Classification Classification { get; set; }

        [ForeignKey("Dealer")]
        public long? DealerId { get; set; }

        public Dealer Dealer { get; set; }

        public string Recipe { get; set; }

        public ICollection<ProductUnit> ProductUnits { get; set; }

        public ICollection<ProductRecipe> ProductRecipes { get; set; }

        public ICollection<ProductPropertyElement> ProductPropertyElements { get; set; }
    }
}