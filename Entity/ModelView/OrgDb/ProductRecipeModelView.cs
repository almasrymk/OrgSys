using Utility;
using Entity.Model;

namespace Entity.ModelView
{
    public class ProductRecipeModelView : BaseModel
    {
        public ProductRecipeModelView()
        {

        }
        public ProductRecipeModelView(ProductRecipe ob)
        {
            if (ob == null)
                ob = new ProductRecipe();

            this.RecipeId = ob.RecipeId;

            this.ProductId = ob.ProductId;

            this.UnitId = ob.UnitId;

            this.Quantity = ob.Quantity;

            this.Id = ob.Id;

            this.CodeNumber = ob.CodeNumber;

            this.Code = ob.Code;

            this.MaskText = ob.MaskText;

            this.ParentId = ob.ParentId;

            this.TypeId = ob.TypeId;

            this.Hide = ob.Hide;

            this.ImgPath = ob.ImgPath;

            this.Status = ob.Status;
        }
        public ProductRecipe Model()
        {
            return new ProductRecipe
            {
                RecipeId = this.RecipeId,
                ProductId = this.ProductId,
                UnitId = this.UnitId,
                Quantity = this.Quantity,
                Id = this.Id,
                CodeNumber = this.CodeNumber,
                Code = this.Code,
                MaskText = this.MaskText,
                ParentId = this.ParentId,
                TypeId = this.TypeId,
                Hide = this.Hide,
                Status = this.Status,
                ImgPath = this.ImgPath
            };
        }

        public long RecipeId { get; set; }

        public long ProductId { get; set; }

        public long UnitId { get; set; }

        public decimal Quantity { get; set; }
    }
}