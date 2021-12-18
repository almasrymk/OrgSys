using Entity.Model;
using Entity.ModelView;

namespace Service
{
    public class OrderProductService : BaseOrgService<OrderProductModelView, OrderProduct>
    {
        public OrderProductService(string Schema) : base(Schema , "Order,Product,Unit") { }
    }
}