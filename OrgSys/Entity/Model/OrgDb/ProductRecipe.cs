using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Model
{
    [Table("ProductRecipe")]
    public class ProductRecipe : BaseModel
    {            
        public virtual long ProductId { get; set; } 
        
        public virtual long RecipeId { get; set; }    
        
        public virtual long UnitId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal Quantity { get; set; }
    }
}