using Entity.Model;
using Entity.ModelView;

namespace Service
{
    public class ProductUnitService : BaseOrgService<ProductUnitModelView, ProductUnit>
    {
        public ProductUnitService(string Schema) : base(Schema , "Product,Unit") { }
    }
}