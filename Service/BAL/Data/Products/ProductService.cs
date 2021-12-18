using Entity.Model;
using Entity.ModelView;

namespace Service
{
    public class ProductService : BaseOrgService<ProductModelView, Product>
    {
        public ProductService(string Schema) : base(Schema , "Classification,Dealer,ProductUnits,ProductRecipes,ProductPropertyElements") { }
    }
}