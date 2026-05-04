using Entity.Model;
using Entity.ModelView;

namespace Service
{
    public class ProductPropertyElementService : BaseOrgService<ProductPropertyElementModelView, ProductPropertyElement>
    {
        public ProductPropertyElementService(string Schema) : base(Schema , "Product,Property,PropertyElement") { }
    }
}