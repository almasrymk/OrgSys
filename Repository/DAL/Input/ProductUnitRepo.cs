using Domain.Entities;

namespace Repository
{
    public class ProductUnitRepo : CurdOrg<ProductUnit>
    {
        public ProductUnitRepo(string Schema) : base(Schema) { }
    }
}