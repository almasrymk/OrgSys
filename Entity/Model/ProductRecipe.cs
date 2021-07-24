using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("ProductRecipe")]
    public class ProductRecipe : BaseModel
    {            
        public long ProductId { get; set; } 
        
        public long RecipeId { get; set; }    
        
        public long UnitId { get; set; }    
        
        public decimal Quantity { get; set; }
    }
}