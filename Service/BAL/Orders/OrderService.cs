using Entity.Model;
using Entity.ModelView;

namespace Service
{
    public class OrderService : BaseOrgService<OrderModelView, Order>
    {
        public OrderService(string Schema) : base(Schema, "Dealer,Table,Invoice,OrderProducts,OrderProducts.Product,OrderProducts.Product.ProductUnits,,OrderProducts.Product.ProductUnits.Unit") { }
    }
}