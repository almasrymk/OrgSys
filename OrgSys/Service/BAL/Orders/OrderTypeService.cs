using Entity.Model;
using Entity.ModelView;

namespace Service
{
    public class OrderTypeService : BaseOrgService<OrderTypeModelView, OrderType>
    {
        public OrderTypeService(string Schema) : base(Schema) { }
    }
}