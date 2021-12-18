using Entity.Model;
using Entity.ModelView;

namespace Service
{
    public class ProductRecipeService : BaseOrgService<ProductRecipeModelView, ProductRecipe>
    {
        public ProductRecipeService(string Schema) : base(Schema) { }
    }
}