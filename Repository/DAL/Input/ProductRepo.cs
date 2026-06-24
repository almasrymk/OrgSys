using Domain.Entities;

namespace Repository
{
    public class ProductRepo : CurdOrg<Product>
    {
        public ProductRepo(string Schema) : base(Schema) { }
    }
}