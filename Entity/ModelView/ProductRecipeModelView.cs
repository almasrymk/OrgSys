using Utility;
using Entity.Model;
using Utility.Resource;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Entity.ModelView
{
   public class ProductRecipeModelView :BaseModel
    {
        public ProductRecipeModelView()
        {

        }
        public ProductRecipeModelView(ProductRecipe ob)
        {
            if (ob == null)
                ob = new ProductRecipe();
            this.Id = ob.Id;
            this.RecipeId = ob.RecipeId;
            this.ProductId = ob.ProductId;
            this.UnitId = ob.UnitId;
            this.Quantity = ob.Quantity;
            this.Status = ob.Status;
            this.ParentId = ob.ParentId;
            this.TypeId = ob.TypeId;
            this.ImgPath = ob.ImgPath;
        }
        public ProductRecipe Model
        {
            get
            {
                return new ProductRecipe
                {
                    Id = this.Id,
                    RecipeId=this.RecipeId,
                    ProductId = this.ProductId,
                    UnitId = this.UnitId,
                    Quantity = this.Quantity,
                    Status = this.Status,
                    MaskText = this.MaskText,
                    ParentId = this.ParentId,
                    TypeId = this.TypeId,
                    ImgPath = this.ImgPath
                };
            }
        }
        [Display(Name = nameof(Title_Designer.Product), ResourceType = typeof(Title_Designer))]
        public long RecipeId { get; set; }
        [Display(Name = nameof(Title_Designer.Product), ResourceType = typeof(Title_Designer))]
        public long ProductId { get; set; }
   

        [Display(Name = nameof(Title_Designer.UnitMeasure), ResourceType = typeof(Title_Designer))]
        public long UnitId { get; set; }       

        [Display(Name = nameof(Title_Designer.Quantity), ResourceType = typeof(Title_Designer))]
        [Required(ErrorMessageResourceName = nameof(Message_Designer.EnterQuantity), ErrorMessageResourceType = typeof(Message_Designer))]
        public decimal Quantity { get; set; }
    }
}
