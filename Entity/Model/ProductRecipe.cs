using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Utility;

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
