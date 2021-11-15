using Entity.Model;

namespace Repository
{
    public class ProductUnitRepo : CurdOrg<ProductUnit>
    {
        public ProductUnitRepo(string Schema) : base(Schema) { }
    }
}