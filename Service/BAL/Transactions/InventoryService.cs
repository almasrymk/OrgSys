using Entity.Model;
using Entity.ModelView;

namespace Service
{
    public class InventoryService : BaseOrgService<InventoryModelView, Inventory>
    {
        public InventoryService(string Schema) : base(Schema, "Store,InventoryProducts,InventoryProducts.Product,InventoryProducts.Unit") { }
    }
}