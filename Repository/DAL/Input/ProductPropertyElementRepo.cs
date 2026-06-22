using Domain.Entities;

namespace Repository
{
    public class ProductPropertyElementRepo : CurdOrg<ProductPropertyElement>
    {
        public ProductPropertyElementRepo(string Schema) : base(Schema) { }
    }
}