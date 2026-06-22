using Domain.Entities;

namespace Repository
{
    public class InventoryProductRepo : CurdOrg<InventoryProduct>
    {
        public InventoryProductRepo(string Schema) : base(Schema) { }
    }
}