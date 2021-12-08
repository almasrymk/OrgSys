using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("GeneralProductRecipe", Schema = "admin")]
    public class GeneralProductRecipe : BaseModel
    {            
        public long ProductId { get; set; } 
        
        public long RecipeId { get; set; }    
        
        public long UnitId { get; set; }    
        
        public decimal Quantity { get; set; }
    }
}