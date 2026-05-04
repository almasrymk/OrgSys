using Entity.Model;
using Entity.ModelView;

namespace Service
{
    public class InventoryProductService : BaseOrgService<InventoryProductModelView, InventoryProduct>
    {
        public InventoryProductService(string Schema) : base(Schema , "Inventory,Product,Unit") { }
    }
}