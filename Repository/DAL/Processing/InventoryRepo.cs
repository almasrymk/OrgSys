using Domain.Entities;

namespace Repository
{
    public class InventoryRepo : CurdOrg<Inventory>
    {
        public InventoryRepo(string Schema) : base(Schema) { }
    }
}