using Entity.Model;

namespace Repository
{
    public class RecipeRepo : CurdOrg<ProductRecipe>
    {
        public RecipeRepo(string Schema) : base(Schema) { }
    }
}